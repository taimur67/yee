using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200065A RID: 1626
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BlockRitualSlotsRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E0F RID: 7695 RVA: 0x000678E2 File Offset: 0x00065AE2
		public BlockRitualSlotsRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000678EF File Offset: 0x00065AEF
		public BlockRitualSlotsRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000678F8 File Offset: 0x00065AF8
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
