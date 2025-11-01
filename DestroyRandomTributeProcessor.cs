using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200070A RID: 1802
	public class DestroyRandomTributeProcessor : EdictEffectModuleProcessor<DestroyRandomTributeInstance, DestroyRandomTributeEffectStaticData>
	{
		// Token: 0x06002281 RID: 8833 RVA: 0x000782A1 File Offset: 0x000764A1
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000782BC File Offset: 0x000764BC
		public void OnTurnEnd()
		{
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				if (this._staticData.TargetRanks.Contains(playerState.Rank))
				{
					IEnumerable<ResourceNFT> random = playerState.Resources.GetRandom(base._currentTurn.Random, this._staticData.TokenCount);
					ListExtensions.Remove<ResourceNFT>(playerState.Resources, random);
				}
			}
			base.RemoveSelf();
		}
	}
}
