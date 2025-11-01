using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B7 RID: 695
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CanFrameRituals : EntityTag
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x00034BC0 File Offset: 0x00032DC0
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CanFrameRituals entityTag_CanFrameRituals = new EntityTag_CanFrameRituals();
			base.DeepCloneEntityTagParts(entityTag_CanFrameRituals);
			clone = entityTag_CanFrameRituals;
		}
	}
}
