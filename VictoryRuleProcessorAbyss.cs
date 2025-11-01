using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006FB RID: 1787
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VictoryRuleProcessorAbyss : VictoryRuleProcessor<VictoryRuleAbyss>
	{
		// Token: 0x0600223D RID: 8765 RVA: 0x000772E0 File Offset: 0x000754E0
		public override bool Process(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			GamePiece pandaemonium = currentTurn.GetPandaemonium();
			if (pandaemonium.Status != GameItemStatus.InPlay)
			{
				return true;
			}
			if (!IEnumerableExtensions.Any<PlayerState>(currentTurn.GetNonEliminatedPlayers()))
			{
				return true;
			}
			bool flag = pandaemonium.IsOwned();
			bool flag2 = IEnumerableExtensions.Any<PlayerState>(currentTurn.GetConclaveMemberPlayers());
			if (flag2 || flag)
			{
				if (this.IsAbyssMode)
				{
					this.Reset(currentTurn);
					if (flag2)
					{
						currentTurn.AddGameEvent<ConclaveRestoredEvent>(new ConclaveRestoredEvent());
					}
				}
				return false;
			}
			bool flag3 = !this.IsAbyssMode;
			currentTurn.SetTurnPhase(TurnPhase.Abyss);
			this.CountdownStep(currentTurn);
			if (this.IsPandaemoniumEliminatedThisTurn(currentTurn))
			{
				return true;
			}
			if (flag3)
			{
				ConclaveDissolvedEvent gameEvent = new ConclaveDissolvedEvent();
				currentTurn.AddGameEvent<ConclaveDissolvedEvent>(gameEvent);
				foreach (DiplomaticPairStatus diplomaticPairStatus in context.Diplomacy.Standings)
				{
					DiplomaticState diplomaticState = diplomaticPairStatus.DiplomaticState;
					if (!(diplomaticState is ExcommunicatedState) && !(diplomaticState is EliminatedState) && !(diplomaticState is BloodFeudState))
					{
						diplomaticPairStatus.SetNeutral(context, true);
					}
				}
				context.RemoveModuleProcessorOfType<VoteCandidateTurnModuleProcessor>();
				context.RemoveModuleProcessorOfType<VotePolicyTurnModuleProcessor>();
			}
			else
			{
				currentTurn.AddGameEvent<AbyssVictoryProgresses>(new AbyssVictoryProgresses());
				context.RemoveModuleProcessorOfType<VoteCandidateTurnModuleProcessor>();
				context.RemoveModuleProcessorOfType<VotePolicyTurnModuleProcessor>();
			}
			return false;
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x00077420 File Offset: 0x00075620
		private void Reset(TurnState turnState)
		{
			this.IsAbyssMode = false;
			turnState.StriderCarryCapacityMultiplier = 1;
			turnState.GetPandaemonium().HealingBonus = 1;
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00077444 File Offset: 0x00075644
		private void CountdownStep(TurnState turnState)
		{
			this.IsAbyssMode = true;
			turnState.StriderCarryCapacityMultiplier = 2;
			GamePiece pandaemonium = turnState.GetPandaemonium();
			pandaemonium.HealingBonus = 0;
			pandaemonium.TotalHP = (int)((double)pandaemonium.TotalHP * 0.5);
			pandaemonium.HP = Math.Min(pandaemonium.HP, pandaemonium.TotalHP);
			pandaemonium.CombatStats.Melee = (int)((double)pandaemonium.CombatStats.Melee * 0.5);
			pandaemonium.CombatStats.Infernal = (int)((double)pandaemonium.CombatStats.Infernal * 0.5);
			pandaemonium.CombatStats.Ranged = (int)((double)pandaemonium.CombatStats.Ranged * 0.5);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00077534 File Offset: 0x00075734
		private bool IsPandaemoniumEliminatedThisTurn(TurnState turnState)
		{
			return turnState.GetPandaemonium().HP <= 0;
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00077547 File Offset: 0x00075747
		public override GameVictory DecideWinner(TurnProcessContext context)
		{
			return new GameVictory(VictoryType.Abyss, new List<int>(), VictoryManipulationType.None, int.MinValue);
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0007755A File Offset: 0x0007575A
		public override string GetDebugName()
		{
			return "Triumph of the Abyss game end";
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x00077561 File Offset: 0x00075761
		public override bool IsInEndGame()
		{
			return this.IsAbyssMode;
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x00077569 File Offset: 0x00075769
		public override void DeepClone(out VictoryRuleProcessor clone)
		{
			clone = new VictoryRuleProcessorAbyss
			{
				IsAbyssMode = this.IsAbyssMode
			};
		}

		// Token: 0x04000F18 RID: 3864
		[JsonProperty]
		[DefaultValue(false)]
		public bool IsAbyssMode;
	}
}
