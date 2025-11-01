using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004E6 RID: 1254
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SendEmissaryDecisionRequest : DiplomaticDecisionRequest<SendEmissaryDecisionResponse>, IOfferPaymentAccessor, ISelectionAccessor, IArmisticeAccessor
	{
		// Token: 0x0600179D RID: 6045 RVA: 0x00055ACC File Offset: 0x00053CCC
		[JsonConstructor]
		protected SendEmissaryDecisionRequest()
		{
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00055AEA File Offset: 0x00053CEA
		public SendEmissaryDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00055B09 File Offset: 0x00053D09
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.EmissarySentRecipient;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060017A0 RID: 6048 RVA: 0x00055B0D File Offset: 0x00053D0D
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.SendEmissary;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x00055B10 File Offset: 0x00053D10
		// (set) Token: 0x060017A2 RID: 6050 RVA: 0x00055B18 File Offset: 0x00053D18
		[JsonProperty]
		public Payment Pending { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x00055B21 File Offset: 0x00053D21
		// (set) Token: 0x060017A4 RID: 6052 RVA: 0x00055B29 File Offset: 0x00053D29
		[JsonProperty]
		public Payment OfferPayment { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060017A5 RID: 6053 RVA: 0x00055B32 File Offset: 0x00053D32
		// (set) Token: 0x060017A6 RID: 6054 RVA: 0x00055B3A File Offset: 0x00053D3A
		[JsonProperty]
		public int ArmisticeLength { get; set; }

		// Token: 0x04000B58 RID: 2904
		[JsonProperty]
		public Cost Cost = new Cost();

		// Token: 0x04000B59 RID: 2905
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
