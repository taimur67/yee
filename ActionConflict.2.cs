using System;

namespace LoG
{
	// Token: 0x02000488 RID: 1160
	[Serializable]
	public abstract class ActionConflict<T> : ActionConflict where T : ActionConflict
	{
		// Token: 0x060015C5 RID: 5573 RVA: 0x00051C8C File Offset: 0x0004FE8C
		public sealed override bool ConflictsWith(ActionConflict other)
		{
			T t = other as T;
			return t != null && this.ConflictsWith(t);
		}

		// Token: 0x060015C6 RID: 5574
		protected abstract bool ConflictsWith(T other);
	}
}
