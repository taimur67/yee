using System;

namespace LoG
{
	// Token: 0x02000740 RID: 1856
	public class ReverseRegencyOrderProcessor : EdictEffectModuleProcessor<ReverseRegencyOrderInstance, ReverseRegencyOrderEffectStaticData>
	{
		// Token: 0x060022EE RID: 8942 RVA: 0x00079445 File Offset: 0x00077645
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x00079460 File Offset: 0x00077660
		public void OnTurnEnd()
		{
			base._currentTurn.RegencyOrder.Reverse();
			base.RemoveSelf();
		}
	}
}
