using System;

namespace LoG
{
	// Token: 0x020006D6 RID: 1750
	public enum TurnLogEntryType
	{
		// Token: 0x04000D3D RID: 3389
		None,
		// Token: 0x04000D3E RID: 3390
		MovementBlocked,
		// Token: 0x04000D3F RID: 3391
		CantonClaimed,
		// Token: 0x04000D40 RID: 3392
		CantonLost,
		// Token: 0x04000D41 RID: 3393
		BattleAttackerVictory,
		// Token: 0x04000D42 RID: 3394
		BattleDefenderVictory,
		// Token: 0x04000D43 RID: 3395
		BattleStalemate,
		// Token: 0x04000D44 RID: 3396
		BattleBothDestroyed = 177,
		// Token: 0x04000D45 RID: 3397
		Insult = 7,
		// Token: 0x04000D46 RID: 3398
		EnemyRitualSufferedDespiteResistance,
		// Token: 0x04000D47 RID: 3399
		InvalidMove,
		// Token: 0x04000D48 RID: 3400
		CounterfeitTribute,
		// Token: 0x04000D49 RID: 3401
		SelectPower = 14,
		// Token: 0x04000D4A RID: 3402
		TrialAssembling,
		// Token: 0x04000D4B RID: 3403
		TrialAssemblingContinues = 239,
		// Token: 0x04000D4C RID: 3404
		TrialDebating = 16,
		// Token: 0x04000D4D RID: 3405
		TrialDebatingContinues = 240,
		// Token: 0x04000D4E RID: 3406
		TrialArguing = 17,
		// Token: 0x04000D4F RID: 3407
		TrialArguingContinues = 241,
		// Token: 0x04000D50 RID: 3408
		TrialDocumenting = 18,
		// Token: 0x04000D51 RID: 3409
		TrialDocumentingContinues = 242,
		// Token: 0x04000D52 RID: 3410
		TrialAnnouncing = 19,
		// Token: 0x04000D53 RID: 3411
		NoVictoryConditionTicking = 244,
		// Token: 0x04000D54 RID: 3412
		PayUpkeep = 20,
		// Token: 0x04000D55 RID: 3413
		PandaemoniumCaptured,
		// Token: 0x04000D56 RID: 3414
		PandaemoniumRestored,
		// Token: 0x04000D57 RID: 3415
		UsurpProgresses,
		// Token: 0x04000D58 RID: 3416
		UpkeepFailed,
		// Token: 0x04000D59 RID: 3417
		DemandTribute = 26,
		// Token: 0x04000D5A RID: 3418
		SeekManuscripts = 254,
		// Token: 0x04000D5B RID: 3419
		OrderProblem = 27,
		// Token: 0x04000D5C RID: 3420
		DiplomaticActionSent,
		// Token: 0x04000D5D RID: 3421
		PowerUpgraded,
		// Token: 0x04000D5E RID: 3422
		RitualCast = 32,
		// Token: 0x04000D5F RID: 3423
		EventCardPlayed,
		// Token: 0x04000D60 RID: 3424
		RitualCastDespiteResistance = 35,
		// Token: 0x04000D61 RID: 3425
		InstalledArtifact,
		// Token: 0x04000D62 RID: 3426
		EventCardDrawn = 39,
		// Token: 0x04000D63 RID: 3427
		TributeOffering = 43,
		// Token: 0x04000D64 RID: 3428
		SchemeOption = 48,
		// Token: 0x04000D65 RID: 3429
		SchemeComplete,
		// Token: 0x04000D66 RID: 3430
		SchemeCompleteWitness = 134,
		// Token: 0x04000D67 RID: 3431
		SchemeInvalidated = 149,
		// Token: 0x04000D68 RID: 3432
		SchemeStarted = 168,
		// Token: 0x04000D69 RID: 3433
		SchemeStartedWitness,
		// Token: 0x04000D6A RID: 3434
		PrivateSchemeComplete = 247,
		// Token: 0x04000D6B RID: 3435
		OrdersRevealed = 55,
		// Token: 0x04000D6C RID: 3436
		VendettaRevealed = 150,
		// Token: 0x04000D6D RID: 3437
		RitualCastFailedDueToResistance = 56,
		// Token: 0x04000D6E RID: 3438
		RitualCastFailed,
		// Token: 0x04000D6F RID: 3439
		EnemyRitualSuffered,
		// Token: 0x04000D70 RID: 3440
		EnemyRitualResisted,
		// Token: 0x04000D71 RID: 3441
		PlayerEliminated,
		// Token: 0x04000D72 RID: 3442
		PraetorDuelStarted,
		// Token: 0x04000D73 RID: 3443
		PraetorDuelOpponentSubmitting = 237,
		// Token: 0x04000D74 RID: 3444
		PraetorDuelProgress = 62,
		// Token: 0x04000D75 RID: 3445
		PraetorDuelVictory,
		// Token: 0x04000D76 RID: 3446
		PraetorDuelDefeat,
		// Token: 0x04000D77 RID: 3447
		LegionLevelUp = 73,
		// Token: 0x04000D78 RID: 3448
		RankIncrease = 132,
		// Token: 0x04000D79 RID: 3449
		RankIncreaseWitnessed,
		// Token: 0x04000D7A RID: 3450
		GamePieceReformed = 135,
		// Token: 0x04000D7B RID: 3451
		GamePieceReformFailed,
		// Token: 0x04000D7C RID: 3452
		BazaarPurchase = 30,
		// Token: 0x04000D7D RID: 3453
		BazaarPurchaseWitness_UnknownPurchaser,
		// Token: 0x04000D7E RID: 3454
		BazaarPurchaseWitness_KnownPurchaser = 65,
		// Token: 0x04000D7F RID: 3455
		BazaarPurchaseFailed_UnknownPurchaser = 12,
		// Token: 0x04000D80 RID: 3456
		BazaarPurchaseFailed_KnownPurchaser,
		// Token: 0x04000D81 RID: 3457
		DemandSentInitiator = 11,
		// Token: 0x04000D82 RID: 3458
		DemandSentRecipient = 74,
		// Token: 0x04000D83 RID: 3459
		DemandSentWitness = 54,
		// Token: 0x04000D84 RID: 3460
		DemandAcceptedInitiator = 37,
		// Token: 0x04000D85 RID: 3461
		DemandAcceptedRecipient = 75,
		// Token: 0x04000D86 RID: 3462
		DemandAcceptedWitness,
		// Token: 0x04000D87 RID: 3463
		DemandRejectedInitiator = 38,
		// Token: 0x04000D88 RID: 3464
		DemandRejectedRecipient = 77,
		// Token: 0x04000D89 RID: 3465
		DemandRejectedWitness,
		// Token: 0x04000D8A RID: 3466
		InsultSentInitiator,
		// Token: 0x04000D8B RID: 3467
		InsultSentRecipient,
		// Token: 0x04000D8C RID: 3468
		InsultSentWitness = 34,
		// Token: 0x04000D8D RID: 3469
		InsultAcceptedInitiator = 41,
		// Token: 0x04000D8E RID: 3470
		InsultAcceptedRecipient = 81,
		// Token: 0x04000D8F RID: 3471
		InsultAcceptedWitness,
		// Token: 0x04000D90 RID: 3472
		InsultRejectedInitiator = 42,
		// Token: 0x04000D91 RID: 3473
		InsultRejectedRecipient = 83,
		// Token: 0x04000D92 RID: 3474
		InsultRejectedWitness,
		// Token: 0x04000D93 RID: 3475
		ExtortionSentInitiator = 52,
		// Token: 0x04000D94 RID: 3476
		ExtortionSentRecipient = 85,
		// Token: 0x04000D95 RID: 3477
		ExtortionSentWitness = 53,
		// Token: 0x04000D96 RID: 3478
		ExtortionAcceptedInitiator = 50,
		// Token: 0x04000D97 RID: 3479
		ExtortionAcceptedRecipient = 86,
		// Token: 0x04000D98 RID: 3480
		ExtortionAcceptedWitness,
		// Token: 0x04000D99 RID: 3481
		ExtortionRejectedInitiator = 51,
		// Token: 0x04000D9A RID: 3482
		ExtortionRejectedRecipient = 88,
		// Token: 0x04000D9B RID: 3483
		ExtortionRejectedWitness,
		// Token: 0x04000D9C RID: 3484
		HumiliateSentInitiator = 44,
		// Token: 0x04000D9D RID: 3485
		HumiliationSentRecipient = 90,
		// Token: 0x04000D9E RID: 3486
		HumiliateSentWitness = 45,
		// Token: 0x04000D9F RID: 3487
		HumiliateAcceptedInitiator,
		// Token: 0x04000DA0 RID: 3488
		HumiliationAcceptedRecipient = 91,
		// Token: 0x04000DA1 RID: 3489
		HumiliationAcceptedWitness,
		// Token: 0x04000DA2 RID: 3490
		HumiliateRejectedInitiator = 47,
		// Token: 0x04000DA3 RID: 3491
		HumiliationRejectedRecipient = 93,
		// Token: 0x04000DA4 RID: 3492
		HumiliationRejectedWitness,
		// Token: 0x04000DA5 RID: 3493
		OfferVassalageSentInitiator = 70,
		// Token: 0x04000DA6 RID: 3494
		OfferVassalageSentRecipient = 95,
		// Token: 0x04000DA7 RID: 3495
		OfferVassalageSentWitness,
		// Token: 0x04000DA8 RID: 3496
		OfferVassalageAcceptedInitiator = 71,
		// Token: 0x04000DA9 RID: 3497
		OfferVassalageAcceptedRecipient = 97,
		// Token: 0x04000DAA RID: 3498
		OfferVassalageAcceptedWitness,
		// Token: 0x04000DAB RID: 3499
		OfferVassalageRejectedInitiator,
		// Token: 0x04000DAC RID: 3500
		OfferVassalageRejectedRecipient,
		// Token: 0x04000DAD RID: 3501
		OfferVassalageRejectedWitness,
		// Token: 0x04000DAE RID: 3502
		OfferLordshipSentInitiator = 69,
		// Token: 0x04000DAF RID: 3503
		OfferLordshipSentRecipient = 102,
		// Token: 0x04000DB0 RID: 3504
		OfferLordshipSentWitness,
		// Token: 0x04000DB1 RID: 3505
		OfferLordshipAcceptedInitiator = 72,
		// Token: 0x04000DB2 RID: 3506
		OfferLordshipAcceptedRecipient = 104,
		// Token: 0x04000DB3 RID: 3507
		OfferLordshipAcceptedWitness,
		// Token: 0x04000DB4 RID: 3508
		OfferLordshipRejectedInitiator,
		// Token: 0x04000DB5 RID: 3509
		OfferLordshipRejectedRecipient,
		// Token: 0x04000DB6 RID: 3510
		OfferLordshipRejectedWitness,
		// Token: 0x04000DB7 RID: 3511
		EmissarySentInitiator,
		// Token: 0x04000DB8 RID: 3512
		EmissarySentRecipient,
		// Token: 0x04000DB9 RID: 3513
		EmissarySentWitness,
		// Token: 0x04000DBA RID: 3514
		EmissaryAcceptedInitiator,
		// Token: 0x04000DBB RID: 3515
		EmissaryAcceptedRecipient,
		// Token: 0x04000DBC RID: 3516
		EmissaryAcceptedWitness,
		// Token: 0x04000DBD RID: 3517
		EmissaryRejectedInitiator,
		// Token: 0x04000DBE RID: 3518
		EmissaryRejectedRecipient,
		// Token: 0x04000DBF RID: 3519
		EmissaryRejectedWitness,
		// Token: 0x04000DC0 RID: 3520
		EmissaryExecutedInitiator,
		// Token: 0x04000DC1 RID: 3521
		EmissaryExecutedRecipient,
		// Token: 0x04000DC2 RID: 3522
		EmissaryExecutedWitness,
		// Token: 0x04000DC3 RID: 3523
		VendettaSetInitiator = 25,
		// Token: 0x04000DC4 RID: 3524
		VendettaStartedInitiator = 121,
		// Token: 0x04000DC5 RID: 3525
		VendettaStartedRecipient,
		// Token: 0x04000DC6 RID: 3526
		VendettaStartedWitness,
		// Token: 0x04000DC7 RID: 3527
		VendettaContinuesInitiator,
		// Token: 0x04000DC8 RID: 3528
		VendettaContinuesRecipient,
		// Token: 0x04000DC9 RID: 3529
		VendettaSuccessfulInitiator,
		// Token: 0x04000DCA RID: 3530
		VendettaSuccessfulRecipient,
		// Token: 0x04000DCB RID: 3531
		VendettaSuccessfulWitness,
		// Token: 0x04000DCC RID: 3532
		VendettaFailedInitiator,
		// Token: 0x04000DCD RID: 3533
		VendettaFailedRecipient,
		// Token: 0x04000DCE RID: 3534
		VendettaFailedWitness,
		// Token: 0x04000DCF RID: 3535
		VendettaNotPursued = 238,
		// Token: 0x04000DD0 RID: 3536
		ManuscriptInvokedManual = 137,
		// Token: 0x04000DD1 RID: 3537
		ManuscriptInvokedPrimer,
		// Token: 0x04000DD2 RID: 3538
		ManuscriptInvokedTreatise,
		// Token: 0x04000DD3 RID: 3539
		ManuscriptInvokedSchematic,
		// Token: 0x04000DD4 RID: 3540
		MachineBuiltReceiver,
		// Token: 0x04000DD5 RID: 3541
		MachineBuiltReceiverProtected,
		// Token: 0x04000DD6 RID: 3542
		BloodFeudInitiator,
		// Token: 0x04000DD7 RID: 3543
		BloodFeudRecipient,
		// Token: 0x04000DD8 RID: 3544
		BloodFeudWitness,
		// Token: 0x04000DD9 RID: 3545
		AssertionOfWeaknessRecipient = 147,
		// Token: 0x04000DDA RID: 3546
		AssertionOfWeaknessWitness,
		// Token: 0x04000DDB RID: 3547
		SchemeDiscardedSelf = 151,
		// Token: 0x04000DDC RID: 3548
		SchemeDiscardedEnemy,
		// Token: 0x04000DDD RID: 3549
		VotingEdictProclaimed,
		// Token: 0x04000DDE RID: 3550
		VotingStarted,
		// Token: 0x04000DDF RID: 3551
		VotingPolicyRevealed,
		// Token: 0x04000DE0 RID: 3552
		VotingCandidateRevealed = 157,
		// Token: 0x04000DE1 RID: 3553
		EmergencyVotingStarted = 156,
		// Token: 0x04000DE2 RID: 3554
		SelectKingmakerTarget = 159,
		// Token: 0x04000DE3 RID: 3555
		UnholyCrusadeSent,
		// Token: 0x04000DE4 RID: 3556
		UnholyCrusadeContinueLegionLost,
		// Token: 0x04000DE5 RID: 3557
		UnholyCrusadeReturnLegionLost,
		// Token: 0x04000DE6 RID: 3558
		UnholyCrusadeReturnLegionSurvive,
		// Token: 0x04000DE7 RID: 3559
		UnholyCrusadeReturnWitness,
		// Token: 0x04000DE8 RID: 3560
		UnholyCrusadeReturnAllLost,
		// Token: 0x04000DE9 RID: 3561
		UnholyCrusadeSentFailed,
		// Token: 0x04000DEA RID: 3562
		UnholyCrusadeExcommunicated,
		// Token: 0x04000DEB RID: 3563
		MpMissedTurnFirstRecipient = 158,
		// Token: 0x04000DEC RID: 3564
		MpMissedTurnSecondRecipient = 170,
		// Token: 0x04000DED RID: 3565
		MpMissedTurnSecondWitness,
		// Token: 0x04000DEE RID: 3566
		MpPossessedRecipient,
		// Token: 0x04000DEF RID: 3567
		MpPossessedWitness,
		// Token: 0x04000DF0 RID: 3568
		MpBanishedWitness,
		// Token: 0x04000DF1 RID: 3569
		MpIdleVoteNullifiedWitness,
		// Token: 0x04000DF2 RID: 3570
		MpPossessedReturnedWitness,
		// Token: 0x04000DF3 RID: 3571
		MpForfeitProposalWitness = 257,
		// Token: 0x04000DF4 RID: 3572
		AbilityGenerateManuscript = 253,
		// Token: 0x04000DF5 RID: 3573
		AbilityGeneratedTribute = 178,
		// Token: 0x04000DF6 RID: 3574
		TributeFromKill,
		// Token: 0x04000DF7 RID: 3575
		RegencyAcquired,
		// Token: 0x04000DF8 RID: 3576
		RegencyAcquiredNoEvent = 252,
		// Token: 0x04000DF9 RID: 3577
		GameItemAttached = 181,
		// Token: 0x04000DFA RID: 3578
		GameItemReassigned,
		// Token: 0x04000DFB RID: 3579
		GameItemVaulted,
		// Token: 0x04000DFC RID: 3580
		PraetorSelectChampion,
		// Token: 0x04000DFD RID: 3581
		PraetorSelectMoves,
		// Token: 0x04000DFE RID: 3582
		PraetorDuelNoShow,
		// Token: 0x04000DFF RID: 3583
		PraetorDuelNoShowAll,
		// Token: 0x04000E00 RID: 3584
		PraetorDuelDraw = 189,
		// Token: 0x04000E01 RID: 3585
		ConclaveFavouriteChanged = 188,
		// Token: 0x04000E02 RID: 3586
		VendettaStartedWitness_HasKnowledge = 190,
		// Token: 0x04000E03 RID: 3587
		ItemsRecovered,
		// Token: 0x04000E04 RID: 3588
		DiplomaticStateCancelled,
		// Token: 0x04000E05 RID: 3589
		EventEffectEnded,
		// Token: 0x04000E06 RID: 3590
		EventEffectContinues,
		// Token: 0x04000E07 RID: 3591
		DraconicRazziaAnnounceInitiator,
		// Token: 0x04000E08 RID: 3592
		DraconicRazziaAnnounceTarget,
		// Token: 0x04000E09 RID: 3593
		DraconicRazziaAnnounceWitness,
		// Token: 0x04000E0A RID: 3594
		DraconicRazziaCommencementInitiator,
		// Token: 0x04000E0B RID: 3595
		DraconicRazziaCommencementTarget,
		// Token: 0x04000E0C RID: 3596
		DraconicRazziaCommencementWitness,
		// Token: 0x04000E0D RID: 3597
		ChainsOfAvariceTributeReceived = 202,
		// Token: 0x04000E0E RID: 3598
		ChainsOfAvariceSentRecipient = 201,
		// Token: 0x04000E0F RID: 3599
		ChainsOfAvariceSentInitiator = 203,
		// Token: 0x04000E10 RID: 3600
		ChainsOfAvariceSentWitness,
		// Token: 0x04000E11 RID: 3601
		ChainsOfAvariceAcceptedRecipient,
		// Token: 0x04000E12 RID: 3602
		ChainsOfAvariceAcceptedInitiator,
		// Token: 0x04000E13 RID: 3603
		ChainsOfAvariceAcceptedWitness,
		// Token: 0x04000E14 RID: 3604
		ChainsOfAvariceRejectedRecipient,
		// Token: 0x04000E15 RID: 3605
		ChainsOfAvariceRejectedInitiator,
		// Token: 0x04000E16 RID: 3606
		ChainsOfAvariceRejectedWitness,
		// Token: 0x04000E17 RID: 3607
		VileCalumnySentInitiator,
		// Token: 0x04000E18 RID: 3608
		VileCalumnySentRecipient,
		// Token: 0x04000E19 RID: 3609
		VileCalumnySentScapegoat,
		// Token: 0x04000E1A RID: 3610
		VileCalumnySentWitness,
		// Token: 0x04000E1B RID: 3611
		VileCalumnyAcceptedScapegoat,
		// Token: 0x04000E1C RID: 3612
		VileCalumnyRejectedScapegoat,
		// Token: 0x04000E1D RID: 3613
		VileCalumnyAcceptedInitiator,
		// Token: 0x04000E1E RID: 3614
		VileCalumnyRejectedInitiator,
		// Token: 0x04000E1F RID: 3615
		ConclaveDissolved,
		// Token: 0x04000E20 RID: 3616
		ConclaveRestored,
		// Token: 0x04000E21 RID: 3617
		AbyssProgresses,
		// Token: 0x04000E22 RID: 3618
		PayUpkeepRitual,
		// Token: 0x04000E23 RID: 3619
		UpkeepFailedRitual,
		// Token: 0x04000E24 RID: 3620
		MessageReceived,
		// Token: 0x04000E25 RID: 3621
		LureOfExcessSentRecipient,
		// Token: 0x04000E26 RID: 3622
		LureOfExcessSentInitiator,
		// Token: 0x04000E27 RID: 3623
		LureOfExcessSentWitness,
		// Token: 0x04000E28 RID: 3624
		LureOfExcessAcceptedRecipient,
		// Token: 0x04000E29 RID: 3625
		LureOfExcessAcceptedInitiator,
		// Token: 0x04000E2A RID: 3626
		LureOfExcessAcceptedWitness,
		// Token: 0x04000E2B RID: 3627
		LureOfExcessRejectedRecipient,
		// Token: 0x04000E2C RID: 3628
		LureOfExcessRejectedInitiator,
		// Token: 0x04000E2D RID: 3629
		LureOfExcessRejectedWitness,
		// Token: 0x04000E2E RID: 3630
		PandaemoniumCapturedInitiator,
		// Token: 0x04000E2F RID: 3631
		PlayerExcommunicatedViolence,
		// Token: 0x04000E30 RID: 3632
		PlayerExcommunicatedGeneric,
		// Token: 0x04000E31 RID: 3633
		RitualCastFailedDueToResistanceOfGameItem = 245,
		// Token: 0x04000E32 RID: 3634
		EnemyRitualResistedByGameItem,
		// Token: 0x04000E33 RID: 3635
		PrivateSchemesAnnounced = 248,
		// Token: 0x04000E34 RID: 3636
		IdlePlayerPossessedContinued,
		// Token: 0x04000E35 RID: 3637
		UpkeepFailedFreeRitual,
		// Token: 0x04000E36 RID: 3638
		VaultOverflowed,
		// Token: 0x04000E37 RID: 3639
		OngoingRitual = 255,
		// Token: 0x04000E38 RID: 3640
		RitualTerrainResisted,
		// Token: 0x04000E39 RID: 3641
		NEXTENTRY = 258
	}
}
