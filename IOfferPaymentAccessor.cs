using System;

namespace LoG
{
	// Token: 0x020005CA RID: 1482
	public interface IOfferPaymentAccessor : ISelectionAccessor
	{
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001BE2 RID: 7138
		// (set) Token: 0x06001BE3 RID: 7139
		Payment Pending { get; set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001BE4 RID: 7140
		// (set) Token: 0x06001BE5 RID: 7141
		Payment OfferPayment { get; set; }
	}
}
