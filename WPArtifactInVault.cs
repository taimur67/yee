using System;

namespace LoG
{
	// Token: 0x02000155 RID: 341
	public class WPArtifactInVault : WorldProperty<WPArtifactInVault>
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x000216BA File Offset: 0x0001F8BA
		public override bool MustBeSuccessfullFulfilledAsPrecondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000216BD File Offset: 0x0001F8BD
		public WPArtifactInVault(Identifier artifactID)
		{
			this.ArtifactID = artifactID;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x000216CC File Offset: 0x0001F8CC
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return IEnumerableExtensions.Contains<Identifier>(playerState.VaultedItems, this.ArtifactID);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000216DF File Offset: 0x0001F8DF
		public override WPProvidesEffect ProvidesEffectInternal(WPArtifactInVault artifactPrecondition)
		{
			if (artifactPrecondition.ArtifactID == this.ArtifactID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x0400030B RID: 779
		public Identifier ArtifactID;
	}
}
