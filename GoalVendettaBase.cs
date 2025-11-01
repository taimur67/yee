using System;

namespace LoG
{
	// Token: 0x0200014C RID: 332
	public abstract class GoalVendettaBase : GoalGOAPNode
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0002131F File Offset: 0x0001F51F
		protected override bool IsFulfilledByMovingOutOfDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00021322 File Offset: 0x0001F522
		public override TargetContext GetTarget()
		{
			TargetContext targetContext = new TargetContext();
			targetContext.SetTargetPlayer(this.ArchfiendID);
			return targetContext;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00021338 File Offset: 0x0001F538
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			VendettaState vendettaState = playerViewOfTurnState.CurrentDiplomaticTurn.GetDiplomaticStatus(playerState.Id, this.ArchfiendID).DiplomaticState as VendettaState;
			if (vendettaState == null)
			{
				return 0f;
			}
			Vendetta vendetta = vendettaState.Vendetta;
			float result = 0.9f;
			float amount = MathF.Min(1f, (float)(vendetta.PrestigeWager + vendetta.PrestigeReward) / 30f);
			ref result.LerpTo01(amount);
			float amount2 = -MathF.Min(1f, (float)(vendetta.TurnRemaining - 3) / 10f);
			ref result.LerpTo01(amount2);
			float amount3 = 0.5f * (1f - vendetta.Objective.StageProgress);
			ref result.LerpTo01(amount3);
			return result;
		}

		// Token: 0x04000303 RID: 771
		public int ArchfiendID;
	}
}
