using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200027C RID: 636
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GamePieceAttachmentRemovedEvent : AttachmentRemovedEvent
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0003159D File Offset: 0x0002F79D
		[JsonIgnore]
		public bool IsNothingToRemove
		{
			get
			{
				return this.AttachmentOwner == this.RemovedAttachment;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x000315AD File Offset: 0x0002F7AD
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x000315B0 File Offset: 0x0002F7B0
		[JsonConstructor]
		protected GamePieceAttachmentRemovedEvent()
		{
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x000315B8 File Offset: 0x0002F7B8
		public static GamePieceAttachmentRemovedEvent NothingToRemove(int triggeringPlayerID, int affectedPlayerID, Identifier attachmentOwner)
		{
			return new GamePieceAttachmentRemovedEvent(triggeringPlayerID, affectedPlayerID, attachmentOwner, attachmentOwner);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x000315C3 File Offset: 0x0002F7C3
		public GamePieceAttachmentRemovedEvent(int triggeringPlayerID, int affectedPlayerID, Identifier removedAttachment, Identifier attachmentOwner) : base(triggeringPlayerID, affectedPlayerID, removedAttachment)
		{
			this.AttachmentOwner = attachmentOwner;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000315D8 File Offset: 0x0002F7D8
		public override void DeepClone(out GameEvent clone)
		{
			GamePieceAttachmentRemovedEvent gamePieceAttachmentRemovedEvent = new GamePieceAttachmentRemovedEvent
			{
				AttachmentOwner = this.AttachmentOwner
			};
			base.DeepCloneAttachmentRemovedEventParts(gamePieceAttachmentRemovedEvent);
			base.DeepCloneGameEventParts<GamePieceAttachmentRemovedEvent>(gamePieceAttachmentRemovedEvent);
			clone = gamePieceAttachmentRemovedEvent;
		}

		// Token: 0x04000566 RID: 1382
		[BindableValue("target", BindingOption.None)]
		[JsonProperty]
		public Identifier AttachmentOwner;
	}
}
