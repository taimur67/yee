using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005D2 RID: 1490
	public abstract class DiplomaticDemandOrder : DiplomaticOrder, IDemandAccessor, ISelectionAccessor
	{
		// Token: 0x06001BFC RID: 7164 RVA: 0x00060E42 File Offset: 0x0005F042
		public DiplomaticDemandOrder(int targetID, Payment payment = null) : base(targetID, payment)
		{
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x00060E53 File Offset: 0x0005F053
		[JsonConstructor]
		protected DiplomaticDemandOrder()
		{
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001BFE RID: 7166 RVA: 0x00060E62 File Offset: 0x0005F062
		// (set) Token: 0x06001BFF RID: 7167 RVA: 0x00060E6A File Offset: 0x0005F06A
		[JsonProperty]
		[DefaultValue(DemandOptions.None)]
		public DemandOptions DemandOption { get; set; } = DemandOptions.None;

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001C00 RID: 7168 RVA: 0x00060E73 File Offset: 0x0005F073
		[JsonIgnore]
		public int NumCards
		{
			get
			{
				return this.DemandOption.ToNumCards();
			}
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00060E80 File Offset: 0x0005F080
		public int GetCurrentSelectionInt(PlayerState player, TurnState turnState)
		{
			return Math.Max(0, this.GetValidOptions(player, turnState).IndexOf(this.DemandOption));
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00060E9B File Offset: 0x0005F09B
		public List<string> GetValidOptionStrings(PlayerState player, TurnState turnState)
		{
			return (from t in this.GetValidOptions(player, turnState)
			select t.ToString()).ToList<string>();
		}

		// Token: 0x06001C03 RID: 7171
		public abstract List<DemandOptions> GetValidOptions(PlayerState player, TurnState turnState);

		// Token: 0x06001C04 RID: 7172 RVA: 0x00060ECE File Offset: 0x0005F0CE
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			yield return new ActionPhase_SingleTarget<DemandOptions>(new Action<DemandOptions>(this.SetDemand), new ActionPhase_SingleTarget<DemandOptions>.IsValidFunc(this.IsValidDemand));
			yield break;
			yield break;
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x00060EF3 File Offset: 0x0005F0F3
		public void SetDemand(DemandOptions option)
		{
			this.DemandOption = option;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x00060EFC File Offset: 0x0005F0FC
		protected Result IsValidDemand(TurnContext context, DemandOptions target, int castingPlayerId)
		{
			if (!this.GetValidOptions(context.CurrentTurn.FindPlayerState(castingPlayerId, null), context.CurrentTurn).Contains(target))
			{
				return Result.Failure;
			}
			return Result.Success;
		}
	}
}
