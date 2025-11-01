using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200029C RID: 668
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CheatLuckyEventDraw : EntityTag
	{
		// Token: 0x06000CFA RID: 3322 RVA: 0x000345AC File Offset: 0x000327AC
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CheatLuckyEventDraw entityTag_CheatLuckyEventDraw = new EntityTag_CheatLuckyEventDraw();
			base.DeepCloneEntityTagParts(entityTag_CheatLuckyEventDraw);
			clone = entityTag_CheatLuckyEventDraw;
		}
	}
}
