using System;
using System.ComponentModel;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000450 RID: 1104
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GameDuration_Data : IdentifiableStaticData
	{
		// Token: 0x04000A66 RID: 2662
		[JsonProperty]
		[DefaultValue(50)]
		public int Duration = 50;
	}
}
