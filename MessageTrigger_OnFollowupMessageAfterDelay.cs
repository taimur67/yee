using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000316 RID: 790
	public class MessageTrigger_OnFollowupMessageAfterDelay : MessageTriggerCondition
	{
		// Token: 0x06000F63 RID: 3939 RVA: 0x0003CFCC File Offset: 0x0003B1CC
		public override string GetDescription()
		{
			return string.Format("Send <{0}> a message <{1} Turns> after sending '<{2}>'", this.RecipientArchfiendId, this.DelayTurnAmount, this.MessageId);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0003CFF0 File Offset: 0x0003B1F0
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			CannedMessageTrigger cannedMessageTrigger = context.CurrentTurn.FindPlayerState(this.TriggerOwnerArchfiendId).MessageTriggers.Find((CannedMessageTrigger x) => x.Id == this.MessageId);
			int num = context.CurrentTurn.TurnValue + 1;
			return cannedMessageTrigger != null && cannedMessageTrigger.IsCompleted && cannedMessageTrigger.CompletedOnTurnValue + 1 + this.DelayTurnAmount == num;
		}

		// Token: 0x04000733 RID: 1843
		public string MessageId;

		// Token: 0x04000734 RID: 1844
		public int DelayTurnAmount;
	}
}
