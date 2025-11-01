using System;

namespace LoG
{
	// Token: 0x020006A8 RID: 1704
	public class ActionPhase_DiplomaticResponse : ActionPhase
	{
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x0006C321 File Offset: 0x0006A521
		public override bool HideUIs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000CF8 RID: 3320
		public IDiplomaticDecisionRequest Request;

		// Token: 0x04000CF9 RID: 3321
		public DiplomaticDecisionResponse Response;
	}
}
