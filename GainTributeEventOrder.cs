using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005FA RID: 1530
	public class GainTributeEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001C98 RID: 7320 RVA: 0x000629C8 File Offset: 0x00060BC8
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
