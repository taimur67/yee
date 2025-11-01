using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000311 RID: 785
	public class MessageTrigger_OnTargetUpgradedRank : MessageTriggerCondition
	{
		// Token: 0x06000F53 RID: 3923 RVA: 0x0003CC80 File Offset: 0x0003AE80
		public override string GetDescription()
		{
			return "Message <" + this.RecipientArchfiendId + "> when they increase their Rank";
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0003CC98 File Offset: 0x0003AE98
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			using (IEnumerator<RankIncreaseEvent> enumerator = context.CurrentTurn.GetGameEvents().OfType<RankIncreaseEvent>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TriggeringPlayerID == playerState.Id)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
