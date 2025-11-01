using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000451 RID: 1105
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GameParam_Data : IdentifiableStaticData
	{
		// Token: 0x060014FA RID: 5370 RVA: 0x0004FA10 File Offset: 0x0004DC10
		public bool TryGetFeatureFrequencyData(TerrainType terrainType, out ConfigRef<FeatureData> frequencyData)
		{
			ConfigRef<FeatureData> configRef;
			switch (terrainType)
			{
			case TerrainType.Mountain:
				configRef = this.MountainsPreset;
				goto IL_6A;
			case TerrainType.Ravine:
				configRef = this.RavinesPreset;
				goto IL_6A;
			case TerrainType.Swamp:
				configRef = this.SwampsPreset;
				goto IL_6A;
			case TerrainType.Lava:
				configRef = this.LavaPreset;
				goto IL_6A;
			case TerrainType.Vent:
				configRef = this.VentsPreset;
				goto IL_6A;
			case TerrainType.Ruin:
				configRef = this.RuinsPreset;
				goto IL_6A;
			}
			configRef = null;
			IL_6A:
			frequencyData = configRef;
			return frequencyData != null;
		}

		// Token: 0x04000A67 RID: 2663
		[JsonProperty]
		public ConfigRef<FeatureData> MountainsPreset;

		// Token: 0x04000A68 RID: 2664
		[JsonProperty]
		public ConfigRef<FeatureData> RavinesPreset;

		// Token: 0x04000A69 RID: 2665
		[JsonProperty]
		public ConfigRef<RiverData> RiversPreset;

		// Token: 0x04000A6A RID: 2666
		[JsonProperty]
		public ConfigRef<FeatureData> VentsPreset;

		// Token: 0x04000A6B RID: 2667
		[JsonProperty]
		public ConfigRef<FeatureData> SwampsPreset;

		// Token: 0x04000A6C RID: 2668
		[JsonProperty]
		public ConfigRef<FeatureData> RuinsPreset;

		// Token: 0x04000A6D RID: 2669
		[JsonProperty]
		public ConfigRef<FeatureData> LavaPreset;

		// Token: 0x04000A6E RID: 2670
		[JsonProperty]
		public ConfigRef<PointOfInterestStaticData> PointOfInterest;
	}
}
