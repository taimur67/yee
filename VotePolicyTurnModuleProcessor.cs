using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200074D RID: 1869
	public class VotePolicyTurnModuleProcessor : VoteTurnModuleProcessor<VotePolicyTurnModuleInstance>
	{
		// Token: 0x0600230D RID: 8973 RVA: 0x00079A80 File Offset: 0x00077C80
		protected override void StartVote()
		{
			EdictPolicyStaticData edictPolicyStaticData = base._database.Fetch<EdictPolicyStaticData>(base.Instance.EdictId);
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				if (!playerState.Excommunicated)
				{
					SelectEdictPolicyDecisionRequest selectEdictPolicyDecisionRequest = new SelectEdictPolicyDecisionRequest(base._currentTurn);
					selectEdictPolicyDecisionRequest.EdictId = edictPolicyStaticData.Id;
					selectEdictPolicyDecisionRequest.Candidates = edictPolicyStaticData.Options;
					selectEdictPolicyDecisionRequest.Unannounced = base.Instance.Unannounced;
					selectEdictPolicyDecisionRequest.TurnModuleInstanceId = base.Instance.Id;
					base._currentTurn.AddDecisionToAskPlayer(playerState.Id, selectEdictPolicyDecisionRequest);
				}
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x00079B4C File Offset: 0x00077D4C
		protected override void ProcessEdict()
		{
			if (base.Instance.Votes.Count <= 0)
			{
				return;
			}
			this.TurnProcessContext.RecalculateAllPlayerModifiers();
			Dictionary<string, Dictionary<int, int>> votes;
			string winningPolicyId = base.CalculateWinner<string>(base.Instance.Votes, out votes);
			VotePolicyRevealedEvent votePolicyRevealedEvent = new VotePolicyRevealedEvent(base.Instance.EdictId);
			votePolicyRevealedEvent.EffectId = winningPolicyId;
			votePolicyRevealedEvent.VoteResult.Votes = votes;
			base._currentTurn.AddGameEvent<VotePolicyRevealedEvent>(votePolicyRevealedEvent);
			EdictPolicyStaticData edictPolicyStaticData = this.TurnProcessContext.Database.Fetch<EdictPolicyStaticData>(base.Instance.EdictId);
			ConfigRef<EdictEffectStaticData> configRef = edictPolicyStaticData.Options.FirstOrDefault((ConfigRef<EdictEffectStaticData> x) => x.Id == winningPolicyId);
			if (configRef == null)
			{
				return;
			}
			EdictEffectStaticData edictEffectStaticData = base._database.Fetch(configRef);
			EdictEffectModuleInstance edictEffectModuleInstance = (EdictEffectModuleInstance)base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, edictEffectStaticData);
			edictEffectModuleInstance.EdictId = edictPolicyStaticData.Id;
			edictEffectModuleInstance.EffectId = edictEffectStaticData.Id;
			base.RemoveSelf();
		}
	}
}
