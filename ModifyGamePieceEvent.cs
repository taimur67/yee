using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200026D RID: 621
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ModifyGamePieceEvent : GameEvent
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00030CCF File Offset: 0x0002EECF
		[JsonIgnore]
		public bool IsFurtherModificationPossible
		{
			get
			{
				return this.AttributeOriginalValue + this.OffsetAmount < this.AttributeUpperBound && this.AttributeOriginalValue + this.OffsetAmount > this.AttributeLowerBound;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00030CFD File Offset: 0x0002EEFD
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00030D00 File Offset: 0x0002EF00
		[JsonConstructor]
		protected ModifyGamePieceEvent()
		{
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00030D10 File Offset: 0x0002EF10
		public ModifyGamePieceEvent(int triggeringPlayerID, GamePiece affectedGamePiece, GamePieceStat stat, int attemptedOffset, int effectiveOffset, bool wasModificationRemoved = false) : base(triggeringPlayerID)
		{
			base.AddAffectedPlayerId(affectedGamePiece.ControllingPlayerId);
			this.Target = affectedGamePiece.Id;
			this.Stat = stat;
			this.AttemptedOffsetAmount = attemptedOffset;
			this.WasModificationRemoved = wasModificationRemoved;
			ModifiableValue modifiableValue = affectedGamePiece.Get(stat);
			this.AttributeOriginalValue = modifiableValue.Value;
			this.AttributeLowerBound = modifiableValue.LowerBound;
			this.AttributeUpperBound = modifiableValue.UpperBound;
			if (effectiveOffset < 0)
			{
				this.OffsetAmount = Math.Max(this.AttributeLowerBound - modifiableValue, effectiveOffset);
				return;
			}
			this.OffsetAmount = Math.Min(this.AttributeUpperBound - modifiableValue, effectiveOffset);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00030DC0 File Offset: 0x0002EFC0
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePiece {0} stat {1} modified by {2}", this.Target, this.Stat, this.OffsetAmount);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00030DF0 File Offset: 0x0002EFF0
		protected void DeepCloneModifyGamePieceEventParts(ModifyGamePieceEvent clone)
		{
			clone.Target = this.Target;
			clone.Stat = this.Stat;
			clone.OffsetAmount = this.OffsetAmount;
			clone.AttemptedOffsetAmount = this.AttemptedOffsetAmount;
			clone.AttributeUpperBound = this.AttributeUpperBound;
			clone.AttributeLowerBound = this.AttributeLowerBound;
			clone.WasModificationRemoved = this.WasModificationRemoved;
			clone.AttributeOriginalValue = this.AttributeOriginalValue;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00030E60 File Offset: 0x0002F060
		public override void DeepClone(out GameEvent clone)
		{
			ModifyGamePieceEvent modifyGamePieceEvent = new ModifyGamePieceEvent();
			base.DeepCloneGameEventParts<ModifyGamePieceEvent>(modifyGamePieceEvent);
			this.DeepCloneModifyGamePieceEventParts(modifyGamePieceEvent);
			clone = modifyGamePieceEvent;
		}

		// Token: 0x04000540 RID: 1344
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target = Identifier.Invalid;

		// Token: 0x04000541 RID: 1345
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public GamePieceStat Stat;

		// Token: 0x04000542 RID: 1346
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int OffsetAmount;

		// Token: 0x04000543 RID: 1347
		[BindableValue("target", BindingOption.None)]
		[JsonProperty]
		public int AttemptedOffsetAmount;

		// Token: 0x04000544 RID: 1348
		[BindableValue("max_value", BindingOption.None)]
		[JsonProperty]
		public int AttributeUpperBound;

		// Token: 0x04000545 RID: 1349
		[BindableValue("min_value", BindingOption.None)]
		[JsonProperty]
		public int AttributeLowerBound;

		// Token: 0x04000546 RID: 1350
		[JsonProperty]
		public bool WasModificationRemoved;

		// Token: 0x04000547 RID: 1351
		[JsonProperty]
		public int AttributeOriginalValue;
	}
}
