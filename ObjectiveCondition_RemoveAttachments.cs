using System;

namespace LoG
{
	// Token: 0x0200056F RID: 1391
	[Serializable]
	public class ObjectiveCondition_RemoveAttachments : ObjectiveCondition_EventFilter<RitualCastEvent>
	{
		// Token: 0x06001AA6 RID: 6822 RVA: 0x0005CF14 File Offset: 0x0005B114
		protected override bool Filter(TurnContext context, RitualCastEvent ritualCastEvent, PlayerState owner, PlayerState target)
		{
			if (!ritualCastEvent.Succeeded)
			{
				return false;
			}
			int num = 0;
			foreach (GamePieceAttachmentRemovedEvent gamePieceAttachmentRemovedEvent in ritualCastEvent.Enumerate<GamePieceAttachmentRemovedEvent>())
			{
				num++;
			}
			return num >= this.RemoveCount && base.Filter(context, ritualCastEvent, owner, target);
		}

		// Token: 0x04000C10 RID: 3088
		public int RemoveCount;
	}
}
