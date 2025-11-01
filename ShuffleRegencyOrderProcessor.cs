using System;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000743 RID: 1859
	public class ShuffleRegencyOrderProcessor : EdictEffectModuleProcessor<ShuffleRegencyOrderInstance, ShuffleRegencyOrderEffectStaticData>
	{
		// Token: 0x060022F4 RID: 8948 RVA: 0x000794AD File Offset: 0x000776AD
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000794C8 File Offset: 0x000776C8
		public void OnTurnEnd()
		{
			base._currentTurn.RegencyOrder.ShuffleContents(base._currentTurn.Random);
			base.RemoveSelf();
		}
	}
}
