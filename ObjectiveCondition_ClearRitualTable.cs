using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200055F RID: 1375
	[Serializable]
	public class ObjectiveCondition_ClearRitualTable : ObjectiveCondition_CastRituals
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x0005C4F0 File Offset: 0x0005A6F0
		public override string LocalizationKey
		{
			get
			{
				return "ClearRitualTable";
			}
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x0005C4F8 File Offset: 0x0005A6F8
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			int num = 0;
			foreach (GameEvent gameEvent in @event.EnumerateAllChildEvents())
			{
				AttachmentRemovedEvent attachmentRemovedEvent = gameEvent as AttachmentRemovedEvent;
				if (attachmentRemovedEvent != null)
				{
					GameItem gameItem;
					if (!context.CurrentTurn.TryFetchGameItem(attachmentRemovedEvent.RemovedAttachment, out gameItem))
					{
						continue;
					}
					Artifact artifact = gameItem as Artifact;
					if (artifact == null || artifact.SubCategory != ArtifactCategory.Sorcery)
					{
						continue;
					}
					num++;
				}
				ItemBanishedEvent itemBanishedEvent = gameEvent as ItemBanishedEvent;
				if (itemBanishedEvent != null && itemBanishedEvent.ItemCategory == GameItemCategory.ActiveRitual)
				{
					num++;
				}
			}
			return num >= this.MinimumSlotsCleared;
		}

		// Token: 0x04000BFE RID: 3070
		[JsonProperty]
		public int MinimumSlotsCleared;
	}
}
