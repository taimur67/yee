using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000211 RID: 529
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PolicyVoteResult
	{
		// Token: 0x040004D0 RID: 1232
		[JsonProperty]
		public Dictionary<string, Dictionary<int, int>> Votes = new Dictionary<string, Dictionary<int, int>>();
	}
}
