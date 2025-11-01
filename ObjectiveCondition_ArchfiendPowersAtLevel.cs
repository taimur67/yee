using System;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000549 RID: 1353
	public class ObjectiveCondition_ArchfiendPowersAtLevel : ObjectiveCondition
	{
		// Token: 0x06001A3C RID: 6716 RVA: 0x0005B78B File Offset: 0x0005998B
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return owner.PowersLevels.Powers.Count((PlayerPowerLevel t) => t.CurrentLevel >= this.TargetLevel);
		}

		// Token: 0x04000BE4 RID: 3044
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public int TargetLevel;
	}
}
