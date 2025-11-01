using System;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020004E8 RID: 1256
	public class SendEmissaryDecisionProcessor : DiplomaticDecisionProcessor<SendEmissaryDecisionRequest, SendEmissaryDecisionResponse>
	{
		// Token: 0x060017AE RID: 6062 RVA: 0x00055BE0 File Offset: 0x00053DE0
		protected override SendEmissaryDecisionResponse GenerateTypedFallbackResponse()
		{
			return new SendEmissaryDecisionResponse
			{
				Choice = YesNo.No
			};
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00055BEE File Offset: 0x00053DEE
		protected override Result EnactImmediate(DiplomaticDecisionResponse response)
		{
			if (response.Choice == YesNo.Yes)
			{
				this._paymentEvent = this.TurnProcessContext.GivePayment(this._player, base.request.OfferPayment, null);
			}
			return Result.Success;
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00055C24 File Offset: 0x00053E24
		protected override Result Enact(SendEmissaryDecisionResponse response)
		{
			SendEmissaryDecisionProcessor.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.RequestingPlayerId);
			CS$<>8__locals1.opponent = base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null);
			DiplomaticState diplomaticState = CS$<>8__locals1.diplomaticStatus.DiplomaticState;
			CS$<>8__locals1.emissaryState = (diplomaticState as PendingDiplomacy_Emissary);
			if (CS$<>8__locals1.emissaryState == null)
			{
				return Result.Failure;
			}
			if (response.Choice == YesNo.No)
			{
				return this.<Enact>g__Reject|3_1(ref CS$<>8__locals1);
			}
			if (response.Choice == YesNo.StrongNo)
			{
				return this.<Enact>g__StrongReject|3_0(ref CS$<>8__locals1);
			}
			return this.<Enact>g__Accept|3_2(ref CS$<>8__locals1);
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00055CD4 File Offset: 0x00053ED4
		private Result PreviewPayment(SendEmissaryDecisionResponse response)
		{
			YesNo choice = response.Choice;
			if (choice != YesNo.Yes)
			{
				if (choice == YesNo.StrongNo)
				{
					this._player.GivePrestige(base.request.PrestigeWager);
				}
			}
			else
			{
				this._player.GivePayment(base.request.OfferPayment);
			}
			return Result.Success;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00055D26 File Offset: 0x00053F26
		protected override Result Preview(SendEmissaryDecisionResponse response)
		{
			return this.PreviewPayment(response);
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00055D38 File Offset: 0x00053F38
		[CompilerGenerated]
		private Result <Enact>g__StrongReject|3_0(ref SendEmissaryDecisionProcessor.<>c__DisplayClass3_0 A_1)
		{
			A_1.diplomaticStatus.SetEmissaryStrongRejected(this.TurnProcessContext);
			PaymentReceivedEvent paymentReceivedEvent = this.TurnProcessContext.GivePrestige(this._player, base.request.PrestigeWager);
			PaymentReceivedEvent paymentReceivedEvent2 = this.TurnProcessContext.GivePayment(A_1.opponent, base.request.OfferPayment, null);
			StatModifier additionalPrestigeReward = new StatModifier(base.request.PrestigeWager, new VendettaWagerContext("Emissary Execution"), ModifierTarget.ValueOffset);
			A_1.diplomaticStatus.SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_Grievance(this._player.Id, OrderTypes.SendEmissary)
			{
				AdditionalPrestigeReward = additionalPrestigeReward
			});
			SendEmissaryResponseEvent sendEmissaryResponseEvent = new SendEmissaryResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.StrongNo, Payment.Empty, base.request.ArmisticeLength);
			sendEmissaryResponseEvent.OrderType = OrderTypes.SendEmissary;
			sendEmissaryResponseEvent.AddChildEvent(new GameEvent[]
			{
				paymentReceivedEvent,
				paymentReceivedEvent2
			});
			base._currentTurn.AddGameEvent<SendEmissaryResponseEvent>(sendEmissaryResponseEvent);
			return Result.Success;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00055E30 File Offset: 0x00054030
		[CompilerGenerated]
		private Result <Enact>g__Reject|3_1(ref SendEmissaryDecisionProcessor.<>c__DisplayClass3_0 A_1)
		{
			A_1.diplomaticStatus.SetEmissaryRejected(this.TurnProcessContext);
			PaymentReceivedEvent ev = this.TurnProcessContext.GivePayment(A_1.opponent, base.request.OfferPayment, null);
			SendEmissaryResponseEvent sendEmissaryResponseEvent = new SendEmissaryResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.No, Payment.Empty, base.request.ArmisticeLength);
			sendEmissaryResponseEvent.OrderType = OrderTypes.SendEmissary;
			sendEmissaryResponseEvent.AddChildEvent<PaymentReceivedEvent>(ev);
			base._currentTurn.AddGameEvent<SendEmissaryResponseEvent>(sendEmissaryResponseEvent);
			return Result.Success;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00055EBC File Offset: 0x000540BC
		[CompilerGenerated]
		private Result <Enact>g__Accept|3_2(ref SendEmissaryDecisionProcessor.<>c__DisplayClass3_0 A_1)
		{
			A_1.diplomaticStatus.SetEmissaryAccepted(this.TurnProcessContext, A_1.emissaryState);
			SendEmissaryResponseEvent sendEmissaryResponseEvent = new SendEmissaryResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.Yes, base.request.OfferPayment, base.request.ArmisticeLength);
			sendEmissaryResponseEvent.OrderType = OrderTypes.SendEmissary;
			sendEmissaryResponseEvent.AddChildEvent(this._paymentEvent);
			base._currentTurn.AddGameEvent<SendEmissaryResponseEvent>(sendEmissaryResponseEvent);
			return Result.Success;
		}

		// Token: 0x04000B5E RID: 2910
		private GameEvent _paymentEvent;
	}
}
