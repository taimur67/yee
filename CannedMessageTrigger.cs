using System;

namespace LoG
{
	// Token: 0x0200030A RID: 778
	[Serializable]
	public class CannedMessageTrigger : IDeepClone<CannedMessageTrigger>
	{
		// Token: 0x06000F41 RID: 3905 RVA: 0x0003C954 File Offset: 0x0003AB54
		public void MarkCompleted()
		{
			this.IsCompleted = true;
			this.CompletedOnTurnValue = this.Message.TurnId;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0003C970 File Offset: 0x0003AB70
		public void DeepClone(out CannedMessageTrigger clone)
		{
			clone = new CannedMessageTrigger
			{
				Id = this.Id.DeepClone(),
				ConditionType = this.ConditionType,
				Condition = this.Condition.DeepClone(CloneFunction.FastClone),
				Message = this.Message.DeepClone(CloneFunction.FastClone),
				IsCompleted = this.IsCompleted,
				CompletedOnTurnValue = this.CompletedOnTurnValue
			};
		}

		// Token: 0x040006FC RID: 1788
		public string Id = Guid.NewGuid().ToString();

		// Token: 0x040006FD RID: 1789
		public MessageTriggerConditions ConditionType;

		// Token: 0x040006FE RID: 1790
		public MessageTriggerCondition Condition;

		// Token: 0x040006FF RID: 1791
		public CannedMessage Message = new CannedMessage();

		// Token: 0x04000700 RID: 1792
		public bool IsCompleted;

		// Token: 0x04000701 RID: 1793
		public int CompletedOnTurnValue;
	}
}
