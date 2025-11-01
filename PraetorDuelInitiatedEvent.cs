using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003AC RID: 940
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuelInitiatedEvent : GameEvent
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x000464F0 File Offset: 0x000446F0
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x000464F3 File Offset: 0x000446F3
		[JsonConstructor]
		public PraetorDuelInitiatedEvent()
		{
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000464FB File Offset: 0x000446FB
		public PraetorDuelInitiatedEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(targetPlayerId);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0004650B File Offset: 0x0004470B
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} challenged {1} to a praetor duel.", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0004652D File Offset: 0x0004472D
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.PraetorDuelStarted;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00046534 File Offset: 0x00044734
		public override void DeepClone(out GameEvent clone)
		{
			PraetorDuelInitiatedEvent praetorDuelInitiatedEvent = new PraetorDuelInitiatedEvent();
			base.DeepCloneGameEventParts<PraetorDuelInitiatedEvent>(praetorDuelInitiatedEvent);
			clone = praetorDuelInitiatedEvent;
		}
	}
}
