using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000278 RID: 632
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ManuscriptEvent : GameEvent
	{
		// Token: 0x06000C63 RID: 3171 RVA: 0x00031401 File Offset: 0x0002F601
		[JsonConstructor]
		private ManuscriptEvent()
		{
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00031410 File Offset: 0x0002F610
		public ManuscriptEvent(int triggeringPlayerId, ManuscriptCategory category, params int[] affectedPlayers) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerIds(affectedPlayers);
			this.Category = category;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00031430 File Offset: 0x0002F630
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				switch (this.Category)
				{
				case ManuscriptCategory.Manual:
					return TurnLogEntryType.ManuscriptInvokedManual;
				case ManuscriptCategory.Primer:
					return TurnLogEntryType.ManuscriptInvokedPrimer;
				case ManuscriptCategory.Treatise:
					return TurnLogEntryType.ManuscriptInvokedTreatise;
				case ManuscriptCategory.Schematic:
					return TurnLogEntryType.ManuscriptInvokedSchematic;
				}
			}
			else if (this.Category == ManuscriptCategory.Schematic)
			{
				if (!this.AffectedPlayerIds.Contains(forPlayerID))
				{
					return TurnLogEntryType.MachineBuiltReceiverProtected;
				}
				return TurnLogEntryType.MachineBuiltReceiver;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x000314A1 File Offset: 0x0002F6A1
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} invoked manuscript {1}", this.TriggeringPlayerID, this.ManuscriptId);
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x000314C4 File Offset: 0x0002F6C4
		public override void DeepClone(out GameEvent clone)
		{
			ManuscriptEvent manuscriptEvent = new ManuscriptEvent
			{
				Category = this.Category,
				ManuscriptId = this.ManuscriptId,
				TargetId = this.TargetId,
				PowerType = this.PowerType
			};
			base.DeepCloneGameEventParts<ManuscriptEvent>(manuscriptEvent);
			clone = manuscriptEvent;
		}

		// Token: 0x0400055C RID: 1372
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public ManuscriptCategory Category;

		// Token: 0x0400055D RID: 1373
		[BindableValue("manuscript", BindingOption.None)]
		[JsonProperty]
		public Identifier ManuscriptId;

		// Token: 0x0400055E RID: 1374
		[DefaultValue(Identifier.Invalid)]
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier TargetId = Identifier.Invalid;

		// Token: 0x0400055F RID: 1375
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public PowerType PowerType;
	}
}
