using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200068C RID: 1676
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class OverrideMovementEvent : GameEvent
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001EC4 RID: 7876 RVA: 0x00069F04 File Offset: 0x00068104
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x00069F07 File Offset: 0x00068107
		[JsonConstructor]
		private OverrideMovementEvent()
		{
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x00069F16 File Offset: 0x00068116
		public OverrideMovementEvent(int triggeringPlayerID, GamePiece affectedGamePiece, bool targetHadAlreadyMoved) : base(triggeringPlayerID)
		{
			this.Target = affectedGamePiece.Id;
			this.TargetHadAlreadyMoved = targetHadAlreadyMoved;
			base.AddAffectedPlayerId(affectedGamePiece.ControllingPlayerId);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x00069F45 File Offset: 0x00068145
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePiece {0} movement overriden", this.Target);
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x00069F5C File Offset: 0x0006815C
		public override void DeepClone(out GameEvent clone)
		{
			OverrideMovementEvent overrideMovementEvent = new OverrideMovementEvent
			{
				Target = this.Target,
				TargetHadAlreadyMoved = this.TargetHadAlreadyMoved
			};
			base.DeepCloneGameEventParts<OverrideMovementEvent>(overrideMovementEvent);
			clone = overrideMovementEvent;
		}

		// Token: 0x04000CE3 RID: 3299
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target = Identifier.Invalid;

		// Token: 0x04000CE4 RID: 3300
		[JsonProperty]
		public bool TargetHadAlreadyMoved;
	}
}
