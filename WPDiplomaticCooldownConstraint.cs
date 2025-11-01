using System;

namespace LoG
{
	// Token: 0x02000163 RID: 355
	public class WPDiplomaticCooldownConstraint : WorldProperty
	{
		// Token: 0x060006EA RID: 1770 RVA: 0x00021F71 File Offset: 0x00020171
		public WPDiplomaticCooldownConstraint(DiplomaticCooldownType cooldown, int duration)
		{
			this.CooldownType = cooldown;
			this.Duration = duration;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00021F88 File Offset: 0x00020188
		internal override bool IsFulfilledInternal(TurnContext turnContext, PlayerState playerState, GOAPPlanner planner)
		{
			int turnValue = turnContext.CurrentTurn.TurnValue;
			int turnDiplomaticAbilityLastUsed = this.OwningPlanner.AIPersistentData.GetTurnDiplomaticAbilityLastUsed(this.CooldownType);
			if (planner.IsEndGame || !playerState.AITags.Contains(AITag.ApplyActionCooldowns))
			{
				return turnValue >= turnDiplomaticAbilityLastUsed + 1;
			}
			int num = this.OwningPlanner.AIPersistentData.DiplomaticCooldownRandomTimer;
			if (turnDiplomaticAbilityLastUsed != -1)
			{
				num += turnDiplomaticAbilityLastUsed + this.Duration;
			}
			return turnValue >= num;
		}

		// Token: 0x0400032F RID: 815
		public DiplomaticCooldownType CooldownType;

		// Token: 0x04000330 RID: 816
		public int Duration;
	}
}
