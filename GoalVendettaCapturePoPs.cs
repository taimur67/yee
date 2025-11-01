using System;

namespace LoG
{
	// Token: 0x0200014D RID: 333
	public class GoalVendettaCapturePoPs : GoalVendettaBase
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x000213FC File Offset: 0x0001F5FC
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Vendetta_Capture_PoPs;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00021400 File Offset: 0x0001F600
		public override string ActionName
		{
			get
			{
				return "Goal - Vendetta: capture PoPs from " + base.Context.DebugName(this.ArchfiendID);
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002141D File Offset: 0x0001F61D
		public GoalVendettaCapturePoPs(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0002142C File Offset: 0x0001F62C
		public override void Prepare()
		{
			base.AddPrecondition(new WPOpportunisticSupport());
			base.AddPrecondition(new WPPoPStolenFrom(this.ArchfiendID));
			base.Prepare();
		}
	}
}
