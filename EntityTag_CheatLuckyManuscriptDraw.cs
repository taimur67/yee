using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200029D RID: 669
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CheatLuckyManuscriptDraw : EntityTag
	{
		// Token: 0x06000CFC RID: 3324 RVA: 0x000345D4 File Offset: 0x000327D4
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CheatLuckyManuscriptDraw entityTag_CheatLuckyManuscriptDraw = new EntityTag_CheatLuckyManuscriptDraw();
			base.DeepCloneEntityTagParts(entityTag_CheatLuckyManuscriptDraw);
			clone = entityTag_CheatLuckyManuscriptDraw;
		}
	}
}
