using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004EF RID: 1263
	[Serializable]
	public class PraetorBattleContext : GrievanceContext
	{
		// Token: 0x060017E8 RID: 6120 RVA: 0x000565B8 File Offset: 0x000547B8
		[JsonConstructor]
		public PraetorBattleContext()
		{
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x000565C0 File Offset: 0x000547C0
		public PraetorBattleContext(PraetorDuelData duel)
		{
			this.DuelData = duel;
			this.BasePrestigeWager = duel.BaseWager;
			this.PrestigeReward = duel.PrestigeReward;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x000565E7 File Offset: 0x000547E7
		public override void DeepClone(out GrievanceContext clone)
		{
			clone = new PraetorBattleContext
			{
				DuelData = this.DuelData.DeepClone<PraetorDuelData>()
			};
			base.DeepCloneGrievanceContextParts(clone);
		}

		// Token: 0x04000B6C RID: 2924
		[JsonProperty]
		public PraetorDuelData DuelData;
	}
}
