using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000606 RID: 1542
	public class NeutraliseGamePieceEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CB7 RID: 7351 RVA: 0x00063173 File Offset: 0x00061373
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
