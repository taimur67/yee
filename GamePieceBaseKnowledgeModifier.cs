using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F9 RID: 761
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceBaseKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x06000ED2 RID: 3794 RVA: 0x0003ADF0 File Offset: 0x00038FF0
		public GamePieceBaseKnowledgeModifier()
		{
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0003ADF8 File Offset: 0x00038FF8
		public GamePieceBaseKnowledgeModifier(Identifier targetGamePiece, GamePieceModifier modifier)
		{
			this.GamePieceId = targetGamePiece;
			this.Modifier = modifier;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0003AE10 File Offset: 0x00039010
		public override void Process(TurnState playerView, in TurnState truth, int knowledgeOwnerId)
		{
			GamePiece item = playerView.FetchGameItem<GamePiece>(this.GamePieceId);
			this.Modifier.InstallInto(item, playerView, true);
		}

		// Token: 0x040006C5 RID: 1733
		[JsonProperty]
		public Identifier GamePieceId;

		// Token: 0x040006C6 RID: 1734
		[JsonProperty]
		public GamePieceModifier Modifier;
	}
}
