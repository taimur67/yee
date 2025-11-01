using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000325 RID: 805
	public class MessageTrigger_OnTargetBecameBloodVassal : MessageTriggerCondition
	{
		// Token: 0x06000F91 RID: 3985 RVA: 0x0003DB6B File Offset: 0x0003BD6B
		public override string GetDescription()
		{
			return "Message <" + this.RecipientArchfiendId + "> when they become a Blood Vassal";
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0003DB84 File Offset: 0x0003BD84
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (OfferVassalageResponseEvent offerVassalageResponseEvent in context.CurrentTurn.GetGameEvents().OfType<OfferVassalageResponseEvent>())
			{
				if (offerVassalageResponseEvent.TriggeringPlayerID == playerState.Id && offerVassalageResponseEvent.Response == YesNo.Yes)
				{
					return true;
				}
			}
			return false;
		}
	}
}
