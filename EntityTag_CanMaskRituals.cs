using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B8 RID: 696
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CanMaskRituals : EntityTag
	{
		// Token: 0x06000D3B RID: 3387 RVA: 0x00034BE8 File Offset: 0x00032DE8
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CanMaskRituals entityTag_CanMaskRituals = new EntityTag_CanMaskRituals();
			base.DeepCloneEntityTagParts(entityTag_CanMaskRituals);
			clone = entityTag_CanMaskRituals;
		}
	}
}
