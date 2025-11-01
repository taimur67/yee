using System;

namespace LoG
{
	// Token: 0x0200017E RID: 382
	public class WPMapHazardWalk : WorldProperty<WPMapHazardWalk>
	{
		// Token: 0x06000738 RID: 1848 RVA: 0x00022AAF File Offset: 0x00020CAF
		public WPMapHazardWalk(Identifier legionID, TerrainType terrainType)
		{
			this.LegionID = legionID;
			this.TerrainType = terrainType;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00022AC8 File Offset: 0x00020CC8
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			GamePiece gamePiece = viewContext.CurrentTurn.FetchGameItem<GamePiece>(this.LegionID);
			return LegionMovementProcessor.CalculateMovementCostType(this.TerrainType, MoveCostType.MovePoints, gamePiece) == MoveCostType.MovePoints;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00022AF7 File Offset: 0x00020CF7
		public override WPProvidesEffect ProvidesEffectInternal(WPMapHazardWalk property)
		{
			if (property.LegionID == this.LegionID && property.TerrainType == this.TerrainType)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x0400034F RID: 847
		public Identifier LegionID;

		// Token: 0x04000350 RID: 848
		public TerrainType TerrainType;
	}
}
