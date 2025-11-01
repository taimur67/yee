using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000554 RID: 1364
	[Serializable]
	public class ObjectiveCondition_CapOutAttributesWithRitual : ObjectiveCondition_CastRituals
	{
		// Token: 0x06001A58 RID: 6744 RVA: 0x0005BEA6 File Offset: 0x0005A0A6
		protected virtual bool IsAcceptableStat(GamePieceStat stat)
		{
			return true;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0005BEA9 File Offset: 0x0005A0A9
		protected virtual bool IsAcceptableGamePiece(GamePiece gamePiece)
		{
			return true;
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0005BEAC File Offset: 0x0005A0AC
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			int num = 0;
			foreach (ModifyGamePieceEvent modifyGamePieceEvent in @event.Enumerate<ModifyGamePieceEvent>())
			{
				GamePiece gamePiece;
				if (context.CurrentTurn.TryFetchGameItem<GamePiece>(modifyGamePieceEvent.Target, out gamePiece) && this.IsAcceptableGamePiece(gamePiece) && this.IsAcceptableStat(modifyGamePieceEvent.Stat) && modifyGamePieceEvent.AttributeOriginalValue < modifyGamePieceEvent.AttributeUpperBound && modifyGamePieceEvent.AttributeOriginalValue > modifyGamePieceEvent.AttributeLowerBound && !modifyGamePieceEvent.IsFurtherModificationPossible)
				{
					num++;
				}
			}
			return num >= this.NumberOfAttributesToCapOut;
		}

		// Token: 0x04000BF1 RID: 3057
		[JsonProperty]
		public int NumberOfAttributesToCapOut;
	}
}
