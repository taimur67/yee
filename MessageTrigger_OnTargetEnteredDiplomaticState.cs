using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200032B RID: 811
	public class MessageTrigger_OnTargetEnteredDiplomaticState : MessageTriggerCondition
	{
		// Token: 0x06000FA4 RID: 4004 RVA: 0x0003DF54 File Offset: 0x0003C154
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when <{1}> enters diplomatic <{2}> against <{3}>", new object[]
			{
				this.RecipientArchfiendId,
				this.TargetArchfiendId,
				this.DiplomaticState,
				this.OtherArchfiendId
			});
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0003DF90 File Offset: 0x0003C190
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(this.OtherArchfiendId);
			return context.CurrentTurn.GetDiplomaticStatus(playerState.Id, playerState2.Id).DiplomaticState.Type == this.DiplomaticState;
		}

		// Token: 0x0400074B RID: 1867
		public string TargetArchfiendId;

		// Token: 0x0400074C RID: 1868
		public string OtherArchfiendId;

		// Token: 0x0400074D RID: 1869
		public DiplomaticStateValue DiplomaticState;
	}
}
