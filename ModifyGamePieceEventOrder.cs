using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000604 RID: 1540
	public class ModifyGamePieceEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CB3 RID: 7347 RVA: 0x0006307B File Offset: 0x0006127B
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
