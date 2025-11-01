using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000750 RID: 1872
	public static class VotingProcessor
	{
		// Token: 0x0600231B RID: 8987 RVA: 0x00079F70 File Offset: 0x00078170
		public static void ProcessTurn(TurnProcessContext context)
		{
			GameRules rules = context.Rules;
			TurnState currentTurn = context.CurrentTurn;
			GameDatabase database = context.Database;
			int votingInterval = rules.VotingInterval;
			int votingFirstTurn = rules.VotingFirstTurn;
			int votingProclamationOffset = rules.VotingProclamationOffset;
			int noNewEdictsFromTurn = rules.NoNewEdictsFromTurn;
			if (currentTurn.TurnPhase != TurnPhase.None)
			{
				return;
			}
			if (votingInterval == 0)
			{
				return;
			}
			int num = currentTurn.TurnValue + 1;
			if (num >= noNewEdictsFromTurn)
			{
				return;
			}
			if (num < votingFirstTurn - votingProclamationOffset)
			{
				return;
			}
			if ((num - votingFirstTurn + votingProclamationOffset) % votingInterval == 0)
			{
				EdictStaticData nextEdict = context.GetNextEdict();
				VotingProcessor.CallVote(context, nextEdict, votingProclamationOffset, false);
				return;
			}
			if (num < votingFirstTurn)
			{
				return;
			}
			if (currentTurn.EmergencyVoteCount >= rules.EmergencyVoteMax)
			{
				return;
			}
			float num2 = (float)(num - votingFirstTurn) * rules.EmergencyVoteChanceIncrement;
			if (currentTurn.Random.NextFloat() <= num2)
			{
				EdictStaticData nextEdict2 = context.GetNextEdict();
				VotingProcessor.CallVote(context, nextEdict2, 0, true);
				currentTurn.EmergencyVoteCount++;
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0007A050 File Offset: 0x00078250
		public static void CallVote(TurnProcessContext context, EdictStaticData edict, int deliberationTurns, bool unannounced = false)
		{
			TurnState currentTurn = context.CurrentTurn;
			EdictType edictType = edict.EdictType;
			VoteTurnModuleInstance voteTurnModuleInstance;
			if (edictType != EdictType.Policy)
			{
				if (edictType != EdictType.Candidate)
				{
					throw new ArgumentOutOfRangeException();
				}
				voteTurnModuleInstance = TurnModuleInstanceFactory.CreateInstance<VoteCandidateTurnModuleInstance>(currentTurn);
			}
			else
			{
				voteTurnModuleInstance = TurnModuleInstanceFactory.CreateInstance<VotePolicyTurnModuleInstance>(currentTurn);
			}
			VoteTurnModuleInstance voteTurnModuleInstance2 = voteTurnModuleInstance;
			voteTurnModuleInstance2.EdictId = edict.Id;
			voteTurnModuleInstance2.Unannounced = unannounced;
			voteTurnModuleInstance2.VoteTurn = currentTurn.TurnValue + deliberationTurns;
			currentTurn.AddActiveTurnModule(context, voteTurnModuleInstance2);
		}
	}
}
