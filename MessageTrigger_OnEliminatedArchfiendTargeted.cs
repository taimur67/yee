using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200031B RID: 795
	public class MessageTrigger_OnEliminatedArchfiendTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F73 RID: 3955 RVA: 0x0003D2AC File Offset: 0x0003B4AC
		public override string GetDescription()
		{
			return string.Concat(new string[]
			{
				"Message <",
				this.RecipientArchfiendId,
				"> when they eliminate <",
				this.TargetArchfiendId,
				">"
			});
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0003D2E4 File Offset: 0x0003B4E4
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			foreach (PlayerEliminatedEvent playerEliminatedEvent in context.CurrentTurn.GetGameEvents().OfType<PlayerEliminatedEvent>())
			{
				if (playerEliminatedEvent.TriggeringPlayerID == playerState.Id && playerEliminatedEvent.AffectedPlayerID == playerState2.Id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000738 RID: 1848
		public string TargetArchfiendId;
	}
}
