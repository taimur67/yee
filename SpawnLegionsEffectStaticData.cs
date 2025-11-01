using System;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000745 RID: 1861
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SpawnLegionsEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F44 RID: 3908
		[JsonProperty]
		public int SpawnCount;

		// Token: 0x04000F45 RID: 3909
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> Legion;

		// Token: 0x04000F46 RID: 3910
		[JsonProperty]
		public ConfigRef<NeutralForceTurnModuleStaticData> Behaviour;
	}
}
