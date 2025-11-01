using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000348 RID: 840
	public class CombatEffect_Stun : CombatPhaseAbilityEffect
	{
		// Token: 0x06001003 RID: 4099 RVA: 0x0003F3D8 File Offset: 0x0003D5D8
		protected override GameEvent OnWinnerDamageDealt(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			BattleAbilityEvent battleAbilityEvent = new BattleAbilityEvent(this.CurrentAbilityStage, source, context, base.TypeName);
			GamePieceModifier gamePieceModifier = new GamePieceModifier(new GamePieceModifierStaticData
			{
				Modifications = 
				{
					new StatModificationBinding<GamePieceStat>
					{
						ModifierTarget = ModifierTarget.ValueScalar,
						StatKey = GamePieceStat.Ranged,
						Value = 0f
					}
				},
				Modifications = 
				{
					new StatModificationBinding<GamePieceStat>
					{
						ModifierTarget = ModifierTarget.ValueScalar,
						StatKey = GamePieceStat.Melee,
						Value = 0f
					}
				},
				Modifications = 
				{
					new StatModificationBinding<GamePieceStat>
					{
						ModifierTarget = ModifierTarget.ValueScalar,
						StatKey = GamePieceStat.Infernal,
						Value = 0f
					}
				}
			});
			battleAbilityEvent.AddAppliedModifier(context.Opponent, gamePieceModifier);
			gamePieceModifier.Source = source;
			gamePieceModifier.ApplyTo(context.TurnContext, context.Opponent);
			return battleAbilityEvent;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0003F4B0 File Offset: 0x0003D6B0
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_Stun combatEffect_Stun = new CombatEffect_Stun();
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_Stun);
			clone = combatEffect_Stun;
		}
	}
}
