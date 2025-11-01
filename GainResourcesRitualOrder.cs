using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000672 RID: 1650
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GainResourcesRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E6B RID: 7787 RVA: 0x00068EE8 File Offset: 0x000670E8
		public GainResourcesRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x00068EF5 File Offset: 0x000670F5
		public GainResourcesRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x00068EFE File Offset: 0x000670FE
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			this.TargetContext.SetTargetPlayer(player.Id);
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
