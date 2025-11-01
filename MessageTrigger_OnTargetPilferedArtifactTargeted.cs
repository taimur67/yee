using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000314 RID: 788
	public class MessageTrigger_OnTargetPilferedArtifactTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F5C RID: 3932 RVA: 0x0003CE8C File Offset: 0x0003B08C
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when they pilfer <{1}> from <{2}>", this.RecipientArchfiendId, this.TargetArtifact, this.TargetArchfiendId);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0003CEAC File Offset: 0x0003B0AC
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			PlayerState playerState2 = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			foreach (StealGameItemRitualEvent stealGameItemRitualEvent in context.CurrentTurn.GetGameEvents().OfType<StealGameItemRitualEvent>())
			{
				GamePiece gamePiece;
				if (stealGameItemRitualEvent.TriggeringPlayerID == playerState.Id && stealGameItemRitualEvent.AffectedPlayerID == playerState2.Id && context.CurrentTurn.TryFetchGameItem<GamePiece>(stealGameItemRitualEvent.TargetContext.ItemId, out gamePiece) && gamePiece.Category == GameItemCategory.Artifact)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000730 RID: 1840
		public GameItem TargetArtifact;

		// Token: 0x04000731 RID: 1841
		public string TargetArchfiendId;
	}
}
