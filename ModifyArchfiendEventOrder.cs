using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000601 RID: 1537
	public class ModifyArchfiendEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CAE RID: 7342 RVA: 0x00062E6F File Offset: 0x0006106F
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
