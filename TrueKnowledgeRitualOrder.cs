using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006C0 RID: 1728
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TrueKnowledgeRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001F9B RID: 8091 RVA: 0x0006C88B File Offset: 0x0006AA8B
		public TrueKnowledgeRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0006C898 File Offset: 0x0006AA98
		public TrueKnowledgeRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x0006C8A1 File Offset: 0x0006AAA1
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield break;
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0006C8B1 File Offset: 0x0006AAB1
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new BalefulGazeConflict();
			yield break;
		}
	}
}
