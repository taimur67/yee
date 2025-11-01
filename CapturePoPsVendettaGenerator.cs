using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200052D RID: 1325
	public class CapturePoPsVendettaGenerator : StandardVendettaObjectiveGenerator
	{
		// Token: 0x060019C9 RID: 6601 RVA: 0x0005A630 File Offset: 0x00058830
		public override IEnumerable<ObjectiveCondition> CreateConditions(TurnState turn, PlayerState target)
		{
			yield return new ObjectiveCondition_CapturePlacesOfPower
			{
				AllowVassalContributions = true,
				TargetingPlayer = new int?(target.Id),
				Target = this.Target
			};
			yield break;
		}

		// Token: 0x04000BC1 RID: 3009
		[JsonProperty]
		public int Target;
	}
}
