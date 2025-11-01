using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200072B RID: 1835
	public class LoseEventCardProcessor : EdictEffectModuleProcessor<LoseEventCardInstance, LoseEventCardEffectStaticData>
	{
		// Token: 0x060022C4 RID: 8900 RVA: 0x00078CF9 File Offset: 0x00076EF9
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x00078D14 File Offset: 0x00076F14
		public void OnTurnEnd()
		{
			using (IEnumerator<PlayerState> enumerator = base._currentTurn.EnumeratePlayerStates(false, false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EventCard item;
					if (IEnumerableExtensions.ToList<EventCard>(enumerator.Current.EnumerateEventCards(base._currentTurn)).TryGetRandom(base._random, out item))
					{
						this.TurnProcessContext.BanishGameItem(item, int.MinValue);
					}
				}
			}
			base.RemoveSelf();
		}
	}
}
