using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003F1 RID: 1009
	[Serializable]
	public class HexProblem : Problem
	{
		// Token: 0x0600142A RID: 5162 RVA: 0x0004D542 File Offset: 0x0004B742
		[JsonConstructor]
		protected HexProblem()
		{
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0004D54A File Offset: 0x0004B74A
		public HexProblem(HexCoord targetHex)
		{
			this.TargetHex = targetHex;
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0004D559 File Offset: 0x0004B759
		public override string LocKey
		{
			get
			{
				return "Result.Hex.DefaultProblem";
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0004D560 File Offset: 0x0004B760
		public override string DebugString
		{
			get
			{
				return string.Format("Hex {0} is invalid", this.TargetHex);
			}
		}

		// Token: 0x040008F7 RID: 2295
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public HexCoord TargetHex;
	}
}
