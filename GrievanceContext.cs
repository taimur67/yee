using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004EE RID: 1262
	[Serializable]
	public abstract class GrievanceContext : IDeepClone<GrievanceContext>
	{
		// Token: 0x060017E3 RID: 6115 RVA: 0x00056561 File Offset: 0x00054761
		public virtual int GetTotalTarget()
		{
			return 1;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00056564 File Offset: 0x00054764
		public virtual int GetDuration()
		{
			return 3;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00056567 File Offset: 0x00054767
		protected void DeepCloneGrievanceContextParts(GrievanceContext grievanceContext)
		{
			grievanceContext.BasePrestigeWager = this.BasePrestigeWager;
			grievanceContext.PrestigeReward = this.PrestigeReward.DeepClone<ModifiableValue>();
			grievanceContext.AdditionalPrestigeReward = this.AdditionalPrestigeReward.DeepClone<StatModifier>();
			grievanceContext.DiplomacyWager = this.DiplomacyWager;
		}

		// Token: 0x060017E6 RID: 6118
		public abstract void DeepClone(out GrievanceContext clone);

		// Token: 0x04000B67 RID: 2919
		public const int DefaultTurnCount = 3;

		// Token: 0x04000B68 RID: 2920
		[JsonProperty]
		public int BasePrestigeWager;

		// Token: 0x04000B69 RID: 2921
		[JsonProperty]
		public int DiplomacyWager;

		// Token: 0x04000B6A RID: 2922
		[JsonProperty]
		[DefaultValue(10)]
		public ModifiableValue PrestigeReward = 10;

		// Token: 0x04000B6B RID: 2923
		public StatModifier AdditionalPrestigeReward;
	}
}
