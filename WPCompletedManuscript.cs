using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200015E RID: 350
	public class WPCompletedManuscript : WorldProperty<WPCompletedManuscript>
	{
		// Token: 0x060006DD RID: 1757 RVA: 0x00021D70 File Offset: 0x0001FF70
		private WPCompletedManuscript(ManuscriptStaticData staticData, string manualCategory, BitMask providesManuscriptCategories)
		{
			this.StaticData = staticData;
			this.ManualCategory = manualCategory;
			this.ProvidesManuscriptCategories = providesManuscriptCategories;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00021D8D File Offset: 0x0001FF8D
		public WPCompletedManuscript(ManuscriptStaticData staticData, string manualCategory = "") : this(staticData, manualCategory, BitMask.None)
		{
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00021D9C File Offset: 0x0001FF9C
		public WPCompletedManuscript(ManuscriptCategory manuscriptCategory) : this(null, string.Empty, BitMask.From((int)manuscriptCategory))
		{
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00021DB0 File Offset: 0x0001FFB0
		public static WPCompletedManuscript CompletesAnyManuscript()
		{
			return new WPCompletedManuscript(null, string.Empty, BitMask.All);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00021DC2 File Offset: 0x0001FFC2
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return viewContext.IsManuscriptCompleted(playerState.Id, this.StaticData);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00021DD8 File Offset: 0x0001FFD8
		public override WPProvidesEffect ProvidesEffectInternal(WPCompletedManuscript otherPrecondition)
		{
			if (!this.ProvidesManuscriptCategories.IsEmpty())
			{
				int manuscriptCategory = (int)otherPrecondition.StaticData.ManuscriptCategory;
				if (!this.ProvidesManuscriptCategories.IsSet(manuscriptCategory))
				{
					return WPProvidesEffect.No;
				}
				return WPProvidesEffect.Yes;
			}
			else
			{
				if (otherPrecondition.StaticData.Id != "" && otherPrecondition.StaticData.Id != this.StaticData.Id)
				{
					return WPProvidesEffect.No;
				}
				if (this.StaticData.ManuscriptCategory != otherPrecondition.StaticData.ManuscriptCategory)
				{
					return WPProvidesEffect.No;
				}
				if (this.StaticData.ManuscriptCategory == ManuscriptCategory.Manual && otherPrecondition.StaticData.ManuscriptCategory == ManuscriptCategory.Manual && this.ManualCategory != otherPrecondition.ManualCategory)
				{
					return WPProvidesEffect.No;
				}
				return WPProvidesEffect.Yes;
			}
		}

		// Token: 0x0400031C RID: 796
		private readonly string ManualCategory;

		// Token: 0x0400031D RID: 797
		private readonly ManuscriptStaticData StaticData;

		// Token: 0x0400031E RID: 798
		private readonly BitMask ProvidesManuscriptCategories;
	}
}
