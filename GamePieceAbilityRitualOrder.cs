using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000674 RID: 1652
	public class GamePieceAbilityRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E70 RID: 7792 RVA: 0x0006904D File Offset: 0x0006724D
		public GamePieceAbilityRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0006905A File Offset: 0x0006725A
		public GamePieceAbilityRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x00069063 File Offset: 0x00067263
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
