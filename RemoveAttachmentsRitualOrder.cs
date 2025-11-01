using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000691 RID: 1681
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RemoveAttachmentsRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EDB RID: 7899 RVA: 0x0006A4A7 File Offset: 0x000686A7
		public RemoveAttachmentsRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0006A4B4 File Offset: 0x000686B4
		public RemoveAttachmentsRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0006A4BD File Offset: 0x000686BD
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
