using System;

namespace LoG
{
	// Token: 0x020005CC RID: 1484
	public interface IGrievanceAccessor : ISelectionAccessor
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001BE8 RID: 7144 RVA: 0x00060C7E File Offset: 0x0005EE7E
		bool PrivateGrievance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001BE9 RID: 7145
		// (set) Token: 0x06001BEA RID: 7146
		GrievanceContext GrievanceResponse { get; set; }
	}
}
