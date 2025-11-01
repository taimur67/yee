using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000701 RID: 1793
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VictoryRuleProcessorUsurp : VictoryRuleProcessor<VictoryRuleUsurp>
	{
		// Token: 0x06002264 RID: 8804 RVA: 0x00077D5F File Offset: 0x00075F5F
		public override void Init(VictoryRuleUsurp victoryRule, TurnState turnState)
		{
			base.Init(victoryRule, turnState);
			this.ResetPandaemoniumOwner(turnState);
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x00077D70 File Offset: 0x00075F70
		public override bool Process(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameRules rules = context.Rules;
			GamePiece pandaemonium = currentTurn.GetPandaemonium();
			if (pandaemonium == null)
			{
				return false;
			}
			if (this.IsUsurpEliminationVictory(currentTurn))
			{
				this.ResetPandaemoniumOwner(currentTurn);
				return true;
			}
			bool flag = pandaemonium.ControllingPlayerId != this.CurrentPandaemoniumOwner;
			bool flag2 = pandaemonium.ControllingPlayerId != -1;
			if (flag2)
			{
				currentTurn.SetTurnPhase(TurnPhase.Usurper);
			}
			this.ResetPandaemoniumOwner(currentTurn);
			if (flag)
			{
				if (flag2)
				{
					this.StartCountdown(rules, currentTurn);
					int turns = this.GameEndsOnTurn - currentTurn.TurnValue;
					PandaemoniumCapturedEvent gameEvent = new PandaemoniumCapturedEvent(pandaemonium.ControllingPlayerId, this.CurrentPandaemoniumOwner, pandaemonium, pandaemonium.LastDefeatedBy, turns);
					currentTurn.AddGameEvent<PandaemoniumCapturedEvent>(gameEvent);
					context.RemoveModuleProcessorOfType<VoteCandidateTurnModuleProcessor>();
					context.RemoveModuleProcessorOfType<VotePolicyTurnModuleProcessor>();
				}
				else
				{
					this.IsCountingDown = false;
					this.GameEndsOnTurn = -1;
				}
			}
			else if (flag2)
			{
				if (this.CheckCountdown(currentTurn))
				{
					return true;
				}
				int turnsUntilGameEnds = this.GameEndsOnTurn - currentTurn.TurnValue;
				currentTurn.AddGameEvent<UsurpVictoryProgresses>(new UsurpVictoryProgresses(turnsUntilGameEnds, this.CurrentPandaemoniumOwner));
				context.RemoveModuleProcessorOfType<VoteCandidateTurnModuleProcessor>();
				context.RemoveModuleProcessorOfType<VotePolicyTurnModuleProcessor>();
				return false;
			}
			return false;
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x00077E7C File Offset: 0x0007607C
		private bool IsUsurpEliminationVictory(TurnState currentTurn)
		{
			return IEnumerableExtensions.ToList<PlayerState>(currentTurn.GetNonEliminatedPlayers()).Count == 1 && currentTurn.GetPandaemonium() != null && currentTurn.GetPandaemonium().IsOwned();
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x00077EA9 File Offset: 0x000760A9
		private void StartCountdown(GameRules rules, TurnState turnState)
		{
			this.GameEndsOnTurn = this.CalcFinalTurnValue(rules, turnState);
			this.IsCountingDown = true;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00077EC0 File Offset: 0x000760C0
		public int CalcFinalTurnValue(GameRules rules, TurnState turnState)
		{
			int val = rules.HoldPandaemonium - Math.Max(0, turnState.PandaemoniumCapturedCount - 1);
			return turnState.TurnValue + Math.Max(1, val);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x00077EF1 File Offset: 0x000760F1
		private bool CheckCountdown(TurnState turnState)
		{
			return this.IsCountingDown && turnState.TurnValue >= this.GameEndsOnTurn;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00077F10 File Offset: 0x00076110
		private void ResetPandaemoniumOwner(TurnState turnState)
		{
			GamePiece pandaemonium = turnState.GetPandaemonium();
			if (pandaemonium != null)
			{
				this.CurrentPandaemoniumOwner = pandaemonium.ControllingPlayerId;
			}
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00077F34 File Offset: 0x00076134
		public override GameVictory DecideWinner(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			int currentPandaemoniumOwner = this.CurrentPandaemoniumOwner;
			VictoryType victoryType = VictoryType.Pandaemonium;
			List<PlayerState> list = IEnumerableExtensions.ToList<PlayerState>(currentTurn.EnumeratePlayerStates(false, true));
			base.SortByWorthiness(currentTurn, list);
			list.Reverse();
			List<int> list2 = IEnumerableExtensions.ToList<int>(from x in list
			select x.Id);
			list2.Remove(currentPandaemoniumOwner);
			list2.Insert(0, currentPandaemoniumOwner);
			int num;
			if (currentTurn.CurrentDiplomaticTurn.IsBloodLordOfAny(currentPandaemoniumOwner, out num))
			{
				PlayerState player = currentTurn.PlayerStates[num];
				if (this.IsEligibleForUsurp(player))
				{
					list2.Remove(num);
					list2.Insert(1, num);
				}
			}
			return new GameVictory(victoryType, list2, VictoryManipulationType.None, int.MinValue);
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x00077FEE File Offset: 0x000761EE
		public bool IsEligibleForUsurp(PlayerState player)
		{
			return !player.Eliminated;
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00077FF9 File Offset: 0x000761F9
		public override bool IsInEndGame()
		{
			return this.IsCountingDown;
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x00078001 File Offset: 0x00076201
		public override string GetDebugName()
		{
			return "Capture Pandaemonium";
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x00078008 File Offset: 0x00076208
		public override void DeepClone(out VictoryRuleProcessor clone)
		{
			clone = new VictoryRuleProcessorUsurp
			{
				IsCountingDown = this.IsCountingDown,
				CurrentPandaemoniumOwner = this.CurrentPandaemoniumOwner,
				GameEndsOnTurn = this.GameEndsOnTurn
			};
		}

		// Token: 0x04000F2A RID: 3882
		[JsonProperty]
		[DefaultValue(false)]
		public bool IsCountingDown;

		// Token: 0x04000F2B RID: 3883
		[JsonProperty]
		[DefaultValue(-1)]
		public int CurrentPandaemoniumOwner = -1;

		// Token: 0x04000F2C RID: 3884
		[JsonProperty]
		[DefaultValue(-1)]
		public int GameEndsOnTurn = -1;
	}
}
