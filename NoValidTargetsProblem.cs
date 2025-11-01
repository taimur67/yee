using System;

namespace LoG
{
	// Token: 0x020003F3 RID: 1011
	[Serializable]
	public class NoValidTargetsProblem : Problem
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0004D586 File Offset: 0x0004B786
		public override string DebugString
		{
			get
			{
				return "No valid targets.";
			}
		}
	}
}
