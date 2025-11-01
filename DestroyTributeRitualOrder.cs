using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200066A RID: 1642
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DestroyTributeRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E4C RID: 7756 RVA: 0x000687C0 File Offset: 0x000669C0
		public DestroyTributeRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000687CD File Offset: 0x000669CD
		public DestroyTributeRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000687D6 File Offset: 0x000669D6
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
