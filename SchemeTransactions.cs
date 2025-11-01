using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006C8 RID: 1736
	public static class SchemeTransactions
	{
		// Token: 0x06001FBD RID: 8125 RVA: 0x0006D270 File Offset: 0x0006B470
		public static Result AddSchemes(this TurnState turn, PlayerState player, IEnumerable<SchemeObjective> schemes)
		{
			foreach (SchemeObjective scheme in schemes)
			{
				Problem problem = turn.AddScheme(player, scheme) as Problem;
				if (problem != null)
				{
					return problem;
				}
			}
			return Result.Success;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0006D2D0 File Offset: 0x0006B4D0
		public static Result AddSchemes(this TurnState turn, PlayerState player, IEnumerable<SchemeObjective> schemes, out List<Identifier> schemeCards)
		{
			schemeCards = new List<Identifier>();
			foreach (SchemeObjective scheme in schemes)
			{
				Identifier item;
				Problem problem = turn.AddScheme(player, scheme, out item) as Problem;
				if (problem != null)
				{
					return problem;
				}
				schemeCards.Add(item);
			}
			return Result.Success;
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0006D340 File Offset: 0x0006B540
		public static Result AddScheme(this TurnState turn, PlayerState player, SchemeObjective scheme, out Identifier schemeCardId)
		{
			SchemeCard schemeCard = turn.SpawnSchemeCard(scheme);
			Result result = player.AddSchemeCard(schemeCard);
			schemeCardId = schemeCard.Id;
			return result;
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0006D364 File Offset: 0x0006B564
		public static Result AddScheme(this TurnState turn, PlayerState player, SchemeObjective scheme)
		{
			Identifier identifier;
			return turn.AddScheme(player, scheme, out identifier);
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0006D37B File Offset: 0x0006B57B
		public static Result RemoveScheme(this TurnState turn, PlayerState player, Identifier schemeId)
		{
			player.RemoveSchemeCard(schemeId);
			return Result.Success;
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0006D38C File Offset: 0x0006B58C
		public static void RemoveAllSchemes(this TurnState turn, PlayerState player)
		{
			foreach (Identifier schemeId in IEnumerableExtensions.ToList<Identifier>(player.ActiveSchemeCards))
			{
				turn.RemoveScheme(player, schemeId);
			}
		}
	}
}
