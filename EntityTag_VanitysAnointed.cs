using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B3 RID: 691
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_VanitysAnointed : EntityTag
	{
		// Token: 0x06000D31 RID: 3377 RVA: 0x00034B14 File Offset: 0x00032D14
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_VanitysAnointed entityTag_VanitysAnointed = new EntityTag_VanitysAnointed();
			base.DeepCloneEntityTagParts(entityTag_VanitysAnointed);
			clone = entityTag_VanitysAnointed;
		}
	}
}
