using System;

namespace LoG
{
	// Token: 0x02000545 RID: 1349
	[Serializable]
	public abstract class BooleanStateObjectiveCondition : ObjectiveCondition
	{
		// Token: 0x06001A2C RID: 6700 RVA: 0x0005B597 File Offset: 0x00059797
		protected sealed override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (!this.CheckCompleteStatus(context, owner, isInitialProgress))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06001A2D RID: 6701
		protected abstract bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress);
	}
}
