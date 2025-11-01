using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000693 RID: 1683
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RevealOrdersRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EE2 RID: 7906 RVA: 0x0006A696 File Offset: 0x00068896
		public RevealOrdersRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0006A6A3 File Offset: 0x000688A3
		public RevealOrdersRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0006A6AC File Offset: 0x000688AC
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetArchfiend(delegate(int x)
			{
				this.TargetContext.SetTargetPlayer(x);
			}, new ActionPhase_SingleTarget<int>.IsValidFunc(this.IsValidArchfiend));
			yield break;
		}
	}
}
