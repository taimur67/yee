using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200067C RID: 1660
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class IncreasePassivePrestigeRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E8D RID: 7821 RVA: 0x0006949F File Offset: 0x0006769F
		public IncreasePassivePrestigeRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x000694AC File Offset: 0x000676AC
		public IncreasePassivePrestigeRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000694B5 File Offset: 0x000676B5
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			this.TargetContext.SetTargetPlayer(player.Id);
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
