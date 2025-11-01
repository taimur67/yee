using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001FA RID: 506
	public static class GameEventExtensions
	{
		// Token: 0x060009D0 RID: 2512 RVA: 0x0002D40C File Offset: 0x0002B60C
		public static void RemoveHiddenGameEvents(this List<GameEvent> events, int playerId)
		{
			for (int i = events.Count - 1; i >= 0; i--)
			{
				GameEvent gameEvent = events[i];
				if (gameEvent.CanStrip(playerId))
				{
					events.RemoveAt(i);
				}
				else
				{
					gameEvent.RemoveHidden(playerId);
				}
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002D44D File Offset: 0x0002B64D
		public static string DebugName(this GameEntity entity)
		{
			if (entity == null)
			{
				return "null";
			}
			return entity.ToString();
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0002D460 File Offset: 0x0002B660
		public static string DebugName(this TurnContext context, Identifier id)
		{
			GameItem entity;
			if (context.CurrentTurn.TryFetchGameItem(id, out entity))
			{
				return entity.DebugName();
			}
			if (id != Identifier.Invalid)
			{
				return "Unknown";
			}
			return "Invalid";
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0002D493 File Offset: 0x0002B693
		public static string DebugName(this TurnContext context, GameEntity entity)
		{
			return entity.DebugName();
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002D49C File Offset: 0x0002B69C
		public static string DebugName(this TurnContext context, int playerId)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(playerId, null);
			if (playerState != null)
			{
				return playerState.DebugName();
			}
			if (playerId == -1)
			{
				return "Force Majeure";
			}
			if (playerId == -2147483648)
			{
				return "Invalid";
			}
			return "Unknown";
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002D4E0 File Offset: 0x0002B6E0
		public static string Debug_GetItemName(this TurnContext context, Identifier id)
		{
			if (id == Identifier.Invalid)
			{
				return "Invalid";
			}
			GameItem gameItem;
			if (context.CurrentTurn.TryFetchGameItem(id, out gameItem))
			{
				return string.Format("{0}({1})", gameItem.StaticDataId, id);
			}
			return string.Format("unknown({0})", id);
		}
	}
}
