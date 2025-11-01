using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000749 RID: 1865
	public class TaxTributeProcessor : EdictEffectModuleProcessor<TaxTributeInstance, TaxTributeEffectStaticData>
	{
		// Token: 0x06002301 RID: 8961 RVA: 0x000796D1 File Offset: 0x000778D1
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x000796EC File Offset: 0x000778EC
		public void OnTurnEnd()
		{
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				int count = (int)Math.Floor((double)((float)playerState.SpendablePrestige / (float)this._staticData.PrestigeTaxInterval));
				IEnumerable<ResourceNFT> random = playerState.Resources.GetRandom(base._currentTurn.Random, count);
				ListExtensions.Remove<ResourceNFT>(playerState.Resources, random);
			}
			base.RemoveSelf();
		}
	}
}
