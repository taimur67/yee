using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000664 RID: 1636
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DamageGamePieceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E36 RID: 7734 RVA: 0x00068279 File Offset: 0x00066479
		public DamageGamePieceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x00068286 File Offset: 0x00066486
		public DamageGamePieceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x0006828F File Offset: 0x0006648F
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
