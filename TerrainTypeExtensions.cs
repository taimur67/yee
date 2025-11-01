using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000479 RID: 1145
	public static class TerrainTypeExtensions
	{
		// Token: 0x06001542 RID: 5442 RVA: 0x00050410 File Offset: 0x0004E610
		public static TerrainType FromChar(char c)
		{
			if (c <= '^')
			{
				if (c == '.')
				{
					return TerrainType.Plain;
				}
				if (c == '^')
				{
					return TerrainType.Volcano;
				}
			}
			else
			{
				if (c == 'b')
				{
					return TerrainType.LandBridge;
				}
				switch (c)
				{
				case 'l':
					return TerrainType.Lava;
				case 'm':
					return TerrainType.Mountain;
				case 'r':
					return TerrainType.River;
				case 's':
					return TerrainType.Swamp;
				case 't':
					return TerrainType.Vent;
				case 'u':
					return TerrainType.Ruin;
				case 'v':
					return TerrainType.Ravine;
				case 'x':
					return TerrainType.Impassable;
				}
			}
			return TerrainType.Plain;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x000504A3 File Offset: 0x0004E6A3
		public static IEnumerable<TerrainType> GetFeatures()
		{
			yield return TerrainType.Vent;
			yield return TerrainType.Swamp;
			yield return TerrainType.Ruin;
			yield return TerrainType.Lava;
			yield return TerrainType.Ravine;
			yield return TerrainType.Mountain;
			yield break;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x000504AC File Offset: 0x0004E6AC
		public static bool IsPassible(this TerrainType type)
		{
			switch (type)
			{
			case TerrainType.Plain:
				return true;
			case TerrainType.Swamp:
				return true;
			case TerrainType.LandBridge:
				return true;
			case TerrainType.Lava:
				return true;
			case TerrainType.Vent:
				return true;
			case TerrainType.Ruin:
				return true;
			}
			return false;
		}
	}
}
