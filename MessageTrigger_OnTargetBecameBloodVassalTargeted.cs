using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000326 RID: 806
	public class MessageTrigger_OnTargetBecameBloodVassalTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F94 RID: 3988 RVA: 0x0003DC0C File Offset: 0x0003BE0C
		public override string GetDescription()
		{
			return string.Concat(new string[]
			{
				"Message <",
				this.RecipientArchfiendId,
				"> when they a Blood Vassal of <",
				this.TargetArchfiendId,
				">"
			});
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0003DC44 File Offset: 0x0003BE44
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			DiplomaticPairStatus diplomaticStatus = context.CurrentTurn.GetDiplomaticStatus(playerState2.Id, playerState.Id);
			if (diplomaticStatus.DiplomaticState.Type != DiplomaticStateValue.BloodVassalage)
			{
				return false;
			}
			BloodVassalageState bloodVassalageState = (BloodVassalageState)diplomaticStatus.DiplomaticState;
			return bloodVassalageState != null && bloodVassalageState.BloodLordId == playerState2.Id;
		}

		// Token: 0x04000743 RID: 1859
		public string TargetArchfiendId;
	}
}
