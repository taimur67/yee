using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020006C7 RID: 1735
	public static class SchemeProcessor
	{
		// Token: 0x06001FB8 RID: 8120 RVA: 0x0006CEB0 File Offset: 0x0006B0B0
		public static void RevealAllSchemes(this TurnProcessContext turnProcessContext, bool announceSchemes)
		{
			Func<Identifier, SchemeCard> <>9__0;
			Func<Identifier, SchemeCard> <>9__2;
			foreach (PlayerState playerState in IEnumerableExtensions.ToList<PlayerState>(turnProcessContext.CurrentTurn.EnumeratePlayerStates(false, false)))
			{
				IEnumerable<Identifier> previousSchemeCards = playerState.PreviousSchemeCards;
				Func<Identifier, SchemeCard> selector;
				if ((selector = <>9__0) == null)
				{
					selector = (<>9__0 = ((Identifier x) => turnProcessContext.CurrentTurn.FetchGameItem<SchemeCard>(x)));
				}
				List<SchemeCard> list = IEnumerableExtensions.ToList<SchemeCard>(from x in previousSchemeCards.Select(selector)
				where x.Scheme.IsPrivate && x.Scheme.IsComplete && !x.Awarded
				select x);
				IEnumerable<Identifier> activeSchemeCards = playerState.ActiveSchemeCards;
				Func<Identifier, SchemeCard> selector2;
				if ((selector2 = <>9__2) == null)
				{
					selector2 = (<>9__2 = ((Identifier x) => turnProcessContext.CurrentTurn.FetchGameItem<SchemeCard>(x)));
				}
				List<SchemeCard> list2 = IEnumerableExtensions.ToList<SchemeCard>(from x in activeSchemeCards.Select(selector2)
				where x.Scheme.IsPrivate && !x.Scheme.IsComplete && !x.Awarded
				select x);
				int num = 0;
				foreach (SchemeCard schemeCard in list)
				{
					schemeCard.Scheme.RevealPrivateScheme();
					num += schemeCard.Scheme.Reward;
					schemeCard.Awarded = true;
				}
				foreach (SchemeCard schemeCard2 in list2)
				{
					schemeCard2.Scheme.RevealPrivateScheme();
					schemeCard2.Scheme.IsPrivate = false;
				}
				if (announceSchemes && list.Count > 0)
				{
					PaymentReceivedEvent ev = turnProcessContext.GivePrestige(playerState, num);
					AnnouncePrivateSchemesEvent announcePrivateSchemesEvent = new AnnouncePrivateSchemesEvent(playerState.Id, list.Count, num);
					announcePrivateSchemesEvent.AddChildEvent<PaymentReceivedEvent>(ev);
					turnProcessContext.CurrentTurn.AddGameEvent<AnnouncePrivateSchemesEvent>(announcePrivateSchemesEvent);
				}
			}
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0006D0EC File Offset: 0x0006B2EC
		public static void UpdateSchemes(this TurnProcessContext context)
		{
			foreach (PlayerState player in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				context.UpdateSchemes(player);
			}
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0006D140 File Offset: 0x0006B340
		public static void UpdateSchemes(this TurnProcessContext context, PlayerState player)
		{
			foreach (Identifier id in IEnumerableExtensions.ToList<Identifier>(player.ActiveSchemeCards))
			{
				SchemeCard schemeCard = context.CurrentTurn.FetchGameItem<SchemeCard>(id);
				if (schemeCard.Scheme.IsValid)
				{
					schemeCard.Scheme.Update(context, player);
					if (schemeCard.Scheme.IsComplete)
					{
						context.OnCompleteScheme(player, schemeCard);
					}
					else if (!schemeCard.Scheme.IsValid)
					{
						context.OnSchemeInvalidated(player, schemeCard);
					}
				}
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0006D1E4 File Offset: 0x0006B3E4
		public static void OnCompleteScheme(this TurnProcessContext context, PlayerState player, SchemeCard schemeCard)
		{
			int reward = schemeCard.Scheme.Reward;
			bool isPublic = schemeCard.Scheme.IsPublic;
			SchemeCompleteEvent schemeCompleteEvent = new SchemeCompleteEvent(player.Id, schemeCard.Scheme)
			{
				PrestigeAward = reward
			};
			if (isPublic)
			{
				PaymentReceivedEvent ev = context.GivePrestige(player, reward);
				schemeCompleteEvent.AddChildEvent<PaymentReceivedEvent>(ev);
				schemeCard.Awarded = true;
			}
			player.SetSchemeComplete(schemeCard);
			context.CurrentTurn.AddGameEvent<SchemeCompleteEvent>(schemeCompleteEvent);
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0006D24F File Offset: 0x0006B44F
		public static void OnSchemeInvalidated(this TurnProcessContext context, PlayerState player, SchemeCard schemeCard)
		{
			context.CurrentTurn.AddGameEvent<SchemeInvalidatedEvent>(new SchemeInvalidatedEvent(player.Id, schemeCard.Scheme));
		}
	}
}
