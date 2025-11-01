using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200031C RID: 796
	public class MessageTrigger_OnTargetUsedDarkArt : MessageTriggerCondition
	{
		// Token: 0x06000F76 RID: 3958 RVA: 0x0003D388 File Offset: 0x0003B588
		public override string GetDescription()
		{
			return "Message <" + this.RecipientArchfiendId + "> when use their Dark Art ability";
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0003D3A0 File Offset: 0x0003B5A0
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState triggeringPlayer = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			List<GameEvent> source = context.CurrentTurn.GetGameEvents().ToList<GameEvent>();
			ArchFiendStaticData archFiendStaticData = database.Fetch<ArchFiendStaticData>(this.RecipientArchfiendId);
			foreach (RitualCastEvent ritualCastEvent in source.OfType<RitualCastEvent>().ToList<RitualCastEvent>())
			{
				if (ritualCastEvent.TriggeringPlayerID == triggeringPlayer.Id && !(ritualCastEvent.RitualId != archFiendStaticData.DarkArt.Id))
				{
					return true;
				}
			}
			return source.OfType<VileCalumnySentEvent>().Any((VileCalumnySentEvent @event) => @event.TriggeringPlayerID == triggeringPlayer.Id) || source.OfType<RequestChainsOfAvariceEvent>().Any((RequestChainsOfAvariceEvent @event) => @event.TriggeringPlayerID == triggeringPlayer.Id) || source.OfType<DraconicRazziaAnnouncementEvent>().Any((DraconicRazziaAnnouncementEvent @event) => @event.TriggeringPlayerID == triggeringPlayer.Id) || source.OfType<RequestLureOfExcessEvent>().Any((RequestLureOfExcessEvent @event) => @event.TriggeringPlayerID == triggeringPlayer.Id);
		}
	}
}
