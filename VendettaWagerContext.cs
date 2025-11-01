using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000389 RID: 905
	[Serializable]
	public class VendettaWagerContext : ModifierContext
	{
		// Token: 0x0600113C RID: 4412 RVA: 0x00042BBE File Offset: 0x00040DBE
		public VendettaWagerContext()
		{
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00042BC6 File Offset: 0x00040DC6
		public VendettaWagerContext(string modifierSource)
		{
			this.ModifierSource = modifierSource;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00042BD5 File Offset: 0x00040DD5
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new VendettaWagerContext
			{
				ModifierSource = this.ModifierSource.DeepClone()
			};
		}

		// Token: 0x040007F8 RID: 2040
		[JsonProperty]
		public string ModifierSource;
	}
}
