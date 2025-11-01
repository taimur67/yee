using System;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020004BB RID: 1211
	public class SelectEdictPolicy_DecisionProcessor : DecisionProcessor<SelectEdictPolicyDecisionRequest, SelectEdictPolicyDecisionResponse>
	{
		// Token: 0x060016A6 RID: 5798 RVA: 0x00053216 File Offset: 0x00051416
		protected override SelectEdictPolicyDecisionResponse GenerateTypedFallbackResponse()
		{
			return new SelectEdictPolicyDecisionResponse
			{
				SelectedPolicyId = base.request.Candidates.GetRandom(this.TurnProcessContext.Random).Id
			};
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00053243 File Offset: 0x00051443
		protected override Result Validate(SelectEdictPolicyDecisionResponse response)
		{
			if (!string.IsNullOrEmpty(response.SelectedPolicyId))
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x00053260 File Offset: 0x00051460
		protected override Result Process(SelectEdictPolicyDecisionResponse response)
		{
			if (string.IsNullOrEmpty(response.SelectedPolicyId))
			{
				return Result.Failure;
			}
			base._currentTurn.GetTurnModule(base.request.TurnModuleInstanceId).Vote(this._player.Id, response.SelectedPolicyId);
			return Result.Success;
		}
	}
}
