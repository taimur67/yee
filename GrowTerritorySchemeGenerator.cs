using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200046A RID: 1130
	[Serializable]
	public class GrowTerritorySchemeGenerator : BasicSchemeGenerator
	{
		// Token: 0x0600151B RID: 5403 RVA: 0x0004FF21 File Offset: 0x0004E121
		protected override IEnumerable<ObjectiveCondition> GenerateConditions(TurnContext context, PlayerState player)
		{
			yield return new ObjectiveCondition_ControlTerritory
			{
				Target = context.HexBoard.GetHexesControlledByPlayer(player.Id).Count<Hex>() + this.Target
			};
			yield break;
		}

		// Token: 0x04000AA1 RID: 2721
		[JsonProperty]
		public int Target;
	}
}
