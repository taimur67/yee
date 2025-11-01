using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020006C6 RID: 1734
	public static class SchemeFactory
	{
		// Token: 0x06001FB3 RID: 8115 RVA: 0x0006CC44 File Offset: 0x0006AE44
		public static IEnumerable<SchemeObjective> GenerateSchemesFor(TurnContext context, PlayerState player)
		{
			List<SchemeObjective> source = IEnumerableExtensions.ToList<SchemeObjective>(SchemeFactory.GenerateValidSchemesFor(context, player));
			source.ShuffleContents(context.Random);
			return (from t in source
			where t.IsSimpleScheme
			select t).Take(player.NumBasicSchemeOptions).Concat((from t in source
			where t.IsGrandScheme
			select t).Take(player.NumGrandSchemeOptions));
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x0006CCD9 File Offset: 0x0006AED9
		public static SchemeObjective GenerateScheme(TurnContext context, PlayerState player)
		{
			return IEnumerableExtensions.ToList<SchemeObjective>(SchemeFactory.GenerateValidSchemesFor(context, player)).GetRandom(context.Random);
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x0006CCF4 File Offset: 0x0006AEF4
		public static IEnumerable<SchemeObjective> GenerateValidSchemesFor(TurnContext context, PlayerState player)
		{
			return from t in SchemeFactory.GenerateSchemes(context, player)
			where SchemeFactory.IsObjectiveValidFor(t, context, player).successful
			select t;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x0006CD38 File Offset: 0x0006AF38
		public static IEnumerable<SchemeObjective> GenerateSchemes(TurnContext context, PlayerState player)
		{
			return (from x in context.Database.Enumerate<SchemeGenerator>()
			where context.Database.IsUnlockedForPlayer(context.CurrentTurn, player, x)
			select x).SelectMany((SchemeGenerator t) => t.GenerateSchemes(context, player));
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0006CD8C File Offset: 0x0006AF8C
		public static Result IsObjectiveValidFor(Objective objective, TurnContext context, PlayerState player)
		{
			if (!objective.IsPossible(context, player))
			{
				return Result.SimulationError("Objective Already Complete");
			}
			SchemeObjective schemeObjective = objective as SchemeObjective;
			if (schemeObjective != null)
			{
				foreach (Identifier id in player.ActiveSchemeCards)
				{
					SchemeCard schemeCard = context.CurrentTurn.FetchGameItem<SchemeCard>(id);
					if (schemeCard.Scheme.SourceId == schemeObjective.SourceId && schemeCard.Scheme.Conditions.Count == schemeObjective.Conditions.Count)
					{
						bool flag = true;
						for (int i = 0; i < schemeCard.Scheme.Conditions.Count; i++)
						{
							if (schemeCard.Scheme.Conditions[i].Name != schemeObjective.Conditions[i].Name)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							return Result.SimulationError("Objective already in progress");
						}
					}
				}
			}
			return Result.Success;
		}
	}
}
