using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B0 RID: 688
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_TerrainMoveCostOverride : EntityTag_TerrainOverride
	{
		// Token: 0x06000D2A RID: 3370 RVA: 0x00034A66 File Offset: 0x00032C66
		public override string ToString()
		{
			return string.Format(" {0} => {1}:{2}", base.ToString(), this.Type, this.MoveCostType);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00034A90 File Offset: 0x00032C90
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_TerrainMoveCostOverride entityTag_TerrainMoveCostOverride = new EntityTag_TerrainMoveCostOverride
			{
				MoveCostType = this.MoveCostType
			};
			base.DeepCloneTerrainOverrideParts(entityTag_TerrainMoveCostOverride);
			clone = entityTag_TerrainMoveCostOverride;
		}

		// Token: 0x040005D7 RID: 1495
		[JsonProperty]
		public MoveCostType MoveCostType;
	}
}
