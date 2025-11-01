using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200067E RID: 1662
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class InfernalJuggernautRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E92 RID: 7826 RVA: 0x00069560 File Offset: 0x00067760
		public InfernalJuggernautRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0006956D File Offset: 0x0006776D
		public InfernalJuggernautRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x00069576 File Offset: 0x00067776
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
