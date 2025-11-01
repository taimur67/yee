using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000313 RID: 787
	public class MessageTrigger_OnTargetPilferedArtifact : MessageTriggerCondition
	{
		// Token: 0x06000F59 RID: 3929 RVA: 0x0003CDCF File Offset: 0x0003AFCF
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when they pilfer <{1}>", this.RecipientArchfiendId, this.TargetArtifact);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0003CDE8 File Offset: 0x0003AFE8
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (StealGameItemRitualEvent stealGameItemRitualEvent in context.CurrentTurn.GetGameEvents().OfType<StealGameItemRitualEvent>())
			{
				GamePiece gamePiece;
				if (stealGameItemRitualEvent.TriggeringPlayerID == playerState.Id && context.CurrentTurn.TryFetchGameItem<GamePiece>(stealGameItemRitualEvent.TargetContext.ItemId, out gamePiece) && gamePiece.Category == GameItemCategory.Artifact)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400072F RID: 1839
		public GameItem TargetArtifact;
	}
}
