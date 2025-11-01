using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200046B RID: 1131
	[Serializable]
	public class IncreasePowerSchemeGenerator : DynamicSchemeGenerator
	{
		// Token: 0x0600151D RID: 5405 RVA: 0x0004FF48 File Offset: 0x0004E148
		protected override IEnumerable<SchemeObjective> GenerateSchemesInternal(TurnContext context, PlayerState player)
		{
			return from x in EnumUtility.GetValues<PowerType>()
			where x != PowerType.None
			select x into t
			select this.GenerateScheme(context, player, t);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0004FFAC File Offset: 0x0004E1AC
		protected virtual SchemeObjective GenerateScheme(TurnContext context, PlayerState player, PowerType power)
		{
			return new SchemeObjective(new ObjectiveCondition[]
			{
				new ObjectiveCondition_ArchfiendPowerLevel(power)
				{
					Target = this.TargetLevel
				}
			});
		}

		// Token: 0x04000AA2 RID: 2722
		[JsonProperty]
		public int TargetLevel;
	}
}
