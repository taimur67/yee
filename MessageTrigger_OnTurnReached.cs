using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200030D RID: 781
	public class MessageTrigger_OnTurnReached : MessageTriggerCondition
	{
		// Token: 0x06000F47 RID: 3911 RVA: 0x0003CA34 File Offset: 0x0003AC34
		public override string GetDescription()
		{
			return string.Format("Message <{0}> on <Turn {1}>", this.RecipientArchfiendId, this.TurnValue);
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0003CA51 File Offset: 0x0003AC51
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			return this.TurnValue == context.CurrentTurn.TurnValue + 1;
		}

		// Token: 0x04000728 RID: 1832
		public int TurnValue = 2;
	}
}
