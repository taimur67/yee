using System;

namespace LoG
{
	// Token: 0x02000492 RID: 1170
	[Serializable]
	public class RankUpConflict : ActionConflict<RankUpConflict>
	{
		// Token: 0x060015E1 RID: 5601 RVA: 0x00051E19 File Offset: 0x00050019
		protected override bool ConflictsWith(RankUpConflict other)
		{
			return true;
		}
	}
}
