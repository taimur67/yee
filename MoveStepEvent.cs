using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000233 RID: 563
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public abstract class MoveStepEvent : GameEvent
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0002F362 File Offset: 0x0002D562
		[JsonIgnore]
		public HexCoord Location
		{
			get
			{
				return this.Destination;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x0002F36A File Offset: 0x0002D56A
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0002F36D File Offset: 0x0002D56D
		[JsonConstructor]
		protected MoveStepEvent()
		{
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0002F375 File Offset: 0x0002D575
		protected MoveStepEvent(int playerId, Identifier legionId, HexCoord from, HexCoord to, PathMode pathMode) : base(playerId)
		{
			this.LegionId = legionId;
			this.StartLocation = from;
			this.Destination = to;
			this.PathMode = pathMode;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0002F39C File Offset: 0x0002D59C
		protected void DeepCloneMoveStepEventParts(MoveStepEvent moveStepEvent)
		{
			moveStepEvent.LegionId = this.LegionId;
			moveStepEvent.StartLocation = this.StartLocation;
			moveStepEvent.Destination = this.Destination;
			moveStepEvent.PathMode = this.PathMode;
			base.DeepCloneGameEventParts<MoveStepEvent>(moveStepEvent);
		}

		// Token: 0x06000B0A RID: 2826
		public abstract override void DeepClone(out GameEvent clone);

		// Token: 0x04000508 RID: 1288
		[JsonProperty]
		public Identifier LegionId;

		// Token: 0x04000509 RID: 1289
		[JsonProperty]
		public HexCoord StartLocation;

		// Token: 0x0400050A RID: 1290
		[JsonProperty]
		public HexCoord Destination;

		// Token: 0x0400050B RID: 1291
		[JsonProperty]
		public PathMode PathMode;
	}
}
