using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003C1 RID: 961
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PraetorDuel_SelectCombatMove_DecisionRequest : DecisionRequest<PraetorDuel_SelectCombatMove_DecisionResponse>, IDiplomaticDecisionRequest
	{
		// Token: 0x060012DB RID: 4827 RVA: 0x000480BC File Offset: 0x000462BC
		[JsonConstructor]
		private PraetorDuel_SelectCombatMove_DecisionRequest()
		{
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x000480C4 File Offset: 0x000462C4
		public PraetorDuel_SelectCombatMove_DecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x000480CD File Offset: 0x000462CD
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.PraetorSelectMoves;
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x000480D4 File Offset: 0x000462D4
		public int RequestingPlayerId
		{
			get
			{
				return this.Contestants.First;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x000480E1 File Offset: 0x000462E1
		public int AffectedPlayerId
		{
			get
			{
				return this.Contestants.Second;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x000480EE File Offset: 0x000462EE
		// (set) Token: 0x060012E1 RID: 4833 RVA: 0x000480F6 File Offset: 0x000462F6
		[JsonIgnore]
		[BindableValue("prestige", BindingOption.None)]
		public int PrestigeWager
		{
			get
			{
				return this._prestigeWager;
			}
			set
			{
				this._prestigeWager = value;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x000480FF File Offset: 0x000462FF
		public OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Vendetta;
			}
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00048102 File Offset: 0x00046302
		public bool RelatesToPlayers(PlayerPair pair)
		{
			return this.Contestants.Equals(pair);
		}

		// Token: 0x040008C1 RID: 2241
		[JsonProperty]
		public PlayerPair Contestants;

		// Token: 0x040008C2 RID: 2242
		[JsonProperty]
		public Identifier Praetor;

		// Token: 0x040008C3 RID: 2243
		[JsonProperty]
		private int _prestigeWager;
	}
}
