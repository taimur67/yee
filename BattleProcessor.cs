using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001C7 RID: 455
	public static class BattleProcessor
	{
		// Token: 0x06000885 RID: 2181 RVA: 0x00028364 File Offset: 0x00026564
		public static BattleContext GenerateContext(TurnContext context, GamePiece attacker, GamePiece defender, AttackOutcomeIntent attackOutcomeIntent)
		{
			return BattleProcessor.GenerateContext(context, attacker, defender, defender.Location, attackOutcomeIntent);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00028378 File Offset: 0x00026578
		public static BattleContext GenerateContext(TurnContext context, GamePiece attacker, GamePiece defender, HexCoord location, AttackOutcomeIntent attackOutcomeIntent)
		{
			return new BattleContext
			{
				Attacker = attacker,
				Defender = defender,
				Location = location,
				AttackerSupport = IEnumerableExtensions.ToList<GamePiece>(context.CurrentTurn.GetSupportingGamePieces(attacker, location)),
				DefenderSupport = IEnumerableExtensions.ToList<GamePiece>(context.CurrentTurn.GetSupportingGamePieces(defender, location)),
				AttackOutcomeIntent = attackOutcomeIntent
			};
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000283D7 File Offset: 0x000265D7
		public static BattleEvent SimulateBattle(TurnProcessContext context, GamePiece attacker, GamePiece defender, HexCoord location)
		{
			return BattleProcessor.SimulateBattle(context, BattleProcessor.GenerateContext(context, attacker, defender, location, AttackOutcomeIntent.Default));
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000283E9 File Offset: 0x000265E9
		public static BattleEvent SimulateBattle(TurnProcessContext context, GamePiece attacker, GamePiece defender)
		{
			return BattleProcessor.SimulateBattle(context, attacker, defender, defender.Location);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x000283FC File Offset: 0x000265FC
		public static int GetBattlePrestigeReward(TurnState turn, GamePiece winner, GamePiece victim)
		{
			PlayerState playerState = turn.FindPlayerState(winner.ControllingPlayerId, null);
			return (int)Math.Ceiling((double)((float)victim.Level * playerState.CombatRewardMultiplier.RawValue * (float)winner.CombatRewardMultiplier));
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00028440 File Offset: 0x00026640
		public static List<BattlePhase> GetOrderedPhases(IReadOnlyList<BattlePhase> startingPhasesOrder, List<BattlePhaseModification> phaseModifications, [AllowNull] BattleEvent battleEvent)
		{
			List<BattlePhase> list = IEnumerableExtensions.ToList<BattlePhase>(startingPhasesOrder);
			phaseModifications.Sort(new Comparison<BattlePhaseModification>(BattlePhaseModification.Compare));
			bool flag = false;
			bool flag2 = false;
			List<BattlePhase> list2 = new List<BattlePhase>();
			List<BattlePhase> list3 = new List<BattlePhase>();
			for (int i = 0; i < phaseModifications.Count; i++)
			{
				BattlePhaseModification battlePhaseModification = phaseModifications[i];
				BattlePhase battlePhase = battlePhaseModification.BattlePhase;
				if (battlePhaseModification.BattlePhaseModificationType == BattlePhaseModificationType.Skip)
				{
					list.Remove(battlePhase);
					if (battleEvent != null)
					{
						battleEvent.AddChildEvent<BattlePhaseModificationEvent>(new BattlePhaseModificationEvent(battlePhaseModification));
					}
				}
				else if (battlePhaseModification.BattlePhaseModificationType == BattlePhaseModificationType.Twice)
				{
					if (!list3.Contains(battlePhase))
					{
						int num = list.IndexOf(battlePhaseModification.BattlePhase);
						if (num >= 0)
						{
							list3.Add(battlePhaseModification.BattlePhase);
							list.Insert(num + 1, battlePhaseModification.BattlePhase);
							if (battleEvent != null)
							{
								battleEvent.AddChildEvent<BattlePhaseModificationEvent>(new BattlePhaseModificationEvent(battlePhaseModification));
							}
						}
					}
				}
				else if (battlePhaseModification.BattlePhaseModificationType == BattlePhaseModificationType.First)
				{
					if (!flag && !list2.Contains(battlePhaseModification.BattlePhase))
					{
						int num2 = list.IndexOf(battlePhaseModification.BattlePhase);
						if (num2 >= 0)
						{
							flag = true;
							list2.Add(battlePhaseModification.BattlePhase);
							int num3 = 0;
							while (num2 < list.Count && list[num2] == battlePhase)
							{
								num3++;
								list.RemoveAt(num2);
							}
							for (int j = 0; j < num3; j++)
							{
								list.Insert(0, battlePhase);
							}
							if (battleEvent != null)
							{
								battleEvent.AddChildEvent<BattlePhaseModificationEvent>(new BattlePhaseModificationEvent(battlePhaseModification));
							}
						}
					}
				}
				else if (battlePhaseModification.BattlePhaseModificationType == BattlePhaseModificationType.Last && !flag2 && !list2.Contains(battlePhaseModification.BattlePhase))
				{
					int num4 = list.IndexOf(battlePhaseModification.BattlePhase);
					if (num4 >= 0)
					{
						flag2 = true;
						list2.Add(battlePhaseModification.BattlePhase);
						int num5 = 0;
						while (num4 < list.Count && list[num4] == battlePhase)
						{
							num5++;
							list.RemoveAt(num4);
						}
						for (int k = 0; k < num5; k++)
						{
							list.Add(battlePhase);
						}
						if (battleEvent != null)
						{
							battleEvent.AddChildEvent<BattlePhaseModificationEvent>(new BattlePhaseModificationEvent(battlePhaseModification));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00028678 File Offset: 0x00026878
		public static BattleEvent SimulateBattle(TurnProcessContext context, BattleContext battle)
		{
			BattleRulesStaticData battleRulesStaticData = context.Database.FetchSingle<BattleRulesStaticData>();
			TurnState currentTurn = context.CurrentTurn;
			GamePiece attacker = battle.Attacker;
			GamePiece defender = battle.Defender;
			HexCoord location = battle.Location;
			context.RecalculateModifiers(attacker);
			context.RecalculateModifiers(defender);
			List<Identifier> attackerSupportingPieces;
			context.RecalculateSupportModifiers(attacker, defender.Location, out attackerSupportingPieces);
			List<Identifier> defenderSupportingPieces;
			context.RecalculateSupportModifiers(defender, defender.Location, out defenderSupportingPieces);
			BattleResult battleResult = new BattleResult(defender, attacker, location);
			battleResult.AttackerSupportingPieces = attackerSupportingPieces;
			battleResult.DefenderSupportingPieces = defenderSupportingPieces;
			BattleEvent battleEvent = new BattleEvent(battleResult);
			if (defender == null || attacker == null)
			{
				return battleEvent;
			}
			CombatAbilityContext combatAbilityContext = new CombatAbilityContext
			{
				TurnContext = context,
				Actor = attacker,
				Opponent = defender,
				BattleRole = BattleRole.Attacker
			};
			CombatAbilityContext combatAbilityContext2 = new CombatAbilityContext
			{
				TurnContext = context,
				Actor = defender,
				Opponent = attacker,
				BattleRole = BattleRole.Defender
			};
			if (defender.IsPandaemonium() && !defender.IsOwned() && battle.AttackOutcomeIntent == AttackOutcomeIntent.ReturnToConclave)
			{
				return battleEvent;
			}
			if (defender.IsPandaemonium() && attacker.IsOwned() && battle.AttackOutcomeIntent != AttackOutcomeIntent.ReturnToConclave)
			{
				PlayerState excommunicatedPlayer = currentTurn.FindPlayerState(attacker.ControllingPlayerId, null);
				currentTurn.CurrentDiplomaticTurn.SetPlayerAsExcommunicated(context, excommunicatedPlayer, ExcommunicationReason.AttackedPandaemonium, -1);
				List<Identifier> attackerSupportingPieces2;
				context.RecalculateSupportModifiers(attacker, defender.Location, out attackerSupportingPieces2);
				battleResult.AttackerSupportingPieces = attackerSupportingPieces2;
			}
			List<ValueTuple<Ability, CombatAbilityEffect>> list = IEnumerableExtensions.ToList<ValueTuple<Ability, CombatAbilityEffect>>(context.GetAbilityEffects(attacker));
			List<ValueTuple<Ability, CombatAbilityEffect>> list2 = IEnumerableExtensions.ToList<ValueTuple<Ability, CombatAbilityEffect>>(context.GetAbilityEffects(defender));
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple in list)
			{
				Ability item = valueTuple.Item1;
				CombatAbilityEffect item2 = valueTuple.Item2;
				battleEvent.AddChildEvent(item2.ProcessAbilityState(CombatAbilityStage.BlockAbilities, item, combatAbilityContext, battle, battleEvent, BattlePhase.Undefined));
			}
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple2 in list2)
			{
				Ability item3 = valueTuple2.Item1;
				CombatAbilityEffect item4 = valueTuple2.Item2;
				battleEvent.AddChildEvent(item4.ProcessAbilityState(CombatAbilityStage.BlockAbilities, item3, combatAbilityContext2, battle, battleEvent, BattlePhase.Undefined));
			}
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple3 in context.GetAbilityEffects(attacker))
			{
				Ability item5 = valueTuple3.Item1;
				CombatAbilityEffect item6 = valueTuple3.Item2;
				battleEvent.AddChildEvent(item6.ProcessAbilityState(CombatAbilityStage.PreBattle, item5, combatAbilityContext, battle, battleEvent, BattlePhase.Undefined));
			}
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple4 in context.GetAbilityEffects(defender))
			{
				Ability item7 = valueTuple4.Item1;
				CombatAbilityEffect item8 = valueTuple4.Item2;
				battleEvent.AddChildEvent(item8.ProcessAbilityState(CombatAbilityStage.PreBattle, item7, combatAbilityContext2, battle, battleEvent, BattlePhase.Undefined));
			}
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple5 in context.GetAbilityEffects(attacker))
			{
				Ability item9 = valueTuple5.Item1;
				CombatAbilityEffect item10 = valueTuple5.Item2;
				battleEvent.AddChildEvent(item10.ProcessAbilityState(CombatAbilityStage.BlockStratagems, item9, combatAbilityContext, battle, battleEvent, BattlePhase.Undefined));
			}
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple6 in context.GetAbilityEffects(defender))
			{
				Ability item11 = valueTuple6.Item1;
				CombatAbilityEffect item12 = valueTuple6.Item2;
				battleEvent.AddChildEvent(item12.ProcessAbilityState(CombatAbilityStage.BlockStratagems, item11, combatAbilityContext2, battle, battleEvent, BattlePhase.Undefined));
			}
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple7 in context.GetAbilityEffects(attacker))
			{
				Ability item13 = valueTuple7.Item1;
				CombatAbilityEffect item14 = valueTuple7.Item2;
				battleEvent.AddChildEvent(item14.ProcessAbilityState(CombatAbilityStage.Stratagems, item13, combatAbilityContext, battle, battleEvent, BattlePhase.Undefined));
			}
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple8 in context.GetAbilityEffects(defender))
			{
				Ability item15 = valueTuple8.Item1;
				CombatAbilityEffect item16 = valueTuple8.Item2;
				battleEvent.AddChildEvent(item16.ProcessAbilityState(CombatAbilityStage.Stratagems, item15, combatAbilityContext2, battle, battleEvent, BattlePhase.Undefined));
			}
			List<BattlePhase> orderedPhases = BattleProcessor.GetOrderedPhases(battleRulesStaticData.DefaultPhaseOrder, battle.PhaseModifications, battleEvent);
			for (int i = 0; i < orderedPhases.Count; i++)
			{
				BattlePhase battlePhase = orderedPhases[i];
				foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple9 in context.GetAbilityEffects(attacker))
				{
					Ability item17 = valueTuple9.Item1;
					CombatPhaseAbilityEffect item18 = valueTuple9.Item2;
					battleEvent.AddChildEvent(item18.ProcessAbilityState(CombatAbilityStage.PreCombatPhase, item17, combatAbilityContext, battle, battleEvent, battlePhase));
				}
				foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple10 in context.GetAbilityEffects(defender))
				{
					Ability item19 = valueTuple10.Item1;
					CombatPhaseAbilityEffect item20 = valueTuple10.Item2;
					battleEvent.AddChildEvent(item20.ProcessAbilityState(CombatAbilityStage.PreCombatPhase, item19, combatAbilityContext2, battle, battleEvent, battlePhase));
				}
				BattlePhaseEvent battlePhaseEvent = BattleProcessor.ProcessBattlePhase(context, battle, combatAbilityContext, combatAbilityContext2, battleEvent, battlePhase);
				battleEvent.AddChildEvent<BattlePhaseEvent>(battlePhaseEvent);
				if (battlePhaseEvent.PhaseResult.Fatal)
				{
					if (!attacker.IsAlive() && !defender.IsAlive())
					{
						battleResult.Outcome = BattleOutcome.Both_Destroyed;
					}
					else if (attacker.IsAlive())
					{
						battleResult.Outcome = BattleOutcome.Victory_Attacker;
					}
					else if (defender.IsAlive())
					{
						battleResult.Outcome = BattleOutcome.Victory_Defender;
					}
					for (int j = i + 1; j < orderedPhases.Count; j++)
					{
						BattlePhaseEvent ev = new BattlePhaseEvent(new BattlePhaseResult(orderedPhases[j], 0, Identifier.Invalid, Identifier.Invalid)
						{
							Unreached = true
						}, battle.Attacker.ControllingPlayerId, battle.Defender.ControllingPlayerId);
						battleEvent.AddChildEvent<BattlePhaseEvent>(ev);
					}
					break;
				}
			}
			battleResult.Attacker_EndState = attacker.DeepClone<GamePiece>();
			battleResult.Defender_EndState = defender.DeepClone<GamePiece>();
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple11 in context.GetAbilityEffects(attacker))
			{
				Ability item21 = valueTuple11.Item1;
				CombatAbilityEffect item22 = valueTuple11.Item2;
				battleEvent.AddChildEvent(item22.ProcessAbilityState(CombatAbilityStage.PostBattle, item21, combatAbilityContext, battle, battleEvent, BattlePhase.Undefined));
			}
			foreach (ValueTuple<Ability, CombatAbilityEffect> valueTuple12 in context.GetAbilityEffects(defender))
			{
				Ability item23 = valueTuple12.Item1;
				CombatAbilityEffect item24 = valueTuple12.Item2;
				battleEvent.AddChildEvent(item24.ProcessAbilityState(CombatAbilityStage.PostBattle, item23, combatAbilityContext2, battle, battleEvent, BattlePhase.Undefined));
			}
			if (battleResult.Outcome == BattleOutcome.Undecided)
			{
				battleResult.Outcome = BattleOutcome.Stalemate;
			}
			GamePiece gamePiece;
			if (battleResult.Outcome != BattleOutcome.Stalemate && battleResult.TryGetWinningPiece_EndState(out gamePiece) && gamePiece.IsAlive())
			{
				GamePiece gamePiece2 = currentTurn.FetchGameItem<GamePiece>(gamePiece);
				PlayerState toPlayer = currentTurn.FindPlayerState(gamePiece.ControllingPlayerId, null);
				gamePiece2.LevelExperience.AdjustBase((float)gamePiece2.CombatExperienceReward);
				GamePiece victim;
				if (battleResult.TryGetLosingPiece_EndState(out victim) && gamePiece.ControllingPlayerId != -1)
				{
					int battlePrestigeReward = BattleProcessor.GetBattlePrestigeReward(currentTurn, gamePiece, victim);
					PaymentReceivedEvent paymentReceivedEvent = context.GivePrestige(toPlayer, battlePrestigeReward);
					paymentReceivedEvent.PublicPayment = true;
					battleEvent.AddChildEvent<PaymentReceivedEvent>(paymentReceivedEvent);
				}
			}
			battleEvent.AddChildEvent(context.DestroyStratagems(attacker));
			battleEvent.AddChildEvent(context.DestroyStratagems(defender));
			context.RecalculateSupportModifiers(attacker, attacker.Location);
			context.RecalculateSupportModifiers(defender, defender.Location);
			attacker.TemporaryHP = 0;
			defender.TemporaryHP = 0;
			return battleEvent;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00028E6C File Offset: 0x0002706C
		private static BattlePhaseEvent ProcessBattlePhase(TurnProcessContext context, BattleContext battleContext, CombatAbilityContext attackerContext, CombatAbilityContext defenderContext, BattleEvent battleEvent, BattlePhase phase)
		{
			BattlePhaseResult battlePhaseResult = new BattlePhaseResult
			{
				BattlePhase = phase,
				AttackerPower = BattleProcessor.GetCombatValue(battleContext.Attacker, phase),
				DefenderPower = BattleProcessor.GetCombatValue(battleContext.Defender, phase)
			};
			BattlePhaseEvent battlePhaseEvent = new BattlePhaseEvent(battlePhaseResult, battleContext.Attacker.ControllingPlayerId, battleContext.Defender.ControllingPlayerId);
			if (battlePhaseResult.StatDifference != 0)
			{
				battlePhaseResult.HPDamage = battlePhaseResult.StatDifference;
				ValueTuple<GamePiece, GamePiece> valueTuple = battleContext.DeduceStrength(battlePhaseResult.AttackerPower, battlePhaseResult.DefenderPower);
				GamePiece item = valueTuple.Item1;
				GamePiece item2 = valueTuple.Item2;
				battlePhaseResult.WinningLegionId = item;
				battlePhaseResult.LosingLegionId = item2;
				CombatAbilityContext phaseVictor = (battlePhaseResult.WinningLegionId == attackerContext.Actor.Id) ? attackerContext : defenderContext;
				CombatAbilityContext phaseLoser = (battlePhaseResult.LosingLegionId == attackerContext.Actor.Id) ? attackerContext : defenderContext;
				BattleProcessor.ProcessBattlePhaseResult(context, battlePhaseResult, phaseVictor, phaseLoser, battlePhaseEvent, battleContext.AttackOutcomeIntent);
			}
			return battlePhaseEvent;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00028F78 File Offset: 0x00027178
		private static void ProcessBattlePhaseResult(TurnProcessContext context, BattlePhaseResult battlePhaseResult, CombatAbilityContext phaseVictor, CombatAbilityContext phaseLoser, BattlePhaseEvent battlePhaseEvent, AttackOutcomeIntent attackOutcomeIntent)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameDatabase database = context.Database;
			List<ValueTuple<Ability, CombatPhaseAbilityEffect>> list = IEnumerableExtensions.ToList<ValueTuple<Ability, CombatPhaseAbilityEffect>>(context.GetAbilityEffects(phaseVictor.Actor));
			List<ValueTuple<Ability, CombatPhaseAbilityEffect>> list2 = IEnumerableExtensions.ToList<ValueTuple<Ability, CombatPhaseAbilityEffect>>(context.GetAbilityEffects(phaseLoser.Actor));
			foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple in list)
			{
				Ability item = valueTuple.Item1;
				CombatPhaseAbilityEffect item2 = valueTuple.Item2;
				battlePhaseEvent.AddChildEvent(item2.OnCombatPhase(CombatAbilityStage.PhaseWinnerDamageModifier, item, phaseVictor, battlePhaseResult));
			}
			foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple2 in list2)
			{
				Ability item3 = valueTuple2.Item1;
				CombatPhaseAbilityEffect item4 = valueTuple2.Item2;
				battlePhaseEvent.AddChildEvent(item4.OnCombatPhase(CombatAbilityStage.PhaseLoserDamageModifier, item3, phaseLoser, battlePhaseResult));
			}
			BattleProcessor.DamageEvent damageEvent = null;
			BattleProcessor.DamageEvent damageEvent2 = null;
			if (battlePhaseResult.HPDamage > 0)
			{
				damageEvent = context.DealDamage(phaseLoser.Actor, phaseVictor.Actor.ControllingPlayerId, battlePhaseResult.HPDamage.Value, battlePhaseResult.BattlePhase.GetDamageType(), battlePhaseResult.PermanentDamage, attackOutcomeIntent);
				battlePhaseEvent.AddChildEvent<BattleProcessor.DamageEvent>(damageEvent);
				foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple3 in list2)
				{
					Ability item5 = valueTuple3.Item1;
					CombatPhaseAbilityEffect item6 = valueTuple3.Item2;
					battlePhaseEvent.AddChildEvent(item6.OnCombatPhase(CombatAbilityStage.PhaseLoserDamageTaken, item5, phaseLoser, battlePhaseResult));
				}
				if (battlePhaseResult.CounterDamage > 0)
				{
					damageEvent2 = context.DealDamage(phaseVictor.Actor, phaseLoser.Actor.ControllingPlayerId, battlePhaseResult.CounterDamage.Value, battlePhaseResult.BattlePhase.GetDamageType(), false, attackOutcomeIntent);
					battlePhaseEvent.AddChildEvent<BattleProcessor.DamageEvent>(damageEvent2);
				}
				if (!phaseLoser.Actor.IsAlive())
				{
					foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple4 in list2)
					{
						Ability item7 = valueTuple4.Item1;
						CombatPhaseAbilityEffect item8 = valueTuple4.Item2;
						battlePhaseEvent.AddChildEvent(item8.OnCombatPhase(CombatAbilityStage.PhaseDeath, item7, phaseLoser, battlePhaseResult));
					}
				}
				if (phaseVictor.Actor.IsAlive())
				{
					using (List<ValueTuple<Ability, CombatPhaseAbilityEffect>>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple5 = enumerator.Current;
							Ability item9 = valueTuple5.Item1;
							CombatPhaseAbilityEffect item10 = valueTuple5.Item2;
							battlePhaseEvent.AddChildEvent(item10.OnCombatPhase(CombatAbilityStage.PhaseWinnerDamageDealt, item9, phaseVictor, battlePhaseResult));
						}
						goto IL_2B8;
					}
				}
				foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple6 in list)
				{
					Ability item11 = valueTuple6.Item1;
					CombatPhaseAbilityEffect item12 = valueTuple6.Item2;
					battlePhaseEvent.AddChildEvent(item12.OnCombatPhase(CombatAbilityStage.PhaseDeath, item11, phaseVictor, battlePhaseResult));
				}
				IL_2B8:
				if (phaseVictor.Actor.IsAlive() && phaseLoser.Actor.IsAlive() && (damageEvent.ReducedTargetToOrBelowZeroHP || (damageEvent2 != null && damageEvent2.ReducedTargetToOrBelowZeroHP)))
				{
					foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple7 in list)
					{
						Ability item13 = valueTuple7.Item1;
						CombatPhaseAbilityEffect item14 = valueTuple7.Item2;
						battlePhaseEvent.AddChildEvent(item14.OnCombatPhase(CombatAbilityStage.PhaseWinnerPreCapture, item13, phaseVictor, battlePhaseResult));
					}
					list2 = IEnumerableExtensions.ToList<ValueTuple<Ability, CombatPhaseAbilityEffect>>(context.GetAbilityEffects(phaseLoser.Actor));
					foreach (ValueTuple<Ability, CombatPhaseAbilityEffect> valueTuple8 in list2)
					{
						Ability item15 = valueTuple8.Item1;
						CombatPhaseAbilityEffect item16 = valueTuple8.Item2;
						battlePhaseEvent.AddChildEvent(item16.OnCombatPhase(CombatAbilityStage.PhaseLoserPreCapture, item15, phaseLoser, battlePhaseResult));
					}
					context.TryCaptureFromDamage(damageEvent2 ?? damageEvent);
				}
			}
			bool flag = damageEvent != null && damageEvent.ReducedTargetToOrBelowZeroHP;
			bool flag2 = damageEvent2 != null && damageEvent2.ReducedTargetToOrBelowZeroHP;
			battlePhaseResult.Fatal = (flag || flag2 || !phaseVictor.Actor.IsAlive() || !phaseLoser.Actor.IsAlive());
			if (flag)
			{
				phaseLoser.Actor.LastDefeatedBy = phaseVictor.Actor;
			}
			if (flag2)
			{
				phaseVictor.Actor.LastDefeatedBy = phaseLoser.Actor;
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00029410 File Offset: 0x00027610
		public static int GetCombatValue(GamePiece piece, BattlePhase phase)
		{
			int result;
			switch (phase)
			{
			case BattlePhase.Ranged:
				result = piece.TotalRanged();
				break;
			case BattlePhase.Melee:
				result = piece.TotalMelee();
				break;
			case BattlePhase.Infernal:
				result = piece.TotalInfernal();
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002944F File Offset: 0x0002764F
		public static BattleProcessor.DamageEvent DealDamage(this TurnProcessContext turnContext, GamePiece target, int playerSource, int damage, DamageType damageType = DamageType.True, bool isPermanent = false, AttackOutcomeIntent attackOutcomeIntent = AttackOutcomeIntent.Default)
		{
			return turnContext.DealDamage(target, new BattleProcessor.DamageContext(playerSource)
			{
				Damage = damage,
				DamageType = damageType,
				IsPermanent = isPermanent,
				AttackOutcomeIntent = attackOutcomeIntent
			});
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00029484 File Offset: 0x00027684
		public static int GetDamageAfterResistance(this TurnProcessContext turnContext, GamePiece target, BattleProcessor.DamageContext damageContext)
		{
			int num = damageContext.Damage;
			DamageType damageType = damageContext.DamageType;
			ModifiableValue modifiableValue;
			if (damageType != DamageType.True && target.CombatStats.Resistances.TryGetValue(damageType, out modifiableValue))
			{
				num = Math.Max(0, num - modifiableValue.Value);
			}
			return num;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x000294D0 File Offset: 0x000276D0
		public static bool WillDamageDefeatTarget(this TurnProcessContext turnContext, GamePiece target, BattleProcessor.DamageContext damageContext)
		{
			int damageAfterResistance = turnContext.GetDamageAfterResistance(target, damageContext);
			return target.HP + target.TemporaryHP - damageAfterResistance <= 0;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x000294FC File Offset: 0x000276FC
		public static BattleProcessor.DamageEvent DealDamage(this TurnProcessContext turnContext, GamePiece target, BattleProcessor.DamageContext damageContext)
		{
			int num = -1;
			if (damageContext.PlayerSource != -2147483648)
			{
				num = damageContext.PlayerSource;
			}
			BattleProcessor.DamageEvent damageEvent = new BattleProcessor.DamageEvent(num)
			{
				Target = target,
				Context = damageContext
			};
			damageEvent.AddAffectedPlayerId(target.ControllingPlayerId);
			int num2 = turnContext.GetDamageAfterResistance(target, damageContext);
			int effectiveHP = target.GetEffectiveHP();
			target.TemporaryHP -= num2;
			int num3 = -Math.Min(0, target.TemporaryHP);
			target.TemporaryHP = Math.Max(0, target.TemporaryHP);
			target.HP -= num3;
			damageEvent.ReducedTargetToOrBelowZeroHP = (target.HP <= 0);
			bool flag = damageEvent.ReducedTargetToOrBelowZeroHP && damageContext.AllowFatal && !target.IsCapturable();
			if (damageContext.IsPermanent && !flag)
			{
				ModifyGamePieceEvent ev = new ModifyGamePieceEvent(num, target, GamePieceStat.MaxHealth, -damageContext.Damage, -num2, false);
				damageEvent.AddChildEvent<ModifyGamePieceEvent>(ev);
				int num4 = Math.Min(num2, target.TotalHP.BaseValue - target.TotalHP.LowerBound);
				if (num4 > 0)
				{
					target.TotalHP.SetBase((float)(target.TotalHP.BaseValue - num4));
				}
			}
			if (damageEvent.ReducedTargetToOrBelowZeroHP && target.IsCapturable())
			{
				target.HP = 1;
				num2 = effectiveHP - target.HP - target.TemporaryHP;
			}
			damageEvent.DamageDealt = num2;
			if (target.GetEffectiveHP() <= 0)
			{
				if (damageContext.AllowFatal)
				{
					damageEvent.KilledTarget = true;
					damageEvent.AddChildEvent<LegionKilledEvent>(turnContext.KillGamePiece(target, num));
				}
				else
				{
					target.HP = 1;
				}
			}
			return damageEvent;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00029690 File Offset: 0x00027890
		public static bool TryCaptureFromDamage(this TurnProcessContext turnContext, BattleProcessor.DamageEvent damageEvent)
		{
			if (!damageEvent.ReducedTargetToOrBelowZeroHP)
			{
				return false;
			}
			GamePiece gamePiece;
			if (!turnContext.CurrentTurn.TryFetchGameItem<GamePiece>(damageEvent.Target, out gamePiece))
			{
				return false;
			}
			if (!gamePiece.IsCurrentlyCapturable() || !damageEvent.Context.AllowCapture)
			{
				return false;
			}
			if (!gamePiece.IsPandaemonium())
			{
				GameItemOwnershipChanged ev = turnContext.CaptureGamePiece(gamePiece, damageEvent.TriggeringPlayerID, true, true);
				damageEvent.AddChildEvent<GameItemOwnershipChanged>(ev);
				return true;
			}
			if (damageEvent.Context.AttackOutcomeIntent != AttackOutcomeIntent.ReturnToConclave)
			{
				turnContext.CurrentTurn.PandaemoniumCapturedCount++;
				GameItemOwnershipChanged ev2 = turnContext.CaptureGamePiece(gamePiece, damageEvent.TriggeringPlayerID, true, true);
				damageEvent.AddChildEvent<GameItemOwnershipChanged>(ev2);
				return true;
			}
			PlayerState playerState = turnContext.CurrentTurn.PlayerStates[damageEvent.TriggeringPlayerID];
			if (playerState.Excommunicated)
			{
				GameEvent ev3 = turnContext.CurrentTurn.CurrentDiplomaticTurn.ReinstateExcommunicatedPlayer(turnContext, playerState);
				playerState.GivePrestige(turnContext.Rules.ExcommedPrestigeForReturningPanda);
				turnContext.CurrentTurn.AddGameEvent<PandaemoniumReturnedToConclaveEvent>(new PandaemoniumReturnedToConclaveEvent(damageEvent.TriggeringPlayerID, turnContext.Rules.ExcommedPrestigeForReturningPanda)).AddChildEvent(ev3);
			}
			else
			{
				playerState.GivePrestige(turnContext.Rules.PrestigeForReturningPanda);
				turnContext.CurrentTurn.AddGameEvent<PandaemoniumReturnedToConclaveEvent>(new PandaemoniumReturnedToConclaveEvent(damageEvent.TriggeringPlayerID, turnContext.Rules.PrestigeForReturningPanda));
			}
			GameItemOwnershipChanged ev4 = turnContext.CaptureGamePiece(gamePiece, -1, true, true);
			damageEvent.AddChildEvent<GameItemOwnershipChanged>(ev4);
			return true;
		}

		// Token: 0x02000886 RID: 2182
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class DamageContext
		{
			// Token: 0x06002859 RID: 10329 RVA: 0x00085F47 File Offset: 0x00084147
			[JsonConstructor]
			protected DamageContext()
			{
			}

			// Token: 0x0600285A RID: 10330 RVA: 0x00085F7A File Offset: 0x0008417A
			public DamageContext(int playerSource)
			{
				this.PlayerSource = playerSource;
			}

			// Token: 0x0400123E RID: 4670
			[JsonProperty]
			[DefaultValue(-2147483648)]
			public int PlayerSource = int.MinValue;

			// Token: 0x0400123F RID: 4671
			[JsonProperty]
			public ModifierContext Source;

			// Token: 0x04001240 RID: 4672
			[JsonProperty]
			public DamageType DamageType = DamageType.True;

			// Token: 0x04001241 RID: 4673
			[JsonProperty]
			[DefaultValue(0)]
			public ModifiableValue Damage = new ModifiableValue();

			// Token: 0x04001242 RID: 4674
			[JsonProperty]
			public bool AllowFatal = true;

			// Token: 0x04001243 RID: 4675
			[JsonProperty]
			public bool AllowCapture = true;

			// Token: 0x04001244 RID: 4676
			[JsonProperty]
			public bool IsPermanent;

			// Token: 0x04001245 RID: 4677
			[JsonProperty]
			public AttackOutcomeIntent AttackOutcomeIntent;
		}

		// Token: 0x02000887 RID: 2183
		[JsonObject(MemberSerialization.OptIn)]
		[BindableGameEvent]
		[Serializable]
		public class DamageEvent : GameEvent
		{
			// Token: 0x170005B7 RID: 1463
			// (get) Token: 0x0600285B RID: 10331 RVA: 0x00085FB4 File Offset: 0x000841B4
			[JsonIgnore]
			protected override GameEventVisibility GameEventVisibility
			{
				get
				{
					return GameEventVisibility.Public;
				}
			}

			// Token: 0x0600285C RID: 10332 RVA: 0x00085FB7 File Offset: 0x000841B7
			[JsonConstructor]
			private DamageEvent()
			{
			}

			// Token: 0x0600285D RID: 10333 RVA: 0x00085FC6 File Offset: 0x000841C6
			public DamageEvent(int triggeringPlayerID) : base(triggeringPlayerID)
			{
			}

			// Token: 0x170005B8 RID: 1464
			// (get) Token: 0x0600285E RID: 10334 RVA: 0x00085FD6 File Offset: 0x000841D6
			[BindableValue("min_value", BindingOption.None)]
			[JsonIgnore]
			public int MinHitpoints
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x170005B9 RID: 1465
			// (get) Token: 0x0600285F RID: 10335 RVA: 0x00085FD9 File Offset: 0x000841D9
			[JsonIgnore]
			public int DamageResisted
			{
				get
				{
					return this.Context.Damage - this.DamageDealt;
				}
			}

			// Token: 0x170005BA RID: 1466
			// (get) Token: 0x06002860 RID: 10336 RVA: 0x00085FF2 File Offset: 0x000841F2
			[JsonIgnore]
			public bool CapturedTarget
			{
				get
				{
					return base.Contains<GameItemOwnershipChanged>((GameItemOwnershipChanged t) => t.Item == this.Target);
				}
			}

			// Token: 0x06002861 RID: 10337 RVA: 0x00086008 File Offset: 0x00084208
			public override string GetDebugName(TurnContext context)
			{
				return string.Format("{0} took {1} damage from {2}", context.CurrentTurn.FetchGameItem(this.Target).NameKey, this.Context.Damage, context.CurrentTurn.FindPlayerState(this.TriggeringPlayerID, null).ArchfiendId);
			}

			// Token: 0x06002862 RID: 10338 RVA: 0x00086058 File Offset: 0x00084258
			public override void DeepClone(out GameEvent clone)
			{
				BattleProcessor.DamageEvent damageEvent = new BattleProcessor.DamageEvent
				{
					Context = this.Context.DeepClone(CloneFunction.FastClone),
					Target = this.Target,
					DamageDealt = this.DamageDealt,
					ReducedTargetToOrBelowZeroHP = this.ReducedTargetToOrBelowZeroHP,
					KilledTarget = this.KilledTarget
				};
				base.DeepCloneGameEventParts<BattleProcessor.DamageEvent>(damageEvent);
				clone = damageEvent;
			}

			// Token: 0x04001246 RID: 4678
			[JsonProperty]
			public BattleProcessor.DamageContext Context;

			// Token: 0x04001247 RID: 4679
			[BindableValue(null, BindingOption.None)]
			[JsonProperty]
			public Identifier Target = Identifier.Invalid;

			// Token: 0x04001248 RID: 4680
			[BindableValue(null, BindingOption.None)]
			[JsonProperty]
			public int DamageDealt;

			// Token: 0x04001249 RID: 4681
			[JsonProperty]
			public bool ReducedTargetToOrBelowZeroHP;

			// Token: 0x0400124A RID: 4682
			[JsonProperty]
			public bool KilledTarget;
		}
	}
}
