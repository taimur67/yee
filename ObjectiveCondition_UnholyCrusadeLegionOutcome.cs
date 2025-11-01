using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005AD RID: 1453
	[Serializable]
	public class ObjectiveCondition_UnholyCrusadeLegionOutcome : ObjectiveCondition_EventFilter<UnholyCrusadeLegionReturnedEvent>
	{
		// Token: 0x06001B44 RID: 6980 RVA: 0x0005EC49 File Offset: 0x0005CE49
		protected override bool Filter(TurnContext context, UnholyCrusadeLegionReturnedEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.Survived == this.MustSurvive && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C54 RID: 3156
		[JsonProperty]
		public bool MustSurvive;
	}
}
