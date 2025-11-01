using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000322 RID: 802
	public class MessageTrigger_OnTargetCastRitual : MessageTriggerCondition
	{
		// Token: 0x06000F88 RID: 3976 RVA: 0x0003D86B File Offset: 0x0003BA6B
		public override string GetDescription()
		{
			return string.Concat(new string[]
			{
				"Message <",
				this.RecipientArchfiendId,
				"> when they cast a <",
				this.CommonRitualId,
				"> Ritual"
			});
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0003D8A4 File Offset: 0x0003BAA4
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (RitualCastEvent ritualCastEvent in context.CurrentTurn.GetGameEvents().OfType<RitualCastEvent>())
			{
				if (ritualCastEvent.TriggeringPlayerID == playerState.Id && !ritualCastEvent.RitualId.Contains(this.CommonRitualId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400073E RID: 1854
		public string CommonRitualId = string.Empty;
	}
}
