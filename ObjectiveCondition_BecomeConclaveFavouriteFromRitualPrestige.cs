using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200054F RID: 1359
	[Serializable]
	public class ObjectiveCondition_BecomeConclaveFavouriteFromRitualPrestige : BooleanStateObjectiveCondition
	{
		// Token: 0x06001A4F RID: 6735 RVA: 0x0005BBCC File Offset: 0x00059DCC
		protected override bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (isInitialProgress)
			{
				return false;
			}
			if (context.CurrentTurn.GetGameEvents<ConclaveFavouriteChangedEvent>().FirstOrDefault((ConclaveFavouriteChangedEvent t) => t.NewFavouriteId == owner.Id) == null)
			{
				return false;
			}
			IEnumerable<RitualCastEvent> source = from t in context.CurrentTurn.GetGameEvents<RitualCastEvent>()
			where t.TriggeringPlayerID == owner.Id
			select t;
			if (!this.Ritual.IsEmpty())
			{
				source = from t in source
				where t.RitualId == this.Ritual.Id
				select t;
			}
			return IEnumerableExtensions.Accumulate<PaymentReceivedEvent>(source.SelectMany((RitualCastEvent t) => t.Enumerate<PaymentReceivedEvent>()), (PaymentReceivedEvent t) => (float)t.Offering.Prestige) > 0f;
		}

		// Token: 0x04000BE6 RID: 3046
		[JsonProperty]
		public ConfigRef<RitualStaticData> Ritual = ConfigRef<RitualStaticData>.Empty;
	}
}
