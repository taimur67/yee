using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200026F RID: 623
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ModifyArchfiendEvent : GameEvent
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00030EF1 File Offset: 0x0002F0F1
		[JsonIgnore]
		public bool IsFurtherModificationPossible
		{
			get
			{
				return this.AttributeOriginalValue + this.OffsetAmount < this.AttributeUpperBound && this.AttributeOriginalValue + this.OffsetAmount > this.AttributeLowerBound;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00030F1F File Offset: 0x0002F11F
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Private;
			}
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00030F22 File Offset: 0x0002F122
		[JsonConstructor]
		protected ModifyArchfiendEvent()
		{
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00030F2C File Offset: 0x0002F12C
		public ModifyArchfiendEvent(int triggeringPlayerID, PlayerState targetPlayer, ArchfiendStat stat, int attemptedOffset, int effectiveOffset, bool wasModificationRemoved = false) : base(triggeringPlayerID)
		{
			this.Stat = stat;
			this.AttemptedOffsetAmount = attemptedOffset;
			this.WasModificationRemoved = wasModificationRemoved;
			base.AddAffectedPlayerId(targetPlayer.Id);
			ModifiableValue modifiableValue = targetPlayer.Get(stat);
			this.AttributeOriginalValue = modifiableValue.Value;
			this.AttributeLowerBound = modifiableValue.LowerBound;
			this.AttributeUpperBound = modifiableValue.UpperBound;
			if (effectiveOffset < 0)
			{
				this.OffsetAmount = Math.Max(modifiableValue.LowerBound - modifiableValue, effectiveOffset);
				return;
			}
			this.OffsetAmount = Math.Min(modifiableValue.UpperBound - modifiableValue, effectiveOffset);
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00030FC9 File Offset: 0x0002F1C9
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Archfiend {0} stat {1} modified by {2}", base.AffectedPlayerID, this.Stat, this.OffsetAmount);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00030FF8 File Offset: 0x0002F1F8
		public override void DeepClone(out GameEvent clone)
		{
			ModifyArchfiendEvent modifyArchfiendEvent = new ModifyArchfiendEvent
			{
				Stat = this.Stat,
				OffsetAmount = this.OffsetAmount,
				AttemptedOffsetAmount = this.AttemptedOffsetAmount,
				AttributeUpperBound = this.AttributeUpperBound,
				AttributeLowerBound = this.AttributeLowerBound,
				WasModificationRemoved = this.WasModificationRemoved,
				AttributeOriginalValue = this.AttributeOriginalValue
			};
			base.DeepCloneGameEventParts<ModifyArchfiendEvent>(modifyArchfiendEvent);
			clone = modifyArchfiendEvent;
		}

		// Token: 0x04000548 RID: 1352
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public ArchfiendStat Stat;

		// Token: 0x04000549 RID: 1353
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int OffsetAmount;

		// Token: 0x0400054A RID: 1354
		[BindableValue("target", BindingOption.None)]
		public int AttemptedOffsetAmount;

		// Token: 0x0400054B RID: 1355
		[BindableValue("max_value", BindingOption.None)]
		[JsonProperty]
		public int AttributeUpperBound;

		// Token: 0x0400054C RID: 1356
		[BindableValue("min_value", BindingOption.None)]
		[JsonProperty]
		public int AttributeLowerBound;

		// Token: 0x0400054D RID: 1357
		[JsonProperty]
		public bool WasModificationRemoved;

		// Token: 0x0400054E RID: 1358
		[JsonProperty]
		public int AttributeOriginalValue;
	}
}
