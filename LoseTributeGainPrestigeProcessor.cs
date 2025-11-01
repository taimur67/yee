using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000731 RID: 1841
	public class LoseTributeGainPrestigeProcessor : EdictEffectModuleProcessor<LoseTributeGainPrestigeInstance, LoseTributeGainPrestigeEffectStaticData>
	{
		// Token: 0x060022D0 RID: 8912 RVA: 0x00078F0D File Offset: 0x0007710D
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x00078F28 File Offset: 0x00077128
		public void OnTurnEnd()
		{
			PlayerState playerState = base._currentTurn.FindPlayerState(base.Instance.TargetPlayerId, null);
			List<ResourceNFT> resources = IEnumerableExtensions.ToList<ResourceNFT>(playerState.Resources.GetRandom(base._currentTurn.Random, this._staticData.TributeLossCount));
			playerState.DestroyResources(resources);
			PaymentReceivedEvent gameEvent = this.TurnProcessContext.GivePrestige(playerState, this._staticData.PrestigeGain);
			base._currentTurn.AddGameEvent<PaymentReceivedEvent>(gameEvent);
			base.RemoveSelf();
		}
	}
}
