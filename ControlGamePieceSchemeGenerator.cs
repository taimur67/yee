using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000467 RID: 1127
	[Serializable]
	public class ControlGamePieceSchemeGenerator : DynamicSchemeGenerator
	{
		// Token: 0x06001515 RID: 5397 RVA: 0x0004FD64 File Offset: 0x0004DF64
		protected override IEnumerable<SchemeObjective> GenerateSchemesInternal(TurnContext context, PlayerState player)
		{
			return from x in context.CurrentTurn.GetActiveGamePieces()
			where x.SubCategory == GamePieceCategory.PoP
			where x.ControllingPlayerId != player.Id
			where x.IsCapturable()
			select x into t
			select this.GenerateScheme(context, player, t);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0004FE08 File Offset: 0x0004E008
		private SchemeObjective GenerateScheme(TurnContext context, PlayerState player, GamePiece pop)
		{
			ObjectiveCondition_ControlGamePiece objectiveCondition_ControlGamePiece = new ObjectiveCondition_ControlGamePiece(pop.Id)
			{
				Params = this.DifficultyParams
			};
			return new SchemeObjective(new ObjectiveCondition[]
			{
				objectiveCondition_ControlGamePiece
			});
		}

		// Token: 0x04000A9B RID: 2715
		[JsonProperty]
		public ObjectiveCondition_ControlGamePiece.DifficultyParams DifficultyParams;
	}
}
