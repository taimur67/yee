using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000608 RID: 1544
	public class PaladinInHellEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CBC RID: 7356 RVA: 0x00063291 File Offset: 0x00061491
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
