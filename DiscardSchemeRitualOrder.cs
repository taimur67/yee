using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200066D RID: 1645
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DiscardSchemeRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E5B RID: 7771 RVA: 0x00068BB6 File Offset: 0x00066DB6
		public DiscardSchemeRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00068BC3 File Offset: 0x00066DC3
		public DiscardSchemeRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x00068BCC File Offset: 0x00066DCC
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetArchfiend(delegate(int targetPlayerId)
			{
				this.TargetContext.SetTargetPlayer(targetPlayerId);
			}, new ActionPhase_SingleTarget<int>.IsValidFunc(base.IsValidArchfiendWithValidGameItem));
			yield return new ActionPhase_SelectScheme(delegate(Identifier targetScheme)
			{
				this.SchemeCardId = targetScheme;
			}, (TurnContext _, List<Identifier> _, Identifier _, int _) => Result.Success, 1);
			yield break;
		}

		// Token: 0x04000CD6 RID: 3286
		[JsonProperty]
		public Identifier SchemeCardId;
	}
}
