using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000331 RID: 817
	public class CombatEffect_ApplyAbilityToParticipants : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FB7 RID: 4023 RVA: 0x0003E38C File Offset: 0x0003C58C
		protected override GameEvent OnPreCombatPhase(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext, BattlePhase phase)
		{
			ItemAbilityStaticData abilityData;
			if (!context.TurnContext.Database.TryFetch<ItemAbilityStaticData>(this.AbilityToApply.Id, out abilityData))
			{
				return null;
			}
			BattleAbilityEvent battleAbilityEvent = new BattleAbilityEvent(this.CurrentAbilityStage, source, context, base.TypeName);
			Func<Ability, bool> <>9__0;
			foreach (GamePiece gamePiece in this.GetParticipants(battleContext))
			{
				IEnumerable<Ability> allAbilitiesFor = context.TurnContext.GetAllAbilitiesFor(gamePiece);
				Func<Ability, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((Ability participantAbility) => participantAbility.SourceId == abilityData.Id));
				}
				if (!allAbilitiesFor.Any(predicate))
				{
					Ability ability = new Ability(abilityData)
					{
						Name = abilityData.Id,
						ProviderId = gamePiece.Id,
						SourceId = abilityData.Id
					};
					GameItemTargetGroup gameItemTargetGroup = new GameItemTargetGroup();
					gameItemTargetGroup.Abilities.Add(ability);
					gameItemTargetGroup.Targets.Add(gamePiece);
					IEnumerable<GamePieceModifierStaticData> source2 = abilityData.GetModifiers().OfType<GamePieceModifierStaticData>();
					gameItemTargetGroup.Modifiers = IEnumerableExtensions.ToList<IModifier>(from staticData in source2
					select new GamePieceModifier(staticData)
					{
						Source = ability
					});
					ability.ModifierGroupId = context.TurnContext.CurrentTurn.PushGlobalModifier(gameItemTargetGroup);
					context.TurnContext.RecalculateModifiers(gamePiece);
					battleAbilityEvent.AddChildEvent<AbilitySetOnGameItemEvent>(new AbilitySetOnGameItemEvent(context.Actor.ControllingPlayerId, gamePiece, this.AbilityToApply, true));
				}
			}
			return battleAbilityEvent;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0003E54C File Offset: 0x0003C74C
		private IEnumerable<GamePiece> GetParticipants(BattleContext battleContext)
		{
			if (!battleContext.Attacker.IsFixture())
			{
				yield return battleContext.Attacker;
			}
			foreach (GamePiece gamePiece in battleContext.AttackerSupport)
			{
				if (!gamePiece.IsFixture())
				{
					yield return gamePiece;
				}
			}
			List<GamePiece>.Enumerator enumerator = default(List<GamePiece>.Enumerator);
			if (!battleContext.Defender.IsFixture())
			{
				yield return battleContext.Defender;
			}
			foreach (GamePiece gamePiece2 in battleContext.DefenderSupport)
			{
				if (!gamePiece2.IsFixture())
				{
					yield return gamePiece2;
				}
			}
			enumerator = default(List<GamePiece>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0003E55C File Offset: 0x0003C75C
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_ApplyAbilityToParticipants combatEffect_ApplyAbilityToParticipants = new CombatEffect_ApplyAbilityToParticipants
			{
				AbilityToApply = this.AbilityToApply
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_ApplyAbilityToParticipants);
			clone = combatEffect_ApplyAbilityToParticipants;
		}

		// Token: 0x04000756 RID: 1878
		public ConfigRef<AbilityStaticData> AbilityToApply;
	}
}
