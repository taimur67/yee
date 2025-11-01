using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000695 RID: 1685
	public class RevealVendettaRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EE8 RID: 7912 RVA: 0x0006A756 File Offset: 0x00068956
		public RevealVendettaRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0006A763 File Offset: 0x00068963
		public RevealVendettaRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x0006A76C File Offset: 0x0006896C
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetArchfiend(delegate(int x)
			{
				this.TargetContext.SetTargetPlayer(x);
				this.VendettaPair.First = x;
			}, new ActionPhase_SingleTarget<int>.IsValidFunc(this.IsValidFirstPlayer));
			yield return new ActionPhase_TargetArchfiend(delegate(int x)
			{
				this.VendettaPair.Second = x;
			}, new ActionPhase_SingleTarget<int>.IsValidFunc(this.IsValidSecondPlayer));
			yield break;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x0006A77C File Offset: 0x0006897C
		private Result IsValidFirstPlayer(TurnContext context, int targetPlayerId, int castingPlayerId)
		{
			if (targetPlayerId == castingPlayerId)
			{
				return Result.Failure;
			}
			if (!IEnumerableExtensions.Any<VendettaState>(context.Diplomacy.DiplomaticStatesOfType<VendettaState>(targetPlayerId)))
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x0006A7A6 File Offset: 0x000689A6
		private Result IsValidSecondPlayer(TurnContext context, int targetPlayerId, int castingPlayerId)
		{
			if (!(context.Diplomacy.GetDiplomaticStatus(this.TargetContext.PlayerId, targetPlayerId).DiplomaticState is VendettaState))
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x04000CE6 RID: 3302
		[JsonProperty]
		public PlayerPair VendettaPair;
	}
}
