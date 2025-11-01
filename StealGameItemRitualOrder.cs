using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200069C RID: 1692
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class StealGameItemRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001F1A RID: 7962 RVA: 0x0006B429 File Offset: 0x00069629
		public StealGameItemRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0006B436 File Offset: 0x00069636
		public StealGameItemRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0006B43F File Offset: 0x0006963F
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGameItem(delegate(Identifier targetItem)
			{
				this.TargetContext.SetTargetGameItem(targetItem, turn.FindControllingPlayer(targetItem).Id);
			}, new ActionPhase_Target<Identifier>.IsValidFunc(this.IsValidGameItem), new ActionPhase_TargetGameItem.IsValidArchfiendFunc(base.IsValidArchfiendWithValidGameItem), new ActionPhase_TargetGameItem.IsSelectableGameItemFunc(base.FilterGameItem), 1, ActionPhaseType.None);
			yield break;
		}
	}
}
