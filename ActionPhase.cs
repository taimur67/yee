using System;

namespace LoG
{
	// Token: 0x020006A5 RID: 1701
	public abstract class ActionPhase
	{
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001F49 RID: 8009 RVA: 0x0006C2D0 File Offset: 0x0006A4D0
		// (set) Token: 0x06001F4A RID: 8010 RVA: 0x0006C2D8 File Offset: 0x0006A4D8
		public ActionPhaseType ActionPhaseType { get; protected set; }

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x0006C2E1 File Offset: 0x0006A4E1
		public virtual bool HideUIs
		{
			get
			{
				return true;
			}
		}
	}
}
