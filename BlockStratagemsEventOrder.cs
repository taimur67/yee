using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005F0 RID: 1520
	public class BlockStratagemsEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001C7F RID: 7295 RVA: 0x000623B4 File Offset: 0x000605B4
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
