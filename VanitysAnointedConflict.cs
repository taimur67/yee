using System;

namespace LoG
{
	// Token: 0x02000494 RID: 1172
	[Serializable]
	public class VanitysAnointedConflict : ActionConflict<VanitysAnointedConflict>
	{
		// Token: 0x060015E6 RID: 5606 RVA: 0x00051E4E File Offset: 0x0005004E
		public VanitysAnointedConflict()
		{
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00051E56 File Offset: 0x00050056
		public VanitysAnointedConflict(Identifier praetor)
		{
			this.TargetPraetor = praetor;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00051E65 File Offset: 0x00050065
		protected override bool ConflictsWith(VanitysAnointedConflict other)
		{
			return other.TargetPraetor == this.TargetPraetor;
		}

		// Token: 0x04000B03 RID: 2819
		public Identifier TargetPraetor;
	}
}
