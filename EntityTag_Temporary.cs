using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002AE RID: 686
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_Temporary : EntityTag
	{
		// Token: 0x06000D26 RID: 3366 RVA: 0x00034A24 File Offset: 0x00032C24
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_Temporary entityTag_Temporary = new EntityTag_Temporary();
			base.DeepCloneEntityTagParts(entityTag_Temporary);
			clone = entityTag_Temporary;
		}
	}
}
