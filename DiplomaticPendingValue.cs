using System;

namespace LoG
{
	// Token: 0x02000427 RID: 1063
	[Flags]
	public enum DiplomaticPendingValue
	{
		// Token: 0x040009EC RID: 2540
		None = 0,
		// Token: 0x040009ED RID: 2541
		MakeDemand = 1,
		// Token: 0x040009EE RID: 2542
		Insult = 2,
		// Token: 0x040009EF RID: 2543
		Extort = 4,
		// Token: 0x040009F0 RID: 2544
		Humiliate = 8,
		// Token: 0x040009F1 RID: 2545
		Emissary = 16,
		// Token: 0x040009F2 RID: 2546
		Contract = 32,
		// Token: 0x040009F3 RID: 2547
		RequestToBeVassal = 64,
		// Token: 0x040009F4 RID: 2548
		Vendetta = 128,
		// Token: 0x040009F5 RID: 2549
		RequestToBeLord = 256,
		// Token: 0x040009F6 RID: 2550
		BloodFeud = 512,
		// Token: 0x040009F7 RID: 2551
		DraconicRazzia = 1024,
		// Token: 0x040009F8 RID: 2552
		ChainsOfAvarice = 2048,
		// Token: 0x040009F9 RID: 2553
		VileCalumny = 4096,
		// Token: 0x040009FA RID: 2554
		LureOfExcess = 8192
	}
}
