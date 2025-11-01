using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200042B RID: 1067
	[Serializable]
	public class GamePropertiesStaticData : IdentifiableStaticData
	{
		// Token: 0x04000A11 RID: 2577
		[JsonProperty]
		public ConfigRef<LegionLevelTable> LegionLevelTable;
	}
}
