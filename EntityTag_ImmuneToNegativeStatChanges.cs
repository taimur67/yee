using System;
using System.Collections.Generic;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002A8 RID: 680
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_ImmuneToNegativeStatChanges : EntityTag
	{
		// Token: 0x06000D18 RID: 3352 RVA: 0x0003483D File Offset: 0x00032A3D
		public bool ProtectsAgainstStat(ArchfiendStat stat)
		{
			return this.Stats.Count == 0 || this.Stats.Contains(stat);
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0003485C File Offset: 0x00032A5C
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_ImmuneToNegativeStatChanges entityTag_ImmuneToNegativeStatChanges = new EntityTag_ImmuneToNegativeStatChanges
			{
				Stats = this.Stats.DeepClone()
			};
			base.DeepCloneEntityTagParts(entityTag_ImmuneToNegativeStatChanges);
			clone = entityTag_ImmuneToNegativeStatChanges;
		}

		// Token: 0x040005CC RID: 1484
		[JsonProperty]
		public List<ArchfiendStat> Stats = new List<ArchfiendStat>();
	}
}
