using System;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000568 RID: 1384
	[Serializable]
	public class ObjectiveCondition_ControlNumberOfGamePiece : ObjectiveCondition
	{
		// Token: 0x06001A99 RID: 6809 RVA: 0x0005CDD5 File Offset: 0x0005AFD5
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return context.CurrentTurn.GetActiveGamePiecesForPlayer(owner.Id).Count((GamePiece x) => x.StaticDataId == this.GamePiece.Id);
		}

		// Token: 0x04000C0E RID: 3086
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> GamePiece;
	}
}
