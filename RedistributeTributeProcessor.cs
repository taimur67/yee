using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000737 RID: 1847
	public class RedistributeTributeProcessor : EdictEffectModuleProcessor<RedistributeTributeInstance, RedistributeTributeEffectStaticData>
	{
		// Token: 0x060022DC RID: 8924 RVA: 0x000790A5 File Offset: 0x000772A5
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000790C0 File Offset: 0x000772C0
		public void OnTurnEnd()
		{
			List<ResourceNFT> list = IEnumerableExtensions.ToList<ResourceNFT>(base._currentTurn.EnumeratePlayerStates(false, false).SelectMany((PlayerState x) => x.Resources));
			int count = list.Count;
			float num = (float)list.Sum((ResourceNFT x) => x.ValueSum) / (float)base._currentTurn.GetNumberOfPlayers(false, false);
			foreach (PlayerState player in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				player.DestroyResources();
			}
			PlayerState playerState = base._currentTurn.FindPlayerState(base._currentTurn.RegentPlayerId, null);
			for (int i = 0; i < count; i++)
			{
				int num2 = playerState.Resources.Sum((ResourceNFT x) => x.ValueSum);
				float targetTokenValue = num - (float)num2;
				ResourceNFT resourceNFT = IEnumerableExtensions.FirstOrDefault<ResourceNFT>(from x in list
				orderby Math.Abs(targetTokenValue - (float)x.ValueSum)
				select x);
				if (resourceNFT == null)
				{
					break;
				}
				playerState.GiveResources(new ResourceNFT[]
				{
					resourceNFT
				});
				list.Remove(resourceNFT);
				playerState = IEnumerableExtensions.FirstOrDefault<PlayerState>(from x in base._currentTurn.EnumeratePlayerStates(false, false)
				orderby x.Resources.Sum((ResourceNFT t) => t.ValueSum)
				select x);
			}
			base.RemoveSelf();
		}
	}
}
