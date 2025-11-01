using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000417 RID: 1047
	[Serializable]
	public class TagLookupEntry
	{
		// Token: 0x040009C0 RID: 2496
		[JsonProperty]
		public AITag Tag;

		// Token: 0x040009C1 RID: 2497
		[JsonProperty]
		public ConfigRef<AIProfileStaticData> Config;
	}
}
