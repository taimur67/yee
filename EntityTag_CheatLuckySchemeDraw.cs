using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200029B RID: 667
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CheatLuckySchemeDraw : EntityTag
	{
		// Token: 0x06000CF8 RID: 3320 RVA: 0x00034584 File Offset: 0x00032784
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CheatLuckySchemeDraw entityTag_CheatLuckySchemeDraw = new EntityTag_CheatLuckySchemeDraw();
			base.DeepCloneEntityTagParts(entityTag_CheatLuckySchemeDraw);
			clone = entityTag_CheatLuckySchemeDraw;
		}
	}
}
