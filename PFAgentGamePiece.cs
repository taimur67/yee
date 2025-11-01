using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001AD RID: 429
	public class PFAgentGamePiece : PFAgent
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x0002446F File Offset: 0x0002266F
		public PFAgentGamePiece(GamePiece gp = null)
		{
			this.GamePiece = gp;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000244A0 File Offset: 0x000226A0
		public override float GenerateCostModifierForNode(Pathfinder pathfinder, PFNode node)
		{
			float num = base.GenerateCostModifierForNode(pathfinder, node);
			PFNodeHexCoord pfnodeHexCoord = node as PFNodeHexCoord;
			if (pfnodeHexCoord == null)
			{
				return num;
			}
			PathfinderHexboard pathfinderHexboard = pathfinder as PathfinderHexboard;
			if (pathfinderHexboard == null)
			{
				return num;
			}
			TerrainType type = pathfinderHexboard.HexBoard[pfnodeHexCoord.Location].Type;
			num += pathfinderHexboard.GetTerrainModifierCostForGamePiece(this.GamePiece, type);
			if (pathfinderHexboard.IsHexDangerous(this.GamePiece, pfnodeHexCoord.Location))
			{
				num += 10f;
			}
			return num;
		}

		// Token: 0x040003A6 RID: 934
		public GamePiece GamePiece;

		// Token: 0x040003A7 RID: 935
		public GamePieceAvoidance AvoidanceType = GamePieceAvoidance.All;

		// Token: 0x040003A8 RID: 936
		public bool DestinationAlwaysValid = true;

		// Token: 0x040003A9 RID: 937
		public bool AllowRedeployToDestination = true;

		// Token: 0x040003AA RID: 938
		public List<int> IgnoreDiplomacyWithPlayers = new List<int>();
	}
}
