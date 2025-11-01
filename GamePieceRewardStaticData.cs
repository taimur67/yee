using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000449 RID: 1097
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceRewardStaticData : StaticDataEntity
	{
		// Token: 0x04000A58 RID: 2648
		[JsonProperty]
		public GamePieceModifierStaticData Reward;
	}
}
