using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000679 RID: 1657
	public class HellsMawRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E82 RID: 7810 RVA: 0x0006933E File Offset: 0x0006753E
		public HellsMawRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0006934B File Offset: 0x0006754B
		public HellsMawRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x00069354 File Offset: 0x00067554
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			this.TargetContext.SetTargetPlayer(player.Id);
			return Enumerable.Empty<ActionPhase>();
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0006936C File Offset: 0x0006756C
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new HellsMawConflict();
			yield break;
		}
	}
}
