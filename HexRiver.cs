using System;

namespace LoG
{
	// Token: 0x02000293 RID: 659
	public class HexRiver : HexIsland
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0003438E File Offset: 0x0003258E
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x00034396 File Offset: 0x00032596
		public bool Connected { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0003439F File Offset: 0x0003259F
		public override bool SuccessfullyCompleted
		{
			get
			{
				return this.Connected;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x000343A7 File Offset: 0x000325A7
		public override bool CanExpandFurther
		{
			get
			{
				return !this.Connected;
			}
		}
	}
}
