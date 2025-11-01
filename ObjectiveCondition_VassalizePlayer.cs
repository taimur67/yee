using System;

namespace LoG
{
	// Token: 0x020005AF RID: 1455
	[Serializable]
	public class ObjectiveCondition_VassalizePlayer : ObjectiveCondition_EventFilter<DiplomaticResponseEvent>
	{
		// Token: 0x06001B48 RID: 6984 RVA: 0x0005ECB0 File Offset: 0x0005CEB0
		protected override bool Filter(TurnContext context, DiplomaticResponseEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.Response == YesNo.Yes && (@event is OfferVassalageResponseEvent || @event is OfferLordshipResponseEvent) && base.Filter(context, @event, owner, target);
		}
	}
}
