using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000404 RID: 1028
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class NeutralForceTurnModuleStaticData : TurnModuleStaticData
	{
		// Token: 0x04000910 RID: 2320
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> GamePieceRef;
	}
}
