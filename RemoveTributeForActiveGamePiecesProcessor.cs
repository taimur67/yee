using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x0200073D RID: 1853
	public class RemoveTributeForActiveGamePiecesProcessor : EdictEffectModuleProcessor<RemoveTributeForActiveGamePiecesInstance, RemoveTributeForActiveGamePiecesEffectStaticData>
	{
		// Token: 0x060022E7 RID: 8935 RVA: 0x000792F1 File Offset: 0x000774F1
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x0007930C File Offset: 0x0007750C
		public void OnTurnEnd()
		{
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				int num = base._currentTurn.GetActiveGamePiecesForPlayer(playerState.Id).Count(delegate(GamePiece x)
				{
					bool flag = !x.IsStronghold() || this._staticData.IncludeStrongholds;
					bool flag2 = !x.IsPersonalGuard(base._currentTurn) || this._staticData.IncludePersonalGuards;
					return this._staticData.Categories.Contains(x.SubCategory) && flag && flag2;
				});
				IEnumerable<ResourceNFT> random = playerState.Resources.GetRandom(base._currentTurn.Random, num * this._staticData.TributePerGamePiece);
				ListExtensions.Remove<ResourceNFT>(playerState.Resources, random);
			}
			base.RemoveSelf();
		}
	}
}
