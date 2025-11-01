using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000285 RID: 645
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OngoingRitualEvent : GameEvent
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00031806 File Offset: 0x0002FA06
		[JsonIgnore]
		public int ApparentSourceId
		{
			get
			{
				if (!this.MaskingContext.MaskingSuccessful)
				{
					return this.TrueSourceId;
				}
				return this.MaskingContext.FramedPlayerId;
			}
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00031827 File Offset: 0x0002FA27
		[JsonConstructor]
		public OngoingRitualEvent()
		{
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0003183A File Offset: 0x0002FA3A
		public OngoingRitualEvent(string ritualId, int triggeringPlayer, RitualMaskingContext masking) : base(triggeringPlayer)
		{
			this.TrueSourceId = triggeringPlayer;
			this.MaskingContext = masking;
			this.RitualId = ritualId;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00031863 File Offset: 0x0002FA63
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.TriggeringPlayerID != forPlayerID)
			{
				return TurnLogEntryType.OngoingRitual;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00031878 File Offset: 0x0002FA78
		public override void DeepClone(out GameEvent clone)
		{
			OngoingRitualEvent ongoingRitualEvent = new OngoingRitualEvent
			{
				RitualId = this.RitualId,
				TrueSourceId = this.TrueSourceId,
				MaskingContext = this.MaskingContext.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneGameEventParts<OngoingRitualEvent>(ongoingRitualEvent);
			clone = ongoingRitualEvent;
		}

		// Token: 0x04000585 RID: 1413
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public RitualMaskingContext MaskingContext = new RitualMaskingContext();

		// Token: 0x04000586 RID: 1414
		[JsonProperty]
		public int TrueSourceId;

		// Token: 0x04000587 RID: 1415
		[JsonProperty]
		[BindableValue(null, BindingOption.StaticDataId)]
		public string RitualId;
	}
}
