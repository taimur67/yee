using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000698 RID: 1688
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RitualResistanceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001F0D RID: 7949 RVA: 0x0006B25C File Offset: 0x0006945C
		public RitualResistanceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x0006B269 File Offset: 0x00069469
		public RitualResistanceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x0006B272 File Offset: 0x00069472
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

		// Token: 0x06001F10 RID: 7952 RVA: 0x0006B290 File Offset: 0x00069490
		public bool IsSelfTargeted(GameDatabase database)
		{
			return database.Fetch<RitualResistanceRitualData>(base.RitualId).PlayerTargetSettings.SelfTarget;
		}
	}
}
