using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B2 RID: 690
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_AnointedOfAsh : EntityTag
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x00034AEC File Offset: 0x00032CEC
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_AnointedOfAsh entityTag_AnointedOfAsh = new EntityTag_AnointedOfAsh();
			base.DeepCloneEntityTagParts(entityTag_AnointedOfAsh);
			clone = entityTag_AnointedOfAsh;
		}
	}
}
