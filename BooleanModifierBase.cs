using System;

namespace LoG
{
	// Token: 0x0200039A RID: 922
	[Serializable]
	public abstract class BooleanModifierBase : IDeepClone<BooleanModifierBase>
	{
		// Token: 0x060011A9 RID: 4521
		public abstract void DeepClone(out BooleanModifierBase clone);

		// Token: 0x0400080C RID: 2060
		public ModifierContext Provider;
	}
}
