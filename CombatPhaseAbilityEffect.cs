using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200035C RID: 860
	[Serializable]
	public abstract class CombatPhaseAbilityEffect : CombatAbilityEffect
	{
		// Token: 0x0600105A RID: 4186 RVA: 0x000405F0 File Offset: 0x0003E7F0
		public override GameEvent ProcessAbilityState(CombatAbilityStage combatAbilityStage, Ability source, CombatAbilityContext abilityContext, BattleContext battleContext, BattleEvent battleEvent, BattlePhase battlePhase = BattlePhase.Undefined)
		{
			this.CurrentAbilityStage = combatAbilityStage;
			if (!this.IsActiveForContext(abilityContext, battlePhase))
			{
				return null;
			}
			GameEvent result;
			if (combatAbilityStage == CombatAbilityStage.PreCombatPhase)
			{
				result = this.OnPreCombatPhase(source, abilityContext, battleEvent, battleContext, battlePhase);
			}
			else
			{
				result = base.ProcessAbilityState(combatAbilityStage, source, abilityContext, battleContext, battleEvent, battlePhase);
			}
			return result;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00040638 File Offset: 0x0003E838
		public bool IsActiveForContext(CombatAbilityContext context, BattlePhase phase)
		{
			if (this.Phase != BattlePhase.Undefined && this.Phase != phase)
			{
				return false;
			}
			if (this.NullifiedByOpponentAbilities.Count > 0)
			{
				GameDatabase database = context.TurnContext.Database;
				TurnState currentTurn = context.TurnContext.CurrentTurn;
				IEnumerable<Ability> opponentAbilities = context.TurnContext.GetAllAbilitiesFor(context.Opponent);
				if (this.NullifiedByOpponentAbilities.Any((ConfigRef<ItemAbilityStaticData> n) => !n.IsEmpty() && opponentAbilities.Any((Ability a) => a.SourceId == n.Id)))
				{
					return false;
				}
			}
			return base.IsActiveForContext(context);
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x000406C0 File Offset: 0x0003E8C0
		public GameEvent OnCombatPhase(CombatAbilityStage phase, Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			this.CurrentAbilityStage = phase;
			if (!this.IsActiveForContext(context, phaseResult.BattlePhase))
			{
				return null;
			}
			switch (phase)
			{
			case CombatAbilityStage.PhaseLoserDamageModifier:
				return this.OnLoserDamageModifier(source, context, phaseResult);
			case CombatAbilityStage.PhaseWinnerDamageModifier:
				return this.OnWinnerDamageModifier(source, context, phaseResult);
			case CombatAbilityStage.PhaseLoserDamageTaken:
				return this.OnLoserDamageTaken(source, context, phaseResult);
			case CombatAbilityStage.PhaseWinnerDamageDealt:
				return this.OnWinnerDamageDealt(source, context, phaseResult);
			case CombatAbilityStage.PhaseDeath:
				return this.OnDeath(source, context, phaseResult);
			case CombatAbilityStage.PhaseLoserPreCapture:
				return this.OnLoserPreCapture(source, context, phaseResult);
			case CombatAbilityStage.PhaseWinnerPreCapture:
				return this.OnWinnerPreCapture(source, context, phaseResult);
			}
			throw new ArgumentOutOfRangeException("phase", phase, null);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0004077E File Offset: 0x0003E97E
		protected virtual GameEvent OnLoserDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			return null;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00040781 File Offset: 0x0003E981
		protected virtual GameEvent OnWinnerDamageDealt(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			return null;
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00040784 File Offset: 0x0003E984
		protected virtual GameEvent OnWinnerDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			return null;
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x00040787 File Offset: 0x0003E987
		protected virtual GameEvent OnLoserDamageTaken(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			return null;
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0004078A File Offset: 0x0003E98A
		protected virtual GameEvent OnDeath(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			return null;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0004078D File Offset: 0x0003E98D
		protected virtual GameEvent OnLoserPreCapture(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			return null;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00040790 File Offset: 0x0003E990
		protected virtual GameEvent OnWinnerPreCapture(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			return null;
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00040793 File Offset: 0x0003E993
		protected void DeepCloneCombatPhaseAbilityEffectParts(CombatPhaseAbilityEffect combatPhaseAbilityEffect)
		{
			combatPhaseAbilityEffect.Phase = this.Phase;
			base.DeepCloneCombatAbilityEffectParts(combatPhaseAbilityEffect);
		}

		// Token: 0x040007A7 RID: 1959
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		[DefaultValue(BattlePhase.Undefined)]
		public BattlePhase Phase = BattlePhase.Undefined;
	}
}
