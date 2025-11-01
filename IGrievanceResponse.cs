using System;

namespace LoG
{
	// Token: 0x020004EA RID: 1258
	public interface IGrievanceResponse
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060017BB RID: 6075
		// (set) Token: 0x060017BC RID: 6076
		Payment Payment { get; set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060017BD RID: 6077
		// (set) Token: 0x060017BE RID: 6078
		GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060017BF RID: 6079
		// (set) Token: 0x060017C0 RID: 6080
		YesNo Choice { get; set; }
	}
}
