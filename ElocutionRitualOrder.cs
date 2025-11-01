using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000670 RID: 1648
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ElocutionRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E66 RID: 7782 RVA: 0x00068E2A File Offset: 0x0006702A
		public ElocutionRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x00068E37 File Offset: 0x00067037
		public ElocutionRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x00068E40 File Offset: 0x00067040
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			this.TargetContext.SetTargetPlayer(player.Id);
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
