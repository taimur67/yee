using System;

namespace LoG
{
	// Token: 0x020003ED RID: 1005
	[Serializable]
	public class Problem : Result
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0004D46D File Offset: 0x0004B66D
		public sealed override bool successful
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0004D470 File Offset: 0x0004B670
		public override string LocKey
		{
			get
			{
				return "Result.DefaultProblem";
			}
		}
	}
}
