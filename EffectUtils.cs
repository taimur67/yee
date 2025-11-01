using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000352 RID: 850
	public static class EffectUtils
	{
		// Token: 0x0600102A RID: 4138 RVA: 0x0003FD78 File Offset: 0x0003DF78
		public static bool TryGetEffect<T>(this GamePiece piece, TurnProcessContext context, out T effect) where T : AbilityEffect
		{
			effect = IEnumerableExtensions.FirstOrDefault<T>(context.GetAllAbilitiesFor(piece).SelectMany((Ability x) => x.Effects.OfType<T>()));
			return effect != null;
		}
	}
}
