using System;

namespace LoG
{
	// Token: 0x0200048F RID: 1167
	[Serializable]
	public class HellsMawConflict : ActionConflict<HellsMawConflict>
	{
		// Token: 0x060015DA RID: 5594 RVA: 0x00051DC8 File Offset: 0x0004FFC8
		protected override bool ConflictsWith(HellsMawConflict other)
		{
			return true;
		}
	}
}
