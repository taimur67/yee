using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004FB RID: 1275
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CannotJoinCrusade : EntityTag
	{
		// Token: 0x0600182E RID: 6190 RVA: 0x00056D20 File Offset: 0x00054F20
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CannotJoinCrusade entityTag_CannotJoinCrusade = new EntityTag_CannotJoinCrusade();
			base.DeepCloneEntityTagParts(entityTag_CannotJoinCrusade);
			clone = entityTag_CannotJoinCrusade;
		}
	}
}
