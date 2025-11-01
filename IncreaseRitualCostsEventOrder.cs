using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005FF RID: 1535
	public class IncreaseRitualCostsEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CAA RID: 7338 RVA: 0x00062D38 File Offset: 0x00060F38
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
