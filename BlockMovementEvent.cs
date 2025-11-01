using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000659 RID: 1625
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class BlockMovementEvent : GameEvent
	{
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x0006785C File Offset: 0x00065A5C
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x0006785F File Offset: 0x00065A5F
		[JsonConstructor]
		protected BlockMovementEvent()
		{
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x00067867 File Offset: 0x00065A67
		public BlockMovementEvent(int triggeringPlayerID, GamePiece affectedGamePiece, bool targetHadAlreadyMoved) : base(triggeringPlayerID)
		{
			this.Target = affectedGamePiece.Id;
			this.TargetHadAlreadyMoved = targetHadAlreadyMoved;
			base.AddAffectedPlayerId(affectedGamePiece.ControllingPlayerId);
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x0006788F File Offset: 0x00065A8F
		[JsonIgnore]
		public bool TargetHadMovementToBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x00067892 File Offset: 0x00065A92
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePiece {0} movement block attempt", this.Target);
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000678AC File Offset: 0x00065AAC
		public override void DeepClone(out GameEvent clone)
		{
			BlockMovementEvent blockMovementEvent = new BlockMovementEvent
			{
				Target = this.Target,
				TargetHadAlreadyMoved = this.TargetHadAlreadyMoved
			};
			base.DeepCloneGameEventParts<BlockMovementEvent>(blockMovementEvent);
			clone = blockMovementEvent;
		}

		// Token: 0x04000CCB RID: 3275
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target;

		// Token: 0x04000CCC RID: 3276
		[JsonProperty]
		public bool TargetHadAlreadyMoved;
	}
}
