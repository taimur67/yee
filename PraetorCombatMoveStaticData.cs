using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003D3 RID: 979
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveStaticData : StaticDataEntity
	{
		// Token: 0x040008D4 RID: 2260
		[JsonProperty]
		public ConfigRef<PraetorCombatMoveStyle> TechniqueType;

		// Token: 0x040008D5 RID: 2261
		[JsonProperty]
		public List<PraetorCombatMoveEffectData> Effects = new List<PraetorCombatMoveEffectData>();

		// Token: 0x040008D6 RID: 2262
		[JsonProperty]
		public int UpgradePower;
	}
}
