using System;
using Game.Simulation.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000431 RID: 1073
	public static class CardGenerationDataExtensions
	{
		// Token: 0x060014D1 RID: 5329 RVA: 0x0004F643 File Offset: 0x0004D843
		public static TributeQualityStaticData GetSelection(this CardGenerationData data, GameDatabase database, int qualityLevel)
		{
			return database.Enumerate(data.TributeDemandQualitySelections).SelectClosestUnderOrEqual((TributeQualityStaticData t) => t.TributeQuality, qualityLevel);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004F676 File Offset: 0x0004D876
		public static TributeQualityStaticData GetSelection(this CardGenerationData data, PlayerState player, GameDatabase database)
		{
			return data.GetSelection(database, player.TributeQuality);
		}
	}
}
