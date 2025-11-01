using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000321 RID: 801
	public class MessageTrigger_OnTargetDiplomaticActionTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F85 RID: 3973 RVA: 0x0003D78F File Offset: 0x0003B98F
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when they make a <{1}> against <{2}>", this.RecipientArchfiendId, this.DiplomaticOrder, this.TargetArchfiendId);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			foreach (DiplomaticEvent diplomaticEvent in context.CurrentTurn.GetGameEvents().OfType<DiplomaticEvent>())
			{
				if (diplomaticEvent.TriggeringPlayerID == playerState.Id && diplomaticEvent.AffectedPlayerID == playerState2.Id && diplomaticEvent.OrderType == this.DiplomaticOrder)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400073C RID: 1852
		public OrderTypes DiplomaticOrder = OrderTypes.Demand;

		// Token: 0x0400073D RID: 1853
		public string TargetArchfiendId;
	}
}
