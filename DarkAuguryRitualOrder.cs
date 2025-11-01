using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000666 RID: 1638
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DarkAuguryRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E3D RID: 7741 RVA: 0x000683EB File Offset: 0x000665EB
		public DarkAuguryRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000683F8 File Offset: 0x000665F8
		public DarkAuguryRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x00068401 File Offset: 0x00066601
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
