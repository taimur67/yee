using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000564 RID: 1380
	[Serializable]
	public class ObjectiveCondition_ControlAllFixtures : BooleanStateObjectiveCondition
	{
		// Token: 0x06001A86 RID: 6790 RVA: 0x0005C814 File Offset: 0x0005AA14
		protected override bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			List<GamePiece> list = new List<GamePiece>();
			foreach (GamePiece gamePiece in context.CurrentTurn.EnumerateAllGamePieces())
			{
				if ((gamePiece.SubCategory == GamePieceCategory.PoP || (this.IncludeStrongholds && gamePiece.SubCategory == GamePieceCategory.Stronghold) || (this.IncludePandaemonium && gamePiece.SubCategory == GamePieceCategory.Pandaemonium)) && gamePiece.IsCapturable())
				{
					list.Add(gamePiece);
				}
			}
			if (list.Count <= 0)
			{
				return false;
			}
			HashSet<int> neededControllingPlayerIds = new HashSet<int>
			{
				owner.Id
			};
			int item;
			if (context.CurrentTurn.CurrentDiplomaticTurn.IsBloodLordOfAny(owner.Id, out item))
			{
				neededControllingPlayerIds.Add(item);
			}
			return list.All((GamePiece x) => neededControllingPlayerIds.Contains(x.ControllingPlayerId));
		}

		// Token: 0x04000C07 RID: 3079
		[JsonProperty]
		public bool IncludePandaemonium;

		// Token: 0x04000C08 RID: 3080
		[JsonProperty]
		public bool IncludeStrongholds;
	}
}
