using System;

namespace LoG
{
	// Token: 0x02000716 RID: 1814
	public class DrawEventCardProcessor : EdictEffectModuleProcessor<DrawEventCardInstance, DrawEventCardEffectStaticData>
	{
		// Token: 0x06002298 RID: 8856 RVA: 0x000786A4 File Offset: 0x000768A4
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_PreRegency, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x000786C0 File Offset: 0x000768C0
		public void OnTurnEnd()
		{
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				this.TurnProcessContext.QueueEventCardDraw(playerState.Id, this._staticData.DrawCount);
			}
			base.RemoveSelf();
		}
	}
}
