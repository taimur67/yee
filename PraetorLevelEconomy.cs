using System;
using System.Collections.Generic;
using Core.StaticData;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003D5 RID: 981
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorLevelEconomy : IdentifiableStaticData
	{
		// Token: 0x06001315 RID: 4885 RVA: 0x0004885D File Offset: 0x00046A5D
		public override bool OnValidate()
		{
			this._levels.SortOnValueAscending((PraetorLevelEconomy.LevelPair t) => t.Level);
			return true;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0004888A File Offset: 0x00046A8A
		public int GetNextRequiredXP(int currentLevel)
		{
			return this.GetNextLevelOrLast(currentLevel).XPRequired;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00048898 File Offset: 0x00046A98
		public bool TryGetNextLevel(int currentLevel, out PraetorLevelEconomy.LevelPair pair)
		{
			foreach (PraetorLevelEconomy.LevelPair levelPair in this._levels)
			{
				if (levelPair.Level > currentLevel)
				{
					pair = levelPair;
					return true;
				}
			}
			pair = default(PraetorLevelEconomy.LevelPair);
			return false;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00048904 File Offset: 0x00046B04
		public PraetorLevelEconomy.LevelPair GetNextLevelOrLast(int currentLevel)
		{
			PraetorLevelEconomy.LevelPair result;
			if (!this.TryGetNextLevel(currentLevel, out result))
			{
				IEnumerable<PraetorLevelEconomy.LevelPair> levels = this._levels;
				PraetorLevelEconomy.LevelPair levelPair = default(PraetorLevelEconomy.LevelPair);
				result = IEnumerableExtensions.LastOrDefault<PraetorLevelEconomy.LevelPair>(levels, ref levelPair);
			}
			return result;
		}

		// Token: 0x040008D8 RID: 2264
		[JsonProperty]
		public List<PraetorLevelEconomy.LevelPair> _levels;

		// Token: 0x02000951 RID: 2385
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public struct LevelPair
		{
			// Token: 0x040015BD RID: 5565
			[JsonProperty]
			public int Level;

			// Token: 0x040015BE RID: 5566
			[JsonProperty]
			public int XPRequired;
		}
	}
}
