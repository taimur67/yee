using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005F6 RID: 1526
	public class EndAllGrievancesEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001C8F RID: 7311 RVA: 0x00062801 File Offset: 0x00060A01
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
