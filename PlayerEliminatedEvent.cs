using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200021E RID: 542
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PlayerEliminatedEvent : GameEvent
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002E839 File Offset: 0x0002CA39
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0002E83C File Offset: 0x0002CA3C
		[JsonConstructor]
		private PlayerEliminatedEvent()
		{
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0002E84B File Offset: 0x0002CA4B
		public PlayerEliminatedEvent(int triggeringPlayerId, int eliminatedPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(eliminatedPlayerId);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0002E862 File Offset: 0x0002CA62
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} was eliminated by player {1}, Taking {2} as a trophy", base.AffectedPlayerID, this.TriggeringPlayerID, this.TrophyItem);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0002E88F File Offset: 0x0002CA8F
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.PlayerEliminated;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002E894 File Offset: 0x0002CA94
		public override void DeepClone(out GameEvent clone)
		{
			PlayerEliminatedEvent playerEliminatedEvent = new PlayerEliminatedEvent
			{
				TrophyItem = this.TrophyItem
			};
			base.DeepCloneGameEventParts<PlayerEliminatedEvent>(playerEliminatedEvent);
			clone = playerEliminatedEvent;
		}

		// Token: 0x040004E3 RID: 1251
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier TrophyItem = Identifier.Invalid;
	}
}
