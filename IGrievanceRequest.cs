using System;

namespace LoG
{
	// Token: 0x020004E9 RID: 1257
	public interface IGrievanceRequest
	{
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060017B7 RID: 6071
		// (set) Token: 0x060017B8 RID: 6072
		int InstigatorPlayerId { get; set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060017B9 RID: 6073
		// (set) Token: 0x060017BA RID: 6074
		int GrievanceTargetPlayerId { get; set; }
	}
}
