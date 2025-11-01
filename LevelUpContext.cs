using System;

namespace LoG
{
	// Token: 0x02000381 RID: 897
	[Serializable]
	public class LevelUpContext : ModifierContext
	{
		// Token: 0x06001120 RID: 4384 RVA: 0x000429C9 File Offset: 0x00040BC9
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new LevelUpContext();
		}
	}
}
