using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200029A RID: 666
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CheatLuckyTributeDraw : EntityTag
	{
		// Token: 0x06000CF6 RID: 3318 RVA: 0x0003455C File Offset: 0x0003275C
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CheatLuckyTributeDraw entityTag_CheatLuckyTributeDraw = new EntityTag_CheatLuckyTributeDraw();
			base.DeepCloneEntityTagParts(entityTag_CheatLuckyTributeDraw);
			clone = entityTag_CheatLuckyTributeDraw;
		}
	}
}
