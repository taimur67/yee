using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000688 RID: 1672
	public class ModifyRitualStateRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EB8 RID: 7864 RVA: 0x00069D09 File Offset: 0x00067F09
		public ModifyRitualStateRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x00069D16 File Offset: 0x00067F16
		public ModifyRitualStateRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x00069D1F File Offset: 0x00067F1F
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			if (!this.IsSelfTargeted(database))
			{
				yield return new ActionPhase_TargetArchfiend(delegate(int x)
				{
					this.TargetContext.SetTargetPlayer(x);
				}, new ActionPhase_SingleTarget<int>.IsValidFunc(this.IsValidArchfiend));
			}
			else
			{
				this.TargetContext.SetTargetPlayer(player.Id);
			}
			yield break;
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x00069D3D File Offset: 0x00067F3D
		public bool IsSelfTargeted(GameDatabase database)
		{
			return database.Fetch<ModifyRitualStateRitualData>(base.RitualId).PlayerTargetSettings.SelfTarget;
		}
	}
}
