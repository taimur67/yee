using System;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020004D6 RID: 1238
	public class HumiliateDecisionProcessor : DiplomaticDecisionProcessor<HumiliateDecisionRequest, HumiliateDecisionResponse>
	{
		// Token: 0x06001744 RID: 5956 RVA: 0x00054B64 File Offset: 0x00052D64
		protected override Result Enact(HumiliateDecisionResponse response)
		{
			HumiliateDecisionProcessor.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.response = response;
			CS$<>8__locals1.diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.RequestingPlayerId);
			CS$<>8__locals1.opponent = base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null);
			if (CS$<>8__locals1.response.Choice != YesNo.Yes)
			{
				return this.<Enact>g__RejectHumiliate|0_0(ref CS$<>8__locals1);
			}
			if (base.request.PrestigeWager > CS$<>8__locals1.response.Payment.Prestige)
			{
				return this.<Enact>g__RejectHumiliate|0_0(ref CS$<>8__locals1);
			}
			if (!base._currentTurn.AcceptPayment(this._player.Id, CS$<>8__locals1.response.Payment).successful)
			{
				return this.<Enact>g__RejectHumiliate|0_0(ref CS$<>8__locals1);
			}
			return this.<Enact>g__AcceptHumiliate|0_1(ref CS$<>8__locals1);
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00054C40 File Offset: 0x00052E40
		private void HasRejected(HumiliateDecisionRequest request, DiplomaticPairStatus diplomaticPairStatus)
		{
			diplomaticPairStatus.SetHumiliateRejected(this.TurnProcessContext);
			GameEvent gameEvent;
			GrievanceProcessor.BeginVendettaOrDuel(this.TurnProcessContext, request.RequestingPlayerId, this._player.Id, request.GrievanceResponse, out gameEvent);
			HumiliateResponseEvent humiliateResponseEvent = new HumiliateResponseEvent(request.RequestingPlayerId, this._player.Id, YesNo.No, base._rules.MinDiplomacyOrderCooldown);
			humiliateResponseEvent.OrderType = OrderTypes.Humiliate;
			humiliateResponseEvent.GrievanceResponse = request.GrievanceResponse;
			humiliateResponseEvent.Prestige = request.PrestigeWager;
			base._currentTurn.AddGameEvent<HumiliateResponseEvent>(humiliateResponseEvent);
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00054CCD File Offset: 0x00052ECD
		protected override Result Preview(HumiliateDecisionResponse response)
		{
			return base._currentTurn.AcceptPayment(this._player.Id, response.Payment);
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00054CF3 File Offset: 0x00052EF3
		[CompilerGenerated]
		private Result <Enact>g__RejectHumiliate|0_0(ref HumiliateDecisionProcessor.<>c__DisplayClass0_0 A_1)
		{
			this.HasRejected(base.request, A_1.diplomaticStatus);
			return Result.Success;
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00054D0C File Offset: 0x00052F0C
		[CompilerGenerated]
		private Result <Enact>g__AcceptHumiliate|0_1(ref HumiliateDecisionProcessor.<>c__DisplayClass0_0 A_1)
		{
			A_1.diplomaticStatus.SetHumiliateConceded(this.TurnProcessContext);
			PaymentReceivedEvent paymentReceivedEvent = base.GiveDiplomacyPayment(A_1.opponent, A_1.response.Payment, 2);
			if (paymentReceivedEvent == null)
			{
				return Result.Failure;
			}
			HumiliateResponseEvent humiliateResponseEvent = new HumiliateResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.Yes, base._rules.MinDiplomacyOrderCooldown);
			humiliateResponseEvent.AddChildEvent<PaymentReceivedEvent>(paymentReceivedEvent);
			humiliateResponseEvent.OrderType = OrderTypes.Humiliate;
			humiliateResponseEvent.GrievanceResponse = base.request.GrievanceResponse;
			humiliateResponseEvent.Prestige = base.request.PrestigeWager;
			base._currentTurn.AddGameEvent<HumiliateResponseEvent>(humiliateResponseEvent);
			return Result.Success;
		}
	}
}
