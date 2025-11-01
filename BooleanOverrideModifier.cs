using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200039C RID: 924
	[Serializable]
	public class BooleanOverrideModifier : BooleanModifierBase
	{
		// Token: 0x060011AE RID: 4526 RVA: 0x00043CD9 File Offset: 0x00041ED9
		[JsonConstructor]
		public BooleanOverrideModifier()
		{
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00043CE1 File Offset: 0x00041EE1
		public BooleanOverrideModifier(bool overrideValue, ModifierContext provider)
		{
			this.Provider = provider;
			this.Value = overrideValue;
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00043CF7 File Offset: 0x00041EF7
		public override void DeepClone(out BooleanModifierBase clone)
		{
			clone = new BooleanOverrideModifier
			{
				Value = this.Value,
				Provider = this.Provider.DeepClone<ModifierContext>()
			};
		}

		// Token: 0x0400080E RID: 2062
		public bool Value;
	}
}
