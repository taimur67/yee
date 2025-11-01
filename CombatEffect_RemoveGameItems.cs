using System;

namespace LoG
{
	// Token: 0x02000343 RID: 835
	public class CombatEffect_RemoveGameItems : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FF1 RID: 4081 RVA: 0x0003EF58 File Offset: 0x0003D158
		protected override GameEvent OnWinnerDamageDealt(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			bool flag = false;
			PlayerState player = context.Turn.FindPlayerState(context.Opponent.ControllingPlayerId, null);
			foreach (Identifier id in IEnumerableExtensions.ToList<Identifier>(context.Opponent.Slots))
			{
				GameItem gameItem = context.Turn.FetchGameItem(id);
				if (gameItem.Category == this.Category)
				{
					context.TurnContext.MoveItemToVault(player, gameItem);
					flag = true;
				}
			}
			if (flag)
			{
				context.TurnContext.RecalculateModifiers(context.Opponent);
			}
			if (flag)
			{
				return new BattleAbilityEvent(this.CurrentAbilityStage, source, context, base.TypeName);
			}
			return null;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003F024 File Offset: 0x0003D224
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_RemoveGameItems combatEffect_RemoveGameItems = new CombatEffect_RemoveGameItems
			{
				Category = this.Category
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_RemoveGameItems);
			clone = combatEffect_RemoveGameItems;
		}

		// Token: 0x04000765 RID: 1893
		public GameItemCategory Category;
	}
}
