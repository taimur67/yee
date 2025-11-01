using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005EE RID: 1518
	public class BlockRitualsEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001C7B RID: 7291 RVA: 0x0006222D File Offset: 0x0006042D
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
