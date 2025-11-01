using System;

namespace LoG
{
	// Token: 0x02000152 RID: 338
	public class WPActionCooldown : WorldProperty
	{
		// Token: 0x060006AE RID: 1710 RVA: 0x000215F2 File Offset: 0x0001F7F2
		public WPActionCooldown(ActionID actionType, int cooldownDuration)
		{
			this.ActionType = actionType;
			this.CooldownDuration = cooldownDuration;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00021608 File Offset: 0x0001F808
		internal override bool IsFulfilledInternal(TurnContext turnContext, PlayerState playerState, GOAPPlanner planner)
		{
			int num = this.CooldownDuration;
			if (planner.IsEndGame || !playerState.AITags.Contains(AITag.ApplyActionCooldowns))
			{
				num = 1;
			}
			return planner.AIPersistentData.GetTurnsSinceLastUsed(this.ActionType, turnContext.CurrentTurn) >= num;
		}

		// Token: 0x04000308 RID: 776
		public readonly ActionID ActionType;

		// Token: 0x04000309 RID: 777
		public readonly int CooldownDuration;
	}
}
