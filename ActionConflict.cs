using System;

namespace LoG
{
	// Token: 0x02000486 RID: 1158
	[Serializable]
	public abstract class ActionConflict
	{
		// Token: 0x060015C0 RID: 5568
		public abstract bool ConflictsWith(ActionConflict other);
	}
}
