using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x0200054B RID: 1355
	[Serializable]
	public class ObjectiveCondition_BanishDuellistPraetor : ObjectiveCondition_CastRituals
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001A44 RID: 6724 RVA: 0x0005B841 File Offset: 0x00059A41
		public override string LocalizationKey
		{
			get
			{
				return "BanishDuellistPraetor";
			}
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x0005B848 File Offset: 0x00059A48
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			TurnState currentTurn = context.CurrentTurn;
			List<Identifier> list = IEnumerableExtensions.ToList<Identifier>(from duellist in this.EnumerateMissingDuellists(currentTurn)
			select duellist.Id);
			foreach (ItemBanishedEvent itemBanishedEvent in @event.Enumerate<ItemBanishedEvent>())
			{
				if (itemBanishedEvent.ItemCategory == GameItemCategory.Praetor && list.Contains(itemBanishedEvent.ItemId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0005B8F8 File Offset: 0x00059AF8
		private IEnumerable<Praetor> EnumerateMissingDuellists(TurnState turn)
		{
			ObjectiveCondition_BanishDuellistPraetor.<>c__DisplayClass3_0 CS$<>8__locals1 = new ObjectiveCondition_BanishDuellistPraetor.<>c__DisplayClass3_0();
			CS$<>8__locals1.turn = turn;
			return from praetor in CS$<>8__locals1.turn.GetGameEvents<PraetorDuelOutcomeEvent>().Where(delegate(PraetorDuelOutcomeEvent outcome)
			{
				DuelResultStatus result = outcome.Result;
				return result == DuelResultStatus.Cancelled || result == DuelResultStatus.DefaultWinner;
			}).SelectMany(new Func<PraetorDuelOutcomeEvent, IEnumerable<Praetor>>(CS$<>8__locals1.<EnumerateMissingDuellists>g__EnumeratePraetors|2))
			where praetor.Status == GameItemStatus.Banished
			select praetor;
		}
	}
}
