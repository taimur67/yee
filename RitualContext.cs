using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200038A RID: 906
	[Serializable]
	public class RitualContext : PlayerContext
	{
		// Token: 0x0600113F RID: 4415 RVA: 0x00042BEF File Offset: 0x00040DEF
		[JsonConstructor]
		public RitualContext()
		{
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00042BF7 File Offset: 0x00040DF7
		public RitualContext(int playerId, string archfiendId, string ritualId)
		{
			this.ArchfiendId = archfiendId;
			this.PlayerId = playerId;
			this.RitualId = ritualId;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00042C14 File Offset: 0x00040E14
		public override string ToString()
		{
			return string.Format("{0} from {1}({2})", this.RitualId, this.ArchfiendId, this.PlayerId);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00042C38 File Offset: 0x00040E38
		public override void DeepClone(out ModifierContext modifierContext)
		{
			RitualContext ritualContext = new RitualContext
			{
				RitualId = this.RitualId.DeepClone()
			};
			base.DeepClonePlayerContextParts(ritualContext);
			modifierContext = ritualContext;
		}

		// Token: 0x040007F9 RID: 2041
		[JsonProperty]
		public string RitualId;
	}
}
