using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000320 RID: 800
	public class MessageTrigger_OnTargetDiplomaticAction : MessageTriggerCondition
	{
		// Token: 0x06000F82 RID: 3970 RVA: 0x0003D6D8 File Offset: 0x0003B8D8
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when they make a <{1}> diplomatic action", this.RecipientArchfiendId, this.DiplomaticOrder);
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0003D6F8 File Offset: 0x0003B8F8
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (DiplomaticEvent diplomaticEvent in context.CurrentTurn.GetGameEvents().OfType<DiplomaticEvent>())
			{
				if (diplomaticEvent.TriggeringPlayerID == playerState.Id && diplomaticEvent.OrderType == this.DiplomaticOrder)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400073B RID: 1851
		public OrderTypes DiplomaticOrder = OrderTypes.Demand;
	}
}
