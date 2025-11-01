using System;

namespace LoG
{
	// Token: 0x0200032F RID: 815
	public class AbilityEffect_RemoveOnDeath : AbilityEffect
	{
		// Token: 0x06000FB0 RID: 4016 RVA: 0x0003E2B4 File Offset: 0x0003C4B4
		public void OnOwnerBanished(Ability ability, TurnContext context, GamePiece owner)
		{
			if (owner.DeathTurn < 0 || owner.Status != GameItemStatus.Banished)
			{
				return;
			}
			if (ability.ModifierGroupId == default(Guid))
			{
				return;
			}
			context.CurrentTurn.GlobalModifierStack.Pop(ability.ModifierGroupId);
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0003E304 File Offset: 0x0003C504
		public override void DeepClone(out AbilityEffect clone)
		{
			AbilityEffect_RemoveOnDeath abilityEffect_RemoveOnDeath = new AbilityEffect_RemoveOnDeath();
			base.DeepCloneAbilityEffectParts(abilityEffect_RemoveOnDeath);
			clone = abilityEffect_RemoveOnDeath;
		}
	}
}
