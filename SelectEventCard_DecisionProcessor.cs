using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020004BE RID: 1214
	public class SelectEventCard_DecisionProcessor : DecisionProcessor<SelectEventCardRequest, SelectEventCardResponse>
	{
		// Token: 0x060016B3 RID: 5811 RVA: 0x000533C0 File Offset: 0x000515C0
		protected override SelectEventCardResponse GenerateTypedFallbackResponse()
		{
			List<Identifier> list = IEnumerableExtensions.ToList<Identifier>(from x in this._player.EnumerateEventCards(base._currentTurn)
			select x.Id);
			int count = this._player.MaxEventCards - list.Count<Identifier>();
			return new SelectEventCardResponse
			{
				SelectedCardIds = IEnumerableExtensions.ToList<Identifier>(base.request.CandidateCards.GetRandom(this.TurnProcessContext.Random, count).Concat(list))
			};
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00053454 File Offset: 0x00051654
		protected override Result Validate(SelectEventCardResponse response)
		{
			if (response.SelectedCardIds.Count > this._player.MaxEventCards)
			{
				return Result.SelectedTooManyOptions;
			}
			if (response.SelectedCardIds.Count < this._player.MaxEventCards)
			{
				return Result.SelectedTooFewOptions;
			}
			if (IEnumerableExtensions.ToList<Identifier>(from id in response.SelectedCardIds
			where base.request.CardOptions.All((Identifier c) => c != id)
			select id).Count <= 0)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000534D8 File Offset: 0x000516D8
		protected override Result Process(SelectEventCardResponse response)
		{
			List<Identifier> selectedCardIds = response.SelectedCardIds;
			foreach (Identifier identifier in base.request.CardOptions)
			{
				if (!selectedCardIds.Contains(identifier))
				{
					EventCard item = base._currentTurn.FetchGameItem<EventCard>(identifier);
					this.TurnProcessContext.BanishGameItemSilent(item);
				}
			}
			List<EventCard> list = IEnumerableExtensions.ToList<EventCard>(this._player.EnumerateEventCards(base._currentTurn));
			foreach (EventCard item2 in list)
			{
				if (!selectedCardIds.Contains(item2))
				{
					this._player.RemoveFromVault(item2);
				}
			}
			using (List<Identifier>.Enumerator enumerator3 = selectedCardIds.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Identifier card = enumerator3.Current;
					if (!list.Any((EventCard x) => x.Id == card))
					{
						Problem problem = this._player.GiveEventCard(base._currentTurn, card) as Problem;
						if (problem != null)
						{
							return problem;
						}
					}
				}
			}
			return Result.Success;
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x00053650 File Offset: 0x00051850
		protected override Result Preview(SelectEventCardResponse response)
		{
			List<Identifier> selectedCardIds = response.SelectedCardIds;
			List<EventCard> list = IEnumerableExtensions.ToList<EventCard>(this._player.EnumerateEventCards(base._currentTurn));
			foreach (EventCard item in list)
			{
				if (!selectedCardIds.Contains(item))
				{
					this._player.RemoveFromVault(item);
				}
			}
			using (List<Identifier>.Enumerator enumerator2 = selectedCardIds.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Identifier card = enumerator2.Current;
					if (!list.Any((EventCard x) => x.Id == card))
					{
						Problem problem = this._player.GiveEventCard(base._currentTurn, card) as Problem;
						if (problem != null)
						{
							return problem;
						}
					}
				}
			}
			return Result.Success;
		}
	}
}
