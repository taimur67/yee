using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000572 RID: 1394
	[Serializable]
	public class ObjectiveCondition_DestroyLegionAffectedByActiveRitual : ObjectiveCondition_EventFilter<LegionKilledEvent>
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x0005D07C File Offset: 0x0005B27C
		public override string LocalizationKey
		{
			get
			{
				return "DestroyLegionAffectedByEnfeeble";
			}
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x0005D084 File Offset: 0x0005B284
		protected override bool Filter(TurnContext context, LegionKilledEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			foreach (ActiveRitual activeRitual2 in from banishedItem in context.CurrentTurn.GetGameEvents<ItemBanishedEvent>()
			where banishedItem.ItemCategory == GameItemCategory.ActiveRitual
			where banishedItem.OriginalOwner == owner.Id
			select context.CurrentTurn.FetchGameItem<ActiveRitual>(banishedItem.ItemId) into activeRitual
			where activeRitual != null
			select activeRitual)
			{
				RitualStaticData ritualStaticData;
				if (activeRitual2.TargetContext.ItemId == @event.ItemId && context.Database.TryFetch<RitualStaticData>(activeRitual2.StaticDataId, out ritualStaticData))
				{
					using (List<ConfigRef<RitualStaticData>>.Enumerator enumerator2 = this.AcceptedRituals.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.Id == ritualStaticData.ConfigRef.Id && activeRitual2.TargetContext.ItemId == @event.GamePieceId)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04000C12 RID: 3090
		public List<ConfigRef<RitualStaticData>> AcceptedRituals = new List<ConfigRef<RitualStaticData>>();
	}
}
