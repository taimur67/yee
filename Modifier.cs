using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000370 RID: 880
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class Modifier<T> : IModifier where T : IModifiable
	{
		// Token: 0x060010BA RID: 4282 RVA: 0x00041A94 File Offset: 0x0003FC94
		public sealed override void ApplyTo(TurnContext context, IModifiable modifiable)
		{
			if (modifiable is T)
			{
				T item = (T)((object)modifiable);
				if (this.CanApplyTo(context, item))
				{
					this.ApplyTo(context, item);
				}
			}
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00041AC4 File Offset: 0x0003FCC4
		public sealed override void PostApplyTo(TurnContext context, IModifiable modifiable)
		{
			if (modifiable is T)
			{
				T item = (T)((object)modifiable);
				if (this.CanApplyTo(context, item))
				{
					this.PostApplyTo(context, item);
				}
			}
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00041AF2 File Offset: 0x0003FCF2
		public virtual bool CanApplyTo(TurnContext context, T item)
		{
			return true;
		}

		// Token: 0x060010BD RID: 4285
		public abstract void ApplyTo(TurnContext context, T item);

		// Token: 0x060010BE RID: 4286 RVA: 0x00041AF5 File Offset: 0x0003FCF5
		public virtual void PostApplyTo(TurnContext context, T item)
		{
		}

		// Token: 0x060010BF RID: 4287
		public abstract void InstallInto(T item, TurnState turn, bool baseAdjust = false);
	}
}
