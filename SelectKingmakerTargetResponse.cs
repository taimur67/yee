using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004C0 RID: 1216
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SelectKingmakerTargetResponse : DecisionResponse
	{
		// Token: 0x060016BC RID: 5820 RVA: 0x000537B6 File Offset: 0x000519B6
		public bool IsSelected(int targetId)
		{
			return this.SelectedTargetId == targetId;
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000537C1 File Offset: 0x000519C1
		public void Select(int targetId)
		{
			this.SelectedTargetId = targetId;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000537CA File Offset: 0x000519CA
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetArchfiend(new Action<int>(this.Select), new ActionPhase_SingleTarget<int>.IsValidFunc(this.IsValidArchfiend));
			yield break;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000537DA File Offset: 0x000519DA
		protected internal Result IsValidArchfiend(TurnContext context, int targetPlayerId, int castingPlayerId)
		{
			if (targetPlayerId != castingPlayerId && targetPlayerId != -1)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x000537F0 File Offset: 0x000519F0
		public override void DeepClone(out DecisionResponse clone)
		{
			SelectKingmakerTargetResponse selectKingmakerTargetResponse = new SelectKingmakerTargetResponse
			{
				SelectedTargetId = this.SelectedTargetId
			};
			base.DeepCloneDecisionResponseParts(selectKingmakerTargetResponse);
			clone = selectKingmakerTargetResponse;
		}

		// Token: 0x04000B38 RID: 2872
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int SelectedTargetId = int.MinValue;
	}
}
