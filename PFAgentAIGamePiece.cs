using System;

namespace LoG
{
	// Token: 0x020001AC RID: 428
	public class PFAgentAIGamePiece : PFAgentGamePiece
	{
		// Token: 0x060007E7 RID: 2023 RVA: 0x00024397 File Offset: 0x00022597
		public PFAgentAIGamePiece(GamePiece movingLegion, GOAPPlanner planner) : base(movingLegion)
		{
			this.Planner = planner;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000243A8 File Offset: 0x000225A8
		public override float GenerateCostModifierForNode(Pathfinder pathfinder, PFNode node)
		{
			float num = base.GenerateCostModifierForNode(pathfinder, node);
			PFNodeHexCoord pfnodeHexCoord = node as PFNodeHexCoord;
			if (pfnodeHexCoord == null)
			{
				return num;
			}
			int controllingPlayerId = this.GamePiece.ControllingPlayerId;
			float num2;
			if (this.Planner.TerrainInfluenceMap.TryGetPylonDesirability(pfnodeHexCoord.Location, out num2) && this.Planner.HexPathfinder.HexBoard[pfnodeHexCoord.Location].ControllingPlayerID != this.Planner.PlayerId)
			{
				num -= num2;
			}
			if (this.Planner.TerrainInfluenceMap.GetHexIsAdjacentTo(pfnodeHexCoord.Location, controllingPlayerId))
			{
				num -= 0.5f;
			}
			if (this.Planner.AIPersistentData.IsHexClaimedForMovement(pfnodeHexCoord.Location))
			{
				num += 10f;
			}
			return Math.Max(num, -1f);
		}

		// Token: 0x040003A5 RID: 933
		public GOAPPlanner Planner;
	}
}
