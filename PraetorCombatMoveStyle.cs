using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003D4 RID: 980
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveStyle : IdentifiableStaticData
	{
		// Token: 0x040008D7 RID: 2263
		[JsonProperty]
		public List<PraetorCombatMoveEffectData> Effects = new List<PraetorCombatMoveEffectData>();
	}
}
