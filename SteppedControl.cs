using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006A6 RID: 1702
	public abstract class SteppedControl : ActionPhase
	{
		// Token: 0x06001F4D RID: 8013
		public abstract List<ActionPhase> GetSteps();
	}
}
