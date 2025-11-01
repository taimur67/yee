using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200061B RID: 1563
	public class UnholyCrusadeEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CED RID: 7405 RVA: 0x00063D1A File Offset: 0x00061F1A
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
