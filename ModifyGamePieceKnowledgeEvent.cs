using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200026E RID: 622
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ModifyGamePieceKnowledgeEvent : ModifyGamePieceEvent
	{
		// Token: 0x06000C37 RID: 3127 RVA: 0x00030E85 File Offset: 0x0002F085
		[JsonConstructor]
		protected ModifyGamePieceKnowledgeEvent()
		{
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00030E8D File Offset: 0x0002F08D
		public ModifyGamePieceKnowledgeEvent(int triggeringPlayerID, GamePiece affectedGamePiece, GamePieceStat stat, int attemptedOffset, int effectiveOffset, bool wasModificationRemoved = false) : base(triggeringPlayerID, affectedGamePiece, stat, attemptedOffset, effectiveOffset, wasModificationRemoved)
		{
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00030E9E File Offset: 0x0002F09E
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePiece {0} stat {1} appears to have been modified by {2}", this.Target, this.Stat, this.OffsetAmount);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00030ECC File Offset: 0x0002F0CC
		public override void DeepClone(out GameEvent clone)
		{
			ModifyGamePieceKnowledgeEvent modifyGamePieceKnowledgeEvent = new ModifyGamePieceKnowledgeEvent();
			base.DeepCloneGameEventParts<ModifyGamePieceKnowledgeEvent>(modifyGamePieceKnowledgeEvent);
			base.DeepCloneModifyGamePieceEventParts(modifyGamePieceKnowledgeEvent);
			clone = modifyGamePieceKnowledgeEvent;
		}
	}
}
