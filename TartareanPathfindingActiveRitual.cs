using System;

namespace LoG
{
	// Token: 0x020002E1 RID: 737
	[Serializable]
	public class TartareanPathfindingActiveRitual : GamePieceAbilityActiveRitual
	{
		// Token: 0x06000E67 RID: 3687 RVA: 0x000395E8 File Offset: 0x000377E8
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			Problem problem = base.EndRitual(context, player, banishedEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			TurnState currentTurn = context.CurrentTurn;
			GamePiece gamePiece = currentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			if (!LegionMovementProcessor.HasRightOfEntry(currentTurn, player, gamePiece, gamePiece.Location))
			{
				RepatriateLegionEvent ev = gamePiece.Repatriate(context);
				banishedEvent.AddChildEvent<RepatriateLegionEvent>(ev);
			}
			return Result.Success;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00039648 File Offset: 0x00037848
		public override void DeepClone(out GameItem gameItem)
		{
			TartareanPathfindingActiveRitual tartareanPathfindingActiveRitual = new TartareanPathfindingActiveRitual();
			base.DeepCloneActiveRitualParts(tartareanPathfindingActiveRitual);
			base.DeepCloneGamePieceAbilityActiveRitualParts(tartareanPathfindingActiveRitual);
			gameItem = tartareanPathfindingActiveRitual;
		}
	}
}
