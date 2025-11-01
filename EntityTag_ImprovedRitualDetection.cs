using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A9 RID: 681
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_ImprovedRitualDetection : EntityTag
	{
		// Token: 0x06000D1B RID: 3355 RVA: 0x000348A0 File Offset: 0x00032AA0
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_ImprovedRitualDetection entityTag_ImprovedRitualDetection = new EntityTag_ImprovedRitualDetection
			{
				MaskingRollLimit = this.MaskingRollLimit
			};
			base.DeepCloneEntityTagParts(entityTag_ImprovedRitualDetection);
			clone = entityTag_ImprovedRitualDetection;
		}

		// Token: 0x040005CD RID: 1485
		[JsonProperty]
		[DefaultValue(0.5f)]
		public float MaskingRollLimit = 0.5f;
	}
}
