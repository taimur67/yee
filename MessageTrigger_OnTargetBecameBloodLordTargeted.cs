using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200032C RID: 812
	public class MessageTrigger_OnTargetBecameBloodLordTargeted : MessageTriggerCondition
	{
		// Token: 0x06000FA7 RID: 4007 RVA: 0x0003DFF4 File Offset: 0x0003C1F4
		public override string GetDescription()
		{
			return string.Concat(new string[]
			{
				"Message <",
				this.RecipientArchfiendId,
				"> when <",
				this.TargetArchfiendId,
				"> becomes a bloodlord of <",
				this.OtherArchfiendId,
				">"
			});
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0003E048 File Offset: 0x0003C248
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(this.OtherArchfiendId);
			DiplomaticPairStatus diplomaticStatus = context.CurrentTurn.GetDiplomaticStatus(playerState.Id, playerState2.Id);
			if (diplomaticStatus.DiplomaticState.Type != DiplomaticStateValue.BloodVassalage)
			{
				return false;
			}
			BloodVassalageState bloodVassalageState = (BloodVassalageState)diplomaticStatus.DiplomaticState;
			return bloodVassalageState != null && bloodVassalageState.BloodLordId == playerState.Id;
		}

		// Token: 0x0400074E RID: 1870
		public string TargetArchfiendId;

		// Token: 0x0400074F RID: 1871
		public string OtherArchfiendId;
	}
}
