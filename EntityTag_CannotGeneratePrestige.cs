using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A5 RID: 677
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CannotGeneratePrestige : EntityTag
	{
		// Token: 0x06000D12 RID: 3346 RVA: 0x000347C8 File Offset: 0x000329C8
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CannotGeneratePrestige entityTag_CannotGeneratePrestige = new EntityTag_CannotGeneratePrestige();
			base.DeepCloneEntityTagParts(entityTag_CannotGeneratePrestige);
			clone = entityTag_CannotGeneratePrestige;
		}
	}
}
