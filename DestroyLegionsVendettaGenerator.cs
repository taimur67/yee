using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200052E RID: 1326
	public class DestroyLegionsVendettaGenerator : StandardVendettaObjectiveGenerator
	{
		// Token: 0x060019CB RID: 6603 RVA: 0x0005A64F File Offset: 0x0005884F
		public override IEnumerable<ObjectiveCondition> CreateConditions(TurnState turn, PlayerState target)
		{
			yield return new ObjectiveCondition_DestroyLegions
			{
				AllowVassalContributions = true,
				TargetingPlayer = new int?(target.Id),
				Target = this.Target,
				AllowedCategories = new List<GamePieceCategory>
				{
					GamePieceCategory.Legion,
					GamePieceCategory.Titan
				}
			};
			yield break;
		}

		// Token: 0x04000BC2 RID: 3010
		[JsonProperty]
		public int Target;
	}
}
