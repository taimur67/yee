using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200044C RID: 1100
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BoardGenerationParametersPreset : IdentifiableStaticData
	{
		// Token: 0x04000A5E RID: 2654
		[JsonProperty]
		public BoardGenerationParameters Parameters;
	}
}
