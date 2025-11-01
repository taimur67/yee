using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000562 RID: 1378
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_CompleteScheme : ObjectiveCondition_EventFilter<SchemeCompleteEvent>
	{
		// Token: 0x06001A82 RID: 6786 RVA: 0x0005C74C File Offset: 0x0005A94C
		protected override bool Filter(TurnContext context, SchemeCompleteEvent @event, PlayerState owner, PlayerState target)
		{
			return (this.Restriction != SchemeTypeRestriction.Grand || @event.Scheme.IsGrandScheme) && (this.Restriction != SchemeTypeRestriction.Simple || @event.Scheme.IsSimpleScheme) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C04 RID: 3076
		[JsonProperty]
		[DefaultValue(SchemeTypeRestriction.Any)]
		public SchemeTypeRestriction Restriction;
	}
}
