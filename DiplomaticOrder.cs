using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005C8 RID: 1480
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class DiplomaticOrder : ActionableOrder
	{
		// Token: 0x06001BD8 RID: 7128 RVA: 0x00060B98 File Offset: 0x0005ED98
		protected DiplomaticOrder()
		{
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x00060BA7 File Offset: 0x0005EDA7
		protected DiplomaticOrder(Payment payment)
		{
			if (payment != null)
			{
				this.Payment = payment;
			}
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x00060BC0 File Offset: 0x0005EDC0
		public virtual IEnumerable<OrderTypes> GetRelatedOrderTypes()
		{
			return Enumerable.Empty<OrderTypes>();
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x00060BC7 File Offset: 0x0005EDC7
		protected DiplomaticOrder(int targetID, Payment payment = null) : this(payment)
		{
			this.TargetID = targetID;
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x00060BD7 File Offset: 0x0005EDD7
		public override Result ConflictProblem(ActionConflict conflict, ActionableOrder conflictingOrder)
		{
			return new Result.QueuedDiplomacyActionAgainstTargetProblem(this.TargetID, this.OrderType, conflictingOrder.OrderType);
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x00060BF0 File Offset: 0x0005EDF0
		public override Result AbilityLockedProblem(PlayerState instigator, AbilityStaticData ability)
		{
			DiplomaticAbilityStaticData diplomaticAbilityStaticData = ability as DiplomaticAbilityStaticData;
			if (diplomaticAbilityStaticData == null)
			{
				return new SimulationError(string.Format("DiplomaticOrder {0} is associated with unhandled ability type {1}", this, ability));
			}
			Rank requiredRank = Rank.Prince - diplomaticAbilityStaticData.RankCostOverrides.Count;
			return new Result.RankTooLowForDiplomacyProblem(this.TargetID, this.OrderType, requiredRank, instigator.Id);
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x00060C3F File Offset: 0x0005EE3F
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			if (this.TargetID == -1 || this.TargetID == -2147483648)
			{
				yield return new ActionPhase_TargetArchfiend(new Action<int>(this.SetTargetPlayer), new ActionPhase_SingleTarget<int>.IsValidFunc(this.ValidateTargetPlayer));
			}
			yield break;
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x00060C4F File Offset: 0x0005EE4F
		public void SetTargetPlayer(int playerId)
		{
			this.TargetID = playerId;
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x00060C58 File Offset: 0x0005EE58
		public Result ValidateTargetPlayer(TurnContext context, int targetId, int castingPlayerId)
		{
			return DiplomaticStateProcessor.ValidateOrderType(context.CurrentTurn, castingPlayerId, targetId, this.OrderType, false);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x00060C6E File Offset: 0x0005EE6E
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new ConductDiplomacyConflict(this.TargetID);
			yield break;
		}

		// Token: 0x04000C69 RID: 3177
		[JsonProperty]
		[DefaultValue(-1)]
		[BindableValue("affected_name", BindingOption.IntPlayerId)]
		public int TargetID = -1;
	}
}
