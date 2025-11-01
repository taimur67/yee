using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A4 RID: 676
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CannotBeNeutral : EntityTag
	{
		// Token: 0x06000D10 RID: 3344 RVA: 0x000347A0 File Offset: 0x000329A0
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CannotBeNeutral entityTag_CannotBeNeutral = new EntityTag_CannotBeNeutral();
			base.DeepCloneEntityTagParts(entityTag_CannotBeNeutral);
			clone = entityTag_CannotBeNeutral;
		}
	}
}
