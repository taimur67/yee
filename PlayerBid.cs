using System;

namespace LoG
{
	// Token: 0x020003DF RID: 991
	public struct PlayerBid
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x0004A57C File Offset: 0x0004877C
		public bool IsValid
		{
			get
			{
				return this.Player != null && this.Payment != null;
			}
		}

		// Token: 0x040008E0 RID: 2272
		public static readonly PlayerBid Invalid;

		// Token: 0x040008E1 RID: 2273
		public PlayerState Player;

		// Token: 0x040008E2 RID: 2274
		public Payment Payment;

		// Token: 0x040008E3 RID: 2275
		public int OrderSlotIndex;
	}
}
