using System;

namespace LoG
{
	// Token: 0x0200014B RID: 331
	public class GoalVendettaCaptureCantons : GoalVendettaBase
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x000212C8 File Offset: 0x0001F4C8
		protected override bool IsFulfilledByMovingOutOfDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x000212CB File Offset: 0x0001F4CB
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Vendetta_Capture_Cantons;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x000212CF File Offset: 0x0001F4CF
		public override string ActionName
		{
			get
			{
				return "Goal - Vendetta: capture cantons from " + base.Context.DebugName(this.ArchfiendID);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000212EC File Offset: 0x0001F4EC
		public GoalVendettaCaptureCantons(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000212FB File Offset: 0x0001F4FB
		public override void Prepare()
		{
			base.AddPrecondition(new WPOpportunisticSupport());
			base.AddPrecondition(new WPStolenCanton(this.ArchfiendID));
			base.Prepare();
		}
	}
}
