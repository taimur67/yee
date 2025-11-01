using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020006C9 RID: 1737
	public class FlowControl : ISteppedFlowControl
	{
		// Token: 0x06001FC3 RID: 8131 RVA: 0x0006D3E8 File Offset: 0x0006B5E8
		public FlowControl(params ActionPhase[] phases)
		{
			this._phases = phases;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0006D3F7 File Offset: 0x0006B5F7
		public IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return this._phases;
		}

		// Token: 0x04000D19 RID: 3353
		private readonly ActionPhase[] _phases;
	}
}
