using System;

namespace LoG
{
	// Token: 0x0200037A RID: 890
	[Serializable]
	public abstract class ModifierContext : IDeepClone<ModifierContext>
	{
		// Token: 0x0600110C RID: 4364
		public abstract void DeepClone(out ModifierContext modifierContext);
	}
}
