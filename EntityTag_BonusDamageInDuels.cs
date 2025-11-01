using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B4 RID: 692
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_BonusDamageInDuels : EntityTag
	{
		// Token: 0x06000D33 RID: 3379 RVA: 0x00034B3C File Offset: 0x00032D3C
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_BonusDamageInDuels entityTag_BonusDamageInDuels = new EntityTag_BonusDamageInDuels
			{
				BonusAmount = this.BonusAmount
			};
			base.DeepCloneEntityTagParts(entityTag_BonusDamageInDuels);
			clone = entityTag_BonusDamageInDuels;
		}

		// Token: 0x040005D8 RID: 1496
		[JsonProperty]
		public int BonusAmount;
	}
}
