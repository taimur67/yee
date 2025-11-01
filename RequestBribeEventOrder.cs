using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000615 RID: 1557
	public class RequestBribeEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CE0 RID: 7392 RVA: 0x00063A68 File Offset: 0x00061C68
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
