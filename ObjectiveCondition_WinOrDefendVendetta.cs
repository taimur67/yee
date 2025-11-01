using System;

namespace LoG
{
	// Token: 0x020005BD RID: 1469
	public class ObjectiveCondition_WinOrDefendVendetta : ObjectiveCondition_BasicEventFilter<VendettaCompletedEvent>
	{
		// Token: 0x06001B72 RID: 7026 RVA: 0x0005F5C7 File Offset: 0x0005D7C7
		protected override bool Filter(TurnContext context, PlayerState owner, VendettaCompletedEvent @event)
		{
			if (!@event.IsAssociatedWith(owner.Id))
			{
				return false;
			}
			if (@event.TriggeringPlayerID == owner.Id)
			{
				return @event.Successful;
			}
			return !@event.Successful;
		}
	}
}
