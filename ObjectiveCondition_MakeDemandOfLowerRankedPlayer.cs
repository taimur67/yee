using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000597 RID: 1431
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_MakeDemandOfLowerRankedPlayer : ObjeciveCondition_MakeDemand
	{
		// Token: 0x06001B17 RID: 6935 RVA: 0x0005E704 File Offset: 0x0005C904
		protected override bool Filter(TurnContext context, MakeDemandEvent @event, PlayerState owner, PlayerState target)
		{
			return base.Filter(context, @event, owner, target) && owner.Rank > target.Rank;
		}
	}
}
