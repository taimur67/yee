using System;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000366 RID: 870
	public static class GamePieceModifierUtils
	{
		// Token: 0x06001096 RID: 4246 RVA: 0x000416A3 File Offset: 0x0003F8A3
		public static float GetSingleValue(this GamePieceModifier modifier)
		{
			StatModificationBinding statModificationBinding = modifier.Data.Bindings.FirstOrDefault((StatModificationBinding x) => x.Value != 0f);
			if (statModificationBinding == null)
			{
				return -1f;
			}
			return statModificationBinding.Value;
		}
	}
}
