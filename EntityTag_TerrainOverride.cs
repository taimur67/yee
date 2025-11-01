using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002AF RID: 687
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class EntityTag_TerrainOverride : EntityTag
	{
		// Token: 0x06000D28 RID: 3368 RVA: 0x00034A49 File Offset: 0x00032C49
		protected void DeepCloneTerrainOverrideParts(EntityTag_TerrainOverride clone)
		{
			base.DeepCloneEntityTagParts(clone);
			clone.Type = this.Type;
		}

		// Token: 0x040005D6 RID: 1494
		[JsonProperty]
		public TerrainType Type;
	}
}
