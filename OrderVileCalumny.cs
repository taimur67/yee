using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005EC RID: 1516
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderVileCalumny : DiplomaticOrder
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001C6E RID: 7278 RVA: 0x00062026 File Offset: 0x00060226
		// (set) Token: 0x06001C6F RID: 7279 RVA: 0x0006202E File Offset: 0x0006022E
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int ScapegoatId { get; private set; } = int.MinValue;

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x00062037 File Offset: 0x00060237
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.VileCalumny;
			}
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x0006203B File Offset: 0x0006023B
		[JsonConstructor]
		public OrderVileCalumny()
		{
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x0006204E File Offset: 0x0006024E
		public OrderVileCalumny(int targetID, int scapegoatId) : base(targetID, null)
		{
			this.SetScapegoat(scapegoatId);
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x0006206A File Offset: 0x0006026A
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			if (this.ScapegoatId == -1 || this.ScapegoatId == -2147483648)
			{
				yield return new ActionPhase_TargetArchfiend(new Action<int>(this.SetScapegoat), new ActionPhase_SingleTarget<int>.IsValidFunc(this.ValidateScapegoatPlayer));
			}
			yield break;
			yield break;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x0006208F File Offset: 0x0006028F
		public void SetScapegoat(int scapegoatId)
		{
			this.ScapegoatId = scapegoatId;
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00062098 File Offset: 0x00060298
		public static Result ValidateScapegoatPlayer(TurnContext context, int scapegoatId, int instigatorId, int targetId)
		{
			context.CurrentTurn.GetDiplomaticStatus(instigatorId, targetId);
			if (scapegoatId == instigatorId || scapegoatId == targetId)
			{
				return new Result.ScapegoatProblem(targetId, OrderTypes.VileCalumny, scapegoatId);
			}
			if (!(context.CurrentTurn.GetDiplomaticStatus(targetId, scapegoatId).DiplomaticState is NeutralState))
			{
				return new Result.ScapegoatRelationshipProblemProblem(targetId, OrderTypes.VileCalumny, scapegoatId, DiplomaticStateValue.Neutral);
			}
			return DiplomaticStateProcessor.ValidateOrderType(context.CurrentTurn, scapegoatId, targetId, OrderTypes.VileCalumny, false);
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000620F9 File Offset: 0x000602F9
		public Result ValidateScapegoatPlayer(TurnContext context, int scapegoatId, int instigatorId)
		{
			return OrderVileCalumny.ValidateScapegoatPlayer(context, scapegoatId, instigatorId, this.TargetID);
		}
	}
}
