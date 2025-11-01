using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000657 RID: 1623
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BlockMovementRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E03 RID: 7683 RVA: 0x0006778F File Offset: 0x0006598F
		[JsonConstructor]
		public BlockMovementRitualOrder()
		{
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x00067797 File Offset: 0x00065997
		public BlockMovementRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000677A0 File Offset: 0x000659A0
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield break;
		}
	}
}
