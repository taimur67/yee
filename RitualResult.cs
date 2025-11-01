using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000282 RID: 642
	[Serializable]
	public class RitualResult
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x000317BB File Offset: 0x0002F9BB
		// (set) Token: 0x06000C81 RID: 3201 RVA: 0x000317C3 File Offset: 0x0002F9C3
		[JsonProperty]
		public bool Succeeded { get; private set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x000317CC File Offset: 0x0002F9CC
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x000317D4 File Offset: 0x0002F9D4
		[JsonProperty]
		public bool Identified { get; private set; }

		// Token: 0x06000C84 RID: 3204 RVA: 0x000317DD File Offset: 0x0002F9DD
		public RitualResult(bool succeeded, bool identified)
		{
			this.Succeeded = succeeded;
			this.Identified = identified;
		}
	}
}
