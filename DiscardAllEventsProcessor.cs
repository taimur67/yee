using System;

namespace LoG
{
	// Token: 0x0200070D RID: 1805
	public class DiscardAllEventsProcessor : EdictEffectModuleProcessor<DiscardAllEventsInstance, DiscardAllEventsEffectStaticData>
	{
		// Token: 0x06002287 RID: 8839 RVA: 0x0007838D File Offset: 0x0007658D
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_PreRegency, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000783A8 File Offset: 0x000765A8
		public void OnTurnEnd()
		{
			foreach (Identifier id in IEnumerableExtensions.ToList<Identifier>(base._currentTurn.FindPlayerState(base.Instance.TargetPlayerId, null).VaultedItems))
			{
				EventCard eventCard = base._currentTurn.FetchGameItem(id) as EventCard;
				if (eventCard != null)
				{
					this.TurnProcessContext.BanishGameItem(eventCard, int.MinValue);
				}
			}
			base.RemoveSelf();
		}
	}
}
