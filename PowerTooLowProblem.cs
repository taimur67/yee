using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003EE RID: 1006
	[Serializable]
	public class PowerTooLowProblem : Problem
	{
		// Token: 0x0600141D RID: 5149 RVA: 0x0004D47F File Offset: 0x0004B67F
		[JsonConstructor]
		protected PowerTooLowProblem()
		{
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0004D487 File Offset: 0x0004B687
		public PowerTooLowProblem(PowerType power, int minimumLevelRequired)
		{
			this.Power = power;
			this.MinimumLevelRequired = minimumLevelRequired;
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x0004D49D File Offset: 0x0004B69D
		protected override string LocKeyScope
		{
			get
			{
				return "Result.PowerTooLowProblem";
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0004D4A4 File Offset: 0x0004B6A4
		public override string LocKey
		{
			get
			{
				return this.LocKeyScope + ".DefaultProblem";
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0004D4B6 File Offset: 0x0004B6B6
		public override string DebugString
		{
			get
			{
				return string.Format("{0} power level {1} is required", this.Power, this.MinimumLevelRequired);
			}
		}

		// Token: 0x040008F3 RID: 2291
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public PowerType Power;

		// Token: 0x040008F4 RID: 2292
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public int MinimumLevelRequired;
	}
}
