using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B1 RID: 689
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CannotBeReassigned : EntityTag
	{
		// Token: 0x06000D2D RID: 3373 RVA: 0x00034AC4 File Offset: 0x00032CC4
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CannotBeReassigned entityTag_CannotBeReassigned = new EntityTag_CannotBeReassigned();
			base.DeepCloneEntityTagParts(entityTag_CannotBeReassigned);
			clone = entityTag_CannotBeReassigned;
		}
	}
}
