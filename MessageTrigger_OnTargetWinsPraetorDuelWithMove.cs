using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200032E RID: 814
	public class MessageTrigger_OnTargetWinsPraetorDuelWithMove : MessageTriggerCondition
	{
		// Token: 0x06000FAD RID: 4013 RVA: 0x0003E180 File Offset: 0x0003C380
		public override string GetDescription()
		{
			return string.Concat(new string[]
			{
				"Message <",
				this.RecipientArchfiendId,
				"> when <",
				this.TargetArchfiendId,
				"> wins a Praetor Duel against <",
				this.OtherArchfiendId,
				"> using <",
				this.CombatMove,
				">"
			});
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0003E1E8 File Offset: 0x0003C3E8
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.OtherArchfiendId);
			foreach (PraetorDuelOutcomeEvent praetorDuelOutcomeEvent in context.CurrentTurn.GetGameEvents().OfType<PraetorDuelOutcomeEvent>())
			{
				if (praetorDuelOutcomeEvent.Winner.PlayerId == playerState.Id && praetorDuelOutcomeEvent.Loser.PlayerId == playerState.Id && !(praetorDuelOutcomeEvent.Winner.GetCombatMove(context.CurrentTurn).Id != this.CombatMove))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000752 RID: 1874
		public string TargetArchfiendId;

		// Token: 0x04000753 RID: 1875
		public string OtherArchfiendId;

		// Token: 0x04000754 RID: 1876
		public string CombatMove;
	}
}
