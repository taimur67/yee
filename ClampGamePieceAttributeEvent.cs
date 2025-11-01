using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002DC RID: 732
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ClampGamePieceAttributeEvent : GameEvent
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x00038D1B File Offset: 0x00036F1B
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00038D1E File Offset: 0x00036F1E
		[JsonConstructor]
		private ClampGamePieceAttributeEvent()
		{
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00038D26 File Offset: 0x00036F26
		public ClampGamePieceAttributeEvent(int triggeringPlayerID, GamePiece affectedGamePiece, GamePieceStat stat, int maximumValue, int duration) : base(triggeringPlayerID)
		{
			this.Target = affectedGamePiece.Id;
			base.AddAffectedPlayerId(affectedGamePiece.ControllingPlayerId);
			this.Stat = stat;
			this.MaximumValue = maximumValue;
			this.Duration = duration;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00038D60 File Offset: 0x00036F60
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePiece {0} stat {1} cannot be raised above {2} for {3} turns", new object[]
			{
				this.Target,
				this.Stat,
				this.MaximumValue,
				this.Duration
			});
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00038DB8 File Offset: 0x00036FB8
		public override void DeepClone(out GameEvent clone)
		{
			ClampGamePieceAttributeEvent clampGamePieceAttributeEvent = new ClampGamePieceAttributeEvent
			{
				Target = this.Target,
				Stat = this.Stat,
				MaximumValue = this.MaximumValue,
				Duration = this.Duration
			};
			base.DeepCloneGameEventParts<ClampGamePieceAttributeEvent>(clampGamePieceAttributeEvent);
			clone = clampGamePieceAttributeEvent;
		}

		// Token: 0x04000651 RID: 1617
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target;

		// Token: 0x04000652 RID: 1618
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public GamePieceStat Stat;

		// Token: 0x04000653 RID: 1619
		[BindableValue("value", BindingOption.None)]
		[JsonProperty]
		public int MaximumValue;

		// Token: 0x04000654 RID: 1620
		[BindableValue("turns", BindingOption.None)]
		[JsonProperty]
		public int Duration;
	}
}
