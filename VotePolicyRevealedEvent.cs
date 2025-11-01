using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000214 RID: 532
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class VotePolicyRevealedEvent : VoteRevealedEvent
	{
		// Token: 0x06000A5E RID: 2654 RVA: 0x0002E213 File Offset: 0x0002C413
		[JsonConstructor]
		private VotePolicyRevealedEvent()
		{
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0002E226 File Offset: 0x0002C426
		public VotePolicyRevealedEvent(string edictId) : base(edictId)
		{
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002E23C File Offset: 0x0002C43C
		public override bool DidPlayerVote(int playerId)
		{
			using (Dictionary<string, Dictionary<int, int>>.ValueCollection.Enumerator enumerator = this.VoteResult.Votes.Values.GetEnumerator())
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

		// Token: 0x06000A61 RID: 2657 RVA: 0x0002E2A0 File Offset: 0x0002C4A0
		public override IEnumerable<int> GetPlayersWhoVotedForWinningOption()
		{
			Dictionary<int, int> dictionary;
			if (this.VoteResult.Votes.TryGetValue(this.EffectId, out dictionary))
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

		// Token: 0x06000A62 RID: 2658 RVA: 0x0002E2B0 File Offset: 0x0002C4B0
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.VotingPolicyRevealed;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0002E2B7 File Offset: 0x0002C4B7
		public override string GetDebugName(TurnContext context)
		{
			return "Edict voting complete. " + this.EffectId + " has been enacted.";
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0002E2D0 File Offset: 0x0002C4D0
		public override void DeepClone(out GameEvent clone)
		{
			VotePolicyRevealedEvent votePolicyRevealedEvent = new VotePolicyRevealedEvent
			{
				VoteResult = this.VoteResult.DeepClone(CloneFunction.FastClone),
				EffectId = this.EffectId.DeepClone()
			};
			base.DeepCloneVoteEventParts(votePolicyRevealedEvent);
			clone = votePolicyRevealedEvent;
		}

		// Token: 0x040004D3 RID: 1235
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public PolicyVoteResult VoteResult = new PolicyVoteResult();

		// Token: 0x040004D4 RID: 1236
		[BindableValue("effect", BindingOption.StaticDataId)]
		[JsonProperty]
		public string EffectId;
	}
}
