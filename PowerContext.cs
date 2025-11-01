using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200038D RID: 909
	[Serializable]
	public class PowerContext : ModifierContext
	{
		// Token: 0x0600114B RID: 4427 RVA: 0x00042D17 File Offset: 0x00040F17
		[JsonConstructor]
		public PowerContext()
		{
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00042D1F File Offset: 0x00040F1F
		public PowerContext(PowerType power)
		{
			this.Power = power;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00042D2E File Offset: 0x00040F2E
		public override string ToString()
		{
			return this.Power.ToString();
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00042D41 File Offset: 0x00040F41
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new PowerContext
			{
				Power = this.Power
			};
		}

		// Token: 0x040007FC RID: 2044
		[JsonProperty]
		public PowerType Power;
	}
}
