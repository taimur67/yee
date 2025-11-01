using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006FF RID: 1791
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VictoryRuleProcessorTrialOfTheThrone : VictoryRuleProcessor<VictoryRuleTrialOfTheThrone>
	{
		// Token: 0x0600224C RID: 8780 RVA: 0x0007760C File Offset: 0x0007580C
		public override void Init(VictoryRuleTrialOfTheThrone victoryRule, TurnState turnState)
		{
			base.Init(victoryRule, turnState);
			this.DecideTrialDurations(victoryRule, turnState);
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00077620 File Offset: 0x00075820
		public override bool Process(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameRules rules = context.Rules;
			if (this.IsTrialEliminationVictory(currentTurn))
			{
				return true;
			}
			if (!this.CanProceedWithTrial(currentTurn))
			{
				return false;
			}
			if (!this.TrialInProgress())
			{
				if (currentTurn.TurnValue >= rules.GameDuration)
				{
					currentTurn.SetTurnPhase(TurnPhase.Assembly);
					this.BeginTrial(context);
					if (this.CurrentPhaseIndex >= this.TrialPhases.Count)
					{
						return true;
					}
				}
				else if (currentTurn.TurnPhase != TurnPhase.None)
				{
					currentTurn.SetTurnPhase(TurnPhase.Assembly);
					currentTurn.AddGameEvent<NoVictoryConditionTickingEvent>(new NoVictoryConditionTickingEvent());
				}
				return false;
			}
			context.RevealAllSchemes(false);
			context.CurrentTurn.IsPrivateSchemeValid = false;
			currentTurn.SetTurnPhase(TurnPhase.Assembly);
			this.CurrentPhaseTurn++;
			if (this.CurrentPhaseIndex >= this.TrialPhases.Count)
			{
				return true;
			}
			TrialPhase trialPhase = this.TrialPhases[this.CurrentPhaseIndex];
			if (this.CurrentPhaseTurn < trialPhase.Duration)
			{
				int conclaveFavouriteId = currentTurn.ConclaveFavouriteId;
				currentTurn.AddGameEvent<TrialProgressesEvent>(new TrialProgressesEvent(conclaveFavouriteId, trialPhase.State, true));
				return false;
			}
			return !this.NextPhase(currentTurn);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x00077730 File Offset: 0x00075930
		public void BeginTrial(TurnProcessContext context)
		{
			context.RevealAllSchemes(true);
			context.CurrentTurn.IsPrivateSchemeValid = false;
			this.TrialState = TrialState.Assembling;
			int conclaveFavouriteId = context.CurrentTurn.ConclaveFavouriteId;
			context.CurrentTurn.AddGameEvent<TrialProgressesEvent>(new TrialProgressesEvent(conclaveFavouriteId, TrialState.Assembling, false));
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x00077777 File Offset: 0x00075977
		public bool CanProceedWithTrial(TurnState turnState)
		{
			return turnState.GetPandaemoniumOwnedByConclave(true) && IEnumerableExtensions.ToList<PlayerState>(turnState.GetConclaveMemberPlayers()).Count != 0;
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x0007779C File Offset: 0x0007599C
		private bool IsTrialEliminationVictory(TurnState currentTurn)
		{
			List<PlayerState> list = IEnumerableExtensions.ToList<PlayerState>(currentTurn.GetNonEliminatedPlayers());
			GamePiece pandaemonium = currentTurn.GetPandaemonium();
			if (list.Count == 1)
			{
				if (list.All((PlayerState x) => !x.Excommunicated) && (pandaemonium == null || !pandaemonium.IsOwned()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000777FC File Offset: 0x000759FC
		private void DecideTrialDurations(VictoryRuleTrialOfTheThrone victoryRule, TurnState turnState)
		{
			this.TrialState = TrialState.Pending;
			int num = turnState.Random.Next(victoryRule.TrialLengthMin, victoryRule.TrialLengthMax);
			this.AddNewTrialPhase(TrialState.Assembling, 1);
			this.AddNewTrialPhase(TrialState.Debating, 1);
			this.AddNewTrialPhase(TrialState.Arguing, 1);
			this.AddNewTrialPhase(TrialState.Documenting, 1);
			this.AddNewTrialPhase(TrialState.Announcing, 1);
			int num2 = Math.Max(0, num - this.TrialPhases.Count);
			for (int i = 0; i < num2; i++)
			{
				int index = turnState.Random.Next(this.TrialPhases.Count);
				this.TrialPhases[index].Duration++;
			}
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000778A0 File Offset: 0x00075AA0
		public bool IsEligibleForThrone(PlayerState player)
		{
			return !player.Eliminated && !player.Excommunicated;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000778B8 File Offset: 0x00075AB8
		public override GameVictory DecideWinner(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			VictoryManipulationType manipulationType = VictoryManipulationType.None;
			int puppetPlayerID = int.MinValue;
			List<PlayerState> list = IEnumerableExtensions.ToList<PlayerState>(currentTurn.EnumeratePlayerStates(false, false).Where(new Func<PlayerState, bool>(this.IsEligibleForThrone)));
			if (list.Count == 0)
			{
				return new GameVictory(VictoryType.Abyss, new List<int>(), VictoryManipulationType.None, int.MinValue);
			}
			base.SortByWorthiness(currentTurn, list);
			list.Reverse();
			PlayerState playerState = list[0];
			List<PlayerState> list2 = new List<PlayerState>();
			list2.Add(playerState);
			for (int i = 1; i < list.Count; i++)
			{
				if (base.CompareTrialWorthiness(currentTurn, playerState, list[i]) == 0)
				{
					list2.Add(playerState);
				}
			}
			int id = list2.GetRandom(currentTurn.Random).Id;
			int num = id;
			int powerBehindTheThrone = this.GetPowerBehindTheThrone(currentTurn, id);
			List<PlayerState> kingmakers = this.GetKingmakers(currentTurn, id);
			base.SortByWorthiness(currentTurn, kingmakers);
			kingmakers.Reverse();
			if (powerBehindTheThrone != -2147483648)
			{
				num = powerBehindTheThrone;
				puppetPlayerID = id;
				manipulationType = VictoryManipulationType.PowerBehindTheThrone;
			}
			else if (IEnumerableExtensions.Any<PlayerState>(kingmakers))
			{
				num = kingmakers[0].Id;
				puppetPlayerID = id;
				manipulationType = VictoryManipulationType.Kingmaker;
				kingmakers.RemoveAt(0);
			}
			List<PlayerState> list3 = IEnumerableExtensions.ToList<PlayerState>(currentTurn.EnumeratePlayerStates(false, true));
			base.SortByWorthiness(currentTurn, list3);
			list3.Reverse();
			List<int> list4 = IEnumerableExtensions.ToList<int>(from x in list3
			select x.Id);
			list4.Remove(num);
			list4.Insert(0, num);
			int num2 = 1;
			int num3;
			if (currentTurn.CurrentDiplomaticTurn.IsBloodLordOfAny(num, out num3))
			{
				PlayerState player = currentTurn.PlayerStates[num3];
				if (this.IsEligibleForThrone(player))
				{
					list4.Remove(num3);
					list4.Insert(num2, num3);
					num2++;
				}
			}
			if (num != id)
			{
				list4.Remove(id);
				list4.Insert(num2, id);
				num2++;
			}
			if (IEnumerableExtensions.Any<PlayerState>(kingmakers))
			{
				ListExtensions.Remove<int>(list4, from x in kingmakers
				select x.Id);
				list4.InsertRange(num2, from x in kingmakers
				select x.Id);
				num2 += kingmakers.Count;
			}
			return new GameVictory(VictoryType.Prestige, list4, manipulationType, puppetPlayerID);
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x00077B20 File Offset: 0x00075D20
		private bool NextPhase(TurnState turnState)
		{
			this.CurrentPhaseIndex++;
			if (this.CurrentPhaseIndex < this.TrialPhases.Count)
			{
				TrialPhase trialPhase = this.TrialPhases[this.CurrentPhaseIndex];
				this.CurrentPhaseTurn = 0;
				int conclaveFavouriteId = turnState.ConclaveFavouriteId;
				turnState.AddGameEvent<TrialProgressesEvent>(new TrialProgressesEvent(conclaveFavouriteId, trialPhase.State, false));
				this.TrialState = trialPhase.State;
				return true;
			}
			return false;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x00077B91 File Offset: 0x00075D91
		public override bool IsInEndGame()
		{
			return this.TrialInProgress();
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00077B99 File Offset: 0x00075D99
		public override string GetDebugName()
		{
			return "Trial of the Throne";
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00077BA0 File Offset: 0x00075DA0
		public int GetTrialLength()
		{
			return (int)IEnumerableExtensions.Accumulate<TrialPhase>(this.TrialPhases, (TrialPhase t) => (float)t.Duration);
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x00077BD0 File Offset: 0x00075DD0
		public int GetTrialLengthFromPhases()
		{
			int num = 0;
			foreach (TrialPhase trialPhase in this.TrialPhases)
			{
				num += trialPhase.Duration;
			}
			return num;
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00077C28 File Offset: 0x00075E28
		public int GetCurrentPhaseIndex()
		{
			return this.CurrentPhaseIndex;
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00077C30 File Offset: 0x00075E30
		public int GetCurrentPhaseTurn()
		{
			return this.CurrentPhaseTurn;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00077C38 File Offset: 0x00075E38
		public bool TrialInProgress()
		{
			return this.TrialState != TrialState.Undefined && this.TrialState != TrialState.Pending;
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x00077C50 File Offset: 0x00075E50
		public void AddNewTrialPhase(TrialState state, int duration)
		{
			TrialPhase item = new TrialPhase(state, duration);
			this.TrialPhases.Add(item);
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00077C74 File Offset: 0x00075E74
		public void ModifyTrialPhase(TrialState state, int delta)
		{
			for (int i = this.TrialPhases.Count - 1; i >= 0; i--)
			{
				if (this.TrialPhases[i].State == state)
				{
					this.TrialPhases[i].Duration += delta;
				}
			}
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x00077CC6 File Offset: 0x00075EC6
		public TrialPhase GetCurrentTrialPhase()
		{
			if (this.TrialPhases.Count < this.CurrentPhaseIndex)
			{
				return this.TrialPhases[this.CurrentPhaseIndex];
			}
			return null;
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x00077CEE File Offset: 0x00075EEE
		public override void DeepClone(out VictoryRuleProcessor clone)
		{
			clone = new VictoryRuleProcessorTrialOfTheThrone
			{
				TrialState = this.TrialState,
				TrialPhases = this.TrialPhases.DeepClone<TrialPhase>(),
				CurrentPhaseIndex = this.CurrentPhaseIndex,
				CurrentPhaseTurn = this.CurrentPhaseTurn
			};
		}

		// Token: 0x04000F26 RID: 3878
		[JsonProperty]
		[DefaultValue(TrialState.Undefined)]
		public TrialState TrialState;

		// Token: 0x04000F27 RID: 3879
		[JsonProperty]
		public List<TrialPhase> TrialPhases = new List<TrialPhase>();

		// Token: 0x04000F28 RID: 3880
		[JsonProperty]
		[DefaultValue(0)]
		public int CurrentPhaseIndex;

		// Token: 0x04000F29 RID: 3881
		[JsonProperty]
		[DefaultValue(0)]
		public int CurrentPhaseTurn;
	}
}
