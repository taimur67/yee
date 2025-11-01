using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200074B RID: 1867
	public class VoteCandidateTurnModuleProcessor : VoteTurnModuleProcessor<VoteCandidateTurnModuleInstance>
	{
		// Token: 0x06002307 RID: 8967 RVA: 0x000797EC File Offset: 0x000779EC
		protected override void StartVote()
		{
			EdictCandidateStaticData edictCandidateStaticData = base._database.Fetch<EdictCandidateStaticData>(base.Instance.EdictId);
			IEnumerable<PlayerState> enumerable;
			if (!base._database.Fetch(edictCandidateStaticData.Effect).SufficientValidCandidates(base._currentTurn, out enumerable))
			{
				bool unannounced = base.Instance.Unannounced;
				return;
			}
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				if (!playerState.Excommunicated)
				{
					SelectEdictCandidateDecisionRequest selectEdictCandidateDecisionRequest = new SelectEdictCandidateDecisionRequest(base._currentTurn);
					selectEdictCandidateDecisionRequest.EdictId = edictCandidateStaticData.Id;
					selectEdictCandidateDecisionRequest.Candidates = IEnumerableExtensions.ToList<int>(from x in IEnumerableExtensions.ExceptFor<PlayerState>(enumerable, new PlayerState[]
					{
						playerState
					})
					select x.Id);
					selectEdictCandidateDecisionRequest.Unannounced = base.Instance.Unannounced;
					selectEdictCandidateDecisionRequest.TurnModuleInstanceId = base.Instance.Id;
					base._currentTurn.AddDecisionToAskPlayer(playerState.Id, selectEdictCandidateDecisionRequest);
				}
			}
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x00079928 File Offset: 0x00077B28
		protected override void ProcessEdict()
		{
			if (base.Instance.Votes.Count <= 0)
			{
				return;
			}
			Dictionary<int, Dictionary<int, int>> votesToPlayer;
			int num = base.CalculateWinner<int>(base.Instance.Votes, out votesToPlayer);
			VoteCandidateRevealedEvent voteCandidateRevealedEvent = new VoteCandidateRevealedEvent(base.Instance.EdictId, num);
			voteCandidateRevealedEvent.VoteResult.VotesToPlayer = votesToPlayer;
			base._currentTurn.AddGameEvent<VoteCandidateRevealedEvent>(voteCandidateRevealedEvent);
			EdictCandidateStaticData edictCandidateStaticData = this.TurnProcessContext.Database.Fetch<EdictCandidateStaticData>(base.Instance.EdictId);
			if (edictCandidateStaticData.Effect == null)
			{
				return;
			}
			EdictEffectStaticData edictEffectStaticData = base._database.Fetch(edictCandidateStaticData.Effect);
			EdictCandidateEffectModuleInstance edictCandidateEffectModuleInstance = (EdictCandidateEffectModuleInstance)base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, edictEffectStaticData);
			voteCandidateRevealedEvent.EffectId = edictEffectStaticData.Id;
			edictCandidateEffectModuleInstance.EdictId = edictCandidateStaticData.Id;
			edictCandidateEffectModuleInstance.EffectId = edictEffectStaticData.Id;
			edictCandidateEffectModuleInstance.TargetPlayerId = num;
			base.RemoveSelf();
		}
	}
}
