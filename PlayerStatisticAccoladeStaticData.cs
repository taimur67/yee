using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000423 RID: 1059
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerStatisticAccoladeStaticData : AccoladeStaticData
	{
		// Token: 0x060014BA RID: 5306 RVA: 0x0004F376 File Offset: 0x0004D576
		public override int DetermineMetric(TurnContext context, PlayerState player)
		{
			if (this._condition == null)
			{
				return 0;
			}
			return this._condition.CalculateProgress(context, player);
		}

		// Token: 0x040009E2 RID: 2530
		[JsonProperty]
		public ObjectiveCondition _condition;
	}
}
