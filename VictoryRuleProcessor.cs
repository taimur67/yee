using System;

namespace LoG
{
	// Token: 0x020006F9 RID: 1785
	public abstract class VictoryRuleProcessor : IDeepClone<VictoryRuleProcessor>
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600222E RID: 8750
		public abstract Type AssociatedVictoryRuleType { get; }

		// Token: 0x0600222F RID: 8751 RVA: 0x000770DD File Offset: 0x000752DD
		public virtual bool Process(TurnProcessContext context)
		{
			return false;
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000770E0 File Offset: 0x000752E0
		public virtual GameVictory DecideWinner(TurnProcessContext context)
		{
			return null;
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000770E3 File Offset: 0x000752E3
		public virtual string GetDebugName()
		{
			return string.Empty;
		}

		// Token: 0x06002232 RID: 8754
		public abstract bool IsInEndGame();

		// Token: 0x06002233 RID: 8755
		public abstract void DeepClone(out VictoryRuleProcessor clone);
	}
}
