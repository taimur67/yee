using System;

namespace LoG
{
	// Token: 0x02000633 RID: 1587
	public class DemandTributeActionActionProcessor : ActionProcessor<OrderDemandTribute>
	{
		// Token: 0x06001D53 RID: 7507 RVA: 0x00065574 File Offset: 0x00063774
		public override Result Process(ActionProcessContext actionProcessContext)
		{
			DemandTributeUtils.TributeParameters demandTributeParameters = this._player.GetDemandTributeParameters(this.TurnProcessContext);
			demandTributeParameters.Quality = DemandTributeActionActionProcessor.GetAdjustedTributeQuality(demandTributeParameters.Quality, actionProcessContext.OrderSlotIndex);
			Problem problem = DemandTributeUtils.CreateAndAddDemandTributeDecisionToPlayer(this.TurnProcessContext, this._player, demandTributeParameters) as Problem;
			if (problem != null)
			{
				return problem;
			}
			this.TurnProcessContext.TributeContext.IncrementDraw(this._player);
			return Result.Success;
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000655E3 File Offset: 0x000637E3
		public static int GetAdjustedTributeQuality(int baseQuality, int orderSlot)
		{
			return Math.Max(0, baseQuality - 2 * orderSlot);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000655F0 File Offset: 0x000637F0
		public override Result IsAvailable()
		{
			Result result = this.IsUnLocked(base.request);
			Problem problem = result as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (!this._player.IsDrawTributeAvailable)
			{
				return Result.Failure;
			}
			return result;
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x0006562F File Offset: 0x0006382F
		public override Result Validate()
		{
			return this.IsAvailable();
		}
	}
}
