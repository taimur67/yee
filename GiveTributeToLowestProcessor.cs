using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000725 RID: 1829
	public class GiveTributeToLowestProcessor : EdictEffectModuleProcessor<GiveTributeToLowestInstance, GiveTributeToLowestEffectStaticData>
	{
		// Token: 0x060022B8 RID: 8888 RVA: 0x00078B1D File Offset: 0x00076D1D
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x00078B38 File Offset: 0x00076D38
		public void OnTurnEnd()
		{
			PlayerState playerState = IEnumerableExtensions.First<PlayerState>(from x in base._currentTurn.EnumeratePlayerStates(false, false)
			orderby x.TotalTributeSum.ValueSum
			select x);
			foreach (PlayerState playerState2 in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState2 != playerState)
				{
					List<ResourceNFT> list = IEnumerableExtensions.ToList<ResourceNFT>(playerState2.Resources.SelectRandom(base._random, this._staticData.TributeCount));
					playerState2.DestroyResources(list);
					playerState.GiveResources(list);
				}
			}
			base.RemoveSelf();
		}
	}
}
