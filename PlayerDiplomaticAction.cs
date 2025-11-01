using System;

namespace LoG
{
	// Token: 0x020003E4 RID: 996
	public abstract class PlayerDiplomaticAction
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x0004AFE8 File Offset: 0x000491E8
		public bool IsValid
		{
			get
			{
				return this.Player != null && this.Payment != null;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060013B0 RID: 5040
		public abstract int TargetId { get; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060013B1 RID: 5041
		public abstract int ActorId { get; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060013B2 RID: 5042
		public abstract OrderTypes OrderType { get; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060013B3 RID: 5043
		public abstract bool IsDecision { get; }

		// Token: 0x040008E6 RID: 2278
		public PlayerState Player;

		// Token: 0x040008E7 RID: 2279
		public Payment Payment;

		// Token: 0x040008E8 RID: 2280
		public int OrderSlotIndex;
	}
}
