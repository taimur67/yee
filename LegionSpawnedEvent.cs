using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000230 RID: 560
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LegionSpawnedEvent : GameEvent
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0002F0D9 File Offset: 0x0002D2D9
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002F0DC File Offset: 0x0002D2DC
		[JsonConstructor]
		private LegionSpawnedEvent()
		{
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002F0E4 File Offset: 0x0002D2E4
		public LegionSpawnedEvent(int playerId, Identifier identifier, HexCoord location, LegionSpawnedEvent.LegionSpawnType spawnType, GamePiece gamePiece) : base(playerId)
		{
			base.AddAffectedPlayerId(playerId);
			this.LegionID = identifier;
			this.Location = location;
			this.SpawnType = spawnType;
			this.GamePieceDataId = gamePiece.StaticDataId;
			this.ControllingPlayerId = gamePiece.ControllingPlayerId;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002F124 File Offset: 0x0002D324
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Legion {0} spawned at {1}", this.LegionID, this.Location);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002F148 File Offset: 0x0002D348
		public override void DeepClone(out GameEvent clone)
		{
			LegionSpawnedEvent legionSpawnedEvent = new LegionSpawnedEvent
			{
				LegionID = this.LegionID,
				Location = this.Location,
				SpawnType = this.SpawnType,
				GamePieceDataId = this.GamePieceDataId,
				ControllingPlayerId = this.ControllingPlayerId
			};
			base.DeepCloneGameEventParts<LegionSpawnedEvent>(legionSpawnedEvent);
			clone = legionSpawnedEvent;
		}

		// Token: 0x040004FD RID: 1277
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier LegionID;

		// Token: 0x040004FE RID: 1278
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public HexCoord Location;

		// Token: 0x040004FF RID: 1279
		[JsonProperty]
		public int ControllingPlayerId;

		// Token: 0x04000500 RID: 1280
		[JsonProperty]
		public string GamePieceDataId;

		// Token: 0x04000501 RID: 1281
		[JsonProperty]
		public LegionSpawnedEvent.LegionSpawnType SpawnType;

		// Token: 0x020008C2 RID: 2242
		public enum LegionSpawnType
		{
			// Token: 0x04001368 RID: 4968
			Bazaar,
			// Token: 0x04001369 RID: 4969
			Ritual,
			// Token: 0x0400136A RID: 4970
			Event,
			// Token: 0x0400136B RID: 4971
			Edict
		}
	}
}
