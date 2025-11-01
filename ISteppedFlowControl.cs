using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200049F RID: 1183
	public interface ISteppedFlowControl
	{
		// Token: 0x0600161A RID: 5658
		IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database);
	}
}
