using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003C2 RID: 962
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuel_SelectCombatMove_DecisionResponse : DecisionResponse
	{
		// Token: 0x060012E4 RID: 4836 RVA: 0x00048110 File Offset: 0x00046310
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			PraetorDuel_SelectCombatMove_DecisionResponse.<>c__DisplayClass3_0 CS$<>8__locals1 = new PraetorDuel_SelectCombatMove_DecisionResponse.<>c__DisplayClass3_0();
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.<>4__this = this;
			PraetorDuel_SelectCombatMove_DecisionRequest request;
			if (!CS$<>8__locals1.player.TryGetDecisionRequest<PraetorDuel_SelectCombatMove_DecisionRequest>(base.DecisionId, out request))
			{
				yield break;
			}
			if (this.CombatMove != null || this.CombatMoveSlot != -1 || !this.Bribe.IsEmpty)
			{
				yield return new ActionPhase_MessageBox(ActionMessageType.ResolvedAction, new Action(this.Reset), null);
			}
			yield return new ActionPhase_ConfigureDuel(request.Contestants, PraetorDuelState.PraetorDuelFlowStage.MoveSelection, new Action<PraetorDuelData>(CS$<>8__locals1.<GetActionPhaseSteps>g__ConfigureCombatMove|0));
			yield break;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00048127 File Offset: 0x00046327
		public void Reset()
		{
			this.CombatMove = null;
			this.CombatMoveSlot = -1;
			this.Bribe = Payment.Empty;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00048144 File Offset: 0x00046344
		public override void DeepClone(out DecisionResponse clone)
		{
			PraetorDuel_SelectCombatMove_DecisionResponse praetorDuel_SelectCombatMove_DecisionResponse = new PraetorDuel_SelectCombatMove_DecisionResponse
			{
				CombatMove = this.CombatMove.DeepClone(),
				CombatMoveSlot = this.CombatMoveSlot,
				Bribe = this.Bribe.DeepClone<Payment>()
			};
			base.DeepCloneDecisionResponseParts(praetorDuel_SelectCombatMove_DecisionResponse);
			clone = praetorDuel_SelectCombatMove_DecisionResponse;
		}

		// Token: 0x040008C4 RID: 2244
		[JsonProperty]
		[DefaultValue(-1)]
		public int CombatMoveSlot = -1;

		// Token: 0x040008C5 RID: 2245
		[JsonProperty]
		public ConfigRef CombatMove;

		// Token: 0x040008C6 RID: 2246
		[JsonProperty]
		public Payment Bribe = new Payment();
	}
}
