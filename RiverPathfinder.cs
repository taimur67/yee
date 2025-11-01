using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020001AF RID: 431
	public class RiverPathfinder : Pathfinder<PFNodeHexCoord, PFAgent>
	{
		// Token: 0x06000804 RID: 2052 RVA: 0x000250AD File Offset: 0x000232AD
		public RiverPathfinder(GameGenerationContext context)
		{
			this.PopulateMap(context);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x000250BC File Offset: 0x000232BC
		public void PopulateMap(GameGenerationContext context)
		{
			this._hexBoard = context.Board;
			ListExtensions.ClearFast<PFNodeHexCoord>(this.Map);
			foreach (Hex hex in this._hexBoard.GetAllHexes())
			{
				HexCoord location = this._hexBoard.ToRelativeHex(hex.HexCoord);
				this.Map.Add(new PFNodeHexCoord(location, this._hexBoard));
			}
			this.ApplyTraversalCosts(context);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00025150 File Offset: 0x00023350
		private void ApplyTraversalCosts(GameGenerationContext context)
		{
			foreach (PFNodeHexCoord pfnodeHexCoord in this.Map)
			{
				if (!context.AvailableHexes.Contains(pfnodeHexCoord.Location))
				{
					pfnodeHexCoord.Disable("Occupied Hex");
				}
				else
				{
					pfnodeHexCoord.AddRawScalarCostModifier(PFCostModifier.Heuristic_Bonus, (float)(context.Rand.Next() % 3));
				}
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000251D4 File Offset: 0x000233D4
		public bool TryExtendRiver(HexRiver river, HexCoord end)
		{
			HexBoard hexBoard = this._hexBoard;
			HexCoord hexCoord = river.Hexes.Last<HexCoord>();
			HexCoord coord = hexBoard.ToRelativeHex(hexCoord);
			end = this._hexBoard.ToRelativeHex(end);
			if (!coord.IsValid || !end.IsValid)
			{
				return false;
			}
			PFNodeHexCoord start = this[coord];
			PFNodeHexCoord destination = this[end];
			for (int i = 1; i < river.Size - 1; i++)
			{
				HexCoord coord2 = river.Hexes[i];
				this[coord2].Disable("Already a river");
				foreach (HexCoord hexCoord2 in this._hexBoard.EnumerateNeighbours(coord2))
				{
					if (!river.Hexes.Contains(hexCoord2))
					{
						this[hexCoord2].Disable("Adjacent to river");
					}
				}
			}
			List<PFNodeHexCoord> source;
			object obj = base.TryFindPath(start, destination, null, out source, null);
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>(from t in source
			select t.Location);
			object obj2;
			if (list.Count > 1)
			{
				hexCoord = list.Last<HexCoord>();
				obj2 = (hexCoord == end);
			}
			else
			{
				obj2 = 0;
			}
			object obj3 = obj & obj2;
			if (obj3 != null)
			{
				river.AddRange(list);
			}
			return obj3 != null;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00025334 File Offset: 0x00023534
		public override IEnumerable<PFNodeHexCoord> GetNeighbours(PFNodeHexCoord currentNode, PFNodeHexCoord destination, PFAgent agent)
		{
			return from t in this._hexBoard.EnumerateNeighboursNormalized(currentNode.Location)
			select this[t] into x
			where !x.IsDisabled()
			select x;
		}

		// Token: 0x1700019B RID: 411
		private PFNodeHexCoord this[HexCoord coord]
		{
			get
			{
				return this.Map[this._hexBoard.ToIndex(coord)];
			}
			set
			{
				this.Map[this._hexBoard.ToIndex(coord)] = value;
			}
		}

		// Token: 0x040003B3 RID: 947
		private HexBoard _hexBoard;
	}
}
