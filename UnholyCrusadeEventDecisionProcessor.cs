using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020004FE RID: 1278
	public class UnholyCrusadeEventDecisionProcessor : DecisionProcessor<UnholyCrusadeEventDecisionRequest, UnholyCrusadeEventDecisionResponse>
	{
		// Token: 0x0600183A RID: 6202 RVA: 0x00056E24 File Offset: 0x00055024
		protected override Result Process(UnholyCrusadeEventDecisionResponse response)
		{
			UnholyCrusadeTurnModuleInstance turnModule = base._currentTurn.GetTurnModule(base.request.TurnModuleInstanceId);
			if (response.Choice == YesNo.No)
			{
				this.NoSubmission();
				turnModule.SetNoSubmission(this._player.Id);
				return Result.Success;
			}
			if (response.SubmittedLegionId == Identifier.Invalid)
			{
				this.NoSubmission();
				turnModule.SetNoSubmission(this._player.Id);
				return Result.Success;
			}
			turnModule.SubmitLegion(this._player.Id, response.SubmittedLegionId);
			return Result.Success;
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00056EB0 File Offset: 0x000550B0
		private void NoSubmission()
		{
			UnholyCrusadeEventStaticData unholyCrusadeEventStaticData = base._database.Fetch<UnholyCrusadeEventStaticData>(base.request.EventEffectId);
			this._player.RemovePrestige(unholyCrusadeEventStaticData.NoSubmissionPrestigePenalty);
		}
	}
}
