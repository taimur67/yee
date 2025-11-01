using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000329 RID: 809
	public class MessageTrigger_OnFollowupMessageAfterDelayTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F9D RID: 3997 RVA: 0x0003DDCD File Offset: 0x0003BFCD
		public override string GetDescription()
		{
			return string.Format("Send <{0}> a message <{1} Turns> after <{2}> has sent message <ID: {3}>", new object[]
			{
				this.RecipientArchfiendId,
				this.DelayTurnAmount,
				this.TargetArchfiendId,
				this.MessageId
			});
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0003DE08 File Offset: 0x0003C008
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			CannedMessageTrigger cannedMessageTrigger = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId).MessageTriggers.Find((CannedMessageTrigger x) => x.Id == this.MessageId);
			int num = context.CurrentTurn.TurnValue + 1;
			return cannedMessageTrigger != null && cannedMessageTrigger.IsCompleted && cannedMessageTrigger.CompletedOnTurnValue + 1 + this.DelayTurnAmount == num;
		}

		// Token: 0x04000746 RID: 1862
		public string TargetArchfiendId;

		// Token: 0x04000747 RID: 1863
		public string MessageId;

		// Token: 0x04000748 RID: 1864
		public int DelayTurnAmount;
	}
}
