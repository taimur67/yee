using System;
using System.ComponentModel;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200044D RID: 1101
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BoardSize_GenerationData : IdentifiableStaticData
	{
		// Token: 0x04000A5F RID: 2655
		[JsonProperty]
		public int X;

		// Token: 0x04000A60 RID: 2656
		[JsonProperty]
		public int Y;

		// Token: 0x04000A61 RID: 2657
		[JsonProperty]
		[DefaultValue(2)]
		public int MinPlayers = 2;

		// Token: 0x04000A62 RID: 2658
		[JsonProperty]
		[DefaultValue(4)]
		public int MaxPlayers = 4;
	}
}
