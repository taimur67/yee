using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000324 RID: 804
	public class MessageTrigger_OnRitualCastOnTarget : MessageTriggerCondition
	{
		// Token: 0x06000F8E RID: 3982 RVA: 0x0003DA58 File Offset: 0x0003BC58
		public override string GetDescription()
		{
			return string.Concat(new string[]
			{
				"Message <",
				this.RecipientArchfiendId,
				"> when they have a <",
				this.CommonRitualId,
				"> on them by <",
				this.SourceArchfiendId,
				">"
			});
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0003DAAC File Offset: 0x0003BCAC
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.SourceArchfiendId);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (RitualCastEvent ritualCastEvent in context.CurrentTurn.GetGameEvents().OfType<RitualCastEvent>())
			{
				if (ritualCastEvent.TriggeringPlayerID == playerState.Id && ritualCastEvent.AffectedPlayerID == playerState2.Id && !ritualCastEvent.RitualId.Contains(this.CommonRitualId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000741 RID: 1857
		public string SourceArchfiendId;

		// Token: 0x04000742 RID: 1858
		public string CommonRitualId = string.Empty;
	}
}
