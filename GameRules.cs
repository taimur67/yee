using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002E9 RID: 745
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GameRules : IDeepClone<GameRules>
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00039E55 File Offset: 0x00038055
		[JsonIgnore]
		public int NoNewEdictsFromTurn
		{
			get
			{
				return this.GameDuration - this.VotingProclamationOffset;
			}
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00039E64 File Offset: 0x00038064
		public int GetRequiredVendettasForBloodFeud(PlayerState player)
		{
			return this.BaseVendettaWinsForBloodFeud + player.VendettasForBloodFeudOffset;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00039E78 File Offset: 0x00038078
		public void DeepClone(out GameRules clone)
		{
			clone = new GameRules
			{
				OfferingFirstTurn = this.OfferingFirstTurn,
				OfferingInterval = this.OfferingInterval,
				NumResourceSlots = this.NumResourceSlots,
				VaultSize = this.VaultSize,
				GameDuration = this.GameDuration,
				HoldPandaemonium = this.HoldPandaemonium,
				PrestigeForReturningPanda = this.PrestigeForReturningPanda,
				ExcommedPrestigeForReturningPanda = this.ExcommedPrestigeForReturningPanda,
				BaseVendettaWinsForBloodFeud = this.BaseVendettaWinsForBloodFeud,
				BaseCombatRewardMultiplier = this.BaseCombatRewardMultiplier,
				ConcessionsForAssertionOfWeakness = this.ConcessionsForAssertionOfWeakness,
				MinDiplomacyOrderCooldown = this.MinDiplomacyOrderCooldown,
				KingmakerDecisionTurns = this.KingmakerDecisionTurns,
				MaximumValueOfIndividualResourceOnToken = this.MaximumValueOfIndividualResourceOnToken,
				BlacklistedEntities = this.BlacklistedEntities.DeepClone(CloneFunction.FastClone),
				VictoryRules = this.VictoryRules.DeepClone<VictoryRule>(),
				RitualResitanceEnabled = this.RitualResitanceEnabled,
				UpkeepEnabled = this.UpkeepEnabled,
				ShowAllBazaarItems = this.ShowAllBazaarItems,
				PrepayGameItemUpkeep = this.PrepayGameItemUpkeep,
				PrepayRitualUpkeep = this.PrepayRitualUpkeep,
				AllowUpkeepOverpay = this.AllowUpkeepOverpay,
				VotingFirstTurn = this.VotingFirstTurn,
				VotingInterval = this.VotingInterval,
				VotingProclamationOffset = this.VotingProclamationOffset,
				EmergencyVoteMax = this.EmergencyVoteMax,
				EmergencyVoteChanceIncrement = this.EmergencyVoteChanceIncrement,
				TriggerGenericCannedMessages = this.TriggerGenericCannedMessages,
				FramedRitualsCanExcommunicate = this.FramedRitualsCanExcommunicate,
				DuelForfeitPenalty = this.DuelForfeitPenalty
			};
		}

		// Token: 0x0400067B RID: 1659
		[JsonProperty]
		[DefaultValue(5)]
		public int OfferingFirstTurn = 5;

		// Token: 0x0400067C RID: 1660
		[JsonProperty]
		[DefaultValue(5)]
		public int OfferingInterval = 5;

		// Token: 0x0400067D RID: 1661
		[JsonProperty]
		[DefaultValue(8)]
		public int NumResourceSlots = 8;

		// Token: 0x0400067E RID: 1662
		[JsonProperty]
		[DefaultValue(48)]
		public int VaultSize = 48;

		// Token: 0x0400067F RID: 1663
		[JsonProperty]
		[DefaultValue(50)]
		public int GameDuration = 50;

		// Token: 0x04000680 RID: 1664
		[JsonProperty]
		[DefaultValue(5)]
		public int HoldPandaemonium = 5;

		// Token: 0x04000681 RID: 1665
		[JsonProperty]
		[DefaultValue(30)]
		public int PrestigeForReturningPanda = 30;

		// Token: 0x04000682 RID: 1666
		[JsonProperty]
		[DefaultValue(15)]
		public int ExcommedPrestigeForReturningPanda = 15;

		// Token: 0x04000683 RID: 1667
		[JsonProperty]
		[DefaultValue(3)]
		public int BaseVendettaWinsForBloodFeud = 3;

		// Token: 0x04000684 RID: 1668
		[JsonProperty]
		[DefaultValue(2)]
		public int BaseCombatRewardMultiplier = 2;

		// Token: 0x04000685 RID: 1669
		[JsonProperty]
		[DefaultValue(2)]
		public int ConcessionsForAssertionOfWeakness = 2;

		// Token: 0x04000686 RID: 1670
		[JsonProperty]
		[DefaultValue(3)]
		public int MinDiplomacyOrderCooldown = 3;

		// Token: 0x04000687 RID: 1671
		[JsonProperty]
		[DefaultValue(10)]
		public int KingmakerDecisionTurns = 10;

		// Token: 0x04000688 RID: 1672
		[JsonProperty]
		[DefaultValue(9)]
		public int MaximumValueOfIndividualResourceOnToken = 9;

		// Token: 0x04000689 RID: 1673
		[JsonProperty]
		[DefaultValue(15)]
		public int DuelForfeitPenalty = 15;

		// Token: 0x0400068A RID: 1674
		[JsonProperty]
		[DefaultValue(true)]
		public bool FramedRitualsCanExcommunicate = true;

		// Token: 0x0400068B RID: 1675
		[JsonProperty]
		public List<VictoryRule> VictoryRules = new List<VictoryRule>();

		// Token: 0x0400068C RID: 1676
		[JsonProperty]
		public List<ConfigRef> BlacklistedEntities = new List<ConfigRef>();

		// Token: 0x0400068D RID: 1677
		[JsonProperty]
		[DefaultValue(true)]
		public bool RitualResitanceEnabled = true;

		// Token: 0x0400068E RID: 1678
		[JsonProperty]
		[DefaultValue(true)]
		public bool UpkeepEnabled = true;

		// Token: 0x0400068F RID: 1679
		[JsonProperty]
		[DefaultValue(false)]
		public bool ShowAllBazaarItems;

		// Token: 0x04000690 RID: 1680
		[JsonProperty]
		[DefaultValue(true)]
		public bool PrepayGameItemUpkeep = true;

		// Token: 0x04000691 RID: 1681
		[JsonProperty]
		[DefaultValue(false)]
		public bool PrepayRitualUpkeep;

		// Token: 0x04000692 RID: 1682
		[JsonProperty]
		[DefaultValue(false)]
		public bool AllowUpkeepOverpay;

		// Token: 0x04000693 RID: 1683
		[JsonProperty]
		[DefaultValue(10)]
		public int VotingFirstTurn = 10;

		// Token: 0x04000694 RID: 1684
		[JsonProperty]
		[DefaultValue(10)]
		public int VotingInterval = 10;

		// Token: 0x04000695 RID: 1685
		[JsonProperty]
		[DefaultValue(3)]
		public int VotingProclamationOffset = 3;

		// Token: 0x04000696 RID: 1686
		[JsonProperty]
		[DefaultValue(1)]
		public int EmergencyVoteMax = 1;

		// Token: 0x04000697 RID: 1687
		[JsonProperty]
		[DefaultValue(0.02f)]
		public float EmergencyVoteChanceIncrement = 0.02f;

		// Token: 0x04000698 RID: 1688
		[JsonProperty]
		[DefaultValue(true)]
		public bool TriggerGenericCannedMessages = true;
	}
}
