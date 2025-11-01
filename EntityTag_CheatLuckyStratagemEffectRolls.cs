using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200029F RID: 671
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CheatLuckyStratagemEffectRolls : EntityTag
	{
		// Token: 0x06000D00 RID: 3328 RVA: 0x00034624 File Offset: 0x00032824
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CheatLuckyStratagemEffectRolls entityTag_CheatLuckyStratagemEffectRolls = new EntityTag_CheatLuckyStratagemEffectRolls();
			base.DeepCloneEntityTagParts(entityTag_CheatLuckyStratagemEffectRolls);
			clone = entityTag_CheatLuckyStratagemEffectRolls;
		}
	}
}
