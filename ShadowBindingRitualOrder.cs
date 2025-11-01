using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200069A RID: 1690
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ShadowBindingRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001F14 RID: 7956 RVA: 0x0006B348 File Offset: 0x00069548
		public ShadowBindingRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0006B355 File Offset: 0x00069555
		public ShadowBindingRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0006B35E File Offset: 0x0006955E
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece gamePiece)
			{
				this.TargetContext.SetTargetGamePiece(gamePiece);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield break;
		}
	}
}
