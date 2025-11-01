using System;

namespace LoG
{
	// Token: 0x0200038E RID: 910
	public class UnknownValueContext : ModifierContext
	{
		// Token: 0x0600114F RID: 4431 RVA: 0x00042D56 File Offset: 0x00040F56
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new UnknownValueContext();
		}
	}
}
