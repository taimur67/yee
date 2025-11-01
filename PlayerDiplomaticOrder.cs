using System;

namespace LoG
{
	// Token: 0x020003E6 RID: 998
	public class PlayerDiplomaticOrder : PlayerDiplomaticAction<DiplomaticOrder, IDiplomaticActionProcessor>
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0004B00D File Offset: 0x0004920D
		public override int TargetId
		{
			get
			{
				return this.Request.TargetID;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x0004B01A File Offset: 0x0004921A
		public override int ActorId
		{
			get
			{
				return this.Player.Id;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0004B027 File Offset: 0x00049227
		public override OrderTypes OrderType
		{
			get
			{
				return this.Request.OrderType;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0004B034 File Offset: 0x00049234
		public override bool IsDecision
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040008EB RID: 2283
		public static readonly PlayerDiplomaticOrder Invalid = new PlayerDiplomaticOrder();
	}
}
