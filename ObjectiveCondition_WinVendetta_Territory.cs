using System;

namespace LoG
{
	// Token: 0x020005C0 RID: 1472
	[Serializable]
	public class ObjectiveCondition_WinVendetta_Territory : ObjectiveCondition_WinVendettaAgainstPlayer
	{
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001B79 RID: 7033 RVA: 0x0005F661 File Offset: 0x0005D861
		public override string LocalizationKey
		{
			get
			{
				return "WinVendetta.Territory";
			}
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0005F668 File Offset: 0x0005D868
		protected override bool Filter(TurnContext context, VendettaCompletedEvent @event, PlayerState owner, PlayerState target)
		{
			return base.Filter(context, @event, owner, target) && @event.Objective.Conditions[0] is ObjectiveCondition_CaptureCantons;
		}
	}
}
