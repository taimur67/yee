using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200068D RID: 1677
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RaiseVolcanoRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EC9 RID: 7881 RVA: 0x00069F92 File Offset: 0x00068192
		public RaiseVolcanoRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x00069F9F File Offset: 0x0006819F
		public RaiseVolcanoRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x00069FA8 File Offset: 0x000681A8
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetHex(delegate(HexCoord x)
			{
				this.TargetContext.SetTargetHex(x);
			}, new ActionPhase_Target<HexCoord>.IsValidFunc(this.IsValidHex), 1);
			yield break;
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x00069FB8 File Offset: 0x000681B8
		public override Result IsValidHex(TurnContext context, List<HexCoord> selected, HexCoord hexCoord, int castingPlayerId)
		{
			Problem problem = base.IsValidHex(context, selected, hexCoord, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePieceAt = context.CurrentTurn.GetGamePieceAt(hexCoord);
			if (gamePieceAt != null && gamePieceAt.IsFixture())
			{
				return Result.Failure;
			}
			return Result.Success;
		}
	}
}
