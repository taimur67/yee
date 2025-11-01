using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000682 RID: 1666
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ModifyArchfiendRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EA3 RID: 7843 RVA: 0x000699B8 File Offset: 0x00067BB8
		public ModifyArchfiendRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000699C5 File Offset: 0x00067BC5
		public ModifyArchfiendRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000699CE File Offset: 0x00067BCE
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

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000699EC File Offset: 0x00067BEC
		public bool IsSelfTargeted(GameDatabase database)
		{
			return database.Fetch<ModifyArchfiendRitualData>(base.RitualId).PlayerTargetSettings.SelfTarget;
		}
	}
}
