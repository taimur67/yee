using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000686 RID: 1670
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ModifyGamePieceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EB2 RID: 7858 RVA: 0x00069BEE File Offset: 0x00067DEE
		public ModifyGamePieceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x00069BFB File Offset: 0x00067DFB
		public ModifyGamePieceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x00069C04 File Offset: 0x00067E04
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
