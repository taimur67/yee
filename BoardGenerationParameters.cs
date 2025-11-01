using System;
using System.ComponentModel;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200028D RID: 653
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BoardGenerationParameters
	{
		// Token: 0x040005A7 RID: 1447
		[JsonProperty]
		public ConfigRef<GameParam_Data> MapPreset;

		// Token: 0x040005A8 RID: 1448
		[JsonProperty]
		[DefaultValue(12)]
		public int BoardRows = 12;

		// Token: 0x040005A9 RID: 1449
		[JsonProperty]
		[DefaultValue(12)]
		public int BoardColumns = 12;

		// Token: 0x040005AA RID: 1450
		[JsonProperty]
		[DefaultValue(2)]
		public int MinPlacesOfPower = 2;

		// Token: 0x040005AB RID: 1451
		[JsonProperty]
		[DefaultValue(4)]
		public int MaxPlacesOfPower = 4;
	}
}
