using System;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200045C RID: 1116
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class NeutralForcePopulationData : StaticDataEntity
	{
		// Token: 0x04000A8A RID: 2698
		[JsonProperty]
		[DefaultValue(0.05f)]
		public float InitialSpawnsPerNeutralCanton = 0.05f;

		// Token: 0x04000A8B RID: 2699
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> StartingNeutralForce;

		// Token: 0x04000A8C RID: 2700
		[JsonProperty]
		public ConfigRef<NeutralForceTurnModuleStaticData> StartingNeutralForceBehaviour;
	}
}
