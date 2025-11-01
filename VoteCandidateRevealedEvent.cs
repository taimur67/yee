using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000213 RID: 531
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class VoteCandidateRevealedEvent : VoteRevealedEvent
	{
		// Token: 0x06000A57 RID: 2647 RVA: 0x0002E10B File Offset: 0x0002C30B
		[JsonConstructor]
		private VoteCandidateRevealedEvent()
		{
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0002E11E File Offset: 0x0002C31E
		public VoteCandidateRevealedEvent(string edictId, int affectedPlayerId) : base(edictId)
		{
			base.AddAffectedPlayerId(affectedPlayerId);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0002E13C File Offset: 0x0002C33C
		public override bool DidPlayerVote(int playerId)
		{
			using (Dictionary<int, Dictionary<int, int>>.ValueCollection.Enumerator enumerator = this.VoteResult.VotesToPlayer.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ContainsKey(playerId))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0002E1A0 File Offset: 0x0002C3A0
		public override IEnumerable<int> GetPlayersWhoVotedForWinningOption()
		{
			Dictionary<int, int> dictionary;
			if (this.VoteResult.VotesToPlayer.TryGetValue(base.AffectedPlayerID, out dictionary))
			{
				foreach (int num in dictionary.Keys)
				{
					yield return num;
				}
				Dictionary<int, int>.KeyCollection.Enumerator enumerator = default(Dictionary<int, int>.KeyCollection.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0002E1B0 File Offset: 0x0002C3B0
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.VotingCandidateRevealed;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0002E1B7 File Offset: 0x0002C3B7
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Edict voting complete. Player {0} will receive the effects of the edict: {1}", base.AffectedPlayerID, this.EdictId);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0002E1D4 File Offset: 0x0002C3D4
		public override void DeepClone(out GameEvent clone)
		{
			VoteCandidateRevealedEvent voteCandidateRevealedEvent = new VoteCandidateRevealedEvent
			{
				VoteResult = this.VoteResult.DeepClone<CandidateVoteResult>(),
				EffectId = this.EffectId.DeepClone()
			};
			base.DeepCloneVoteEventParts(voteCandidateRevealedEvent);
			clone = voteCandidateRevealedEvent;
		}

		// Token: 0x040004D1 RID: 1233
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public CandidateVoteResult VoteResult = new CandidateVoteResult();

		// Token: 0x040004D2 RID: 1234
		[BindableValue("effect", BindingOption.StaticDataId)]
		[JsonProperty]
		public string EffectId;
	}
}
