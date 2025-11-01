using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000318 RID: 792
	public class MessageTrigger_OnTargetLiberatedPandaemonium : MessageTriggerCondition
	{
		// Token: 0x06000F6A RID: 3946 RVA: 0x0003D154 File Offset: 0x0003B354
		public override string GetDescription()
		{
			return "Message <" + this.RecipientArchfiendId + "> if they liberate Pandaemonium";
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0003D16C File Offset: 0x0003B36C
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			using (IEnumerator<PandaemoniumReturnedToConclaveEvent> enumerator = context.CurrentTurn.GetGameEvents().OfType<PandaemoniumReturnedToConclaveEvent>().GetEnumerator())
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
