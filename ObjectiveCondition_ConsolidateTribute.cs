using System;

namespace LoG
{
	// Token: 0x02000578 RID: 1400
	[Serializable]
	public class ObjectiveCondition_ConsolidateTribute : ObjectiveCondition_EventFilter<ConsolidateTributeEvent>
	{
		// Token: 0x06001ABC RID: 6844 RVA: 0x0005D6C5 File Offset: 0x0005B8C5
		protected override bool Filter(TurnContext context, ConsolidateTributeEvent @event, PlayerState owner, PlayerState target)
		{
			return !@event.ResultResource.Values.AnyLessThan(this.MinValues, false) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x0005D6ED File Offset: 0x0005B8ED
		public override int GetHashCode()
		{
			return (base.GetHashCode() * 23 + this.MinValues) * 29;
		}

		// Token: 0x04000C22 RID: 3106
		[BindableValue("target_value", BindingOption.None)]
		public int MinValues;
	}
}
