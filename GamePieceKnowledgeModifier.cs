using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F7 RID: 759
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x06000ECC RID: 3788 RVA: 0x0003AD01 File Offset: 0x00038F01
		public GamePieceKnowledgeModifier()
		{
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0003AD09 File Offset: 0x00038F09
		public GamePieceKnowledgeModifier(Identifier targetGamePiece, GamePieceModifier modifier)
		{
			this.GamePieceId = targetGamePiece;
			this.Modifier = modifier;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0003AD20 File Offset: 0x00038F20
		public override void Process(TurnState playerView, in TurnState truth, int knowledgeOwnerId)
		{
			GameItemTargetGroup gameItemTarget = new GameItemTargetGroup(this.Modifier, new Identifier[]
			{
				this.GamePieceId
			});
			playerView.GlobalModifierStack.Push(gameItemTarget);
		}

		// Token: 0x040006C2 RID: 1730
		[JsonProperty]
		public Identifier GamePieceId;

		// Token: 0x040006C3 RID: 1731
		[JsonProperty]
		public GamePieceModifier Modifier;
	}
}
