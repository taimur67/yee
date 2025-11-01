using System;
using System.Linq;

namespace LoG
{
	// Token: 0x0200033D RID: 829
	[Serializable]
	public class CombatEffect_DestroyOpponent : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FDF RID: 4063 RVA: 0x0003EBE0 File Offset: 0x0003CDE0
		protected override GameEvent OnDeath(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			if (context.Actor.IsAlive())
			{
				return null;
			}
			if (!context.Opponent.IsAlive())
			{
				return null;
			}
			if (context.TurnContext.GetAllAbilitiesFor(context.Opponent).SelectMany((Ability ability) => ability.Effects).Any((AbilityEffect effect) => effect is CombatEffect_SalvageGameItems))
			{
				return null;
			}
			BattleAbilityEvent battleAbilityEvent = new BattleAbilityEvent(this.CurrentAbilityStage, source, context, base.TypeName);
			if (context.Opponent.IsPandaemonium())
			{
				int num = -1;
				bool allowCapture = context.Opponent.ControllingPlayerId != num;
				BattleProcessor.DamageContext damageContext = new BattleProcessor.DamageContext(context.Actor.ControllingPlayerId)
				{
					Damage = context.Opponent.TotalHP,
					AllowFatal = false,
					AttackOutcomeIntent = AttackOutcomeIntent.ReturnToConclave,
					AllowCapture = allowCapture,
					IsPermanent = false
				};
				BattleProcessor.DamageEvent ev = context.TurnContext.DealDamage(context.Opponent, damageContext);
				battleAbilityEvent.AddChildEvent<BattleProcessor.DamageEvent>(ev);
			}
			else
			{
				battleAbilityEvent.AddChildEvent<LegionKilledEvent>(context.TurnContext.KillGamePiece(context.Opponent, context.Actor.ControllingPlayerId));
			}
			return battleAbilityEvent;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0003ED20 File Offset: 0x0003CF20
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_DestroyOpponent combatEffect_DestroyOpponent = new CombatEffect_DestroyOpponent();
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_DestroyOpponent);
			clone = combatEffect_DestroyOpponent;
		}
	}
}
