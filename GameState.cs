using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.Utils;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002ED RID: 749
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GameState : ISaveState<SaveGameVersion>, ICloneable, IDeepClone<GameState>
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0003A0E5 File Offset: 0x000382E5
		[JsonIgnore]
		public TurnState CurrentTurn
		{
			get
			{
				return this.TurnHistory.LastOrDefault<TurnState>();
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0003A0F2 File Offset: 0x000382F2
		[JsonIgnore]
		public int CurrentTurnValue
		{
			get
			{
				return this.CurrentTurn.TurnValue;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0003A0FF File Offset: 0x000382FF
		[JsonIgnore]
		public int TurnCount
		{
			get
			{
				if (this.TurnHistory.Count <= 0)
				{
					return 0;
				}
				return this.CurrentTurnValue + 1;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0003A119 File Offset: 0x00038319
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x0003A121 File Offset: 0x00038321
		[JsonProperty]
		public SaveGameVersion Version { get; set; } = SaveGameVersionUtils.LatestVersion;

		// Token: 0x06000E93 RID: 3731 RVA: 0x0003A12C File Offset: 0x0003832C
		public TurnState FetchTurnStateByTurnId(int turnId)
		{
			return this.TurnHistory.FirstOrDefault((TurnState t) => t.TurnValue == turnId);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0003A15D File Offset: 0x0003835D
		public TurnState CreateNewTurn()
		{
			return this.CreateNewTurn(this.CurrentTurnValue);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0003A16C File Offset: 0x0003836C
		public TurnState CreateNewTurn(int turnValue)
		{
			if (turnValue < 0 || turnValue > this.CurrentTurnValue)
			{
				return null;
			}
			TurnState turn = this.PrepareNewTurn(this.FetchTurnStateByTurnId(turnValue));
			turnValue++;
			return this.AddTurn(turn, turnValue);
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0003A1A3 File Offset: 0x000383A3
		public TurnState CreateNewTurn(TurnState turn)
		{
			turn = this.PrepareNewTurn(turn);
			return this.AddTurn(turn);
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0003A1B5 File Offset: 0x000383B5
		public TurnState AddTurn(TurnState turn)
		{
			return this.AddTurn(turn, turn.TurnValue);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0003A1C4 File Offset: 0x000383C4
		public TurnState AddTurn(TurnState turn, int turnValue)
		{
			if (turnValue >= this.TurnCount)
			{
				this.TurnHistory.Add(turn);
			}
			else
			{
				int index = this.TurnHistory.FindIndex((TurnState x) => x.TurnValue == turnValue);
				this.TurnHistory[index] = turn;
			}
			return turn;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0003A220 File Offset: 0x00038420
		public TurnState PrepareNewTurn(TurnState previousTurn)
		{
			return previousTurn.DeepClone<TurnState>().PrepareForNewTurn(previousTurn.TurnValue + 1);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0003A238 File Offset: 0x00038438
		public void MergeUsingExistingGameState(GameState existingState)
		{
			foreach (TurnState turnState in existingState.TurnHistory)
			{
				bool flag = true;
				for (int i = 0; i < this.TurnHistory.Count; i++)
				{
					if (this.TurnHistory[i].TurnValue == turnState.TurnValue)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.TurnHistory.Add(turnState);
				}
			}
			ListExtensions.OrderBy<TurnState, int>(this.TurnHistory, (TurnState x) => x.TurnValue);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0003A2F4 File Offset: 0x000384F4
		public void RewindToTurnValue(int turnValue)
		{
			int num = -1;
			for (int i = 0; i < this.TurnHistory.Count; i++)
			{
				if (this.TurnHistory[i].TurnValue == turnValue)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				int num2 = num + 1;
				int num3 = this.TurnHistory.Count - num2;
				if (num3 > 0)
				{
					this.TurnHistory.RemoveRange(num2, num3);
				}
			}
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0003A358 File Offset: 0x00038558
		public void AddHistory(IEnumerable<TurnState> states)
		{
			using (IEnumerator<TurnState> enumerator = states.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TurnState state = enumerator.Current;
					this.TurnHistory.RemoveAll((TurnState t) => t.TurnValue == state.TurnValue);
				}
			}
			this.TurnHistory.AddRange(states);
			this.TurnHistory.SortOnValueAscending((TurnState t) => t.TurnValue);
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0003A3F4 File Offset: 0x000385F4
		public int MaxIndex
		{
			get
			{
				TurnState turnState = this.TurnHistory.LastOrDefault<TurnState>();
				if (turnState == null)
				{
					return 0;
				}
				return turnState.TurnValue;
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0003A40C File Offset: 0x0003860C
		public IEnumerable<int> CalculateMissingTurns()
		{
			return this.CalculateMissingTurns(this.MaxIndex);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0003A41A File Offset: 0x0003861A
		public IEnumerable<int> CalculateMissingTurns(int maxIndex)
		{
			return from t in Enumerable.Range(0, maxIndex)
			where !this.HasTurn(t)
			select t;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0003A434 File Offset: 0x00038634
		public bool HasTurn(int turnValue)
		{
			TurnState turnState;
			return this.TryGetTurn(turnValue, out turnState);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0003A44C File Offset: 0x0003864C
		public bool TryGetTurn(int turnValue, out TurnState state)
		{
			if (turnValue >= 0 && turnValue < this.TurnHistory.Count)
			{
				state = this.TurnHistory[turnValue];
				if (state.TurnValue == turnValue)
				{
					return true;
				}
			}
			state = this.TurnHistory.FirstOrDefault((TurnState t) => t.TurnValue == turnValue);
			return state != null;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0003A4C4 File Offset: 0x000386C4
		protected void DeepCloneGameStateParts(GameState gameState)
		{
			gameState.GameType = this.GameType;
			gameState.Rules = this.Rules.DeepClone<GameRules>();
			gameState.TurnHistory = this.TurnHistory.DeepClone<TurnState>();
			gameState.Guid = this.Guid;
			gameState.GamePhrase = this.GamePhrase.DeepClone();
			gameState.Seed = this.Seed;
			gameState.Revision = this.Revision;
			gameState.Version = this.Version;
			gameState.LoadFileName = this.LoadFileName.DeepClone();
			gameState.ScenarioId = this.ScenarioId.DeepClone();
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0003A562 File Offset: 0x00038762
		public virtual void DeepClone(out GameState clone)
		{
			clone = new GameState();
			this.DeepCloneGameStateParts(clone);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0003A574 File Offset: 0x00038774
		protected T ShallowCloneParts<T>(T state) where T : GameState
		{
			state.GameType = this.GameType;
			state.Rules = this.Rules;
			state.TurnHistory = this.TurnHistory;
			state.Guid = this.Guid;
			state.GamePhrase = this.GamePhrase;
			state.Seed = this.Seed;
			state.Revision = this.Revision;
			state.Version = this.Version;
			state.LoadFileName = this.LoadFileName;
			state.ScenarioId = this.ScenarioId;
			return state;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0003A62C File Offset: 0x0003882C
		public virtual object Clone()
		{
			return this.ShallowCloneParts<GameState>(new GameState());
		}

		// Token: 0x040006A9 RID: 1705
		[JsonProperty]
		public GameType GameType;

		// Token: 0x040006AA RID: 1706
		[JsonProperty]
		public GameRules Rules;

		// Token: 0x040006AB RID: 1707
		[JsonProperty]
		public List<TurnState> TurnHistory = new List<TurnState>();

		// Token: 0x040006AC RID: 1708
		[JsonProperty]
		public Guid Guid = Guid.NewGuid();

		// Token: 0x040006AD RID: 1709
		[JsonProperty]
		public string GamePhrase;

		// Token: 0x040006AE RID: 1710
		[JsonProperty]
		public int Seed;

		// Token: 0x040006AF RID: 1711
		[JsonProperty]
		public int Revision;

		// Token: 0x040006B1 RID: 1713
		[JsonProperty]
		public string LoadFileName = string.Empty;

		// Token: 0x040006B2 RID: 1714
		[JsonProperty]
		public string ScenarioId = string.Empty;
	}
}
