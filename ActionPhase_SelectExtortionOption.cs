using System;

namespace LoG
{
	// Token: 0x020006B4 RID: 1716
	public class ActionPhase_SelectExtortionOption : ActionPhase
	{
		// Token: 0x06001F65 RID: 8037 RVA: 0x0006C404 File Offset: 0x0006A604
		public ActionPhase_SelectExtortionOption(Action<DemandOptions> setTarget)
		{
			this.SetTarget = setTarget;
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x0006C413 File Offset: 0x0006A613
		public Action<DemandOptions> SetTarget { get; }
	}
}
