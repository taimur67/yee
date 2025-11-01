using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200052C RID: 1324
	[Serializable]
	public class CantonCaptureVendettaGenerator : StandardVendettaObjectiveGenerator
	{
		// Token: 0x060019C7 RID: 6599 RVA: 0x0005A611 File Offset: 0x00058811
		public override IEnumerable<ObjectiveCondition> CreateConditions(TurnState turn, PlayerState target)
		{
			yield return new ObjectiveCondition_CaptureCantons
			{
				AllowVassalContributions = true,
				TargetingPlayer = new int?(target.Id),
				Target = this.Target
			};
			yield break;
		}

		// Token: 0x04000BC0 RID: 3008
		[JsonProperty]
		public int Target;
	}
}
