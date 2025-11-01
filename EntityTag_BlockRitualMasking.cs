using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A3 RID: 675
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_BlockRitualMasking : EntityTag
	{
		// Token: 0x06000D0E RID: 3342 RVA: 0x0003475C File Offset: 0x0003295C
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_BlockRitualMasking entityTag_BlockRitualMasking = new EntityTag_BlockRitualMasking
			{
				AllowDuringPhase = this.AllowDuringPhase.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneEntityTagParts(entityTag_BlockRitualMasking);
			clone = entityTag_BlockRitualMasking;
		}

		// Token: 0x040005CB RID: 1483
		[JsonProperty]
		public List<TurnPhase> AllowDuringPhase = new List<TurnPhase>();
	}
}
