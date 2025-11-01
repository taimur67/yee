using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200069E RID: 1694
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class StealTributeRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001F21 RID: 7969 RVA: 0x0006B581 File Offset: 0x00069781
		public StealTributeRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0006B58E File Offset: 0x0006978E
		public StealTributeRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0006B597 File Offset: 0x00069797
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
