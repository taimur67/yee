using System;

namespace LoG
{
	// Token: 0x0200019F RID: 415
	public abstract class WorldProperty<T> : WorldProperty where T : WorldProperty
	{
		// Token: 0x06000799 RID: 1945 RVA: 0x000235CC File Offset: 0x000217CC
		public sealed override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			T t = precondition as T;
			if (t != null)
			{
				return this.ProvidesEffectInternal(t);
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x0600079A RID: 1946
		public abstract WPProvidesEffect ProvidesEffectInternal(T property);
	}
}
