using System;

namespace LoG
{
	// Token: 0x020004C4 RID: 1220
	public class SelectPower_DecisionProcessor : DecisionProcessor<SelectPowerDecisionRequest, SelectPowerDecisionResponse>
	{
		// Token: 0x060016CE RID: 5838 RVA: 0x000539BD File Offset: 0x00051BBD
		protected override Result Validate(SelectPowerDecisionResponse response)
		{
			if (response.SelectedPower < 0)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x000539D4 File Offset: 0x00051BD4
		protected override Result Process(SelectPowerDecisionResponse response)
		{
			if (this._player.PowersLevels[base.request.PowerType].CurrentLevel < base.request.Level)
			{
				return Result.Failure;
			}
			this._player.PowersLevels[base.request.PowerType].SetChosenAbility(base.request.Level, response.SelectedPower);
			return Result.Success;
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00053A4F File Offset: 0x00051C4F
		protected override Result Preview(SelectPowerDecisionResponse response)
		{
			return this.Process(response);
		}
	}
}
