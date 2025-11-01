using System;

namespace LoG
{
	// Token: 0x020004CD RID: 1229
	public class UpkeepDecisionProcessor : DecisionProcessor<UpkeepDecisionRequest, UpkeepDecisionResponse>
	{
		// Token: 0x0600170D RID: 5901 RVA: 0x00054455 File Offset: 0x00052655
		protected override Result Validate(UpkeepDecisionResponse response)
		{
			if (response.Choice == YesNo.No)
			{
				return Result.Success;
			}
			if (response.Choice != YesNo.Yes)
			{
				return Result.Failure;
			}
			return this._player.CanAfford(base.request.RequiredPayment);
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0005448B File Offset: 0x0005268B
		protected override Result Preview(UpkeepDecisionResponse response)
		{
			return this.Process(response);
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00054494 File Offset: 0x00052694
		protected override Result Process(UpkeepDecisionResponse response)
		{
			if (response.Choice != YesNo.Yes)
			{
				return Result.Success;
			}
			GameItem gameItem = base._currentTurn.FetchGameItem(base.request.GameItemId);
			if (gameItem == null)
			{
				return Result.SimulationError(string.Format("Requested upkeep for invalid game item {0}", base.request.GameItemId));
			}
			Problem problem = this._player.RemovePayment(response.UpkeepPayment) as Problem;
			if (problem != null)
			{
				return problem;
			}
			gameItem.PayUpkeep(response.UpkeepPayment);
			return Result.Success;
		}
	}
}
