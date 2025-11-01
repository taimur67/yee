using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200027D RID: 637
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GamePieceStratagemsDestroyedEvent : GameEvent
	{
		// Token: 0x06000C75 RID: 3189 RVA: 0x00031609 File Offset: 0x0002F809
		[JsonConstructor]
		protected GamePieceStratagemsDestroyedEvent()
		{
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00031611 File Offset: 0x0002F811
		public GamePieceStratagemsDestroyedEvent(int triggeringPlayerID, GamePiece owner, int numberDestroyed) : base(triggeringPlayerID)
		{
			base.AddAffectedPlayerId(owner.ControllingPlayerId);
			this.NumberDestroyed = numberDestroyed;
			this.AttachmentOwner = owner.Id;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0003163C File Offset: 0x0002F83C
		public override void DeepClone(out GameEvent clone)
		{
			GamePieceStratagemsDestroyedEvent gamePieceStratagemsDestroyedEvent = new GamePieceStratagemsDestroyedEvent();
			base.DeepCloneGameEventParts<GamePieceStratagemsDestroyedEvent>(gamePieceStratagemsDestroyedEvent);
			gamePieceStratagemsDestroyedEvent.NumberDestroyed = this.NumberDestroyed;
			gamePieceStratagemsDestroyedEvent.AttachmentOwner = this.AttachmentOwner;
			clone = gamePieceStratagemsDestroyedEvent;
		}

		// Token: 0x04000567 RID: 1383
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int NumberDestroyed;

		// Token: 0x04000568 RID: 1384
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier AttachmentOwner;
	}
}
