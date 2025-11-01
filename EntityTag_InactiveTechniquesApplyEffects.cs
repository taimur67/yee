using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B6 RID: 694
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_InactiveTechniquesApplyEffects : EntityTag
	{
		// Token: 0x06000D37 RID: 3383 RVA: 0x00034B98 File Offset: 0x00032D98
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_InactiveTechniquesApplyEffects entityTag_InactiveTechniquesApplyEffects = new EntityTag_InactiveTechniquesApplyEffects();
			base.DeepCloneEntityTagParts(entityTag_InactiveTechniquesApplyEffects);
			clone = entityTag_InactiveTechniquesApplyEffects;
		}
	}
}
