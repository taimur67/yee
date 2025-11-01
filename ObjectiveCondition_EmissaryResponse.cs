using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200057F RID: 1407
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_EmissaryResponse : ObjectiveCondition_EventFilter<SendEmissaryResponseEvent>
	{
		// Token: 0x06001ACF RID: 6863 RVA: 0x0005DA13 File Offset: 0x0005BC13
		protected override bool Filter(TurnContext context, SendEmissaryResponseEvent @event, PlayerState owner, PlayerState target)
		{
			return (this.ExpectedResponse == YesNo.Undefined || @event.Response == this.ExpectedResponse) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C31 RID: 3121
		[JsonProperty]
		[DefaultValue(YesNo.Undefined)]
		public YesNo ExpectedResponse;
	}
}
