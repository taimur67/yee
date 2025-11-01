using System;

namespace LoG
{
	// Token: 0x0200016E RID: 366
	public class WPHasForcesNearby : WorldProperty<WPHasForcesNearby>
	{
		// Token: 0x0600070C RID: 1804 RVA: 0x000224BC File Offset: 0x000206BC
		public WPHasForcesNearby(GamePiece target, int maxTurnsToReach = 1)
		{
			this.Target = target;
			this.MaxTurnsToReach = maxTurnsToReach;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x000224D9 File Offset: 0x000206D9
		public override WPProvidesEffect ProvidesEffectInternal(WPHasForcesNearby nearbyPrecondition)
		{
			if (nearbyPrecondition.Target.Id != this.Target.Id)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000224F8 File Offset: 0x000206F8
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			InfluenceData influenceData;
			if (!planner.TerrainInfluenceMap.InfMap.TryGetValue(this.Target.Location, out influenceData))
			{
				return false;
			}
			foreach (GamePiece gamePiece in viewContext.CurrentTurn.GetActiveGamePiecesForPlayer(playerState.Id))
			{
				int num;
				if (gamePiece.IsLegionOrTitan() && influenceData.TryGetTurnsToReach(gamePiece, out num, true) && num <= this.MaxTurnsToReach)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400033F RID: 831
		public GamePiece Target;

		// Token: 0x04000340 RID: 832
		public int MaxTurnsToReach = 1;
	}
}
