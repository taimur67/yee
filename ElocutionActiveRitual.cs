using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D4 RID: 724
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ElocutionActiveRitual : ArchfiendModifierActiveRitual
	{
		// Token: 0x06000E1D RID: 3613 RVA: 0x00037C94 File Offset: 0x00035E94
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			ElocutionRitualData elocutionRitualData = context.Database.Fetch<ElocutionRitualData>(base.StaticDataId);
			ritualCastEvent.AddChildEvent<PrestigeFromThirdPartyDiplomacyEvent>(new PrestigeFromThirdPartyDiplomacyEvent(player.Id, elocutionRitualData.MinPrestigePerDiplomaticAction, elocutionRitualData.MaxPrestigePerDiplomaticAction, false));
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00037CDC File Offset: 0x00035EDC
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			ElocutionRitualData elocutionRitualData = context.Database.Fetch<ElocutionRitualData>(base.StaticDataId);
			banishedEvent.AddChildEvent<PrestigeFromThirdPartyDiplomacyEvent>(new PrestigeFromThirdPartyDiplomacyEvent(player.Id, elocutionRitualData.MinPrestigePerDiplomaticAction, elocutionRitualData.MaxPrestigePerDiplomaticAction, true));
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00037D24 File Offset: 0x00035F24
		public override Result OnProcessPrestige(TurnProcessContext context, PlayerState player)
		{
			ElocutionRitualData elocutionRitualData = context.Database.Fetch<ElocutionRitualData>(base.StaticDataId);
			foreach (GameEvent gameEvent in context.CurrentTurn.GetGameEvents())
			{
				if (gameEvent is DiplomaticEvent && !(gameEvent is DiplomaticResponseEvent))
				{
					VileCalumnySentEvent vileCalumnySentEvent = gameEvent as VileCalumnySentEvent;
					bool flag = vileCalumnySentEvent != null && vileCalumnySentEvent.TriggeringPlayerID == player.Id;
					if (!gameEvent.IsAssociatedWith(player.Id) || flag)
					{
						int prestige = context.CurrentTurn.Random.Next(elocutionRitualData.MinPrestigePerDiplomaticAction, elocutionRitualData.MaxPrestigePerDiplomaticAction);
						PaymentReceivedEvent ev = context.GivePrestige(player, prestige);
						gameEvent.AddChildEvent<PaymentReceivedEvent>(ev);
					}
				}
			}
			return base.OnProcessPrestige(context, player);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00037E00 File Offset: 0x00036000
		public sealed override void DeepClone(out GameItem gameItem)
		{
			ElocutionActiveRitual elocutionActiveRitual = new ElocutionActiveRitual();
			base.DeepCloneActiveRitualParts(elocutionActiveRitual);
			base.DeepCloneArchfiendModifierActiveRitualParts(elocutionActiveRitual);
			gameItem = elocutionActiveRitual;
		}
	}
}
