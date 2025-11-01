using System;

namespace LoG
{
	// Token: 0x02000532 RID: 1330
	public class PlayerDiplomaticDecision : PlayerDiplomaticAction<IDiplomaticDecisionRequest, IDiplomaticDecisionProcessor>
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x0005A6C4 File Offset: 0x000588C4
		public override int TargetId
		{
			get
			{
				return this.Request.AffectedPlayerId;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x0005A6D4 File Offset: 0x000588D4
		public override int ActorId
		{
			get
			{
				VileCalumnyRequest vileCalumnyRequest = this.Request as VileCalumnyRequest;
				if (vileCalumnyRequest != null)
				{
					return vileCalumnyRequest.ScapegoatId;
				}
				return this.Request.RequestingPlayerId;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x0005A702 File Offset: 0x00058902
		public override OrderTypes OrderType
		{
			get
			{
				return this.Request.OrderType;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x0005A70F File Offset: 0x0005890F
		public override bool IsDecision
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000BC6 RID: 3014
		public static readonly PlayerDiplomaticDecision Invalid = new PlayerDiplomaticDecision();

		// Token: 0x04000BC7 RID: 3015
		public DiplomaticDecisionResponse Response;
	}
}
