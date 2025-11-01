using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x0200074F RID: 1871
	public class VoteTurnModuleProcessor<T> : TurnModuleProcessor<T> where T : VoteTurnModuleInstance
	{
		// Token: 0x06002313 RID: 8979 RVA: 0x00079CB0 File Offset: 0x00077EB0
		public override void OnAdded()
		{
			if (!base.Instance.Unannounced)
			{
				this.ProclaimNewVote();
				return;
			}
			base._currentTurn.CompletedEdicts.Add(base.Instance.EdictId);
			this.StartVote();
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x00079CFC File Offset: 0x00077EFC
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_Edicts, new TurnModuleProcessor.ProcessEvent(this.ProcessEdict));
			if (!base.Instance.Unannounced)
			{
				base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.CheckVoteReady));
			}
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x00079D4C File Offset: 0x00077F4C
		private void ProclaimNewVote()
		{
			if (string.IsNullOrEmpty(base.Instance.EdictId))
			{
				return;
			}
			base._currentTurn.NextEdictId = base.Instance.EdictId;
			base._currentTurn.CompletedEdicts.Add(base.Instance.EdictId);
			VoteProclamationEvent voteProclamationEvent = new VoteProclamationEvent(base.Instance.EdictId);
			voteProclamationEvent.DeliberationTurns = base.Instance.VoteTurn - base._currentTurn.TurnValue;
			base._currentTurn.AddGameEvent<VoteProclamationEvent>(voteProclamationEvent);
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x00079DF1 File Offset: 0x00077FF1
		private void CheckVoteReady()
		{
			if (base._currentTurn.TurnValue == base.Instance.VoteTurn)
			{
				this.StartVote();
			}
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x00079E16 File Offset: 0x00078016
		protected virtual void StartVote()
		{
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x00079E18 File Offset: 0x00078018
		protected Q CalculateWinner<Q>(Dictionary<int, Q> votes, out Dictionary<Q, Dictionary<int, int>> playerScaledVotes)
		{
			playerScaledVotes = new Dictionary<Q, Dictionary<int, int>>();
			Dictionary<Q, int> dictionary = new Dictionary<Q, int>();
			foreach (KeyValuePair<int, Q> keyValuePair in votes)
			{
				int num;
				Q q;
				keyValuePair.Deconstruct(out num, out q);
				int num2 = num;
				Q key = q;
				ModifiableValue votingPower = base._currentTurn.FindPlayerState(num2, null).VotingPower;
				if (!playerScaledVotes.ContainsKey(key))
				{
					playerScaledVotes[key] = new Dictionary<int, int>();
				}
				playerScaledVotes[key][num2] = votingPower;
				dictionary.AddOrSetValue(key, votingPower);
			}
			int winningVal = dictionary.Values.Max();
			List<KeyValuePair<Q, int>> winningPolicies = IEnumerableExtensions.ToList<KeyValuePair<Q, int>>(from x in dictionary
			where x.Value == winningVal
			select x);
			if (winningPolicies.Count == 1)
			{
				return winningPolicies[0].Key;
			}
			return IEnumerableExtensions.First<KeyValuePair<int, Q>>(from x in votes
			where winningPolicies.Any(delegate(KeyValuePair<Q, int> y)
			{
				Q key2 = y.Key;
				return key2.Equals(x.Value);
			})
			orderby this._currentTurn.TurnsUntilRegency(this._currentTurn.FindPlayerState(x.Key, null))
			select x).Value;
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x00079F64 File Offset: 0x00078164
		protected virtual void ProcessEdict()
		{
		}
	}
}
