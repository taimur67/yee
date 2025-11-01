using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000264 RID: 612
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MultiplayerPossessedPlayerReturnedEvent : GameEvent
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x00030826 File Offset: 0x0002EA26
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00030829 File Offset: 0x0002EA29
		[JsonConstructor]
		private MultiplayerPossessedPlayerReturnedEvent()
		{
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00030831 File Offset: 0x0002EA31
		public MultiplayerPossessedPlayerReturnedEvent(int triggeringPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(triggeringPlayerId);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00030841 File Offset: 0x0002EA41
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} has returned and is no longer possessed!", this.TriggeringPlayerID);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00030858 File Offset: 0x0002EA58
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.MpPossessedReturnedWitness;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00030860 File Offset: 0x0002EA60
		public override void DeepClone(out GameEvent clone)
		{
			MultiplayerPossessedPlayerReturnedEvent multiplayerPossessedPlayerReturnedEvent = new MultiplayerPossessedPlayerReturnedEvent();
			base.DeepCloneGameEventParts<MultiplayerPossessedPlayerReturnedEvent>(multiplayerPossessedPlayerReturnedEvent);
			clone = multiplayerPossessedPlayerReturnedEvent;
		}
	}
}
