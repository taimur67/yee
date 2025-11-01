using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005BF RID: 1471
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_WinVendettaInTurns : ObjectiveCondition_EventFilter<VendettaCompletedEvent>
	{
		// Token: 0x06001B77 RID: 7031 RVA: 0x0005F632 File Offset: 0x0005D832
		protected override bool Filter(TurnContext context, VendettaCompletedEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.Successful && @event.TurnsTaken <= this.MinimumTurns && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C61 RID: 3169
		[JsonProperty]
		public int MinimumTurns;
	}
}
