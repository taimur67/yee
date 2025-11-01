using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000327 RID: 807
	public class MessageTrigger_OnTargetIncreasedPowerToLevel : MessageTriggerCondition
	{
		// Token: 0x06000F97 RID: 3991 RVA: 0x0003DCC4 File Offset: 0x0003BEC4
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when they increase their <{1}> to <{2}>", this.RecipientArchfiendId, this.PowerType, this.TargetLevel);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0003DCEC File Offset: 0x0003BEEC
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (PowerUpgradedEvent powerUpgradedEvent in context.CurrentTurn.GetGameEvents().OfType<PowerUpgradedEvent>())
			{
				if (powerUpgradedEvent.TriggeringPlayerID == playerState.Id && powerUpgradedEvent.PowerType == this.PowerType && powerUpgradedEvent.Level == this.TargetLevel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000744 RID: 1860
		public PowerType PowerType = PowerType.Charisma;

		// Token: 0x04000745 RID: 1861
		public int TargetLevel = 6;
	}
}
