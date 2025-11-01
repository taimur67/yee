using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006FD RID: 1789
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TrialPhase : IDeepClone<TrialPhase>
	{
		// Token: 0x06002246 RID: 8774 RVA: 0x00077586 File Offset: 0x00075786
		public TrialPhase()
		{
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x0007758E File Offset: 0x0007578E
		public TrialPhase(TrialState state, int duration)
		{
			this.State = state;
			this.Duration = duration;
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000775A4 File Offset: 0x000757A4
		public void DeepClone(out TrialPhase clone)
		{
			clone = new TrialPhase
			{
				State = this.State,
				Duration = this.Duration
			};
		}

		// Token: 0x04000F22 RID: 3874
		[JsonProperty]
		[DefaultValue(TrialState.Undefined)]
		public TrialState State;

		// Token: 0x04000F23 RID: 3875
		[JsonProperty]
		[DefaultValue(0)]
		public int Duration;
	}
}
