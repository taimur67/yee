using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000594 RID: 1428
	public class ObjectiveCondition_LastPlayerAlive : ObjectiveCondition
	{
		// Token: 0x06001B11 RID: 6929 RVA: 0x0005E628 File Offset: 0x0005C828
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			List<PlayerState> list = IEnumerableExtensions.ToList<PlayerState>(context.CurrentTurn.EnumeratePlayerStates(false, false));
			if (list.Count > 1)
			{
				return 0;
			}
			ArchFiendStaticData archFiendStaticData = context.Database.Fetch(this.RemainingArchfiend);
			if (!(list[0].ArchfiendId == archFiendStaticData.Id))
			{
				return 0;
			}
			return this.Target;
		}

		// Token: 0x04000C49 RID: 3145
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> RemainingArchfiend;
	}
}
