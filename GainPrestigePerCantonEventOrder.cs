using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005F8 RID: 1528
	public class GainPrestigePerCantonEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001C94 RID: 7316 RVA: 0x00062908 File Offset: 0x00060B08
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
