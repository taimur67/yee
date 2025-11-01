using System;

namespace LoG
{
	// Token: 0x02000589 RID: 1417
	[Serializable]
	public class ObjectiveCondition_FrameOpponentForRitual : ObjectiveCondition_SuccessfullyCastRitual<RitualCastEvent>
	{
		// Token: 0x06001AF3 RID: 6899 RVA: 0x0005E030 File Offset: 0x0005C230
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			return base.Filter(context, @event, owner, target) && @event.MaskingContext.MaskingMode == RitualMaskingMode.Framed && @event.MaskingContext.MaskingSuccessful;
		}
	}
}
