using System;

namespace LoG
{
	// Token: 0x0200071C RID: 1820
	public class ExcommunicatePlayerProcessor : EdictEffectModuleProcessor<ExcommunicatePlayerInstance, ExcommunicatePlayerEffectStaticData>
	{
		// Token: 0x060022A6 RID: 8870 RVA: 0x00078813 File Offset: 0x00076A13
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_PostEdicts, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x0007882F File Offset: 0x00076A2F
		public void OnTurnEnd()
		{
			base._currentTurn.CurrentDiplomaticTurn.SetPlayerAsExcommunicated(this.TurnProcessContext, base._currentTurn.FindPlayerState(base.Instance.TargetPlayerId, null), ExcommunicationReason.EdictEffect, -1);
			base.RemoveSelf();
		}
	}
}
