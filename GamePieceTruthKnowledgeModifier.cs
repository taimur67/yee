using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F8 RID: 760
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceTruthKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0003AD54 File Offset: 0x00038F54
		public override int Priority
		{
			get
			{
				return -10;
			}
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0003AD58 File Offset: 0x00038F58
		public GamePieceTruthKnowledgeModifier(Identifier target)
		{
			this.GamePieceId = target;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0003AD68 File Offset: 0x00038F68
		public override void Process(TurnState playerView, in TurnState truth, int knowledgeOwnerId)
		{
			GamePiece gamePiece;
			if (!truth.TryFetchGameItem<GamePiece>(this.GamePieceId, out gamePiece))
			{
				playerView.RemoveGameItem(this.GamePieceId);
				return;
			}
			foreach (Identifier id in gamePiece.Slots)
			{
				Stratagem item;
				if (truth.TryFetchGameItem<Stratagem>(id, out item))
				{
					playerView.ReplaceGameItem(item);
				}
			}
			playerView.ReplaceGameItem(gamePiece);
		}

		// Token: 0x040006C4 RID: 1732
		[JsonProperty]
		public Identifier GamePieceId;
	}
}
