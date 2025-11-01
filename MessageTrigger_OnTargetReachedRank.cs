using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000312 RID: 786
	public class MessageTrigger_OnTargetReachedRank : MessageTriggerCondition
	{
		// Token: 0x06000F56 RID: 3926 RVA: 0x0003CD18 File Offset: 0x0003AF18
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when they reach <Rank {1}>", this.RecipientArchfiendId, this.TargetRank);
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0003CD38 File Offset: 0x0003AF38
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (RankIncreaseEvent rankIncreaseEvent in context.CurrentTurn.GetGameEvents().OfType<RankIncreaseEvent>())
			{
				if (rankIncreaseEvent.TriggeringPlayerID == playerState.Id && rankIncreaseEvent.NewRank == this.TargetRank)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400072E RID: 1838
		public Rank TargetRank = Rank.Prince;
	}
}
