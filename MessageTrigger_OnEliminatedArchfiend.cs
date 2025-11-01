using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200031A RID: 794
	public class MessageTrigger_OnEliminatedArchfiend : MessageTriggerCondition
	{
		// Token: 0x06000F70 RID: 3952 RVA: 0x0003D215 File Offset: 0x0003B415
		public override string GetDescription()
		{
			return "Message <" + this.RecipientArchfiendId + "> when they eliminated any archfiend";
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0003D22C File Offset: 0x0003B42C
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			using (IEnumerator<PlayerEliminatedEvent> enumerator = context.CurrentTurn.GetGameEvents().OfType<PlayerEliminatedEvent>().GetEnumerator())
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
