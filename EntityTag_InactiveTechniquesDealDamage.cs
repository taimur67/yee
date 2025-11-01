using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B5 RID: 693
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_InactiveTechniquesDealDamage : EntityTag
	{
		// Token: 0x06000D35 RID: 3381 RVA: 0x00034B70 File Offset: 0x00032D70
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_InactiveTechniquesDealDamage entityTag_InactiveTechniquesDealDamage = new EntityTag_InactiveTechniquesDealDamage();
			base.DeepCloneEntityTagParts(entityTag_InactiveTechniquesDealDamage);
			clone = entityTag_InactiveTechniquesDealDamage;
		}
	}
}
