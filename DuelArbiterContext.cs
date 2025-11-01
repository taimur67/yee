using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200037E RID: 894
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DuelArbiterContext : ModifierContext
	{
		// Token: 0x06001117 RID: 4375 RVA: 0x00042926 File Offset: 0x00040B26
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new DuelArbiterContext();
		}
	}
}
