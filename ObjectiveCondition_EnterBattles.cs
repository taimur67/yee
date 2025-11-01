using System;

namespace LoG
{
	// Token: 0x02000581 RID: 1409
	[Serializable]
	public class ObjectiveCondition_EnterBattles : ObjectiveCondition_EventFilter<BattleEvent>
	{
		// Token: 0x06001AD3 RID: 6867 RVA: 0x0005DA87 File Offset: 0x0005BC87
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.IsAssociatedWith(owner.Id) && (target == null || @event.IsAssociatedWith(target.Id));
		}
	}
}
