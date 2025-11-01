using System;

namespace LoG
{
	// Token: 0x02000544 RID: 1348
	[Serializable]
	public abstract class IncrementingObjectiveCondition : ObjectiveCondition
	{
		// Token: 0x06001A28 RID: 6696 RVA: 0x0005B563 File Offset: 0x00059763
		public override Result IsValidFor(TurnContext context, PlayerState owner)
		{
			return Result.Success;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0005B56C File Offset: 0x0005976C
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (isInitialProgress)
			{
				return 0;
			}
			int num = this.CalculateProgressIncrement(context, owner);
			return this.Count + num;
		}

		// Token: 0x06001A2A RID: 6698
		protected abstract int CalculateProgressIncrement(TurnContext context, PlayerState owner);
	}
}
