using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200031F RID: 799
	public class MessageTrigger_OnTargetWonPraetorDuel : MessageTriggerCondition
	{
		// Token: 0x06000F7F RID: 3967 RVA: 0x0003D620 File Offset: 0x0003B820
		public override string GetDescription()
		{
			return "Message <" + this.RecipientArchfiendId + "> when they win a Praetor Duel";
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0003D638 File Offset: 0x0003B838
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (PraetorDuelOutcomeEvent praetorDuelOutcomeEvent in context.CurrentTurn.GetGameEvents().OfType<PraetorDuelOutcomeEvent>())
			{
				if (praetorDuelOutcomeEvent.AffectedPlayerIds.Contains(playerState.Id) && praetorDuelOutcomeEvent.Winner != null && praetorDuelOutcomeEvent.Winner.PlayerId == playerState.Id)
				{
					return true;
				}
			}
			return false;
		}
	}
}
