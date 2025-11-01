using System;

namespace LoG
{
	// Token: 0x02000493 RID: 1171
	[Serializable]
	public class SummonLegionConflict : ActionConflict<SummonLegionConflict>
	{
		// Token: 0x060015E3 RID: 5603 RVA: 0x00051E24 File Offset: 0x00050024
		public SummonLegionConflict()
		{
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00051E2C File Offset: 0x0005002C
		public SummonLegionConflict(HexCoord hexCoord)
		{
			this.HexCoord = hexCoord;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00051E3B File Offset: 0x0005003B
		protected override bool ConflictsWith(SummonLegionConflict other)
		{
			return other.HexCoord == this.HexCoord;
		}

		// Token: 0x04000B02 RID: 2818
		public HexCoord HexCoord;
	}
}
