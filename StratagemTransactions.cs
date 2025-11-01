using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200062E RID: 1582
	public static class StratagemTransactions
	{
		// Token: 0x06001D35 RID: 7477 RVA: 0x00064EE6 File Offset: 0x000630E6
		public static Stratagem SpawnStratagem(this TurnState turn, StratagemStaticData data)
		{
			return turn.SpawnGameItem(data, null, false);
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x00064EF1 File Offset: 0x000630F1
		public static Stratagem SpawnStratagem(this TurnState turn, StratagemStaticData data, PlayerState player, params StratagemTacticLevelStaticData[] tactics)
		{
			return turn.SpawnStratagem(data, player, tactics.AsEnumerable<StratagemTacticLevelStaticData>());
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x00064F01 File Offset: 0x00063101
		public static Stratagem SpawnStratagem(this TurnState turn, StratagemStaticData data, PlayerState player, IEnumerable<StratagemTacticLevelStaticData> tactics)
		{
			Stratagem stratagem = turn.SpawnStratagem(data);
			stratagem.MaxSlots = player.StratagemTacticSlots;
			stratagem.AddTactics(tactics);
			return stratagem;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x00064F24 File Offset: 0x00063124
		public static Result SpawnStratagem(this TurnProcessContext context, GamePiece piece, PlayerState player, IEnumerable<StratagemTacticLevelStaticData> tactics, out Stratagem stratagem)
		{
			StratagemStaticData data = context.Database.FetchSingle<StratagemStaticData>();
			stratagem = context.CurrentTurn.SpawnStratagem(data, player, tactics);
			return context.AttachItemToGamePiece(player, stratagem, piece);
		}
	}
}
