using System;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000496 RID: 1174
	public static class Extensions
	{
		// Token: 0x060015EF RID: 5615 RVA: 0x00051FA0 File Offset: 0x000501A0
		public static AbilityStaticData GetDataForRequest(this GameDatabase database, ActionableOrder order)
		{
			AbilityStaticData result;
			if (database.TryFetch<AbilityStaticData>(order.AbilityId, out result))
			{
				return result;
			}
			Type type;
			if (ActionProcessorFactory.TryGetDataTypeForRequest(order.GetType(), out type))
			{
				return database.FetchSingle<AbilityStaticData>(type);
			}
			return null;
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00051FD8 File Offset: 0x000501D8
		public static D GetDataForRequest<D>(this GameDatabase database, ActionableOrder order) where D : IStaticData
		{
			D d = database.Fetch<D>(order.AbilityId);
			if (d == null)
			{
				return database.FetchSingle<D>();
			}
			return d;
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x00052002 File Offset: 0x00050202
		public static AbilityStaticData GetDataForRequest(this TurnContext context, ActionableOrder order)
		{
			return context.Database.GetDataForRequest(order);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00052010 File Offset: 0x00050210
		public static D GetDataForRequest<D>(this TurnContext context, ActionableOrder order) where D : IStaticData
		{
			return context.Database.GetDataForRequest(order);
		}
	}
}
