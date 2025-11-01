using System;

namespace LoG
{
	// Token: 0x020006E2 RID: 1762
	public enum HeuristicKey
	{
		// Token: 0x04000E6D RID: 3693
		Invalid = -1,
		// Token: 0x04000E6E RID: 3694
		TurnsProcessed,
		// Token: 0x04000E6F RID: 3695
		ActivePlayerTurns,
		// Token: 0x04000E70 RID: 3696
		CantonsCapturedFromForceMajeure,
		// Token: 0x04000E71 RID: 3697
		CantonsCapturedFromOtherPlayers,
		// Token: 0x04000E72 RID: 3698
		BattlesBetweenPlayers,
		// Token: 0x04000E73 RID: 3699
		BattlesWithForceMajeure,
		// Token: 0x04000E74 RID: 3700
		InsultsThrown,
		// Token: 0x04000E75 RID: 3701
		InsultAccepted,
		// Token: 0x04000E76 RID: 3702
		DemandsMade,
		// Token: 0x04000E77 RID: 3703
		DemandAccepted,
		// Token: 0x04000E78 RID: 3704
		VendettasDeclared,
		// Token: 0x04000E79 RID: 3705
		VendettasWon,
		// Token: 0x04000E7A RID: 3706
		VendettaSuccessRate,
		// Token: 0x04000E7B RID: 3707
		PoPsCapturedByPlayers,
		// Token: 0x04000E7C RID: 3708
		OrderConflicts,
		// Token: 0x04000E7D RID: 3709
		MaxCardsHeld,
		// Token: 0x04000E7E RID: 3710
		VendettasCompleted,
		// Token: 0x04000E7F RID: 3711
		GrandEventsPlayed,
		// Token: 0x04000E80 RID: 3712
		DuelsDeclared,
		// Token: 0x04000E81 RID: 3713
		Exceptions,
		// Token: 0x04000E82 RID: 3714
		GameItemsAttached,
		// Token: 0x04000E83 RID: 3715
		TitanPurchased,
		// Token: 0x04000E84 RID: 3716
		LegionPurchased,
		// Token: 0x04000E85 RID: 3717
		ManuscriptPurchased,
		// Token: 0x04000E86 RID: 3718
		VendettaDeclared_DestroyLegion,
		// Token: 0x04000E87 RID: 3719
		VendettaDeclared_CaptureCantons,
		// Token: 0x04000E88 RID: 3720
		VendettaDeclared_CapturePoPs,
		// Token: 0x04000E89 RID: 3721
		OrderFailed,
		// Token: 0x04000E8A RID: 3722
		SupportMove,
		// Token: 0x04000E8B RID: 3723
		BattlesWithSupport,
		// Token: 0x04000E8C RID: 3724
		HealMove,
		// Token: 0x04000E8D RID: 3725
		FlankToBuffAttack,
		// Token: 0x04000E8E RID: 3726
		ReinforceStrongholdMove,
		// Token: 0x04000E8F RID: 3727
		ThreatenBorders,
		// Token: 0x04000E90 RID: 3728
		VileCalumny,
		// Token: 0x04000E91 RID: 3729
		PraetorAttachedToLegion,
		// Token: 0x04000E92 RID: 3730
		ArtifactAttachedToLegion,
		// Token: 0x04000E93 RID: 3731
		PraetorAttachedToFixture,
		// Token: 0x04000E94 RID: 3732
		ArtifactAttachedToFixture,
		// Token: 0x04000E95 RID: 3733
		BattlesWithStratagemsAttached,
		// Token: 0x04000E96 RID: 3734
		VassalageAccepted,
		// Token: 0x04000E97 RID: 3735
		LavaDamage,
		// Token: 0x04000E98 RID: 3736
		Extorts_Praetor,
		// Token: 0x04000E99 RID: 3737
		Extorts_Artifact,
		// Token: 0x04000E9A RID: 3738
		Humiliates,
		// Token: 0x04000E9B RID: 3739
		RankPurchased,
		// Token: 0x04000E9C RID: 3740
		RankPurchased_Duke,
		// Token: 0x04000E9D RID: 3741
		RankPurchased_Marquis,
		// Token: 0x04000E9E RID: 3742
		RankPurchased_Prince,
		// Token: 0x04000E9F RID: 3743
		BloodFeud,
		// Token: 0x04000EA0 RID: 3744
		LureOfExcess,
		// Token: 0x04000EA1 RID: 3745
		DraconicRazzia,
		// Token: 0x04000EA2 RID: 3746
		ChainsOfAvarice,
		// Token: 0x04000EA3 RID: 3747
		Elimination,
		// Token: 0x04000EA4 RID: 3748
		Upgrade_Destruction_1,
		// Token: 0x04000EA5 RID: 3749
		Upgrade_Destruction_2,
		// Token: 0x04000EA6 RID: 3750
		Upgrade_Destruction_3,
		// Token: 0x04000EA7 RID: 3751
		Upgrade_Destruction_4,
		// Token: 0x04000EA8 RID: 3752
		Upgrade_Destruction_5,
		// Token: 0x04000EA9 RID: 3753
		Upgrade_Destruction_6,
		// Token: 0x04000EAA RID: 3754
		Upgrade_Prophecy_1,
		// Token: 0x04000EAB RID: 3755
		Upgrade_Prophecy_2,
		// Token: 0x04000EAC RID: 3756
		Upgrade_Prophecy_3,
		// Token: 0x04000EAD RID: 3757
		Upgrade_Prophecy_4,
		// Token: 0x04000EAE RID: 3758
		Upgrade_Prophecy_5,
		// Token: 0x04000EAF RID: 3759
		Upgrade_Prophecy_6,
		// Token: 0x04000EB0 RID: 3760
		Upgrade_Wrath_1,
		// Token: 0x04000EB1 RID: 3761
		Upgrade_Wrath_2,
		// Token: 0x04000EB2 RID: 3762
		Upgrade_Wrath_3,
		// Token: 0x04000EB3 RID: 3763
		Upgrade_Wrath_4,
		// Token: 0x04000EB4 RID: 3764
		Upgrade_Wrath_5,
		// Token: 0x04000EB5 RID: 3765
		Upgrade_Wrath_6,
		// Token: 0x04000EB6 RID: 3766
		Upgrade_Deceit_1,
		// Token: 0x04000EB7 RID: 3767
		Upgrade_Deceit_2,
		// Token: 0x04000EB8 RID: 3768
		Upgrade_Deceit_3,
		// Token: 0x04000EB9 RID: 3769
		Upgrade_Deceit_4,
		// Token: 0x04000EBA RID: 3770
		Upgrade_Deceit_5,
		// Token: 0x04000EBB RID: 3771
		Upgrade_Deceit_6,
		// Token: 0x04000EBC RID: 3772
		Upgrade_Charisma_1,
		// Token: 0x04000EBD RID: 3773
		Upgrade_Charisma_2,
		// Token: 0x04000EBE RID: 3774
		Upgrade_Charisma_3,
		// Token: 0x04000EBF RID: 3775
		Upgrade_Charisma_4,
		// Token: 0x04000EC0 RID: 3776
		Upgrade_Charisma_5,
		// Token: 0x04000EC1 RID: 3777
		Upgrade_Charisma_6,
		// Token: 0x04000EC2 RID: 3778
		SeekTribute,
		// Token: 0x04000EC3 RID: 3779
		DanseMacabre,
		// Token: 0x04000EC4 RID: 3780
		BalefulGaze,
		// Token: 0x04000EC5 RID: 3781
		VanityAnointed,
		// Token: 0x04000EC6 RID: 3782
		HellsMaw,
		// Token: 0x04000EC7 RID: 3783
		ArmisticeAccepted,
		// Token: 0x04000EC8 RID: 3784
		ArmistaceSent,
		// Token: 0x04000EC9 RID: 3785
		CastRitual_UndyingVigor,
		// Token: 0x04000ECA RID: 3786
		CastRitual_LootTheVaults,
		// Token: 0x04000ECB RID: 3787
		CastRitual_PilferArtifacts,
		// Token: 0x04000ECC RID: 3788
		CastRitual_BribePraetor,
		// Token: 0x04000ECD RID: 3789
		CastRitual_ConvertLegion,
		// Token: 0x04000ECE RID: 3790
		CastRitual_DarkAugury,
		// Token: 0x04000ECF RID: 3791
		CastRitual_Enfeeble,
		// Token: 0x04000ED0 RID: 3792
		CastRitual_InfernalAffliction,
		// Token: 0x04000ED1 RID: 3793
		CastRitual_CorruptTribute,
		// Token: 0x04000ED2 RID: 3794
		CastRitual_BanishPraetor,
		// Token: 0x04000ED3 RID: 3795
		CastRitual_DireDissipation,
		// Token: 0x04000ED4 RID: 3796
		CastRitual_MaledictionOfTheSeer,
		// Token: 0x04000ED5 RID: 3797
		CastRitual_DemonicInterference,
		// Token: 0x04000ED6 RID: 3798
		CastRitual_PlanarLock,
		// Token: 0x04000ED7 RID: 3799
		MaxOrderSlotsReached,
		// Token: 0x04000ED8 RID: 3800
		MaskedRituals,
		// Token: 0x04000ED9 RID: 3801
		FramedRituals,
		// Token: 0x04000EDA RID: 3802
		Manuscript_Invoked_Primer,
		// Token: 0x04000EDB RID: 3803
		Manuscript_Invoked_Treatise,
		// Token: 0x04000EDC RID: 3804
		Manuscript_Invoked_Schematic,
		// Token: 0x04000EDD RID: 3805
		Manuscript_Invoked_Manual,
		// Token: 0x04000EDE RID: 3806
		Bid_Manuscript,
		// Token: 0x04000EDF RID: 3807
		Manuscript_Seek,
		// Token: 0x04000EE0 RID: 3808
		Bid_Legion,
		// Token: 0x04000EE1 RID: 3809
		Bid_Artifact,
		// Token: 0x04000EE2 RID: 3810
		Bid_Praetor,
		// Token: 0x04000EE3 RID: 3811
		Bid_Other,
		// Token: 0x04000EE4 RID: 3812
		CastRitual_ProcuredHonour,
		// Token: 0x04000EE5 RID: 3813
		CastRitual_DemandOfSupplication,
		// Token: 0x04000EE6 RID: 3814
		CastRitual_BurntOfferings,
		// Token: 0x04000EE7 RID: 3815
		CastRitual_InfernalJuggernaut,
		// Token: 0x04000EE8 RID: 3816
		AssertionOfWeakness,
		// Token: 0x04000EE9 RID: 3817
		Event_WrathOfTheTyrant,
		// Token: 0x04000EEA RID: 3818
		Event_AngelicHost,
		// Token: 0x04000EEB RID: 3819
		Event_AbyssLeviathan,
		// Token: 0x04000EEC RID: 3820
		Event_Other,
		// Token: 0x04000EED RID: 3821
		Event_OfConquestsPast,
		// Token: 0x04000EEE RID: 3822
		CastRitual_RaidTheLibrary,
		// Token: 0x04000EEF RID: 3823
		CastRitual_BlightWisdom
	}
}
