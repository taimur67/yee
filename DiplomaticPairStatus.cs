using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000504 RID: 1284
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DiplomaticPairStatus : IDeepClone<DiplomaticPairStatus>
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x000575F9 File Offset: 0x000557F9
		[JsonIgnore]
		public List<Vendetta> PreviousVendettas
		{
			get
			{
				return this._previousVendettas;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600185B RID: 6235 RVA: 0x00057601 File Offset: 0x00055801
		[JsonIgnore]
		public PlayerPair PlayerPair
		{
			get
			{
				return this._playerPair;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x00057609 File Offset: 0x00055809
		[JsonIgnore]
		public DiplomaticState DiplomaticState
		{
			get
			{
				return this._diplomaticState ?? new NeutralState();
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x0005761A File Offset: 0x0005581A
		// (set) Token: 0x0600185E RID: 6238 RVA: 0x00057622 File Offset: 0x00055822
		[JsonIgnore]
		public int ActorId
		{
			get
			{
				return this._actorId;
			}
			set
			{
				this._actorId = value;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x0600185F RID: 6239 RVA: 0x0005762B File Offset: 0x0005582B
		[JsonIgnore]
		public int TargetID
		{
			get
			{
				if (this.ActorId == this._playerPair.First)
				{
					return this._playerPair.Second;
				}
				return this._playerPair.First;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001860 RID: 6240 RVA: 0x00057658 File Offset: 0x00055858
		[JsonIgnore]
		public int ArmisticeTurnCount
		{
			get
			{
				ArmisticeState armisticeState = this._diplomaticState as ArmisticeState;
				if (armisticeState == null)
				{
					return 0;
				}
				return armisticeState.ArmisticeCount;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x0005767C File Offset: 0x0005587C
		[JsonIgnore]
		public int VendettaTurnCount
		{
			get
			{
				VendettaState vendettaState = this._diplomaticState as VendettaState;
				if (vendettaState == null)
				{
					return 0;
				}
				return vendettaState.Vendetta.TurnRemaining;
			}
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x000576A5 File Offset: 0x000558A5
		[JsonConstructor]
		public DiplomaticPairStatus()
		{
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x000576E4 File Offset: 0x000558E4
		public DiplomaticPairStatus(PlayerPair playerPair)
		{
			this._playerPair = playerPair;
			this._actorId = playerPair.First;
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00057741 File Offset: 0x00055941
		public DiplomaticPairStatus(PlayerPair playerPair, DiplomaticState diplomaticState) : this()
		{
			this._playerPair = playerPair;
			this._diplomaticState = diplomaticState;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00057757 File Offset: 0x00055957
		public void SetDiplomacyPending(TurnProcessContext context, PendingDiplomacyState state)
		{
			this.ChangeState<PendingDiplomacyState>(context, state, false);
			this._actorId = ((state != null) ? state.ActorId : int.MinValue);
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00057779 File Offset: 0x00055979
		public bool IsDiplomacyActionOnCooldown(OrderTypes orderType)
		{
			return this.GetCooldownCount(orderType) > 0;
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00057785 File Offset: 0x00055985
		public bool IsDiplomacyPending()
		{
			return this._diplomaticState is PendingDiplomacyState;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00057795 File Offset: 0x00055995
		public bool IsDiplomacyStatusOfPlayer(int playerId)
		{
			return this._playerPair.AssociatedWith(playerId);
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x000577A3 File Offset: 0x000559A3
		public override int GetHashCode()
		{
			return this._playerPair.GetHashCode();
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x000577B8 File Offset: 0x000559B8
		public int GetCooldownCount(OrderTypes orderType)
		{
			int result;
			if (!this._diplomacyCooldown.TryGetValue(orderType, out result))
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x000577D8 File Offset: 0x000559D8
		public void ClearCooldownCount(OrderTypes orderType)
		{
			this._diplomacyCooldown[orderType] = 0;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x000577E8 File Offset: 0x000559E8
		public void SetBloodFeud(TurnProcessContext context, int triggeringPlayerId)
		{
			this._diplomacyCooldown[OrderTypes.DeclareBloodFeud] = (this._diplomacyCooldown[OrderTypes.AssertWeakness] = context.Rules.MinDiplomacyOrderCooldown);
			this.ChangeState<BloodFeudState>(context, new BloodFeudState(), false);
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0005782C File Offset: 0x00055A2C
		public void SetAssertWeaknessCooldown(TurnProcessContext context)
		{
			this._diplomacyCooldown[OrderTypes.DeclareBloodFeud] = (this._diplomacyCooldown[OrderTypes.AssertWeakness] = context.Rules.MinDiplomacyOrderCooldown);
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00057861 File Offset: 0x00055A61
		public void SetVendetta(TurnProcessContext context, Vendetta vendetta)
		{
			this.ChangeState<VendettaState>(context, new VendettaState(vendetta), false);
			this._actorId = vendetta.ActorId;
			this.ResetDemandConcededCount(this._playerPair.First);
			this.ResetDemandConcededCount(this._playerPair.Second);
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x000578A0 File Offset: 0x00055AA0
		public PraetorDuelState SetPraetorDuel(TurnProcessContext context, int challenger, Identifier praetor)
		{
			int defenderId;
			if (!this._playerPair.GetOther(challenger, out defenderId))
			{
				return null;
			}
			return this.SetPraetorDuel(context, new PraetorDuelData(challenger, defenderId, praetor));
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x000578D0 File Offset: 0x00055AD0
		public PraetorDuelState SetPraetorDuel(TurnProcessContext context, PraetorDuelData data)
		{
			this.ChangeState<PraetorDuelState>(context, new PraetorDuelState(data), false);
			this._actorId = data.Challenger.PlayerId;
			this.ResetDemandConcededCount(this._playerPair.First);
			this.ResetDemandConcededCount(this._playerPair.Second);
			return (PraetorDuelState)this._diplomaticState;
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0005792C File Offset: 0x00055B2C
		public DiplomaticState_ChainsOfAvarice SetChainsOfAvarice(TurnProcessContext context, int actorId, int length, bool interrupt = false)
		{
			DiplomaticState_ChainsOfAvarice diplomaticState_ChainsOfAvarice = this.ChangeState<DiplomaticState_ChainsOfAvarice>(context, new DiplomaticState_ChainsOfAvarice(actorId, length), interrupt);
			this._actorId = diplomaticState_ChainsOfAvarice.ActorId;
			return diplomaticState_ChainsOfAvarice;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00057958 File Offset: 0x00055B58
		public DiplomaticState_LureOfExcess SetLureOfExcess(TurnProcessContext context, int actorId, int duration, bool interrupt = false)
		{
			DiplomaticState_LureOfExcess diplomaticState_LureOfExcess = this.ChangeState<DiplomaticState_LureOfExcess>(context, new DiplomaticState_LureOfExcess(actorId, duration), interrupt);
			this._actorId = diplomaticState_LureOfExcess.ActorId;
			return diplomaticState_LureOfExcess;
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00057983 File Offset: 0x00055B83
		public T ChangeState<T>(TurnProcessContext context, T state, bool interrupt = false) where T : DiplomaticState
		{
			this.ChangeState_Internal(context, state, interrupt);
			return state;
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x00057994 File Offset: 0x00055B94
		private void ChangeState_Internal(TurnProcessContext context, DiplomaticState state, bool interrupt = false)
		{
			DiplomaticState diplomaticState = this._diplomaticState;
			this._diplomaticState = state;
			if (state != null)
			{
				state.OnStateEntered(context, this, diplomaticState, interrupt);
			}
			if (diplomaticState != null)
			{
				diplomaticState.OnStateExited(context, this, state, interrupt);
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x000579C9 File Offset: 0x00055BC9
		public void SetNeutral(TurnProcessContext context, bool interrupt = false)
		{
			this.ChangeState<NeutralState>(context, new NeutralState(), interrupt);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x000579D9 File Offset: 0x00055BD9
		public void SetForceMajeure(TurnProcessContext context)
		{
			this.ChangeState<ForceMajeureState>(context, new ForceMajeureState(), false);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x000579E9 File Offset: 0x00055BE9
		public void SetSelf(TurnProcessContext context)
		{
			this.ChangeState<SelfDiplomaticState>(context, new SelfDiplomaticState(), false);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x000579F9 File Offset: 0x00055BF9
		public void SetExcommunicated(TurnProcessContext context, int excommId)
		{
			this.ChangeState<ExcommunicatedState>(context, new ExcommunicatedState(), false);
			this._actorId = excommId;
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00057A10 File Offset: 0x00055C10
		public void SetEliminated(TurnProcessContext context, int elimId)
		{
			this.ChangeState<EliminatedState>(context, new EliminatedState(), false);
			this._actorId = elimId;
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00057A27 File Offset: 0x00055C27
		public void SetVassalised(TurnProcessContext context, int vassalId, int bloodLordId)
		{
			this.ChangeState<VassalisedState>(context, new VassalisedState(vassalId, bloodLordId), false);
			this._actorId = vassalId;
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00057A40 File Offset: 0x00055C40
		public void UpdateDiplomaticState(TurnProcessContext context)
		{
			this._diplomaticState.Update(context, this);
			this.UpdateCooldowns();
			this.UpdateArmisticeState(context);
			this.UpdateVendetta(context);
			this.UpdateDuel(context);
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00057A6C File Offset: 0x00055C6C
		private void UpdateDuel(TurnProcessContext context)
		{
			PraetorDuelState praetorDuelState = this._diplomaticState as PraetorDuelState;
			if (praetorDuelState == null)
			{
				return;
			}
			if (!praetorDuelState.ProgressDuel(context) || praetorDuelState.Stage == PraetorDuelState.PraetorDuelFlowStage.Complete)
			{
				this.SetNeutral(context, false);
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00057AA4 File Offset: 0x00055CA4
		private void UpdateCooldowns()
		{
			foreach (OrderTypes orderTypes in from key in IEnumerableExtensions.ToList<OrderTypes>(this._diplomacyCooldown.Keys)
			where this._diplomacyCooldown[key] > 0
			select key)
			{
				Dictionary<OrderTypes, int> diplomacyCooldown = this._diplomacyCooldown;
				OrderTypes key2 = orderTypes;
				int num = diplomacyCooldown[key2];
				diplomacyCooldown[key2] = num - 1;
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00057B20 File Offset: 0x00055D20
		public void ResetCooldowns()
		{
			this._diplomacyCooldown.Clear();
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x00057B30 File Offset: 0x00055D30
		private void UpdateArmisticeState(TurnProcessContext context)
		{
			ArmisticeState armisticeState = this._diplomaticState as ArmisticeState;
			if (armisticeState == null)
			{
				return;
			}
			this.UpdateArmisticeCount();
			if (armisticeState != null && armisticeState.ArmisticeCount <= 0)
			{
				this.SetNeutral(context, false);
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00057B68 File Offset: 0x00055D68
		private void UpdateVendetta(TurnProcessContext context)
		{
			VendettaState vendettaState = this._diplomaticState as VendettaState;
			if (vendettaState == null)
			{
				return;
			}
			Vendetta vendetta = vendettaState.Vendetta;
			if (vendetta == null)
			{
				return;
			}
			TurnState currentTurn = context.CurrentTurn;
			PlayerState owner = currentTurn.FindPlayerState(vendetta.ActorId, null);
			vendetta.Objective.Update(context, owner);
			vendetta.OnTurnEnd();
			if (vendetta.IsComplete(currentTurn))
			{
				VendettaCompletedEvent vendettaCompletedEvent = new VendettaCompletedEvent(vendetta.ActorId, vendetta.TargetId, vendetta.Objective, vendetta.PrestigeWager, vendetta.PrestigeReward, vendetta.TurnTotal);
				vendettaCompletedEvent.TurnsTaken = context.CurrentTurn.TurnValue - vendetta.TurnStarted;
				bool flag = vendetta.Objective.IsCompleted(currentTurn);
				vendettaCompletedEvent.Successful = flag;
				if (flag)
				{
					this.IncrementVendettasWon(vendetta.ActorId);
				}
				currentTurn.AddGameEvent<VendettaCompletedEvent>(vendettaCompletedEvent);
				vendetta.PrestigeCompletePayment(currentTurn);
				this._previousVendettas.Add(vendetta);
				this.SetNeutral(context, false);
				return;
			}
			if (context.PaperworkRestructurePlayed)
			{
				return;
			}
			if (vendetta.TurnTotal == vendetta.TurnRemaining)
			{
				return;
			}
			VendettaInProgressEvent gameEvent = new VendettaInProgressEvent(vendetta.ActorId, vendetta.TargetId, vendetta.Objective, vendetta.PrestigeWager, vendetta.TurnRemaining);
			currentTurn.AddGameEvent<VendettaInProgressEvent>(gameEvent);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00057CAC File Offset: 0x00055EAC
		public void IncrementVendettasWon(int actorId)
		{
			int vendettaWinCount = this.GetVendettaWinCount(actorId);
			this.SetVendettasWon(actorId, vendettaWinCount + 1);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00057CCB File Offset: 0x00055ECB
		public void SetVendettasWon(int actorId, int val)
		{
			this._vendettasWon[actorId] = val;
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00057CDC File Offset: 0x00055EDC
		public int GetVendettaWinCount(int targetPlayerID)
		{
			int result;
			if (!this._vendettasWon.TryGetValue(targetPlayerID, out result))
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x00057CFC File Offset: 0x00055EFC
		private bool HasArmisticeCount()
		{
			return this.ArmisticeTurnCount > 0;
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00057D08 File Offset: 0x00055F08
		public void SetExtortConceded(TurnProcessContext context, int targetPlayerID)
		{
			this._diplomacyCooldown[OrderTypes.Extort] = (this._diplomacyCooldown[OrderTypes.Demand] = context.Rules.MinDiplomacyOrderCooldown);
			this.EndPendingDiplomacy(context);
			this.UpdateDemandConcededCount(targetPlayerID, true);
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00057D4C File Offset: 0x00055F4C
		public void SetExtortRejected(TurnProcessContext context, int targetPlayerID)
		{
			this._diplomacyCooldown[OrderTypes.Extort] = (this._diplomacyCooldown[OrderTypes.Demand] = context.Rules.MinDiplomacyOrderCooldown);
			PendingDiplomacyState pendingDiplomacyState = this._diplomaticState as PendingDiplomacyState;
			if (pendingDiplomacyState != null)
			{
				pendingDiplomacyState.SetResponse(YesNo.No);
			}
			this.UpdateDemandConcededCount(targetPlayerID, false);
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00057DA0 File Offset: 0x00055FA0
		public void SetDemandConceded(TurnProcessContext context, int targetPlayerID)
		{
			this._diplomacyCooldown[OrderTypes.Extort] = (this._diplomacyCooldown[OrderTypes.Demand] = context.Rules.MinDiplomacyOrderCooldown);
			this.EndPendingDiplomacy(context);
			this.UpdateDemandConcededCount(targetPlayerID, true);
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00057DE4 File Offset: 0x00055FE4
		public void SetDemandRejected(TurnProcessContext context, int targetPlayerID)
		{
			this._diplomacyCooldown[OrderTypes.Extort] = (this._diplomacyCooldown[OrderTypes.Demand] = context.Rules.MinDiplomacyOrderCooldown);
			PendingDiplomacyState pendingDiplomacyState = this._diplomaticState as PendingDiplomacyState;
			if (pendingDiplomacyState != null)
			{
				pendingDiplomacyState.SetResponse(YesNo.No);
			}
			this.UpdateDemandConcededCount(targetPlayerID, false);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00057E38 File Offset: 0x00056038
		public void SetInsultConceded(TurnProcessContext context)
		{
			this._diplomacyCooldown[OrderTypes.Insult] = (this._diplomacyCooldown[OrderTypes.Humiliate] = context.Rules.MinDiplomacyOrderCooldown);
			this.EndPendingDiplomacy(context);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00057E74 File Offset: 0x00056074
		public void SetInsultRejected(TurnProcessContext context)
		{
			this._diplomacyCooldown[OrderTypes.Insult] = (this._diplomacyCooldown[OrderTypes.Humiliate] = context.Rules.MinDiplomacyOrderCooldown);
			PendingDiplomacyState pendingDiplomacyState = this._diplomaticState as PendingDiplomacyState;
			if (pendingDiplomacyState != null)
			{
				pendingDiplomacyState.SetResponse(YesNo.No);
			}
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00057EC0 File Offset: 0x000560C0
		public void SetHumiliateConceded(TurnProcessContext context)
		{
			this._diplomacyCooldown[OrderTypes.Insult] = (this._diplomacyCooldown[OrderTypes.Humiliate] = context.Rules.MinDiplomacyOrderCooldown);
			this.EndPendingDiplomacy(context);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00057EFC File Offset: 0x000560FC
		public void SetHumiliateRejected(TurnProcessContext context)
		{
			this._diplomacyCooldown[OrderTypes.Insult] = (this._diplomacyCooldown[OrderTypes.Humiliate] = context.Rules.MinDiplomacyOrderCooldown);
			this.EndPendingDiplomacy(context);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00057F38 File Offset: 0x00056138
		public void SetVassalageConceded(TurnProcessContext context, int bloodLordId)
		{
			this._diplomacyCooldown[OrderTypes.Vassalage] = (this._diplomacyCooldown[OrderTypes.SendEmissary] = context.Rules.MinDiplomacyOrderCooldown);
			this.EndPendingDiplomacy(context);
			this.SetVassalage(context, bloodLordId);
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00057F7C File Offset: 0x0005617C
		public void SetVassalageRejected(TurnProcessContext context)
		{
			this._diplomacyCooldown[OrderTypes.Vassalage] = (this._diplomacyCooldown[OrderTypes.SendEmissary] = context.Rules.MinDiplomacyOrderCooldown);
			this.EndPendingDiplomacy(context);
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x00057FB8 File Offset: 0x000561B8
		public void SetEmissaryAccepted(TurnProcessContext context, PendingDiplomacy_Emissary emissaryState)
		{
			this._diplomacyCooldown[OrderTypes.Vassalage] = (this._diplomacyCooldown[OrderTypes.SendEmissary] = Math.Max(context.Rules.MinDiplomacyOrderCooldown, emissaryState.ArmisticeLength));
			this.EndPendingDiplomacy(context);
			this.SetArmistice(context, emissaryState.ArmisticeLength, emissaryState.Wager);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00058011 File Offset: 0x00056211
		public void SetEmissaryRejected(TurnProcessContext context)
		{
			this._diplomacyCooldown[OrderTypes.SendEmissary] = context.Rules.MinDiplomacyOrderCooldown;
			this.EndPendingDiplomacy(context);
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00058034 File Offset: 0x00056234
		public void SetEmissaryStrongRejected(TurnProcessContext context)
		{
			this._diplomacyCooldown[OrderTypes.SendEmissary] = context.Rules.MinDiplomacyOrderCooldown;
			PendingDiplomacyState pendingDiplomacyState = this._diplomaticState as PendingDiplomacyState;
			if (pendingDiplomacyState != null)
			{
				pendingDiplomacyState.SetResponse(YesNo.StrongNo);
			}
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00058070 File Offset: 0x00056270
		public void SetArmistice(TurnProcessContext context, int armisticeVal, int initialWager = 0)
		{
			this.AddArmisticeCount(context, armisticeVal + 1);
			ArmisticeState armisticeState = this._diplomaticState as ArmisticeState;
			if (armisticeState != null)
			{
				armisticeState.InitialWager = initialWager;
			}
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0005809D File Offset: 0x0005629D
		private void EndPendingDiplomacy(TurnProcessContext context)
		{
			if (this.IsDiplomacyPending())
			{
				this.SetNeutral(context, false);
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x000580B0 File Offset: 0x000562B0
		public void AddArmisticeCount(TurnProcessContext context, int armisticeVal)
		{
			ArmisticeState armisticeState = this._diplomaticState as ArmisticeState;
			if (armisticeState != null)
			{
				armisticeState.AddCount(armisticeVal);
				return;
			}
			this.ChangeState<ArmisticeState>(context, new ArmisticeState(armisticeVal), false);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x000580E3 File Offset: 0x000562E3
		public void ResetDemandConcededCount(int targetID)
		{
			this._playerDemandsConceded[targetID] = 0;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x000580F2 File Offset: 0x000562F2
		private void UpdateDemandConcededCount(int playerID, bool hasConceded)
		{
			if (hasConceded)
			{
				this.IncrementDemandConcededCount(playerID);
				return;
			}
			this.ResetDemandConcededCount(playerID);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00058108 File Offset: 0x00056308
		public void IncrementDemandConcededCount(int playerId)
		{
			int demandConcededCount = this.GetDemandConcededCount(playerId);
			this._playerDemandsConceded[playerId] = demandConcededCount + 1;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0005812C File Offset: 0x0005632C
		public int GetDemandConcededCount(int targetID)
		{
			int result;
			if (!this._playerDemandsConceded.TryGetValue(targetID, out result))
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0005814C File Offset: 0x0005634C
		public Result IsResponseAllowed(TurnState turn, OrderTypes orderType, int instigatorId)
		{
			if (orderType == OrderTypes.None)
			{
				return new SimulationError(string.Format("Invalid order type {0}", orderType));
			}
			int num = (instigatorId == this._playerPair.First) ? this._playerPair.Second : this._playerPair.First;
			if (this.TargetID == this.ActorId || this.TargetID == -1 || this.TargetID == -2147483648)
			{
				return new Result.DiplomacyProblem(num, orderType);
			}
			PlayerState playerState = turn.FindPlayerState(instigatorId, null);
			DiplomaticState diplomaticState = this._diplomaticState;
			Result result;
			if (!(diplomaticState is ExcommunicatedState))
			{
				if (!(diplomaticState is EliminatedState))
				{
					result = Result.Success;
				}
				else
				{
					result = new Result.CannotWhileEliminatedProblem(num, orderType, playerState.Eliminated ? playerState.Id : num);
				}
			}
			else
			{
				result = new Result.ExcommunicatedProblem(num, orderType, playerState.Excommunicated ? playerState.Id : num, playerState.Id);
			}
			Problem problem = result as Problem;
			if (problem != null)
			{
				return problem;
			}
			int bloodLordPlayerId;
			if (turn.CurrentDiplomaticTurn.IsVassalOfAny(instigatorId, out bloodLordPlayerId))
			{
				return new Result.CannotWhileVassalProblem(num, orderType, bloodLordPlayerId, instigatorId);
			}
			int bloodLordPlayerId2;
			if (turn.CurrentDiplomaticTurn.IsVassalOfAny(num, out bloodLordPlayerId2))
			{
				return new Result.CannotTargetVassalProblem(num, orderType, bloodLordPlayerId2, num, instigatorId);
			}
			return Result.Success;
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00058278 File Offset: 0x00056478
		public Result IsOrderAllowed(TurnState turn, OrderTypes orderType, int instigatorId)
		{
			Problem problem = this.IsResponseAllowed(turn, orderType, instigatorId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			int num = (instigatorId == this._playerPair.First) ? this._playerPair.Second : this._playerPair.First;
			if (turn.FindPlayerState(num, null).DiplomaticImmunity && orderType.IsBlockedByDiplomaticImmunity())
			{
				return new Result.TargetHasDiplomaticImmunityProblem(num, orderType);
			}
			turn.FindPlayerState(instigatorId, null);
			DiplomaticState diplomaticState = this._diplomaticState;
			ArmisticeState armisticeState = diplomaticState as ArmisticeState;
			Result result;
			if (armisticeState == null)
			{
				PendingDiplomacyState pendingDiplomacyState = diplomaticState as PendingDiplomacyState;
				if (pendingDiplomacyState == null)
				{
					if (!(diplomaticState is VassalisedState))
					{
						result = Result.Success;
					}
					else
					{
						result = new SimulationError("VassalisedState should have been handled by previous checks");
					}
				}
				else
				{
					result = new Result.DiplomacyPendingProblem(num, orderType, pendingDiplomacyState.DiplomaticPendingValue);
				}
			}
			else
			{
				result = new Result.TemporaryStateProblem(num, orderType, this._diplomaticState.Type, armisticeState.ArmisticeCount);
			}
			Problem problem2 = result as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			if (!this._diplomaticState.AllowAnyDiplomacy)
			{
				return new Result.InvalidDiplomaticStateProblem(num, orderType, this._diplomaticState.Type);
			}
			if (this.IsDiplomacyActionOnCooldown(orderType))
			{
				return new Result.DiplomacyCooldownProblem(num, orderType, this.GetCooldownCount(orderType));
			}
			return Result.Success;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x000583A5 File Offset: 0x000565A5
		public void SetVassalage(TurnProcessContext context, int bloodLord)
		{
			this.ChangeState<BloodVassalageState>(context, new BloodVassalageState(bloodLord, this.GetPairedPlayerID(bloodLord)), false);
			this._actorId = bloodLord;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000583C4 File Offset: 0x000565C4
		private void UpdateArmisticeCount()
		{
			ArmisticeState armisticeState = this._diplomaticState as ArmisticeState;
			if (armisticeState != null)
			{
				armisticeState.ReduceCount(1);
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x000583E7 File Offset: 0x000565E7
		public bool TryGetOtherPlayer(int playerId, out int otherPlayerId)
		{
			return this._playerPair.GetOther(playerId, out otherPlayerId);
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x000583F8 File Offset: 0x000565F8
		private int GetPairedPlayerID(int playerID)
		{
			int result;
			this._playerPair.GetOther(playerID, out result);
			return result;
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00058418 File Offset: 0x00056618
		public static DiplomaticPendingValue OrderToPendingValue(OrderTypes orderType)
		{
			switch (orderType)
			{
			case OrderTypes.Insult:
				return DiplomaticPendingValue.Insult;
			case OrderTypes.Demand:
				return DiplomaticPendingValue.MakeDemand;
			case OrderTypes.SendEmissary:
				return DiplomaticPendingValue.Emissary;
			case OrderTypes.Humiliate:
				return DiplomaticPendingValue.Humiliate;
			case OrderTypes.Extort:
				return DiplomaticPendingValue.Extort;
			case OrderTypes.Vendetta:
				return DiplomaticPendingValue.Vendetta;
			default:
			{
				switch (orderType)
				{
				case OrderTypes.OfferContract:
					return DiplomaticPendingValue.Contract;
				case OrderTypes.Vassalage:
					return DiplomaticPendingValue.RequestToBeVassal;
				case OrderTypes.RequestToBeVassalizedByTarget:
					return DiplomaticPendingValue.RequestToBeVassal;
				case OrderTypes.RequestToBeBloodLordOfTarget:
					return DiplomaticPendingValue.RequestToBeVassal;
				case OrderTypes.DeclareBloodFeud:
					return DiplomaticPendingValue.Vendetta;
				case OrderTypes.AssertWeakness:
					return DiplomaticPendingValue.Vendetta;
				case OrderTypes.DraconicRazzia:
					return DiplomaticPendingValue.DraconicRazzia;
				case OrderTypes.ChainsOfAvarice:
					return DiplomaticPendingValue.ChainsOfAvarice;
				case OrderTypes.VileCalumny:
					return DiplomaticPendingValue.VileCalumny;
				case OrderTypes.LureOfExcess:
					return DiplomaticPendingValue.LureOfExcess;
				}
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Unhandled Pending Diplomacy Sprite lookup for orderType {0}", orderType));
				}
				return DiplomaticPendingValue.None;
			}
			}
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x000584FC File Offset: 0x000566FC
		public void DeepClone(out DiplomaticPairStatus clone)
		{
			clone = new DiplomaticPairStatus
			{
				_diplomaticState = this._diplomaticState.DeepClone<DiplomaticState>(),
				_previousVendettas = this._previousVendettas.DeepClone<Vendetta>(),
				_playerDemandsConceded = this._playerDemandsConceded.DeepClone(),
				_vendettasWon = this._vendettasWon.DeepClone(),
				_diplomacyCooldown = this._diplomacyCooldown.DeepClone<OrderTypes>(),
				_actorId = this._actorId,
				_playerPair = this._playerPair
			};
		}

		// Token: 0x04000B7E RID: 2942
		[JsonProperty]
		private DiplomaticState _diplomaticState = new NeutralState();

		// Token: 0x04000B7F RID: 2943
		[JsonProperty]
		private List<Vendetta> _previousVendettas = new List<Vendetta>();

		// Token: 0x04000B80 RID: 2944
		[JsonProperty]
		private Dictionary<int, int> _playerDemandsConceded = new Dictionary<int, int>();

		// Token: 0x04000B81 RID: 2945
		[JsonProperty]
		private Dictionary<int, int> _vendettasWon = new Dictionary<int, int>();

		// Token: 0x04000B82 RID: 2946
		[JsonProperty]
		private Dictionary<OrderTypes, int> _diplomacyCooldown = new Dictionary<OrderTypes, int>();

		// Token: 0x04000B83 RID: 2947
		[JsonProperty]
		private int _actorId;

		// Token: 0x04000B84 RID: 2948
		[JsonProperty]
		private PlayerPair _playerPair;
	}
}
