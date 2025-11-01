using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002C3 RID: 707
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HealGamePieceContext
	{
		// Token: 0x06000DBE RID: 3518 RVA: 0x0003679E File Offset: 0x0003499E
		[JsonConstructor]
		protected HealGamePieceContext()
		{
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000367BC File Offset: 0x000349BC
		public HealGamePieceContext(int playerSource, int healingAmount)
		{
			this.PlayerSource = playerSource;
			this.HealingAmount = healingAmount;
		}

		// Token: 0x04000621 RID: 1569
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int PlayerSource = int.MinValue;

		// Token: 0x04000622 RID: 1570
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue HealingAmount = new ModifiableValue();
	}
}
