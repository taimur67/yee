using System;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020004D3 RID: 1235
	public class ExtortDecisionProcessor : DiplomaticDecisionProcessor<ExtortDecisionRequest, ExtortDecisionResponse>
	{
		// Token: 0x06001729 RID: 5929 RVA: 0x00054767 File Offset: 0x00052967
		protected override ExtortDecisionResponse GenerateTypedFallbackResponse()
		{
			return new ExtortDecisionResponse
			{
				Choice = YesNo.No
			};
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00054778 File Offset: 0x00052978
		protected override Result Enact(ExtortDecisionResponse response)
		{
			ExtortDecisionProcessor.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.response = response;
			CS$<>8__locals1.diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.RequestingPlayerId);
			CS$<>8__locals1.opponent = base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null);
			if (CS$<>8__locals1.response.Choice != YesNo.Yes)
			{
				return this.<Enact>g__RejectDemand|1_0(ref CS$<>8__locals1);
			}
			if (CS$<>8__locals1.response.ExtortedItem == Identifier.Invalid)
			{
				return this.<Enact>g__RejectDemand|1_0(ref CS$<>8__locals1);
			}
			return this.<Enact>g__AcceptDemand|1_1(ref CS$<>8__locals1);
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00054814 File Offset: 0x00052A14
		private void HasRejected(ExtortDecisionRequest request, DiplomaticPairStatus diplomaticPairStatus)
		{
			PaymentReceivedEvent gameEvent = this.TurnProcessContext.GivePrestige(this._player, request.PrestigeWager);
			base._currentTurn.AddGameEvent<PaymentReceivedEvent>(gameEvent);
			diplomaticPairStatus.SetExtortRejected(this.TurnProcessContext, this._player.Id);
			PendingDiplomacy_Grievance state = new PendingDiplomacy_Grievance(this._player.Id, OrderTypes.Extort);
			diplomaticPairStatus.SetDiplomacyPending(this.TurnProcessContext, state);
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0005487C File Offset: 0x00052A7C
		protected override Result Preview(ExtortDecisionResponse response)
		{
			if (response.Choice != YesNo.Yes)
			{
				return base._currentTurn.AcceptPayment(this._player.Id, response.Payment);
			}
			return Result.Success;
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x000548B4 File Offset: 0x00052AB4
		[CompilerGenerated]
		private Result <Enact>g__RejectDemand|1_0(ref ExtortDecisionProcessor.<>c__DisplayClass1_0 A_1)
		{
			this.HasRejected(base.request, A_1.diplomaticStatus);
			ExtortResponseEvent extortResponseEvent = new ExtortResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.No, base._rules.MinDiplomacyOrderCooldown);
			extortResponseEvent.OrderType = OrderTypes.Extort;
			extortResponseEvent.ExtortedItem = Identifier.Invalid;
			base._currentTurn.AddGameEvent<ExtortResponseEvent>(extortResponseEvent);
			return Result.Success;
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0005491C File Offset: 0x00052B1C
		[CompilerGenerated]
		private Result <Enact>g__AcceptDemand|1_1(ref ExtortDecisionProcessor.<>c__DisplayClass1_0 A_1)
		{
			GameItem gameItem = base._currentTurn.FetchGameItem(A_1.response.ExtortedItem);
			if (gameItem.Status != GameItemStatus.InPlay)
			{
				return this.<Enact>g__RejectFromFailure|1_2(Result.Failure, ref A_1);
			}
			if (base._currentTurn.FindControllingPlayer(gameItem).Id != this._player.Id)
			{
				return this.<Enact>g__RejectFromFailure|1_2(Result.Failure, ref A_1);
			}
			A_1.diplomaticStatus.SetExtortConceded(this.TurnProcessContext, this._player.Id);
			GameEvent gameEvent = this.TurnProcessContext.TransferOwnership(A_1.response.ExtortedItem, base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null), false);
			PaymentReceivedEvent paymentReceivedEvent = base.GiveDiplomacyPayment(A_1.opponent, A_1.response.Payment, 1);
			if (paymentReceivedEvent == null)
			{
				return this.<Enact>g__RejectFromFailure|1_2(Result.Failure, ref A_1);
			}
			ExtortResponseEvent extortResponseEvent = new ExtortResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.Yes, base._rules.MinDiplomacyOrderCooldown);
			extortResponseEvent.AddChildEvent(new GameEvent[]
			{
				gameEvent,
				paymentReceivedEvent
			});
			extortResponseEvent.OrderType = OrderTypes.Extort;
			extortResponseEvent.ExtortedItem = A_1.response.ExtortedItem;
			base._currentTurn.AddGameEvent<ExtortResponseEvent>(extortResponseEvent);
			return Result.Success;
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x00054A5B File Offset: 0x00052C5B
		[CompilerGenerated]
		private Problem <Enact>g__RejectFromFailure|1_2(Problem problem, ref ExtortDecisionProcessor.<>c__DisplayClass1_0 A_2)
		{
			this.HasRejected(base.request, A_2.diplomaticStatus);
			return problem;
		}
	}
}
