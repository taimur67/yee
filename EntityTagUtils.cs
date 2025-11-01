using System;
using System.Collections.Generic;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020002A2 RID: 674
	public static class EntityTagUtils
	{
		// Token: 0x06000D0A RID: 3338 RVA: 0x000346D1 File Offset: 0x000328D1
		public static GameEntityModifier CreateModifierWithTag<T>(T tag) where T : EntityTag
		{
			return new GameEntityModifier(new ModifierStaticData
			{
				Tags = new List<EntityTag>
				{
					tag
				}
			});
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x000346F4 File Offset: 0x000328F4
		public static GameEntityModifier CreateModifierWithTag<T>() where T : EntityTag
		{
			return EntityTagUtils.CreateModifierWithTag<T>(Activator.CreateInstance<T>());
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00034700 File Offset: 0x00032900
		public static bool TryGetTag<T>(this TurnState turn, GameItem gameItem, out T tag) where T : EntityTag
		{
			if (gameItem.TryGetTag<T>(out tag))
			{
				return true;
			}
			tag = default(T);
			GamePiece controllingPiece = turn.GetControllingPiece(gameItem);
			if (controllingPiece != null && controllingPiece.TryGetTag<T>(out tag))
			{
				return true;
			}
			tag = default(T);
			return false;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00034742 File Offset: 0x00032942
		public static IEnumerable<T> EnumerateTags<T>(this TurnState turn, GameItem gameItem) where T : EntityTag
		{
			foreach (T t in gameItem.EnumerateTags<T>())
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			GamePiece controllingPiece = turn.GetControllingPiece(gameItem);
			if (controllingPiece != null)
			{
				foreach (T t2 in controllingPiece.EnumerateTags<T>())
				{
					yield return t2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}
	}
}
