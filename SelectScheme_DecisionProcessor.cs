using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020004C7 RID: 1223
	public class SelectScheme_DecisionProcessor : DecisionProcessor<SelectSchemeDecisionRequest, SelectSchemeDecisionResponse>
	{
		// Token: 0x060016E6 RID: 5862 RVA: 0x00053C68 File Offset: 0x00051E68
		protected override Result Validate(SelectSchemeDecisionResponse response)
		{
			if (!response.AcceptSchemes)
			{
				return Result.Success;
			}
			if (response.Selected.Count < 1)
			{
				return Result.SelectedTooFewOptions;
			}
			if (response.Selected.Count > base.request.NumSelections)
			{
				return Result.SelectedTooManyOptions;
			}
			return Result.Success;
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00053CBA File Offset: 0x00051EBA
		protected override Result Process(SelectSchemeDecisionResponse response)
		{
			return this.ProcessInternal(response, true);
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00053CC4 File Offset: 0x00051EC4
		private Result ProcessInternal(SelectSchemeDecisionResponse response, bool createEvents)
		{
			if (!response.AcceptSchemes)
			{
				return Result.Success;
			}
			int num = Math.Min(base.request.NumSelections, this._player.AvailableSchemeSlots);
			while (response.Selected.Count > num)
			{
				response.Selected.Remove(IEnumerableExtensions.First<SchemeId>(response.Selected));
			}
			List<SchemeObjective> list = IEnumerableExtensions.ToList<SchemeObjective>(from t in base.request.Options
			where response.IsSelected(t.Id)
			select t);
			foreach (SchemeObjective schemeObjective in list)
			{
				bool flag;
				if (response.SchemesVisibility.TryGetValue(schemeObjective.Id, out flag))
				{
					schemeObjective.IsPrivate = (flag && base._currentTurn.IsPrivateSchemeValid);
				}
				else
				{
					schemeObjective.IsPrivate = false;
				}
			}
			List<Identifier> list2;
			Result result = base._currentTurn.AddSchemes(this._player, list, out list2);
			if (!createEvents)
			{
				return result;
			}
			foreach (Identifier id in list2)
			{
				SchemeCard schemeCard = base._currentTurn.FetchGameItem<SchemeCard>(id);
				SchemeStartedEvent gameEvent = new SchemeStartedEvent(this._player.Id, schemeCard.Scheme);
				base._currentTurn.AddGameEvent<SchemeStartedEvent>(gameEvent);
			}
			return result;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00053E6C File Offset: 0x0005206C
		protected override Result Preview(SelectSchemeDecisionResponse response)
		{
			return this.ProcessInternal(response, false);
		}
	}
}
