using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006BE RID: 1726
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TeleportGamePieceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001F93 RID: 8083 RVA: 0x0006C76A File Offset: 0x0006A96A
		public TeleportGamePieceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0006C777 File Offset: 0x0006A977
		public TeleportGamePieceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0006C780 File Offset: 0x0006A980
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield return new ActionPhase_TargetHex(delegate(HexCoord x)
			{
				this.TargetContext.SetTargetHex(x);
			}, new ActionPhase_Target<HexCoord>.IsValidFunc(this.IsValidHex), 1);
			yield break;
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0006C790 File Offset: 0x0006A990
		public override Result IsValidHex(TurnContext context, List<HexCoord> selected, HexCoord hexCoord, int castingPlayerId)
		{
			Problem problem = base.IsValidHex(context, selected, hexCoord, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(this.TargetContext.ItemId);
			Problem problem2 = LegionMovementProcessor.CanEnterCanton(context, gamePiece, hexCoord, PathMode.Teleport, null) as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			return Result.Success;
		}
	}
}
