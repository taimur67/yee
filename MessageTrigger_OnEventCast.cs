using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200031D RID: 797
	public class MessageTrigger_OnEventCast : MessageTriggerCondition
	{
		// Token: 0x06000F79 RID: 3961 RVA: 0x0003D4DC File Offset: 0x0003B6DC
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when <{1}> has been cast", this.RecipientArchfiendId, this.EventCard);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0003D4F4 File Offset: 0x0003B6F4
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			using (IEnumerator<GrandEventPlayed> enumerator = context.CurrentTurn.GetGameEvents().OfType<GrandEventPlayed>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current.EventEffectId != this.EventCard.StaticDataId))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000739 RID: 1849
		public GameItem EventCard;
	}
}
