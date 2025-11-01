using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000286 RID: 646
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class RitualCastEvent : GameEvent
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x000318C0 File Offset: 0x0002FAC0
		[JsonIgnore]
		public bool Succeeded
		{
			get
			{
				RitualCastResult result = this.Result;
				return result == RitualCastResult.TargetResistanceOvercome || result == RitualCastResult.AutomaticSuccess;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x000318E8 File Offset: 0x0002FAE8
		[BindableValue(null, BindingOption.IntPlayerId)]
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

		// Token: 0x06000C8D RID: 3213 RVA: 0x00031909 File Offset: 0x0002FB09
		[JsonConstructor]
		public RitualCastEvent()
		{
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00031928 File Offset: 0x0002FB28
		public override string GetDebugName(TurnContext context)
		{
			string text;
			switch (this.Result)
			{
			case RitualCastResult.TargetResistanceOvercome:
				text = string.Format("successful ({0} <= {1})", this.ResistanceRoll, this.SuccessChance);
				break;
			case RitualCastResult.ResistedByTarget:
				text = string.Format("resisted ({0} > {1})", this.ResistanceRoll, this.SuccessChance);
				break;
			case RitualCastResult.AutomaticSuccess:
				text = "automatically successful";
				break;
			case RitualCastResult.AutomaticFailure:
				text = "automatically unsuccessful";
				break;
			case RitualCastResult.ResistedByTerrain:
				text = "unsuccessful due to the terrain";
				break;
			default:
				text = "ERROR";
				break;
			}
			string text2 = text;
			switch (this.MaskingContext.MaskingMode)
			{
			case RitualMaskingMode.NoMasking:
				text = "no masking";
				break;
			case RitualMaskingMode.Masked:
				text = (this.MaskingContext.MaskingSuccessful ? string.Format("successful masking ({0} <= {1})", this.MaskingContext.DetectionRoll, this.MaskingContext.MaskingSuccessChance) : string.Format("resisted masking ({0} > {1})", this.MaskingContext.DetectionRoll, this.MaskingContext.MaskingSuccessChance));
				break;
			case RitualMaskingMode.Framed:
				text = (this.MaskingContext.MaskingSuccessful ? string.Format("successful framing of player {0} ({1} <= {2})", this.MaskingContext.FramedPlayerId, this.MaskingContext.DetectionRoll, this.MaskingContext.MaskingSuccessChance) : string.Format("resisted framing of player {0} ({1} > {2})", this.MaskingContext.FramedPlayerId, this.MaskingContext.DetectionRoll, this.MaskingContext.MaskingSuccessChance));
				break;
			default:
				text = "unknown masking mode";
				break;
			}
			string text3 = text;
			return string.Format("Player {0} was {1} performing {2} with {3}", new object[]
			{
				this.TriggeringPlayerID,
				text2,
				this.RitualId,
				text3
			});
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00031B14 File Offset: 0x0002FD14
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			bool flag = this.TargetContext.ItemId != Identifier.Invalid;
			if (this.TriggeringPlayerID == forPlayerID)
			{
				TurnLogEntryType result;
				switch (this.Result)
				{
				case RitualCastResult.TargetResistanceOvercome:
					result = TurnLogEntryType.RitualCastDespiteResistance;
					break;
				case RitualCastResult.ResistedByTarget:
					result = (flag ? TurnLogEntryType.RitualCastFailedDueToResistanceOfGameItem : TurnLogEntryType.RitualCastFailedDueToResistance);
					break;
				case RitualCastResult.AutomaticSuccess:
					result = TurnLogEntryType.RitualCast;
					break;
				case RitualCastResult.AutomaticFailure:
					result = TurnLogEntryType.RitualCastFailed;
					break;
				case RitualCastResult.ResistedByTerrain:
					result = TurnLogEntryType.RitualTerrainResisted;
					break;
				default:
					result = TurnLogEntryType.None;
					break;
				}
				return result;
			}
			if (this.AffectedPlayerIds.Contains(forPlayerID))
			{
				TurnLogEntryType result;
				switch (this.Result)
				{
				case RitualCastResult.TargetResistanceOvercome:
					result = TurnLogEntryType.EnemyRitualSufferedDespiteResistance;
					break;
				case RitualCastResult.ResistedByTarget:
					result = (flag ? TurnLogEntryType.EnemyRitualResistedByGameItem : TurnLogEntryType.EnemyRitualResisted);
					break;
				case RitualCastResult.AutomaticSuccess:
					result = TurnLogEntryType.EnemyRitualSuffered;
					break;
				default:
					result = TurnLogEntryType.None;
					break;
				}
				return result;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00031BCF File Offset: 0x0002FDCF
		public RitualCastEvent SetTargetContext(TargetContext targetContext)
		{
			this.TargetContext.CopyFrom(targetContext);
			return this;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00031BDE File Offset: 0x0002FDDE
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return this.VisibilityType;
			}
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00031BE8 File Offset: 0x0002FDE8
		protected void DeepCloneRitualCastEventParts(RitualCastEvent ritualCastEvent)
		{
			ritualCastEvent.RitualId = this.RitualId.DeepClone();
			ritualCastEvent.MaskingContext = this.MaskingContext.DeepClone(CloneFunction.FastClone);
			ritualCastEvent.TrueSourceId = this.TrueSourceId;
			ritualCastEvent.Result = this.Result;
			ritualCastEvent.SuccessChance = this.SuccessChance;
			ritualCastEvent.ResistanceRoll = this.ResistanceRoll;
			ritualCastEvent.TargetContext = this.TargetContext.DeepClone<TargetContext>();
			ritualCastEvent.RitualCategory = this.RitualCategory;
			ritualCastEvent.VisibilityType = this.VisibilityType;
			base.DeepCloneGameEventParts<RitualCastEvent>(ritualCastEvent);
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00031C7C File Offset: 0x0002FE7C
		public override void DeepClone(out GameEvent clone)
		{
			RitualCastEvent ritualCastEvent = new RitualCastEvent();
			this.DeepCloneRitualCastEventParts(ritualCastEvent);
			clone = ritualCastEvent;
		}

		// Token: 0x04000588 RID: 1416
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public string RitualId;

		// Token: 0x04000589 RID: 1417
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public RitualMaskingContext MaskingContext = new RitualMaskingContext();

		// Token: 0x0400058A RID: 1418
		[JsonProperty]
		public int TrueSourceId;

		// Token: 0x0400058B RID: 1419
		[JsonProperty]
		public RitualCastResult Result;

		// Token: 0x0400058C RID: 1420
		[JsonProperty]
		public float SuccessChance;

		// Token: 0x0400058D RID: 1421
		[JsonProperty]
		public float ResistanceRoll;

		// Token: 0x0400058E RID: 1422
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public TargetContext TargetContext = new TargetContext();

		// Token: 0x0400058F RID: 1423
		[JsonProperty]
		public PowerType RitualCategory;

		// Token: 0x04000590 RID: 1424
		[JsonProperty]
		public GameEventVisibility VisibilityType;
	}
}
