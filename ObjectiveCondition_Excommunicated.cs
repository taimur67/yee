using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000587 RID: 1415
	[Serializable]
	public class ObjectiveCondition_Excommunicated : ObjectiveCondition_EventFilter<PlayerExcommunicatedEvent>
	{
		// Token: 0x06001AEF RID: 6895 RVA: 0x0005DF7F File Offset: 0x0005C17F
		protected override bool Filter(TurnContext context, PlayerExcommunicatedEvent @event, PlayerState owner, PlayerState target)
		{
			return (this.Reason == ExcommunicationReason.Unknown || this.Reason == @event.Reason) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C3F RID: 3135
		[JsonProperty]
		[DefaultValue(ExcommunicationReason.Unknown)]
		public ExcommunicationReason Reason;
	}
}
