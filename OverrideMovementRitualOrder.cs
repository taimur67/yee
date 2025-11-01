using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200068A RID: 1674
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OverrideMovementRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EBF RID: 7871 RVA: 0x00069DF4 File Offset: 0x00067FF4
		public OverrideMovementRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x00069E01 File Offset: 0x00068001
		public OverrideMovementRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x00069E15 File Offset: 0x00068015
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGameItem(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield return new ActionPhase_MovementInput(delegate(List<HexCoord> x)
			{
				this.MovePath = x;
			}, () => turn.FetchGameItem<GamePiece>(this.TargetItemId));
			yield break;
		}

		// Token: 0x04000CE2 RID: 3298
		[JsonProperty]
		public List<HexCoord> MovePath = new List<HexCoord>();
	}
}
