using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200039B RID: 923
	[Serializable]
	public class BooleanModifier : BooleanModifierBase
	{
		// Token: 0x060011AB RID: 4523 RVA: 0x00043C95 File Offset: 0x00041E95
		[JsonConstructor]
		public BooleanModifier()
		{
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00043C9D File Offset: 0x00041E9D
		public BooleanModifier(bool value, ModifierContext provider)
		{
			this.Provider = provider;
			this.Value = value;
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00043CB3 File Offset: 0x00041EB3
		public override void DeepClone(out BooleanModifierBase clone)
		{
			clone = new BooleanModifier
			{
				Value = this.Value,
				Provider = this.Provider.DeepClone<ModifierContext>()
			};
		}

		// Token: 0x0400080D RID: 2061
		public bool Value;
	}
}
