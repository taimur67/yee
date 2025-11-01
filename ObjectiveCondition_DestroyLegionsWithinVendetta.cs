using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000575 RID: 1397
	[Serializable]
	public class ObjectiveCondition_DestroyLegionsWithinVendetta : ObjectiveCondition_DestroyLegions
	{
		// Token: 0x06001AB3 RID: 6835 RVA: 0x0005D2FC File Offset: 0x0005B4FC
		public override void Update(TurnContext context, PlayerState owner)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
			{
				if (!(currentTurn.GetDiplomaticStatus(owner, playerState).DiplomaticState is VendettaState))
				{
					this.VendettaKills[playerState.Id] = 0;
				}
			}
			base.Update(context, owner);
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x0005D37C File Offset: 0x0005B57C
		protected override void OnCommitted(PlayerState owner, PlayerState target, LegionKilledEvent @event)
		{
			base.OnCommitted(owner, target, @event);
			if (this.VendettaKills.ContainsKey(@event.AffectedPlayerID))
			{
				Dictionary<int, int> vendettaKills = this.VendettaKills;
				int affectedPlayerID = @event.AffectedPlayerID;
				int num = vendettaKills[affectedPlayerID];
				vendettaKills[affectedPlayerID] = num + 1;
				return;
			}
			this.VendettaKills.Add(@event.AffectedPlayerID, 1);
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x0005D3D6 File Offset: 0x0005B5D6
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (isInitialProgress)
			{
				return 0;
			}
			this.CalculateProgressIncrement(context, owner);
			if (this.VendettaKills.Count > 0)
			{
				return this.VendettaKills.Values.Max();
			}
			return 0;
		}

		// Token: 0x04000C15 RID: 3093
		[JsonProperty]
		public Dictionary<int, int> VendettaKills = new Dictionary<int, int>();
	}
}
