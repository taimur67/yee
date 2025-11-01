using System;

namespace LoG
{
	// Token: 0x020006AF RID: 1711
	public class ActionPhase_ConfigureDuel : ActionPhase
	{
		// Token: 0x06001F5E RID: 8030 RVA: 0x0006C3AA File Offset: 0x0006A5AA
		public ActionPhase_ConfigureDuel(PlayerPair contestants, PraetorDuelState.PraetorDuelFlowStage stage, Action<PraetorDuelData> setDuel)
		{
			this.SetDuel = setDuel;
			this.Contestants = contestants;
			this.Stage = stage;
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001F5F RID: 8031 RVA: 0x0006C3C7 File Offset: 0x0006A5C7
		// (set) Token: 0x06001F60 RID: 8032 RVA: 0x0006C3CF File Offset: 0x0006A5CF
		public Action<PraetorDuelData> SetDuel { get; set; }

		// Token: 0x04000CFE RID: 3326
		public PlayerPair Contestants;

		// Token: 0x04000CFF RID: 3327
		public PraetorDuelState.PraetorDuelFlowStage Stage;
	}
}
