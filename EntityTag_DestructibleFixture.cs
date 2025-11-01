using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A7 RID: 679
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_DestructibleFixture : EntityTag
	{
		// Token: 0x06000D16 RID: 3350 RVA: 0x00034818 File Offset: 0x00032A18
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_DestructibleFixture entityTag_DestructibleFixture = new EntityTag_DestructibleFixture();
			base.DeepCloneEntityTagParts(entityTag_DestructibleFixture);
			clone = entityTag_DestructibleFixture;
		}
	}
}
