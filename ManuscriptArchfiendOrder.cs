using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200061F RID: 1567
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ManuscriptArchfiendOrder : InvokeManuscriptOrder
	{
		// Token: 0x06001CFB RID: 7419 RVA: 0x000641EC File Offset: 0x000623EC
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x000641F3 File Offset: 0x000623F3
		public override bool NeedsTargetContext
		{
			get
			{
				return false;
			}
		}
	}
}
