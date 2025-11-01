using System;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020004F3 RID: 1267
	public class VileCalumnyDecisionProcessor : DiplomaticDecisionProcessor<VileCalumnyRequest, VileCalumnyResponse>
	{
		// Token: 0x06001802 RID: 6146 RVA: 0x00056763 File Offset: 0x00054963
		protected override VileCalumnyResponse GenerateTypedFallbackResponse()
		{
			return new VileCalumnyResponse
			{
				Choice = YesNo.Yes
			};
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x00056774 File Offset: 0x00054974
		protected override Result Enact(VileCalumnyResponse response)
		{
			VileCalumnyDecisionProcessor.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.response = response;
			CS$<>8__locals1.diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.ScapegoatId);
			CS$<>8__locals1.scapegoat = base._currentTurn.FindPlayerState(base.request.ScapegoatId, null);
			CS$<>8__locals1.source = base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null);
			CS$<>8__locals1.diplomaticStatus.SetNeutral(this.TurnProcessContext, false);
			if (CS$<>8__locals1.response.Choice == YesNo.Yes)
			{
				return this.<Enact>g__AcceptInsult|1_1(ref CS$<>8__locals1);
			}
			if (CS$<>8__locals1.response.Choice != YesNo.Yes)
			{
				return this.<Enact>g__RejectInsult|1_0(ref CS$<>8__locals1);
			}
			if (base.request.PrestigeWager > CS$<>8__locals1.response.Payment.Prestige)
			{
				return this.<Enact>g__RejectInsult|1_0(ref CS$<>8__locals1);
			}
			if (!base._currentTurn.AcceptPayment(this._player.Id, CS$<>8__locals1.response.Payment).successful)
			{
				return this.<Enact>g__RejectInsult|1_0(ref CS$<>8__locals1);
			}
			return this.<Enact>g__AcceptInsult|1_1(ref CS$<>8__locals1);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00056894 File Offset: 0x00054A94
		private void HasRejected(VileCalumnyRequest req, DiplomaticPairStatus diplomaticPairStatus)
		{
			diplomaticPairStatus.SetInsultRejected(this.TurnProcessContext);
			PaymentReceivedEvent gameEvent = this.TurnProcessContext.GivePrestige(this._player, req.PrestigeWager);
			base._currentTurn.AddGameEvent<PaymentReceivedEvent>(gameEvent);
			PendingDiplomacy_Grievance state = new PendingDiplomacy_Grievance(req.ScapegoatId, OrderTypes.Insult);
			diplomaticPairStatus.SetDiplomacyPending(this.TurnProcessContext, state);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000568EC File Offset: 0x00054AEC
		protected override Result Preview(VileCalumnyResponse response)
		{
			return base._currentTurn.AcceptPayment(this._player.Id, response.Payment);
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00056914 File Offset: 0x00054B14
		[CompilerGenerated]
		private Result <Enact>g__RejectInsult|1_0(ref VileCalumnyDecisionProcessor.<>c__DisplayClass1_0 A_1)
		{
			GameEvent gameEvent;
			Result result = GrievanceProcessor.BeginVendettaOrDuel(this.TurnProcessContext, this._player.Id, base.request.ScapegoatId, A_1.response.GrievanceResponse, out gameEvent);
			if (gameEvent != null)
			{
				base._currentTurn.AddGameEvent<GameEvent>(gameEvent);
			}
			A_1.diplomaticStatus.SetInsultRejected(this.TurnProcessContext);
			this._player.RemovePayment(A_1.response.Payment);
			VileCalumnyResponseEvent vileCalumnyResponseEvent = new VileCalumnyResponseEvent(this._player.Id, A_1.source.Id, base.request.ScapegoatId, YesNo.No, base._rules.MinDiplomacyOrderCooldown);
			vileCalumnyResponseEvent.OrderType = OrderTypes.Insult;
			vileCalumnyResponseEvent.GrievanceResponse = A_1.response.GrievanceResponse;
			base._currentTurn.AddGameEvent<VileCalumnyResponseEvent>(vileCalumnyResponseEvent);
			return result;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x000569E0 File Offset: 0x00054BE0
		[CompilerGenerated]
		private Result <Enact>g__AcceptInsult|1_1(ref VileCalumnyDecisionProcessor.<>c__DisplayClass1_0 A_1)
		{
			A_1.diplomaticStatus.SetInsultConceded(this.TurnProcessContext);
			PaymentReceivedEvent paymentReceivedEvent = base.GiveDiplomacyPayment(A_1.scapegoat, A_1.response.Payment, 1);
			PaymentReceivedEvent paymentReceivedEvent2 = base.GiveDiplomacyPayment(A_1.source, A_1.response.Payment, 1);
			if (paymentReceivedEvent == null || paymentReceivedEvent.Offering.IsEmpty || paymentReceivedEvent2 == null || paymentReceivedEvent2.Offering.IsEmpty)
			{
				return Result.Failure;
			}
			VileCalumnyResponseEvent vileCalumnyResponseEvent = new VileCalumnyResponseEvent(this._player.Id, A_1.source.Id, base.request.ScapegoatId, YesNo.Yes, base._rules.MinDiplomacyOrderCooldown);
			vileCalumnyResponseEvent.Prestige = base.request.PrestigeWager;
			vileCalumnyResponseEvent.AddChildEvent(new GameEvent[]
			{
				paymentReceivedEvent2,
				paymentReceivedEvent
			});
			vileCalumnyResponseEvent.OrderType = OrderTypes.VileCalumny;
			base._currentTurn.AddGameEvent<VileCalumnyResponseEvent>(vileCalumnyResponseEvent);
			return Result.Success;
		}
	}
}
