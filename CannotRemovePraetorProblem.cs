using System;

namespace LoG
{
	// Token: 0x02000499 RID: 1177
	public class CannotRemovePraetorProblem : Problem
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00052048 File Offset: 0x00050248
		public override string DebugString
		{
			get
			{
				return "Cannot Remove Praetor";
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x0005204F File Offset: 0x0005024F
		public override string LocKey
		{
			get
			{
				return this.LocKeyScope + ".CannotRemovePraetor";
			}
		}
	}
}
