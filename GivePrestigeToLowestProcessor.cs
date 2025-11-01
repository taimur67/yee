using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000722 RID: 1826
	public class GivePrestigeToLowestProcessor : EdictEffectModuleProcessor<GivePrestigeToLowestInstance, GivePrestigeToLowestEffectStaticData>
	{
		// Token: 0x060022B2 RID: 8882 RVA: 0x000789F9 File Offset: 0x00076BF9
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x00078A14 File Offset: 0x00076C14
		public void OnTurnEnd()
		{
			PlayerState playerState = IEnumerableExtensions.First<PlayerState>(from x in base._currentTurn.EnumeratePlayerStates(false, false)
			orderby x.SpendablePrestige
			select x);
			int num = 0;
			foreach (PlayerState playerState2 in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState2 != playerState)
				{
					int num2 = Math.Min(playerState2.SpendablePrestige, this._staticData.PrestigeAmount);
					num += num2;
					playerState2.RemovePrestige(num2);
				}
			}
			PaymentReceivedEvent gameEvent = this.TurnProcessContext.GivePrestige(playerState, num);
			base._currentTurn.AddGameEvent<PaymentReceivedEvent>(gameEvent);
			base.RemoveSelf();
		}
	}
}
