using System;

namespace LoG
{
	// Token: 0x02000445 RID: 1093
	public static class GameItemCategoryExtensions
	{
		// Token: 0x060014EC RID: 5356 RVA: 0x0004F931 File Offset: 0x0004DB31
		public static bool IsPlaceholder(this GameItemCategory category)
		{
			return category == GameItemCategory.AbilityPlaceholder;
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0004F937 File Offset: 0x0004DB37
		public static bool CanBePlacedInVault(this GameItemCategory category)
		{
			return category == GameItemCategory.Artifact || category == GameItemCategory.Praetor || category == GameItemCategory.ManuscriptPiece || category == GameItemCategory.EventCard;
		}
	}
}
