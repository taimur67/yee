using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020004ED RID: 1261
	public class GrievanceProcessor : DiplomaticDecisionProcessor<GrievanceDecisionRequest, GrievanceDecisionResponse>
	{
		// Token: 0x060017D5 RID: 6101 RVA: 0x000560A1 File Offset: 0x000542A1
		protected override GrievanceDecisionResponse GenerateTypedFallbackResponse()
		{
			return base.GenerateTypedFallbackResponse();
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x000560AC File Offset: 0x000542AC
		public static Result BeginVendettaOrDuel(TurnProcessContext turnContext, int player, int target, GrievanceContext context, out GameEvent gameEvent)
		{
			DiplomaticPairStatus diplomaticStatus = turnContext.CurrentTurn.GetDiplomaticStatus(player, target);
			gameEvent = null;
			VendettaContext vendettaContext = context as VendettaContext;
			Result result;
			if (vendettaContext == null)
			{
				PraetorBattleContext praetorBattleContext = context as PraetorBattleContext;
				if (praetorBattleContext == null)
				{
					result = Result.SimulationError("Unknown Choice Context");
				}
				else
				{
					result = GrievanceProcessor.StartPraetorDuel(turnContext, player, diplomaticStatus, praetorBattleContext, out gameEvent);
				}
			}
			else
			{
				result = GrievanceProcessor.StartVendetta(turnContext, player, diplomaticStatus, vendettaContext, out gameEvent);
			}
			return result;
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0005610C File Offset: 0x0005430C
		public static Result StartVendetta(TurnProcessContext context, int player, DiplomaticPairStatus diplomaticStatus, VendettaContext battleContext, out GameEvent gameEvent)
		{
			gameEvent = null;
			int num;
			if (!diplomaticStatus.PlayerPair.GetOther(player, out num))
			{
				return Result.SimulationError("Incorrect diplomatic status");
			}
			if (diplomaticStatus.DiplomaticState is ExcommunicatedState)
			{
				return new Result.ExcommunicatedProblem(num, OrderTypes.None, num, player);
			}
			TurnState currentTurn = context.CurrentTurn;
			Rank rank = currentTurn.FindPlayerState(num, null).Rank;
			Vendetta vendetta = new Vendetta(currentTurn.GenerateIdentifier(), player, num, rank, battleContext.Objective, battleContext.TurnTotal, battleContext.BasePrestigeWager, battleContext.AdditionalPrestigeReward);
			diplomaticStatus.SetVendetta(context, vendetta);
			VendettaStartedEvent vendettaStartedEvent = new VendettaStartedEvent(player, num, vendetta.Objective, vendetta.PrestigeWager, vendetta.TurnRemaining - 1);
			foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.HasVendettaKnowledge)
				{
					vendettaStartedEvent.VendettaKnowledgeHolders.Set(playerState.Id);
				}
			}
			vendettaStartedEvent.GrievanceResponse = battleContext;
			gameEvent = vendettaStartedEvent;
			return Result.Success;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00056224 File Offset: 0x00054424
		public static Result StartPraetorDuel(TurnProcessContext context, int player, DiplomaticPairStatus diplomaticStatus, PraetorBattleContext praetorContext, out GameEvent gameEvent)
		{
			gameEvent = null;
			diplomaticStatus.SetPraetorDuel(context, praetorContext.DuelData);
			return Result.Success;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0005623D File Offset: 0x0005443D
		public static IEnumerable<VendettaObjective> GenerateVendettaObjective(GameDatabase database, TurnState turnState, PlayerState player, PlayerState target)
		{
			return VendettaObjective.GenerateVendettaObjectiveOptions(database, turnState, player, target);
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00056248 File Offset: 0x00054448
		public static ModifiableValue CalculateVendettaPrestigeReward(int basePrestigeWager, int turnCount, VendettaObjective objective, StatModifier additionalPrestigeReward, Rank targetRank)
		{
			float num = (objective != null) ? objective.ObjectiveDifficulty : 0f;
			float num2 = 2.9f * MathF.Pow((float)turnCount, -0.94f);
			float num3 = 1f + 0.2f * (float)targetRank;
			float num4 = MathF.Pow(num / 3f * num2, 0.6f) * num3;
			float num5 = 5f + MathF.Max(0f, (float)basePrestigeWager * num4 + num - (float)turnCount);
			ModifiableValue modifiableValue = new ModifiableValue((float)basePrestigeWager, 0, int.MaxValue, RoundingMode.RoundDown);
			modifiableValue.AddModifier(new StatModifier((int)Math.Ceiling((double)num5), new VendettaWagerContext("Objective"), ModifierTarget.ValueOffset));
			if (additionalPrestigeReward != null)
			{
				modifiableValue.AddModifier(additionalPrestigeReward);
			}
			return modifiableValue;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x000562F8 File Offset: 0x000544F8
		public static ModifiableValue CalculatePraetorDuelPrestigeReward(int basePrestigeWager, GameDatabase db, Rank targetRank)
		{
			float num = 1f + 0.1f * (float)targetRank;
			float num2 = 3f + (float)basePrestigeWager * num;
			ModifiableValue modifiableValue = new ModifiableValue((float)basePrestigeWager, 0, int.MaxValue, RoundingMode.RoundDown);
			modifiableValue.AddModifier(new StatModifier((int)Math.Ceiling((double)num2), new VendettaWagerContext(), ModifierTarget.ValueOffset));
			return modifiableValue;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00056348 File Offset: 0x00054548
		[return: TupleElementNames(new string[]
		{
			"lowerBound",
			"upperBound"
		})]
		public static ValueTuple<int, int> CalculateVendettaWagerBounds(GameDatabase database, TurnState turn, int actorID, int targetID)
		{
			PlayerState playerState = turn.FindPlayerState(actorID, null);
			int minVendettaWager = database.GetArchfiendRank((int)playerState.Rank).MinVendettaWager;
			int num = database.GetArchfiendRank((int)turn.FindPlayerState(targetID, null).Rank).MaxVendettaWager;
			if (playerState.SpendablePrestige < num)
			{
				num = Math.Max(minVendettaWager, playerState.SpendablePrestige);
			}
			return new ValueTuple<int, int>(minVendettaWager, num);
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000563A8 File Offset: 0x000545A8
		[return: TupleElementNames(new string[]
		{
			"lowerBound",
			"upperBound"
		})]
		public static ValueTuple<int, int> CalculatePraetorDuelWagerBounds(GameDatabase database, TurnState turn, PlayerState challenger, PlayerState defender)
		{
			int minPraetorDuelWager = database.GetArchfiendRank(challenger.Rank).MinPraetorDuelWager;
			int num = database.GetArchfiendRank(defender.Rank).MaxPraetorDuelWager;
			if (challenger.SpendablePrestige < num)
			{
				num = Math.Max(minPraetorDuelWager, challenger.SpendablePrestige);
			}
			return new ValueTuple<int, int>(minPraetorDuelWager, num);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x000563F8 File Offset: 0x000545F8
		protected override Result Enact(GrievanceDecisionResponse response)
		{
			base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.GrievanceTargetPlayerId).SetNeutral(this.TurnProcessContext, false);
			if (response.GrievanceResponse == null && !base.request.MustAccept)
			{
				VendettaNotPursuedEvent gameEvent = new VendettaNotPursuedEvent(this._player.Id, base.request.GrievanceTargetPlayerId);
				base._currentTurn.AddGameEvent<VendettaNotPursuedEvent>(gameEvent);
				return this.ProcessRejectPayment(response);
			}
			Problem problem = this.ProcessPayment(response) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (response.GrievanceResponse != null)
			{
				response.GrievanceResponse.AdditionalPrestigeReward = base.request.AdditionalPrestigeReward;
			}
			GameEvent gameEvent2;
			Result result = GrievanceProcessor.BeginVendettaOrDuel(this.TurnProcessContext, this._player.Id, base.request.GrievanceTargetPlayerId, response.GrievanceResponse, out gameEvent2);
			if (gameEvent2 != null)
			{
				base._currentTurn.AddGameEvent<GameEvent>(gameEvent2);
			}
			Problem problem2 = result as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			if (response.GrievanceResponse == null)
			{
				return new DebugProblem("Insult Vendetta cannot be rejected - the vendetta was initiated.");
			}
			return result;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00056504 File Offset: 0x00054704
		public Result ProcessRejectPayment(GrievanceDecisionResponse response)
		{
			return this._player.AcceptPayment(response.Payment);
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00056517 File Offset: 0x00054717
		public Result ProcessPayment(GrievanceDecisionResponse response)
		{
			PlayerState player = this._player;
			Payment payment = new Payment();
			GrievanceContext grievanceResponse = response.GrievanceResponse;
			return player.AcceptPayment(payment.AddPrestige((grievanceResponse != null) ? grievanceResponse.BasePrestigeWager : 0));
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00056540 File Offset: 0x00054740
		protected override Result Preview(GrievanceDecisionResponse response)
		{
			if (response.GrievanceResponse == null)
			{
				return this.ProcessRejectPayment(response);
			}
			return this.ProcessPayment(response);
		}

		// Token: 0x04000B63 RID: 2915
		public const int BasePrestigeBonusForDuel = 3;

		// Token: 0x04000B64 RID: 2916
		public const int BasePrestigeBonusForVendetta = 5;

		// Token: 0x04000B65 RID: 2917
		public const float PrestigeBonusPerTargetRankForDuel = 0.1f;

		// Token: 0x04000B66 RID: 2918
		public const float PrestigeBonusPerTargetRankForVendetta = 0.2f;
	}
}
