using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000447 RID: 1095
	public static class GamePieceCategoryExtensions
	{
		// Token: 0x060014EE RID: 5358 RVA: 0x0004F94B File Offset: 0x0004DB4B
		public static bool IsFixture(this GamePieceCategory category)
		{
			return !category.IsLegion();
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0004F956 File Offset: 0x0004DB56
		public static bool IsPlaceOfPower(this GamePieceCategory category, bool excludePandaemonium = true, bool excludeStrongholds = true)
		{
			return (!excludePandaemonium || category != GamePieceCategory.Pandaemonium) && (!excludeStrongholds || category != GamePieceCategory.Stronghold) && category.IsFixture();
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0004F970 File Offset: 0x0004DB70
		public static bool IsLegion(this GamePieceCategory category)
		{
			return category == GamePieceCategory.Legion || category == GamePieceCategory.Titan;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0004F97B File Offset: 0x0004DB7B
		public static SlotType GetProvidedSlotType(this GamePieceCategory category)
		{
			if (category.IsLegion())
			{
				return SlotType.Legion;
			}
			if (category.IsFixture())
			{
				return SlotType.Fixture;
			}
			return SlotType.CantSlot;
		}
	}
}
