using System;

namespace LoG
{
	// Token: 0x020004B8 RID: 1208
	public class SelectEdictCandidate_DecisionProcessor : DecisionProcessor<SelectEdictCandidateDecisionRequest, SelectEdictCandidateDecisionResponse>
	{
		// Token: 0x0600169C RID: 5788 RVA: 0x000530E4 File Offset: 0x000512E4
		protected override SelectEdictCandidateDecisionResponse GenerateTypedFallbackResponse()
		{
			SelectEdictCandidateDecisionResponse selectEdictCandidateDecisionResponse = new SelectEdictCandidateDecisionResponse();
			int selectedPlayerId;
			if (base.request.Candidates.TryGetRandom(this.TurnProcessContext.Random, out selectedPlayerId))
			{
				selectEdictCandidateDecisionResponse.SelectedPlayerId = selectedPlayerId;
			}
			return selectEdictCandidateDecisionResponse;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00053120 File Offset: 0x00051320
		protected override Result Validate(SelectEdictCandidateDecisionResponse response)
		{
			int selectedPlayerId = response.SelectedPlayerId;
			if (selectedPlayerId != -2147483648 && selectedPlayerId != -1)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0005314C File Offset: 0x0005134C
		protected override Result Process(SelectEdictCandidateDecisionResponse response)
		{
			int selectedPlayerId = response.SelectedPlayerId;
			if (selectedPlayerId == -2147483648 || selectedPlayerId == -1)
			{
				return Result.Failure;
			}
			base._currentTurn.GetTurnModule(base.request.TurnModuleInstanceId).Vote(this._player.Id, response.SelectedPlayerId);
			return Result.Success;
		}
	}
}
