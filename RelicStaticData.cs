using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200045D RID: 1117
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RelicStaticData : GameItemStaticData
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0004FC07 File Offset: 0x0004DE07
		public override GameItemCategory GameItemCategory
		{
			get
			{
				return GameItemCategory.Relic;
			}
		}

		// Token: 0x04000A8D RID: 2701
		[JsonProperty]
		public int RelicValue;

		// Token: 0x04000A8E RID: 2702
		[JsonProperty]
		public RelicType Type;
	}
}
