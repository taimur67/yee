using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000677 RID: 1655
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HealGamePieceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E7C RID: 7804 RVA: 0x0006923F File Offset: 0x0006743F
		public HealGamePieceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0006924C File Offset: 0x0006744C
		public HealGamePieceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x00069255 File Offset: 0x00067455
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
