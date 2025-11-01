using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200050C RID: 1292
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class DiplomaticState : IDeepClone<DiplomaticState>
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x00058BB5 File Offset: 0x00056DB5
		[JsonIgnore]
		public virtual DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.Neutral;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x00058BB8 File Offset: 0x00056DB8
		[JsonIgnore]
		public virtual bool AllowAnyDiplomacy
		{
			get
			{
				return this.AllowFriendlyDiplomacy || this.AllowHostileDiplomacy;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x00058BCA File Offset: 0x00056DCA
		[JsonIgnore]
		public virtual bool AllowFriendlyDiplomacy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x00058BCD File Offset: 0x00056DCD
		[JsonIgnore]
		public virtual bool AllowHostileDiplomacy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x00058BD0 File Offset: 0x00056DD0
		public virtual bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return false;
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00058BD3 File Offset: 0x00056DD3
		public virtual bool AllowRedeployThroughFixtures(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return this.AllowSupport && !this.AllowCombat(diplomacy, requestingPlayer, targetPlayer) && this.AllowMovementIntoTerritory(diplomacy, requestingPlayer, targetPlayer);
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x00058BF3 File Offset: 0x00056DF3
		public bool AllowCantonCapture(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return this.GetCantonCaptureRules(diplomacy, requestingPlayer, targetPlayer) > CantonCaptureRule.CannotCapture;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x00058C01 File Offset: 0x00056E01
		public virtual bool AllowCombat(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return false;
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x00058C04 File Offset: 0x00056E04
		public virtual bool AllowNearbyHealingProvidedBy(int providingPlayerId)
		{
			return false;
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x00058C07 File Offset: 0x00056E07
		public virtual bool AllowStrongholdCapture(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return false;
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00058C0A File Offset: 0x00056E0A
		public virtual bool AllowDuelling(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return false;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00058C0D File Offset: 0x00056E0D
		public virtual CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRule.CannotCapture;
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x00058C10 File Offset: 0x00056E10
		public virtual CantonCaptureRestrictions GetCantonCaptureRestrictions(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRestrictions.None;
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x00058C13 File Offset: 0x00056E13
		[JsonIgnore]
		public virtual bool AllowSupport
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x00058C16 File Offset: 0x00056E16
		[JsonIgnore]
		public virtual bool AllowVassalRelationshipRequest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x00058C19 File Offset: 0x00056E19
		public virtual void Update(TurnProcessContext context, DiplomaticPairStatus relationship)
		{
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00058C1B File Offset: 0x00056E1B
		public virtual void OnStateEntered(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState, bool interrupt)
		{
			if (interrupt)
			{
				this.HandleDiplomacyCancellation(context, pair, prevState);
			}
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00058C2A File Offset: 0x00056E2A
		public virtual void OnStateExited(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState nextState, bool interrupt)
		{
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00058C2C File Offset: 0x00056E2C
		protected void HandleDiplomacyCancellation(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState)
		{
			bool flag = false;
			int actorId = pair.ActorId;
			int num = actorId;
			int targetID = pair.TargetID;
			int num2 = 0;
			PendingDiplomacy_VileCalumny pendingDiplomacy_VileCalumny = prevState as PendingDiplomacy_VileCalumny;
			if (pendingDiplomacy_VileCalumny == null)
			{
				PendingDiplomacyState pendingDiplomacyState = prevState as PendingDiplomacyState;
				if (pendingDiplomacyState == null)
				{
					VendettaState vendettaState = prevState as VendettaState;
					if (vendettaState == null)
					{
						ArmisticeState armisticeState = prevState as ArmisticeState;
						if (armisticeState == null)
						{
							PraetorDuelState praetorDuelState = prevState as PraetorDuelState;
							if (praetorDuelState != null)
							{
								num2 = praetorDuelState.DuelData.BaseWager;
								flag = true;
							}
						}
						else
						{
							num2 = armisticeState.InitialWager;
							flag = true;
						}
					}
					else
					{
						num2 = vendettaState.Vendetta.PrestigeWager;
						flag = true;
					}
				}
				else
				{
					num2 = pendingDiplomacyState.Wager;
					flag = true;
				}
			}
			else
			{
				num = pendingDiplomacy_VileCalumny.SourceId;
				num2 = pendingDiplomacy_VileCalumny.Wager;
				flag = true;
			}
			DiplomaticStateCancelledEvent diplomaticStateCancelledEvent = new DiplomaticStateCancelledEvent(num, targetID);
			PlayerState actor = context.CurrentTurn.FindPlayerState(actorId, null);
			PlayerState playerState = context.CurrentTurn.FindPlayerState(num, null);
			PlayerState target = context.CurrentTurn.FindPlayerState(targetID, null);
			if (num2 > 0 && playerState.CanReceivePrestige())
			{
				diplomaticStateCancelledEvent.AddChildEvent<PaymentReceivedEvent>(context.GivePrestige(playerState, num2));
			}
			if (flag)
			{
				foreach (GameEvent gameEvent in context.CurrentTurn.GameEvents)
				{
					ICancellableGameEvent cancellableGameEvent = null;
					DiplomaticResponseEvent diplomaticResponseEvent = gameEvent as DiplomaticResponseEvent;
					if (diplomaticResponseEvent != null)
					{
						cancellableGameEvent = diplomaticResponseEvent;
					}
					else
					{
						VendettaStartedEvent vendettaStartedEvent = gameEvent as VendettaStartedEvent;
						if (vendettaStartedEvent != null)
						{
							cancellableGameEvent = vendettaStartedEvent;
						}
						else
						{
							VendettaInProgressEvent vendettaInProgressEvent = gameEvent as VendettaInProgressEvent;
							if (vendettaInProgressEvent != null)
							{
								cancellableGameEvent = vendettaInProgressEvent;
							}
						}
					}
					if (cancellableGameEvent != null && gameEvent.IsAssociatedWith(num) && gameEvent.IsAssociatedWith(targetID))
					{
						cancellableGameEvent.Cancelled = true;
					}
				}
				this.CleanupPendingDecisions(actor, target, pair.PlayerPair);
				context.CurrentTurn.AddGameEvent<DiplomaticStateCancelledEvent>(diplomaticStateCancelledEvent);
			}
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00058DF8 File Offset: 0x00056FF8
		private void CleanupPendingDecisions(PlayerState actor, PlayerState target, PlayerPair pair)
		{
			this.RemoveDecisionsBetweenPair(actor, pair);
			this.RemoveDecisionsBetweenPair(target, pair);
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00058E0C File Offset: 0x0005700C
		private void RemoveDecisionsBetweenPair(PlayerState player, PlayerPair pair)
		{
			for (int i = player.DecisionRequests.Count - 1; i >= 0; i--)
			{
				IDiplomaticDecisionRequest diplomaticDecisionRequest = player.DecisionRequests[i] as IDiplomaticDecisionRequest;
				if (diplomaticDecisionRequest != null && diplomaticDecisionRequest.RelatesToPlayers(pair))
				{
					player.DecisionRequests.RemoveAt(i);
				}
			}
		}

		// Token: 0x060018F2 RID: 6386
		public abstract void DeepClone(out DiplomaticState clone);
	}
}
