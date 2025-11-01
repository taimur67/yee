using System;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020004DF RID: 1247
	public class MakeDemandDecisionProcessor : DiplomaticDecisionProcessor<MakeDemandDecisionRequest, MakeDemandDecisionResponse>
	{
		// Token: 0x0600177B RID: 6011 RVA: 0x0005539C File Offset: 0x0005359C
		protected override MakeDemandDecisionResponse GenerateTypedFallbackResponse()
		{
			return new MakeDemandDecisionResponse
			{
				Choice = YesNo.No
			};
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x000553AC File Offset: 0x000535AC
		protected override Result Enact(MakeDemandDecisionResponse response)
		{
			MakeDemandDecisionProcessor.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.response = response;
			CS$<>8__locals1.diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.RequestingPlayerId);
			CS$<>8__locals1.opponent = base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null);
			if (CS$<>8__locals1.response.Choice != YesNo.Yes)
			{
				return this.<Enact>g__RejectDemand|1_0(ref CS$<>8__locals1);
			}
			if (base.request.NumCards > CS$<>8__locals1.response.Payment.Resources.Count)
			{
				return this.<Enact>g__RejectDemand|1_0(ref CS$<>8__locals1);
			}
			if (!base._currentTurn.AcceptPayment(this._player.Id, CS$<>8__locals1.response.Payment).successful)
			{
				return this.<Enact>g__RejectDemand|1_0(ref CS$<>8__locals1);
			}
			return this.<Enact>g__ConcedeDemand|1_1(ref CS$<>8__locals1);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x0005548C File Offset: 0x0005368C
		private void HasRejected(MakeDemandDecisionRequest request, DiplomaticPairStatus diplomaticPairStatus)
		{
			diplomaticPairStatus.SetDemandRejected(this.TurnProcessContext, this._player.Id);
			PaymentReceivedEvent gameEvent = this.TurnProcessContext.GivePrestige(this._player, request.PrestigeWager);
			base._currentTurn.AddGameEvent<PaymentReceivedEvent>(gameEvent);
			PendingDiplomacy_Grievance state = new PendingDiplomacy_Grievance(this._player.Id, OrderTypes.Demand);
			diplomaticPairStatus.SetDiplomacyPending(this.TurnProcessContext, state);
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x000554F4 File Offset: 0x000536F4
		protected override Result Preview(MakeDemandDecisionResponse response)
		{
			return base._currentTurn.AcceptPayment(this._player.Id, response.Payment);
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x0005551C File Offset: 0x0005371C
		[CompilerGenerated]
		private Result <Enact>g__RejectDemand|1_0(ref MakeDemandDecisionProcessor.<>c__DisplayClass1_0 A_1)
		{
			this.HasRejected(base.request, A_1.diplomaticStatus);
			MakeDemandResponseEvent makeDemandResponseEvent = new MakeDemandResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.No, base._rules.MinDiplomacyOrderCooldown);
			makeDemandResponseEvent.OrderType = OrderTypes.Demand;
			base._currentTurn.AddGameEvent<MakeDemandResponseEvent>(makeDemandResponseEvent);
			return Result.Success;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x0005557C File Offset: 0x0005377C
		[CompilerGenerated]
		private Result <Enact>g__ConcedeDemand|1_1(ref MakeDemandDecisionProcessor.<>c__DisplayClass1_0 A_1)
		{
			A_1.diplomaticStatus.SetDemandConceded(this.TurnProcessContext, this._player.Id);
			Payment payment = A_1.response.Payment;
			payment.AddPrestige(base.request.PrestigeWager);
			PaymentReceivedEvent paymentReceivedEvent = base.GiveDiplomacyPayment(A_1.opponent, payment, 1);
			if (paymentReceivedEvent == null)
			{
				return Result.Failure;
			}
			MakeDemandResponseEvent makeDemandResponseEvent = new MakeDemandResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.Yes, base._rules.MinDiplomacyOrderCooldown);
			makeDemandResponseEvent.AddChildEvent<PaymentReceivedEvent>(paymentReceivedEvent);
			makeDemandResponseEvent.OrderType = OrderTypes.Demand;
			base._currentTurn.AddGameEvent<MakeDemandResponseEvent>(makeDemandResponseEvent);
			return Result.Success;
		}
	}
}
