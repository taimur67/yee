using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000323 RID: 803
	public class MessageTrigger_OnTargetCastRitualTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F8B RID: 3979 RVA: 0x0003D944 File Offset: 0x0003BB44
		public override string GetDescription()
		{
			return string.Concat(new string[]
			{
				"Message <",
				this.RecipientArchfiendId,
				"> when they cast a <",
				this.CommonRitualId,
				"> Ritual against <",
				this.TargetArchfiendId,
				">"
			});
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0003D998 File Offset: 0x0003BB98
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			foreach (RitualCastEvent ritualCastEvent in context.CurrentTurn.GetGameEvents().OfType<RitualCastEvent>())
			{
				if (ritualCastEvent.TriggeringPlayerID == playerState.Id && ritualCastEvent.AffectedPlayerID == playerState2.Id && !ritualCastEvent.RitualId.Contains(this.CommonRitualId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400073F RID: 1855
		public string CommonRitualId = string.Empty;

		// Token: 0x04000740 RID: 1856
		public string TargetArchfiendId;
	}
}
