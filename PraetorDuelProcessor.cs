using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020003BC RID: 956
	public static class PraetorDuelProcessor
	{
		// Token: 0x060012A9 RID: 4777 RVA: 0x00047034 File Offset: 0x00045234
		public static DuelEvent ProcessDuel(TurnProcessContext turnContext, PraetorDuelData duel)
		{
			DuelProcessContext duelProcessContext = new DuelProcessContext(duel);
			Result result = PraetorDuelProcessor.ValidateContestant(turnContext, duel.Challenger, out duelProcessContext.ChallengerInstance);
			Result result2 = PraetorDuelProcessor.ValidateContestant(turnContext, duel.Defender, out duelProcessContext.DefenderInstance);
			if (!result || !result2)
			{
				return PraetorDuelProcessor.ProcessInvalidContestants(turnContext, duel, result, result2);
			}
			return PraetorDuelProcessor.ProcessDuelCombat(turnContext, duelProcessContext);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00047094 File Offset: 0x00045294
		private static PaymentRemovedEvent ApplyForfeitPenalty(TurnProcessContext turnContext, PlayerState forfeitingPlayer)
		{
			return turnContext.RemovePayment(forfeitingPlayer, new Payment
			{
				Prestige = turnContext.Rules.DuelForfeitPenalty
			}, null);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x000470C4 File Offset: 0x000452C4
		public static DuelEvent ProcessInvalidContestants(TurnProcessContext turnContext, PraetorDuelData duel, Result challengerValid, Result defenderValid)
		{
			TurnState currentTurn = turnContext.CurrentTurn;
			DuelEvent duelEvent = new DuelEvent(duel);
			PaymentRemovedEvent paymentRemovedEvent = null;
			PaymentRemovedEvent paymentRemovedEvent2 = null;
			if (!challengerValid)
			{
				PlayerState playerState = currentTurn.FindPlayerState(duel.Challenger.PlayerId, null);
				if (playerState != null)
				{
					paymentRemovedEvent = PraetorDuelProcessor.ApplyForfeitPenalty(turnContext, playerState);
				}
			}
			if (!defenderValid)
			{
				PlayerState playerState2 = currentTurn.FindPlayerState(duel.Defender.PlayerId, null);
				if (playerState2 != null)
				{
					paymentRemovedEvent2 = PraetorDuelProcessor.ApplyForfeitPenalty(turnContext, playerState2);
				}
			}
			if (!challengerValid && !defenderValid)
			{
				PraetorDuelOutcomeEvent praetorDuelOutcomeEvent = PraetorDuelProcessor.ProcessDefaultDrawOutcome(turnContext, duel);
				duelEvent.AddChildEvent<PraetorDuelOutcomeEvent>(praetorDuelOutcomeEvent);
				if (paymentRemovedEvent != null)
				{
					praetorDuelOutcomeEvent.AddChildEvent<PaymentRemovedEvent>(paymentRemovedEvent);
					praetorDuelOutcomeEvent.LoserPrestigePenalty = paymentRemovedEvent.Payment.Prestige;
				}
				if (paymentRemovedEvent2 != null)
				{
					praetorDuelOutcomeEvent.AddChildEvent<PaymentRemovedEvent>(paymentRemovedEvent2);
					praetorDuelOutcomeEvent.LoserPrestigePenalty = paymentRemovedEvent2.Payment.Prestige;
				}
			}
			else
			{
				PraetorDuelParticipantData winner = challengerValid.successful ? duel.Challenger : duel.Defender;
				PraetorDuelParticipantData loser = challengerValid.successful ? duel.Defender : duel.Challenger;
				PraetorDuelOutcomeEvent praetorDuelOutcomeEvent2 = PraetorDuelProcessor.ProcessDefaultWinnerOutcome(turnContext, duel, winner, loser);
				duelEvent.AddChildEvent<PraetorDuelOutcomeEvent>(praetorDuelOutcomeEvent2);
				praetorDuelOutcomeEvent2.AddChildEvent<PaymentRemovedEvent>(paymentRemovedEvent);
				praetorDuelOutcomeEvent2.AddChildEvent<PaymentRemovedEvent>(paymentRemovedEvent2);
				praetorDuelOutcomeEvent2.LoserPrestigePenalty = (from x in duelEvent.EnumerateAllChildEvents().OfType<PaymentRemovedEvent>()
				where x.AffectedPlayerID == loser.PlayerId
				select x).Sum((PaymentRemovedEvent x) => x.Payment.Prestige);
			}
			return duelEvent;
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0004724D File Offset: 0x0004544D
		public static IEnumerable<GameEvent> ProcessStyleEffects(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source)
		{
			foreach (PraetorCombatMoveEffectData praetorCombatMoveEffectData in duel.GetStyleEffects(context, source))
			{
				PraetorCombatMoveEffectTriggered praetorCombatMoveEffectTriggered = new PraetorCombatMoveEffectTriggered(source.Data, praetorCombatMoveEffectData);
				if (praetorCombatMoveEffectData.PreDamage(praetorCombatMoveEffectTriggered, context, duel, source))
				{
					yield return praetorCombatMoveEffectTriggered;
				}
			}
			IEnumerator<PraetorCombatMoveEffectData> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0004726B File Offset: 0x0004546B
		public static IEnumerable<GameEvent> ProcessPreCombatMoveEffects(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source)
		{
			foreach (PraetorCombatMoveEffectData praetorCombatMoveEffectData in duel.GetActiveEffects(context, source))
			{
				PraetorCombatMoveEffectTriggered praetorCombatMoveEffectTriggered = new PraetorCombatMoveEffectTriggered(source.Data, praetorCombatMoveEffectData);
				if (praetorCombatMoveEffectData.PreDamage(praetorCombatMoveEffectTriggered, context, duel, source))
				{
					yield return praetorCombatMoveEffectTriggered;
				}
			}
			IEnumerator<PraetorCombatMoveEffectData> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00047289 File Offset: 0x00045489
		public static IEnumerable<GameEvent> ProcessCombatMoveDamage(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source)
		{
			EntityTag_InactiveTechniquesDealDamage entityTag_InactiveTechniquesDealDamage;
			bool flag = context.CurrentTurn.TryGetTag(source.Praetor, out entityTag_InactiveTechniquesDealDamage);
			if (!source.TechniqueActive && !flag)
			{
				yield break;
			}
			int damage = source.CombatMoveInstance.Power;
			foreach (PraetorCombatMoveEffectData praetorCombatMoveEffectData in duel.GetActiveEffects(context, source))
			{
				int oldValue = damage;
				if (praetorCombatMoveEffectData.ModifyPower(context, duel, source, ref damage))
				{
					PraetorCombatMoveEffectTriggered praetorCombatMoveEffectTriggered = new PraetorCombatMoveEffectTriggered(source.Data, praetorCombatMoveEffectData);
					praetorCombatMoveEffectTriggered.AddChildEvent<PraetorCombatMoveDamageModified>(new PraetorCombatMoveDamageModified(source.Praetor, oldValue, damage));
					yield return praetorCombatMoveEffectTriggered;
				}
			}
			IEnumerator<PraetorCombatMoveEffectData> enumerator = null;
			yield return duel.ContributeDamage(source, damage, new CombatMoveContext(source.CombatMoveData.Id));
			yield break;
			yield break;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x000472A7 File Offset: 0x000454A7
		public static IEnumerable<GameEvent> ProcessPostCombatMoveEffects(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source)
		{
			foreach (PraetorCombatMoveEffectData praetorCombatMoveEffectData in duel.GetActiveEffects(context, source))
			{
				PraetorCombatMoveEffectTriggered praetorCombatMoveEffectTriggered = new PraetorCombatMoveEffectTriggered(source.Data, praetorCombatMoveEffectData);
				if (praetorCombatMoveEffectData.PostDamage(praetorCombatMoveEffectTriggered, context, duel, source))
				{
					yield return praetorCombatMoveEffectTriggered;
				}
			}
			IEnumerator<PraetorCombatMoveEffectData> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000472C8 File Offset: 0x000454C8
		public static PraetorDuelPhaseEvent ProcessCombatMoves(TurnProcessContext context, DuelProcessContext duel)
		{
			PraetorDuelPhaseEvent praetorDuelPhaseEvent = new PraetorDuelPhaseEvent(PraetorDuelPhase.CombatMoves);
			praetorDuelPhaseEvent.OnStart(duel.DamageTally);
			bool moveCountered = duel.DefenderInstance.MoveCountered;
			praetorDuelPhaseEvent.AddChildEvent(PraetorDuelProcessor.ProcessStyleEffects(context, duel, duel.ChallengerInstance));
			bool moveCountered2 = duel.DefenderInstance.MoveCountered;
			duel.DefenderInstance.MoveCountered = moveCountered;
			praetorDuelPhaseEvent.AddChildEvent(PraetorDuelProcessor.ProcessStyleEffects(context, duel, duel.DefenderInstance));
			duel.DefenderInstance.MoveCountered = moveCountered2;
			praetorDuelPhaseEvent.AddChildEvent(PraetorDuelProcessor.ProcessPreCombatMoveEffects(context, duel, duel.ChallengerInstance));
			bool moveCountered3 = duel.DefenderInstance.MoveCountered;
			duel.DefenderInstance.MoveCountered = moveCountered2;
			praetorDuelPhaseEvent.AddChildEvent(PraetorDuelProcessor.ProcessPreCombatMoveEffects(context, duel, duel.DefenderInstance));
			duel.DefenderInstance.MoveCountered = moveCountered3;
			praetorDuelPhaseEvent.AddChildEvent(PraetorDuelProcessor.ProcessCombatMoveDamage(context, duel, duel.ChallengerInstance));
			praetorDuelPhaseEvent.AddChildEvent(PraetorDuelProcessor.ProcessCombatMoveDamage(context, duel, duel.DefenderInstance));
			praetorDuelPhaseEvent.AddChildEvent(PraetorDuelProcessor.ProcessPostCombatMoveEffects(context, duel, duel.ChallengerInstance));
			praetorDuelPhaseEvent.AddChildEvent(PraetorDuelProcessor.ProcessPostCombatMoveEffects(context, duel, duel.DefenderInstance));
			praetorDuelPhaseEvent.OnEnd(duel.DamageTally);
			return praetorDuelPhaseEvent;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000473E0 File Offset: 0x000455E0
		private static DuelEvent ProcessDuelCombat(TurnProcessContext context, DuelProcessContext duel)
		{
			PraetorDuelData duelData = duel.DuelData;
			TurnState currentTurn = context.CurrentTurn;
			Praetor praetor = duel.ChallengerInstance.Praetor;
			Praetor praetor2 = duel.DefenderInstance.Praetor;
			DuelEvent duelEvent = new DuelEvent(duelData);
			PraetorDuelPhaseEvent praetorDuelPhaseEvent = duelEvent.AddChildEvent<PraetorDuelPhaseEvent>(new PraetorDuelPhaseEvent(PraetorDuelPhase.LevelCheck));
			praetorDuelPhaseEvent.OnStart(duel.DamageTally);
			praetorDuelPhaseEvent.AddChildEvent<PraetorDuelDamageEvent>(duel.ContributeDamage(duel.ChallengerInstance, praetor.Level, new PraetorStrengthContext(praetor)));
			praetorDuelPhaseEvent.AddChildEvent<PraetorDuelDamageEvent>(duel.ContributeDamage(duel.DefenderInstance, praetor2.Level, new PraetorStrengthContext(praetor2)));
			praetorDuelPhaseEvent.OnEnd(duel.DamageTally);
			duelEvent.AddChildEvent<PraetorDuelPhaseEvent>(PraetorDuelProcessor.ProcessCombatMoves(context, duel));
			PraetorDuelPhaseEvent praetorDuelPhaseEvent2 = duelEvent.AddChildEvent<PraetorDuelPhaseEvent>(new PraetorDuelPhaseEvent(PraetorDuelPhase.SpecialAbilities));
			praetorDuelPhaseEvent2.OnStart(duel.DamageTally);
			IEnumerable<EntityTag_BonusDamageInDuels> enumerable = currentTurn.EnumerateTags(praetor);
			IEnumerable<EntityTag_BonusDamageInDuels> enumerable2 = currentTurn.EnumerateTags(praetor2);
			foreach (EntityTag_BonusDamageInDuels entityTag_BonusDamageInDuels in enumerable)
			{
				praetorDuelPhaseEvent2.AddChildEvent<PraetorDuelDamageEvent>(duel.ContributeDamage(duel.ChallengerInstance, entityTag_BonusDamageInDuels.BonusAmount, entityTag_BonusDamageInDuels.Source));
			}
			foreach (EntityTag_BonusDamageInDuels entityTag_BonusDamageInDuels2 in enumerable2)
			{
				praetorDuelPhaseEvent2.AddChildEvent<PraetorDuelDamageEvent>(duel.ContributeDamage(duel.DefenderInstance, entityTag_BonusDamageInDuels2.BonusAmount, entityTag_BonusDamageInDuels2.Source));
			}
			praetorDuelPhaseEvent2.OnEnd(duel.DamageTally);
			PraetorDuelPhaseEvent praetorDuelPhaseEvent3 = new PraetorDuelPhaseEvent(PraetorDuelPhase.ArbiterInterference);
			praetorDuelPhaseEvent3.OnStart(duel.DamageTally);
			GameEvent ev = praetorDuelPhaseEvent3.AddChildEvent(PraetorDuelProcessor.ProcessArbiterIntervention(context, duel));
			praetorDuelPhaseEvent3.OnEnd(duel.DamageTally);
			DuelParticipantInstance winner;
			PraetorDuelOutcomeEvent praetorDuelOutcomeEvent;
			if (duel.DetermineWinner<DuelParticipantInstance>(duel.ChallengerInstance, duel.DefenderInstance, out winner))
			{
				praetorDuelOutcomeEvent = PraetorDuelProcessor.ProcessCleanWinnerOutcome(context, duel, winner);
				duelEvent.AddChildEvent<PraetorDuelOutcomeEvent>(praetorDuelOutcomeEvent);
			}
			else
			{
				praetorDuelOutcomeEvent = PraetorDuelProcessor.ProcessCombatDrawOutcome(context, duel);
				duelEvent.AddChildEvent<PraetorDuelOutcomeEvent>(praetorDuelOutcomeEvent);
			}
			praetorDuelOutcomeEvent.AddChildEvent(ev);
			praetorDuelOutcomeEvent.Challenger.MoveCountered = duel.ChallengerInstance.MoveCountered;
			praetorDuelOutcomeEvent.Defender.MoveCountered = duel.DefenderInstance.MoveCountered;
			return duelEvent;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00047624 File Offset: 0x00045824
		public static GameEvent TryKillPraetor(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance participant)
		{
			PraetorCombatMoveEffectData_Unkillable effect;
			if (duel.TryFindActiveEffect(context, participant, out effect))
			{
				return new PraetorCombatMoveEffectTriggered(participant.Data, effect);
			}
			DuelParticipantInstance duelParticipantInstance;
			duel.TryGetOther(participant, out duelParticipantInstance);
			ItemBanishedEvent itemBanishedEvent = context.BanishGameItem(participant.Praetor.Id, duelParticipantInstance.Player.Id);
			PraetorCombatMoveEffectData_KillOpponentOnDeath effect2;
			if (duel.TryFindActiveEffect(context, participant, out effect2) && duelParticipantInstance.Praetor.IsActive)
			{
				itemBanishedEvent.AddChildEvent<PraetorCombatMoveEffectTriggered>(new PraetorCombatMoveEffectTriggered(participant.Data, effect2)).AddChildEvent(PraetorDuelProcessor.TryKillPraetor(context, duel, duelParticipantInstance));
			}
			return itemBanishedEvent;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x000476AC File Offset: 0x000458AC
		private static PraetorDuelOutcomeEvent ProcessCombatDrawOutcome(TurnProcessContext context, DuelProcessContext duel)
		{
			PraetorDuelOutcomeEvent praetorDuelOutcomeEvent = new PraetorDuelOutcomeEvent(duel.DuelData);
			praetorDuelOutcomeEvent.Result = DuelResultStatus.Draw;
			praetorDuelOutcomeEvent.FinalTally = duel.DamageTally;
			praetorDuelOutcomeEvent.AddChildEvent(PraetorDuelProcessor.TryKillPraetor(context, duel, duel.ChallengerInstance));
			praetorDuelOutcomeEvent.AddChildEvent(PraetorDuelProcessor.TryKillPraetor(context, duel, duel.DefenderInstance));
			return praetorDuelOutcomeEvent;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00047700 File Offset: 0x00045900
		private static PraetorDuelOutcomeEvent ProcessCleanWinnerOutcome(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance winner)
		{
			PraetorDuelOutcomeEvent praetorDuelOutcomeEvent = new PraetorDuelOutcomeEvent(duel.DuelData)
			{
				FinalTally = duel.DamageTally,
				Result = DuelResultStatus.CombatWinner,
				Winner = winner.Data
			};
			DuelParticipantInstance duelParticipantInstance;
			if (duel.TryGetOther(winner, out duelParticipantInstance))
			{
				praetorDuelOutcomeEvent.Loser = duelParticipantInstance.Data;
				PraetorCombatMoveEffectData_CaptureOpponent effect;
				if (duel.TryFindActiveEffect(context, winner, out effect))
				{
					PraetorCombatMoveEffectTriggered praetorCombatMoveEffectTriggered = praetorDuelOutcomeEvent.AddChildEvent<PraetorCombatMoveEffectTriggered>(new PraetorCombatMoveEffectTriggered(winner.Data, effect));
					ValueTuple<Result, GameEvent> valueTuple = context.StealItem(winner.Player, duelParticipantInstance.Player, duelParticipantInstance.Praetor);
					Result item = valueTuple.Item1;
					GameEvent item2 = valueTuple.Item2;
					if (item)
					{
						praetorCombatMoveEffectTriggered.AddChildEvent(item2);
					}
				}
				else
				{
					praetorDuelOutcomeEvent.AddChildEvent(PraetorDuelProcessor.TryKillPraetor(context, duel, duelParticipantInstance));
				}
			}
			PaymentReceivedEvent paymentReceivedEvent = PraetorDuelProcessor.ProcessPrestigeWinnings(context, duel.DuelData, winner.Data, duelParticipantInstance.Data);
			praetorDuelOutcomeEvent.AddChildEvent<PaymentReceivedEvent>(paymentReceivedEvent);
			praetorDuelOutcomeEvent.AddChildEvent(PraetorDuelProcessor.IncrementPraetorVictories(context, winner));
			praetorDuelOutcomeEvent.WinnerVictories = winner.Praetor.Victories;
			praetorDuelOutcomeEvent.WinnerPrestigeGain = paymentReceivedEvent.Offering.Prestige;
			return praetorDuelOutcomeEvent;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00047814 File Offset: 0x00045A14
		private static PaymentReceivedEvent ProcessPrestigeWinnings(TurnProcessContext context, PraetorDuelData duel, PraetorDuelParticipantData winner, PraetorDuelParticipantData loser)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(winner.PlayerId, null);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(loser.PlayerId, null);
			if (playerState.Id == duel.Challenger.PlayerId)
			{
				int prestige = (int)Math.Floor((double)((float)(duel.PrestigeReward ?? 0) * playerState.DuelRewardMultiplier.RawValue));
				PaymentReceivedEvent paymentReceivedEvent = context.GivePrestige(playerState, prestige);
				playerState2.RemovePrestige(duel.BaseWager);
				paymentReceivedEvent.PublicPayment = true;
				return paymentReceivedEvent;
			}
			int prestige2 = (int)Math.Floor((double)((float)duel.BaseWager * playerState.DuelRewardMultiplier.RawValue));
			PaymentReceivedEvent paymentReceivedEvent2 = context.GivePrestige(playerState, prestige2);
			paymentReceivedEvent2.AddAffectedPlayerId(playerState2.Id);
			paymentReceivedEvent2.PublicPayment = true;
			return paymentReceivedEvent2;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x000478D8 File Offset: 0x00045AD8
		private static GameEvent IncrementPraetorVictories(TurnProcessContext context, DuelParticipantInstance winner)
		{
			winner.Praetor.Victories++;
			return context.ApplyDuelExperience(winner.Player.Id, winner.Praetor, 1);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00047908 File Offset: 0x00045B08
		private static PraetorDuelOutcomeEvent ProcessDefaultWinnerOutcome(TurnProcessContext context, PraetorDuelData duel, PraetorDuelParticipantData winner, PraetorDuelParticipantData loser)
		{
			PraetorDuelOutcomeEvent praetorDuelOutcomeEvent = new PraetorDuelOutcomeEvent(duel);
			praetorDuelOutcomeEvent.Result = DuelResultStatus.DefaultWinner;
			praetorDuelOutcomeEvent.Winner = winner;
			praetorDuelOutcomeEvent.Loser = loser;
			PaymentReceivedEvent paymentReceivedEvent = PraetorDuelProcessor.ProcessPrestigeWinnings(context, duel, winner, loser);
			praetorDuelOutcomeEvent.AddChildEvent<PaymentReceivedEvent>(paymentReceivedEvent);
			praetorDuelOutcomeEvent.WinnerPrestigeGain = paymentReceivedEvent.Offering.Prestige;
			return praetorDuelOutcomeEvent;
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00047953 File Offset: 0x00045B53
		private static PraetorDuelOutcomeEvent ProcessDefaultDrawOutcome(TurnProcessContext context, PraetorDuelData duel)
		{
			return new PraetorDuelOutcomeEvent(duel)
			{
				Result = DuelResultStatus.Cancelled
			};
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00047964 File Offset: 0x00045B64
		public static IEnumerable<PraetorCombatMoveEffectData> GetActiveEffects(this DuelProcessContext duel, TurnProcessContext context, DuelParticipantInstance participant)
		{
			EntityTag_InactiveTechniquesApplyEffects entityTag_InactiveTechniquesApplyEffects;
			bool flag = context.CurrentTurn.TryGetTag(participant.Praetor, out entityTag_InactiveTechniquesApplyEffects);
			if (!participant.TechniqueActive && !flag)
			{
				return Enumerable.Empty<PraetorCombatMoveEffectData>();
			}
			return from t in participant.CombatMoveData.Effects
			where t.CheckRestrictions(duel, participant)
			select t;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000479D8 File Offset: 0x00045BD8
		public static IEnumerable<PraetorCombatMoveEffectData> GetStyleEffects(this DuelProcessContext duel, TurnProcessContext context, DuelParticipantInstance participant)
		{
			if (participant.CombatMoveData == null)
			{
				return Enumerable.Empty<PraetorCombatMoveEffectData>();
			}
			return from t in context.Database.Fetch(participant.CombatMoveData.TechniqueType).Effects
			where t.CheckRestrictions(duel, participant)
			select t;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00047A3D File Offset: 0x00045C3D
		public static bool TryFindActiveEffect<T>(this DuelProcessContext duel, TurnProcessContext context, DuelParticipantInstance instance, out T effect) where T : PraetorCombatMoveEffectData
		{
			effect = IEnumerableExtensions.FirstOrDefault<T>(duel.GetActiveEffects(context, instance).OfType<T>());
			return effect != null;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00047A68 File Offset: 0x00045C68
		public static GameEvent ProcessArbiterIntervention(TurnProcessContext context, DuelProcessContext duel)
		{
			PraetorCombatMoveEffectData_ArbiterCancellation effect;
			if (duel.TryFindActiveEffect(context, duel.ChallengerInstance, out effect))
			{
				return new PraetorCombatMoveEffectTriggered(duel.ChallengerInstance.Data, effect);
			}
			if (duel.TryFindActiveEffect(context, duel.DefenderInstance, out effect))
			{
				return new PraetorCombatMoveEffectTriggered(duel.DefenderInstance.Data, effect);
			}
			PlayerState playerState = context.CurrentTurn.FindPlayerState(duel.DuelData.Challenger.PlayerId, null);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(duel.DuelData.Defender.PlayerId, null);
			Payment bribe = duel.DuelData.Challenger.Bribe;
			bool flag;
			if (bribe == null)
			{
				flag = false;
			}
			else
			{
				int valueSum = bribe.Total.ValueSum;
				flag = true;
			}
			int num = (flag && playerState.CanBribeArbiter) ? duel.DuelData.Challenger.Bribe.Total.ValueSum : 0;
			Payment bribe2 = duel.DuelData.Defender.Bribe;
			bool flag2;
			if (bribe2 == null)
			{
				flag2 = false;
			}
			else
			{
				int valueSum2 = bribe2.Total.ValueSum;
				flag2 = true;
			}
			int num2 = (flag2 && playerState2.CanBribeArbiter) ? duel.DuelData.Defender.Bribe.Total.ValueSum : 0;
			if (num == 0 && num2 == 0)
			{
				return null;
			}
			int num3 = num.CompareTo(num2);
			GameEvent result;
			if (num3 != -1)
			{
				if (num3 != 1)
				{
					result = PraetorDuelProcessor.ProcessArbiterDraw();
				}
				else
				{
					result = PraetorDuelProcessor.ProcessArbiterIntervention(context, duel, duel.ChallengerInstance, num);
				}
			}
			else
			{
				result = PraetorDuelProcessor.ProcessArbiterIntervention(context, duel, duel.DefenderInstance, num2);
			}
			return result;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00047BE4 File Offset: 0x00045DE4
		private static ArbiterDrawEvent ProcessArbiterDraw()
		{
			return new ArbiterDrawEvent();
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00047BEC File Offset: 0x00045DEC
		public static ArbiterInterventionEvent ProcessArbiterIntervention(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance winningParticipant, int winnerBribeAmount)
		{
			GameEvent ev = null;
			DuelParticipantInstance duelParticipantInstance;
			PraetorCombatMoveEffectData_ArbiterConversion effect;
			if (duel.TryGetOther(winningParticipant, out duelParticipantInstance) && duel.TryFindActiveEffect(context, duelParticipantInstance, out effect))
			{
				DuelParticipantInstance duelParticipantInstance2 = duelParticipantInstance;
				DuelParticipantInstance duelParticipantInstance3 = winningParticipant;
				winningParticipant = duelParticipantInstance2;
				duelParticipantInstance = duelParticipantInstance3;
				ev = new PraetorCombatMoveEffectTriggered(duelParticipantInstance.Data, effect);
			}
			ArbiterInterventionEvent arbiterInterventionEvent = new ArbiterInterventionEvent(winningParticipant.Player.Id, duelParticipantInstance.Player.Id, winnerBribeAmount);
			arbiterInterventionEvent.AddChildEvent(ev);
			ModifiableValue val = winningParticipant.Player.Get(ArchfiendStat.Charisma);
			arbiterInterventionEvent.AddChildEvent<PraetorDuelDamageEvent>(duel.ContributeDamage(winningParticipant, val, new DuelArbiterContext()));
			return arbiterInterventionEvent;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00047C74 File Offset: 0x00045E74
		public static Result ValidateContestant(TurnContext context, PraetorDuelParticipantData participant, out DuelParticipantInstance instance)
		{
			instance = new DuelParticipantInstance(participant);
			Praetor praetor;
			if (!context.CurrentTurn.TryFetchGameItem<Praetor>(participant.Praetor, out praetor))
			{
				return Result.Failure;
			}
			instance.Praetor = praetor;
			PlayerState playerState;
			if (!context.CurrentTurn.TryFindControllingPlayer(instance.Praetor, out playerState))
			{
				return Result.Failure;
			}
			if (playerState.Id != participant.PlayerId)
			{
				return Result.Failure;
			}
			instance.Player = playerState;
			if (praetor.Status == GameItemStatus.Banished)
			{
				return Result.Failure;
			}
			if (!praetor.IsActive)
			{
				return Result.Failure;
			}
			context.TryGetPraetorCombatMove(praetor, participant.GetCombatMove(context.CurrentTurn), out instance.CombatMoveInstance, out instance.CombatMoveData);
			return Result.Success;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00047D28 File Offset: 0x00045F28
		public static PraetorDuelDamageEvent ContributeDamage(this DuelProcessContext context, DuelParticipantInstance source, int damage, ModifierContext damageContext)
		{
			DuelParticipantInstance duelParticipantInstance;
			if (!context.TryGetOther(source, out duelParticipantInstance))
			{
				return null;
			}
			source.DamageGiven.AddModifier(new StatModifier(damage, damageContext, ModifierTarget.ValueOffset));
			return new PraetorDuelDamageEvent(source.Praetor.Id, duelParticipantInstance.Praetor.Id, damage);
		}
	}
}
