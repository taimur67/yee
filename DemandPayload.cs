using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000634 RID: 1588
	[Serializable]
	public class DemandPayload
	{
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0006563F File Offset: 0x0006383F
		[JsonIgnore]
		public int Total
		{
			get
			{
				return this.Tokens.Count + this.Manuscripts.Count;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x00065658 File Offset: 0x00063858
		[JsonIgnore]
		public IEnumerable<int> ItemIds
		{
			get
			{
				return (from x in this.Tokens
				select x.Id).Concat(from x in this.Manuscripts
				select (int)x.Id);
			}
		}

		// Token: 0x04000C89 RID: 3209
		public List<ResourceNFT> Tokens = new List<ResourceNFT>();

		// Token: 0x04000C8A RID: 3210
		public List<Manuscript> Manuscripts = new List<Manuscript>();
	}
}
