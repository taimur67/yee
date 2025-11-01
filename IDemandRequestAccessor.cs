using System;

namespace LoG
{
	// Token: 0x020004F6 RID: 1270
	public interface IDemandRequestAccessor
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001816 RID: 6166
		// (set) Token: 0x06001817 RID: 6167
		DemandOptions DemandOption { get; set; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001818 RID: 6168
		// (set) Token: 0x06001819 RID: 6169
		int NumCards { get; set; }
	}
}
