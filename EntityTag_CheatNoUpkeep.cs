using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A0 RID: 672
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_CheatNoUpkeep : EntityTag
	{
		// Token: 0x06000D02 RID: 3330 RVA: 0x0003464C File Offset: 0x0003284C
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_CheatNoUpkeep entityTag_CheatNoUpkeep = new EntityTag_CheatNoUpkeep
			{
				NoUpkeepFromRituals = this.NoUpkeepFromRituals
			};
			base.DeepCloneEntityTagParts(entityTag_CheatNoUpkeep);
			clone = entityTag_CheatNoUpkeep;
		}

		// Token: 0x040005C9 RID: 1481
		[JsonProperty]
		public bool NoUpkeepFromRituals;
	}
}
