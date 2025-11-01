using System;

namespace LoG
{
	// Token: 0x020001B2 RID: 434
	public class MoveVsPraetorData
	{
		// Token: 0x06000810 RID: 2064 RVA: 0x00025744 File Offset: 0x00023944
		public MoveVsPraetorData(float averagePower, float maxOverkill)
		{
			this.AveragePower = averagePower;
			this.MaxOverkill = maxOverkill;
		}

		// Token: 0x040003BD RID: 957
		public float AveragePower;

		// Token: 0x040003BE RID: 958
		public float MaxOverkill;
	}
}
