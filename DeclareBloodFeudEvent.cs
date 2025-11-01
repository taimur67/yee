using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001F5 RID: 501
	[Serializable]
	public class DeclareBloodFeudEvent : DiplomaticEvent
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0002D201 File Offset: 0x0002B401
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002D204 File Offset: 0x0002B404
		[JsonConstructor]
		private DeclareBloodFeudEvent()
		{
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002D20C File Offset: 0x0002B40C
		public DeclareBloodFeudEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0002D216 File Offset: 0x0002B416
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} has declared a blood feud with {1}", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002D238 File Offset: 0x0002B438
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.BloodFeudInitiator;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				return TurnLogEntryType.BloodFeudRecipient;
			}
			return TurnLogEntryType.BloodFeudWitness;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002D260 File Offset: 0x0002B460
		public override void DeepClone(out GameEvent clone)
		{
			DeclareBloodFeudEvent declareBloodFeudEvent = new DeclareBloodFeudEvent();
			base.DeepCloneDiplomaticEventParts(declareBloodFeudEvent);
			clone = declareBloodFeudEvent;
		}
	}
}
