using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200029E RID: 670
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CheatLuckyRitualEffectRolls : EntityTag
	{
		// Token: 0x06000CFE RID: 3326 RVA: 0x000345FC File Offset: 0x000327FC
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CheatLuckyRitualEffectRolls entityTag_CheatLuckyRitualEffectRolls = new EntityTag_CheatLuckyRitualEffectRolls();
			base.DeepCloneEntityTagParts(entityTag_CheatLuckyRitualEffectRolls);
			clone = entityTag_CheatLuckyRitualEffectRolls;
		}
	}
}
