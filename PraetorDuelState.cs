using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000522 RID: 1314
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuelState : DiplomaticState
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001977 RID: 6519 RVA: 0x00059BAD File Offset: 0x00057DAD
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.PraetorDuel;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001978 RID: 6520 RVA: 0x00059BB0 File Offset: 0x00057DB0
		// (set) Token: 0x06001979 RID: 6521 RVA: 0x00059BB8 File Offset: 0x00057DB8
		[JsonIgnore]
		public PraetorDuelData DuelData
		{
			get
			{
				return this._duelData;
			}
			set
			{
				this._duelData = value;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600197A RID: 6522 RVA: 0x00059BC1 File Offset: 0x00057DC1
		// (set) Token: 0x0600197B RID: 6523 RVA: 0x00059BC9 File Offset: 0x00057DC9
		[JsonProperty]
		[DefaultValue(PraetorDuelState.PraetorDuelFlowStage.Declared)]
		public PraetorDuelState.PraetorDuelFlowStage Stage { get; protected set; }

		// Token: 0x0600197C RID: 6524 RVA: 0x00059BD2 File Offset: 0x00057DD2
		[JsonConstructor]
		public PraetorDuelState()
		{
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00059BDA File Offset: 0x00057DDA
		public PraetorDuelState(PraetorDuelData duel)
		{
			this._duelData = duel;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x00059BE9 File Offset: 0x00057DE9
		public override bool AllowDuelling(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return true;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00059BEC File Offset: 0x00057DEC
		public bool ProgressDuel(TurnProcessContext context)
		{
			return this.ProgressDuel(context, this._duelData);
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x00059BFC File Offset: 0x00057DFC
		public bool ProgressDuel(TurnProcessContext context, PraetorDuelData duel)
		{
			if (!PraetorDuelTransactions.DuelIsStillValid(context, duel))
			{
				return false;
			}
			PraetorDuelState.PraetorDuelFlowStage nextStage = this.GetNextStage(context, this.Stage);
			return this.SetStage(context, nextStage);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00059C2A File Offset: 0x00057E2A
		public void QueueStage(PraetorDuelState.PraetorDuelFlowStage stage)
		{
			this.SetStageInternal(stage - 1);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00059C35 File Offset: 0x00057E35
		public PraetorDuelState.PraetorDuelFlowStage GetNextStage(TurnProcessContext context, PraetorDuelState.PraetorDuelFlowStage stage)
		{
			if (stage <= PraetorDuelState.PraetorDuelFlowStage.Complete)
			{
				return stage + 1;
			}
			return PraetorDuelState.PraetorDuelFlowStage.Complete;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00059C40 File Offset: 0x00057E40
		public bool SetStage(TurnProcessContext context, PraetorDuelState.PraetorDuelFlowStage stage)
		{
			if (this.Stage == stage)
			{
				return true;
			}
			switch (stage)
			{
			case PraetorDuelState.PraetorDuelFlowStage.DefenderSelection:
				context.CurrentTurn.AddDecisionToAskPlayer(this._duelData.Defender.PlayerId, new PraetorDuel_SelectChampion_DecisionRequest(context.CurrentTurn, this._duelData.Players));
				context.CurrentTurn.AddGameEvent<PraetorDuelOpponentSelectionEvent>(new PraetorDuelOpponentSelectionEvent(this._duelData.Challenger.PlayerId, this._duelData.Defender.PlayerId));
				break;
			case PraetorDuelState.PraetorDuelFlowStage.MoveSelection:
			{
				DuelParticipantInstance duelParticipantInstance;
				Result result = PraetorDuelProcessor.ValidateContestant(context, this._duelData.Challenger, out duelParticipantInstance);
				DuelParticipantInstance duelParticipantInstance2;
				Result result2 = PraetorDuelProcessor.ValidateContestant(context, this._duelData.Defender, out duelParticipantInstance2);
				if (result && result2)
				{
					duelParticipantInstance.Player.AddDecisionRequest(new PraetorDuel_SelectCombatMove_DecisionRequest(context.CurrentTurn)
					{
						Contestants = this._duelData.Players,
						Praetor = this._duelData.Challenger.Praetor,
						PrestigeWager = this.DuelData.BaseWager
					});
					duelParticipantInstance2.Player.AddDecisionRequest(new PraetorDuel_SelectCombatMove_DecisionRequest(context.CurrentTurn)
					{
						Contestants = this._duelData.Players,
						Praetor = this._duelData.Defender.Praetor,
						PrestigeWager = this.DuelData.BaseWager
					});
				}
				else
				{
					DuelEvent gameEvent = PraetorDuelProcessor.ProcessInvalidContestants(context, this._duelData, result, result2);
					context.CurrentTurn.AddGameEvent<DuelEvent>(gameEvent);
					stage = PraetorDuelState.PraetorDuelFlowStage.Invalid;
				}
				break;
			}
			case PraetorDuelState.PraetorDuelFlowStage.Battle:
			{
				DuelEvent gameEvent2 = PraetorDuelProcessor.ProcessDuel(context, this._duelData);
				context.CurrentTurn.AddGameEvent<DuelEvent>(gameEvent2);
				break;
			}
			}
			this.SetStageInternal(stage);
			return this.Stage != PraetorDuelState.PraetorDuelFlowStage.Invalid;
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00059E16 File Offset: 0x00058016
		private void SetStageInternal(PraetorDuelState.PraetorDuelFlowStage stage)
		{
			stage = (PraetorDuelState.PraetorDuelFlowStage)Math.Clamp((int)stage, -1, 4);
			this.Stage = stage;
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00059E2C File Offset: 0x0005802C
		private bool TryAskToSelectCombatMoveDecision(TurnContext context, PlayerPair pair, PraetorDuelParticipantData participant)
		{
			DuelParticipantInstance duelParticipantInstance;
			if (!PraetorDuelProcessor.ValidateContestant(context, participant, out duelParticipantInstance))
			{
				return false;
			}
			duelParticipantInstance.Player.AddDecisionRequest(new PraetorDuel_SelectCombatMove_DecisionRequest(context.CurrentTurn)
			{
				Contestants = pair,
				Praetor = participant.Praetor,
				PrestigeWager = this.DuelData.BaseWager
			});
			return true;
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00059E8B File Offset: 0x0005808B
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new PraetorDuelState
			{
				Stage = this.Stage,
				_duelData = this._duelData.DeepClone<PraetorDuelData>()
			};
		}

		// Token: 0x04000BA9 RID: 2985
		[JsonProperty]
		private PraetorDuelData _duelData;

		// Token: 0x020009F8 RID: 2552
		public enum PraetorDuelFlowStage
		{
			// Token: 0x0400182D RID: 6189
			Invalid = -1,
			// Token: 0x0400182E RID: 6190
			Declared,
			// Token: 0x0400182F RID: 6191
			DefenderSelection,
			// Token: 0x04001830 RID: 6192
			MoveSelection,
			// Token: 0x04001831 RID: 6193
			Battle,
			// Token: 0x04001832 RID: 6194
			Complete
		}
	}
}
