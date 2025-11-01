using System;

namespace LoG
{
	// Token: 0x02000199 RID: 409
	public class WPTrueKnowledgeGamePiece : WorldProperty
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x00023350 File Offset: 0x00021550
		public static bool Check(TurnState turn, PlayerState caster, Identifier target = Identifier.Invalid)
		{
			if (target != Identifier.Invalid)
			{
				return turn.IsRitualActiveAndTargeting(caster, "lilith_baleful_gaze", target);
			}
			return turn.IsRitualActive(caster, "lilith_baleful_gaze");
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00023370 File Offset: 0x00021570
		public WPTrueKnowledgeGamePiece(Identifier gamePieceID = Identifier.Invalid)
		{
			this.GamePieceID = gamePieceID;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002337F File Offset: 0x0002157F
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPTrueKnowledgeGamePiece.Check(viewContext.CurrentTurn, playerState, this.GamePieceID);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00023394 File Offset: 0x00021594
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPTrueKnowledgeGamePiece wptrueKnowledgeGamePiece = precondition as WPTrueKnowledgeGamePiece;
			if (wptrueKnowledgeGamePiece == null)
			{
				return WPProvidesEffect.No;
			}
			if (wptrueKnowledgeGamePiece.GamePieceID != Identifier.Invalid && this.GamePieceID != wptrueKnowledgeGamePiece.GamePieceID)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400036D RID: 877
		public Identifier GamePieceID;
	}
}
