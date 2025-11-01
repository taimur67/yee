using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F5 RID: 757
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerSpendablePrestigeKnowledgeModifier : PlayerTargetedKnowledgeModifier
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x0003AC30 File Offset: 0x00038E30
		public int PrestigeMod
		{
			get
			{
				return this._prestigeMod;
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0003AC38 File Offset: 0x00038E38
		public PlayerSpendablePrestigeKnowledgeModifier(int prestigeMod)
		{
			this._prestigeMod = prestigeMod;
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0003AC47 File Offset: 0x00038E47
		public override void Process(TurnState view, PlayerState playerState)
		{
			playerState.SpendablePrestige += this._prestigeMod;
			playerState.SpendablePrestige = Math.Max(playerState.SpendablePrestige, 0);
		}

		// Token: 0x040006C0 RID: 1728
		[JsonProperty]
		private int _prestigeMod;
	}
}
