using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000617 RID: 1559
	public class RestoreExcommunicatedEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CE5 RID: 7397 RVA: 0x00063BD7 File Offset: 0x00061DD7
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
