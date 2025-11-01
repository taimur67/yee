using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000472 RID: 1138
	[Serializable]
	public class StealCantonSchemeGenerator : DynamicSchemeGenerator
	{
		// Token: 0x06001535 RID: 5429 RVA: 0x00050197 File Offset: 0x0004E397
		protected override IEnumerable<SchemeObjective> GenerateSchemesInternal(TurnContext context, PlayerState player)
		{
			yield return new SchemeObjective(this.GetPossibleObjectives(context, player));
			yield break;
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x000501B5 File Offset: 0x0004E3B5
		private IEnumerable<ObjectiveCondition_StealCantonOfType> GetPossibleObjectives(TurnContext context, PlayerState player)
		{
			return from terrainType in this.GetTerrainTypes(context, player)
			select new ObjectiveCondition_StealCantonOfType(terrainType, false);
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x000501E4 File Offset: 0x0004E3E4
		private IEnumerable<TerrainType> GetTerrainTypes(TurnContext context, PlayerState player)
		{
			return (from hex in context.HexBoard.Hexes
			group hex by hex.Type into @group
			select @group.Key).Where(new Func<TerrainType, bool>(this.CanBeStolen));
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x00050255 File Offset: 0x0004E455
		private bool CanBeStolen(TerrainType t)
		{
			return t.IsPassible() && t != TerrainType.Ruin && t != TerrainType.LandBridge;
		}
	}
}
