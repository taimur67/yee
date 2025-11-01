using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200065C RID: 1628
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BuffGamePieceHitPointsRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E16 RID: 7702 RVA: 0x00067C17 File Offset: 0x00065E17
		public BuffGamePieceHitPointsRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x00067C24 File Offset: 0x00065E24
		public BuffGamePieceHitPointsRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x00067C2D File Offset: 0x00065E2D
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			this.TargetContext.SetTargetPlayer(player.Id);
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
