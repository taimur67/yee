using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200055C RID: 1372
	[Serializable]
	public class ObjectiveCondition_CastRitual : ObjectiveCondition_EventFilter<RitualCastEvent>
	{
		// Token: 0x06001A75 RID: 6773 RVA: 0x0005C323 File Offset: 0x0005A523
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			return (this.Ritual.IsEmpty() || !(this.Ritual.Id != @event.RitualId)) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000BFC RID: 3068
		[JsonProperty]
		public ConfigRef<RitualStaticData> Ritual = ConfigRef<RitualStaticData>.Empty;
	}
}
