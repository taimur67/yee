using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020001A3 RID: 419
	public class GameGeneratorPathfinder : Pathfinder<PFNodeHexCoord, PFAgent>
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x000239D8 File Offset: 0x00021BD8
		public GameGeneratorPathfinder(GameGenerationContext context)
		{
			this.PopulateMap(context);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x000239E8 File Offset: 0x00021BE8
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

		// Token: 0x060007B8 RID: 1976 RVA: 0x00023A7C File Offset: 0x00021C7C
		private bool CanBeTraversed(GameGenerationContext context, HexCoord hexCoord)
		{
			if (context.Structures.Contains(hexCoord))
			{
				return false;
			}
			Hex hex = context.Board[hexCoord];
			if (hex == null)
			{
				return false;
			}
			TerrainType type = hex.Type;
			if (type != TerrainType.Plain)
			{
			}
			return true;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00023ABC File Offset: 0x00021CBC
		private void ApplyTraversalCosts(GameGenerationContext context)
		{
			HexBoard board = context.Board;
			foreach (PFNodeHexCoord pfnodeHexCoord in this.Map)
			{
				if (!this.CanBeTraversed(context, pfnodeHexCoord.Location))
				{
					pfnodeHexCoord.Disable("Hex cannot be traversed");
				}
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00023B2C File Offset: 0x00021D2C
		public int DistanceToClosest(HexCoord startingPoint, IEnumerable<HexCoord> destinations)
		{
			if (!IEnumerableExtensions.Any<HexCoord>(destinations))
			{
				return int.MaxValue;
			}
			return destinations.Min((HexCoord dest) => this.GetDistance(startingPoint, dest));
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00023B6D File Offset: 0x00021D6D
		public int DistanceToClosest(HexCoord startingPoint, IEnumerable<GamePiece> gamePieces)
		{
			return this.DistanceToClosest(startingPoint, from gamePiece in gamePieces
			select gamePiece.Location);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00023B9C File Offset: 0x00021D9C
		public bool TryFindPath(HexCoord start, HexCoord end, out List<HexCoord> path)
		{
			PFNodeHexCoord start2 = this[start];
			PFNodeHexCoord destination = this[end];
			List<PFNodeHexCoord> source;
			bool flag = base.TryFindPath(start2, destination, null, out source, null);
			path = IEnumerableExtensions.ToList<HexCoord>(from t in source
			select t.Location);
			bool flag2;
			if (path.Count > 1)
			{
				HexCoord hexCoord = path.Last<HexCoord>();
				flag2 = (hexCoord == end);
			}
			else
			{
				flag2 = false;
			}
			return flag && flag2;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00023C10 File Offset: 0x00021E10
		public bool TryGetDistance(HexCoord start, HexCoord end, out int distance)
		{
			distance = int.MaxValue;
			List<HexCoord> list;
			if (!this.TryFindPath(start, end, out list))
			{
				return false;
			}
			distance = list.Count;
			return true;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00023C3C File Offset: 0x00021E3C
		public int GetDistance(HexCoord start, HexCoord end)
		{
			int result;
			if (!this.TryGetDistance(start, end, out result))
			{
				return int.MaxValue;
			}
			return result;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00023C5C File Offset: 0x00021E5C
		public override IEnumerable<PFNodeHexCoord> GetNeighbours(PFNodeHexCoord currentNode, PFNodeHexCoord destination, PFAgent agent)
		{
			return from t in this._hexBoard.EnumerateNeighboursNormalized(currentNode.Location)
			select this[t] into x
			where !x.IsDisabled()
			select x;
		}

		// Token: 0x17000195 RID: 405
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

		// Token: 0x0400038A RID: 906
		private HexBoard _hexBoard;
	}
}
