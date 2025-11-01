using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005EA RID: 1514
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderSendEmissary : DiplomaticOrder, IOfferPaymentAccessor, ISelectionAccessor
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001C5F RID: 7263 RVA: 0x00061EDA File Offset: 0x000600DA
		public Cost MinimumOffering
		{
			get
			{
				return new Cost(1);
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x00061EE2 File Offset: 0x000600E2
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.SendEmissary;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001C61 RID: 7265 RVA: 0x00061EE5 File Offset: 0x000600E5
		// (set) Token: 0x06001C62 RID: 7266 RVA: 0x00061EED File Offset: 0x000600ED
		[JsonIgnore]
		public Payment OfferPayment
		{
			get
			{
				return this._offerPayment;
			}
			set
			{
				this._offerPayment = value;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001C63 RID: 7267 RVA: 0x00061EF6 File Offset: 0x000600F6
		// (set) Token: 0x06001C64 RID: 7268 RVA: 0x00061EFE File Offset: 0x000600FE
		[JsonIgnore]
		public Payment Pending
		{
			get
			{
				return this._pending;
			}
			set
			{
				this._pending = value;
			}
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00061F07 File Offset: 0x00060107
		[JsonConstructor]
		public OrderSendEmissary()
		{
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00061F25 File Offset: 0x00060125
		public OrderSendEmissary(int targetID, Payment payment) : base(targetID, payment)
		{
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x00061F45 File Offset: 0x00060145
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			yield return new ActionPhase_Tribute(new Action<Payment>(this.SetOffering), this.MinimumOffering);
			yield break;
			yield break;
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x00061F6A File Offset: 0x0006016A
		public void SetOffering(Payment payment)
		{
			this.OfferPayment = payment;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x00061F73 File Offset: 0x00060173
		public override IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			yield return OrderTypes.RequestToBeVassalizedByTarget;
			yield return OrderTypes.RequestToBeBloodLordOfTarget;
			yield break;
		}

		// Token: 0x04000C7A RID: 3194
		[JsonProperty]
		private Payment _offerPayment = new Payment();

		// Token: 0x04000C7B RID: 3195
		[JsonProperty]
		private Payment _pending = new Payment();
	}
}
