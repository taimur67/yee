using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000619 RID: 1561
	public class StartTurnModuleEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CE9 RID: 7401 RVA: 0x00063CA4 File Offset: 0x00061EA4
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
