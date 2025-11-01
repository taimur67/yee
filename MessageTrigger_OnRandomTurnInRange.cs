using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200030E RID: 782
	public class MessageTrigger_OnRandomTurnInRange : MessageTriggerCondition
	{
		// Token: 0x06000F4A RID: 3914 RVA: 0x0003CA77 File Offset: 0x0003AC77
		public override string GetDescription()
		{
			return string.Format("Message <{0}> as early as <Turn {1}> and as late as <Turn {2}>", this.RecipientArchfiendId, this.TurnMin, this.TurnMax);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0003CAA0 File Offset: 0x0003ACA0
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			Random random = new Random();
			int num = context.CurrentTurn.TurnValue + 1;
			bool flag = num == random.Next(this.TurnMin, this.TurnMax);
			return num == this.TurnMax || flag;
		}

		// Token: 0x04000729 RID: 1833
		public int TurnMin = 1;

		// Token: 0x0400072A RID: 1834
		public int TurnMax = 2;
	}
}
