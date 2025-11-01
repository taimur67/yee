using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200037B RID: 891
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorStrengthContext : ModifierContext
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x0004287C File Offset: 0x00040A7C
		[JsonConstructor]
		public PraetorStrengthContext()
		{
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00042884 File Offset: 0x00040A84
		public PraetorStrengthContext(Identifier praetorId)
		{
			this.PraetorId = praetorId;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00042893 File Offset: 0x00040A93
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new PraetorStrengthContext
			{
				PraetorId = this.PraetorId
			};
		}

		// Token: 0x040007E7 RID: 2023
		[JsonProperty]
		public Identifier PraetorId;
	}
}
