using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200030B RID: 779
	[Serializable]
	public class MessageTriggerCondition
	{
		// Token: 0x06000F44 RID: 3908 RVA: 0x0003CA17 File Offset: 0x0003AC17
		public virtual string GetDescription()
		{
			return "<No Description Available>";
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0003CA1E File Offset: 0x0003AC1E
		public virtual bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			return false;
		}

		// Token: 0x04000702 RID: 1794
		public string Id = string.Empty;

		// Token: 0x04000703 RID: 1795
		public string TriggerOwnerArchfiendId;

		// Token: 0x04000704 RID: 1796
		public string RecipientArchfiendId;
	}
}
