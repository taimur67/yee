using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005B2 RID: 1458
	[Serializable]
	public class ObjectiveCondition_VoteOnWinningEdictWithActiveRitual : ObjectiveCondition_VoteOnWinningEdict
	{
		// Token: 0x06001B4E RID: 6990 RVA: 0x0005ED1D File Offset: 0x0005CF1D
		protected override bool Filter(TurnContext context, VoteRevealedEvent @event, PlayerState owner, PlayerState target)
		{
			return base.Filter(context, @event, owner, target) && context.CurrentTurn.GetActiveRituals(owner).Any((ActiveRitual current) => this.ValidActiveRituals.Any((ConfigRef<RitualStaticData> valid) => valid.Id == current.StaticDataId));
		}

		// Token: 0x04000C55 RID: 3157
		[JsonProperty]
		public List<ConfigRef<RitualStaticData>> ValidActiveRituals = new List<ConfigRef<RitualStaticData>>();
	}
}
