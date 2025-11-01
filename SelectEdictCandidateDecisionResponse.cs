using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004B7 RID: 1207
	[Serializable]
	public class SelectEdictCandidateDecisionResponse : DecisionResponse
	{
		// Token: 0x06001696 RID: 5782 RVA: 0x0005301E File Offset: 0x0005121E
		public void Select(int playerId)
		{
			this.SelectedPlayerId = playerId;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00053027 File Offset: 0x00051227
		public bool IsSelected(int playerId)
		{
			return this.SelectedPlayerId == playerId;
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x00053032 File Offset: 0x00051232
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetArchfiend(new Action<int>(this.Select), new ActionPhase_SingleTarget<int>.IsValidFunc(this.IsValidArchfiend));
			yield break;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00053044 File Offset: 0x00051244
		private Result IsValidArchfiend(TurnContext context, int targetPlayerId, int castingPlayerId)
		{
			SelectEdictCandidateDecisionRequest selectEdictCandidateDecisionRequest;
			if (context.CurrentTurn.FindPlayerState(castingPlayerId, null).TryGetDecisionRequest<SelectEdictCandidateDecisionRequest>(base.DecisionId, out selectEdictCandidateDecisionRequest))
			{
				if (!selectEdictCandidateDecisionRequest.Candidates.Contains(targetPlayerId))
				{
					return Result.Failure;
				}
				PlayerState playerState = context.CurrentTurn.FindPlayerState(targetPlayerId, null);
				if (playerState == null || playerState.Eliminated)
				{
					return Result.Failure;
				}
			}
			return Result.Success;
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x000530A8 File Offset: 0x000512A8
		public override void DeepClone(out DecisionResponse clone)
		{
			SelectEdictCandidateDecisionResponse selectEdictCandidateDecisionResponse = new SelectEdictCandidateDecisionResponse
			{
				SelectedPlayerId = this.SelectedPlayerId
			};
			base.DeepCloneDecisionResponseParts(selectEdictCandidateDecisionResponse);
			clone = selectEdictCandidateDecisionResponse;
		}

		// Token: 0x04000B2E RID: 2862
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int SelectedPlayerId = int.MinValue;
	}
}
