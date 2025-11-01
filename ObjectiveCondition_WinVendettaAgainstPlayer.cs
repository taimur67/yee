using System;

namespace LoG
{
	// Token: 0x020005BE RID: 1470
	[Serializable]
	public class ObjectiveCondition_WinVendettaAgainstPlayer : ObjectiveCondition_EventFilter<VendettaCompletedEvent>
	{
		// Token: 0x06001B74 RID: 7028 RVA: 0x0005F5FF File Offset: 0x0005D7FF
		public ObjectiveCondition_WinVendettaAgainstPlayer()
		{
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0005F607 File Offset: 0x0005D807
		public ObjectiveCondition_WinVendettaAgainstPlayer(int playerId)
		{
			base.TargetingPlayer = new int?(playerId);
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0005F61B File Offset: 0x0005D81B
		protected override bool Filter(TurnContext context, VendettaCompletedEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.Successful && base.Filter(context, @event, owner, target);
		}
	}
}
