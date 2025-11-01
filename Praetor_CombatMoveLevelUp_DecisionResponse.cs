using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003C5 RID: 965
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Praetor_CombatMoveLevelUp_DecisionResponse : DecisionResponse
	{
		// Token: 0x060012F0 RID: 4848 RVA: 0x00048350 File Offset: 0x00046550
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			Praetor_CombatMoveLevelUp_DecisionRequest request;
			if (player.TryGetDecisionRequest<Praetor_CombatMoveLevelUp_DecisionRequest>(base.DecisionId, out request))
			{
				yield return new ActionPhase_SelectPraetorCombatMove(request.Praetor, new Action<ConfigRef<PraetorCombatMoveStaticData>>(this.SetMove), (TurnContext context, ConfigRef<PraetorCombatMoveStaticData> value, int playerId) => this.IsValid(context, request.Praetor, value, playerId));
			}
			yield break;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00048367 File Offset: 0x00046567
		public void SetMove(ConfigRef<PraetorCombatMoveStaticData> move)
		{
			this.CombatMove = move;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00048370 File Offset: 0x00046570
		public Result IsValid(TurnContext context, Identifier praetorId, ConfigRef<PraetorCombatMoveStaticData> target, int castingPlayerId)
		{
			Praetor praetor;
			if (!context.CurrentTurn.TryFetchGameItem<Praetor>(praetorId, out praetor))
			{
				return Result.Failure;
			}
			if (!praetor.CombatMoves.Any((PraetorCombatMoveInstance t) => t.CombatMoveReference.Equals(target)))
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000483C4 File Offset: 0x000465C4
		public override void DeepClone(out DecisionResponse clone)
		{
			Praetor_CombatMoveLevelUp_DecisionResponse praetor_CombatMoveLevelUp_DecisionResponse = new Praetor_CombatMoveLevelUp_DecisionResponse
			{
				CombatMove = this.CombatMove.DeepClone<PraetorCombatMoveStaticData>()
			};
			base.DeepCloneDecisionResponseParts(praetor_CombatMoveLevelUp_DecisionResponse);
			clone = praetor_CombatMoveLevelUp_DecisionResponse;
		}

		// Token: 0x040008C9 RID: 2249
		[JsonProperty]
		public ConfigRef<PraetorCombatMoveStaticData> CombatMove;
	}
}
