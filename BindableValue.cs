using System;

namespace LoG
{
	// Token: 0x020001F2 RID: 498
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class BindableValue : Attribute
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x0002D1E3 File Offset: 0x0002B3E3
		public BindableValue(string key = null, BindingOption bindingOptions = BindingOption.None)
		{
			this.Key = key;
			this.BindingOptions = bindingOptions;
		}

		// Token: 0x040004AE RID: 1198
		public readonly BindingOption BindingOptions;

		// Token: 0x040004AF RID: 1199
		public readonly string Key;
	}
}
