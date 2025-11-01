using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003B7 RID: 951
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DuelProcessDamageTally : IDeepClone<DuelProcessDamageTally>
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x00046EBC File Offset: 0x000450BC
		public int ChallengerDamageReceived
		{
			get
			{
				return this.DefenderDamageDealt;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x00046EC9 File Offset: 0x000450C9
		public int DefenderDamageReceived
		{
			get
			{
				return this.ChallengerDamageDealt;
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00046ED6 File Offset: 0x000450D6
		public void DeepClone(out DuelProcessDamageTally clone)
		{
			clone = new DuelProcessDamageTally
			{
				ChallengerDamageDealt = this.ChallengerDamageDealt.DeepClone<ModifiableValue>(),
				DefenderDamageDealt = this.DefenderDamageDealt.DeepClone<ModifiableValue>()
			};
		}

		// Token: 0x040008A9 RID: 2217
		[JsonProperty]
		public ModifiableValue ChallengerDamageDealt = 0;

		// Token: 0x040008AA RID: 2218
		[JsonProperty]
		public ModifiableValue DefenderDamageDealt = 0;
	}
}
