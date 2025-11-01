using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020001A6 RID: 422
	public class PFNodeHexCoord : PFNode
	{
		// Token: 0x060007D6 RID: 2006 RVA: 0x00023E92 File Offset: 0x00022092
		public PFNodeHexCoord(HexCoord location, HexBoard hexBoard)
		{
			this.Location = location;
			this._hexBoard = hexBoard;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00023EA8 File Offset: 0x000220A8
		public override float Heuristic(PFNode destination)
		{
			PFNodeHexCoord pfnodeHexCoord = destination as PFNodeHexCoord;
			return this._hexBoard.ShortestDistancef(this.Location, pfnodeHexCoord.Location);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00023ED3 File Offset: 0x000220D3
		public override IEnumerable<PFNode> SelectValidNeighbours(PFAgent agent, PFNode finalDestination)
		{
			return Enumerable.Empty<PFNode>();
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00023EDA File Offset: 0x000220DA
		public override IEnumerable<PFNode> GetNeighbours()
		{
			return Enumerable.Empty<PFNode>();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00023EE1 File Offset: 0x000220E1
		public override bool IsValidNeighbour(PFAgent agent, PFNode callingNode)
		{
			return false;
		}

		// Token: 0x04000399 RID: 921
		public HexCoord Location;

		// Token: 0x0400039A RID: 922
		private HexBoard _hexBoard;
	}
}
