using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A6 RID: 678
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CannotRegenerateHealth : EntityTag
	{
		// Token: 0x06000D14 RID: 3348 RVA: 0x000347F0 File Offset: 0x000329F0
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CannotRegenerateHealth entityTag_CannotRegenerateHealth = new EntityTag_CannotRegenerateHealth();
			base.DeepCloneEntityTagParts(entityTag_CannotRegenerateHealth);
			clone = entityTag_CannotRegenerateHealth;
		}
	}
}
