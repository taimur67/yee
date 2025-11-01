using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002CE RID: 718
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RitualResistanceActiveRitual : ArchfiendModifierActiveRitual
	{
		// Token: 0x06000E00 RID: 3584 RVA: 0x000375F0 File Offset: 0x000357F0
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			RitualResistanceRitualData ritualResistanceRitualData = context.Database.Fetch<RitualResistanceRitualData>(base.StaticDataId);
			ritualCastEvent.AddChildEvent<PrestigeFromResistingRitualEvent>(new PrestigeFromResistingRitualEvent(player.Id, ritualResistanceRitualData.PowerToResistForPrestigeBonus, ritualResistanceRitualData.MinPrestigePerResistance, ritualResistanceRitualData.MaxPrestigePerResistance, false));
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00037640 File Offset: 0x00035840
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			RitualResistanceRitualData ritualResistanceRitualData = context.Database.Fetch<RitualResistanceRitualData>(base.StaticDataId);
			banishedEvent.AddChildEvent<PrestigeFromResistingRitualEvent>(new PrestigeFromResistingRitualEvent(player.Id, ritualResistanceRitualData.PowerToResistForPrestigeBonus, ritualResistanceRitualData.MinPrestigePerResistance, ritualResistanceRitualData.MaxPrestigePerResistance, true));
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00037690 File Offset: 0x00035890
		public override Result OnProcessPrestige(TurnProcessContext context, PlayerState player)
		{
			RitualResistanceRitualData ritualResistanceRitualData = context.Database.Fetch<RitualResistanceRitualData>(base.StaticDataId);
			foreach (RitualCastEvent ritualCastEvent in context.CurrentTurn.GetGameEvents<RitualCastEvent>())
			{
				if (ritualCastEvent.AffectedPlayerIds.Contains(player.Id) && ritualCastEvent.RitualCategory == ritualResistanceRitualData.PowerToResistForPrestigeBonus && !ritualCastEvent.Succeeded)
				{
					int prestige = context.CurrentTurn.Random.Next(ritualResistanceRitualData.MinPrestigePerResistance, ritualResistanceRitualData.MaxPrestigePerResistance);
					PaymentReceivedEvent ev = context.GivePrestige(player, prestige);
					ritualCastEvent.AddChildEvent<PaymentReceivedEvent>(ev);
				}
			}
			return base.OnProcessPrestige(context, player);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00037750 File Offset: 0x00035950
		public sealed override void DeepClone(out GameItem gameItem)
		{
			RitualResistanceActiveRitual ritualResistanceActiveRitual = new RitualResistanceActiveRitual();
			base.DeepCloneActiveRitualParts(ritualResistanceActiveRitual);
			base.DeepCloneArchfiendModifierActiveRitualParts(ritualResistanceActiveRitual);
			gameItem = ritualResistanceActiveRitual;
		}
	}
}
