using System;

namespace LoG
{
	// Token: 0x02000410 RID: 1040
	public enum ActionID
	{
		// Token: 0x04000925 RID: 2341
		Undefined = -1,
		// Token: 0x04000926 RID: 2342
		Demand_Tribute,
		// Token: 0x04000927 RID: 2343
		Seek_Manuscripts = 108,
		// Token: 0x04000928 RID: 2344
		Diplo_Demand = 1,
		// Token: 0x04000929 RID: 2345
		Diplo_Insult,
		// Token: 0x0400092A RID: 2346
		Diplo_Extort,
		// Token: 0x0400092B RID: 2347
		Diplo_Send_Emissary = 23,
		// Token: 0x0400092C RID: 2348
		Diplo_Assert_Weakness = 29,
		// Token: 0x0400092D RID: 2349
		Diplo_Humiliate,
		// Token: 0x0400092E RID: 2350
		Diplo_Declare_Blood_Feud = 46,
		// Token: 0x0400092F RID: 2351
		Diplo_Declare_Rebellion,
		// Token: 0x04000930 RID: 2352
		Diplo_Armistice = 78,
		// Token: 0x04000931 RID: 2353
		Diplo_Propose_Become_Vassal,
		// Token: 0x04000932 RID: 2354
		Diplo_Propose_Become_Blood_Lord,
		// Token: 0x04000933 RID: 2355
		Diplo_Draconic_Razzia = 89,
		// Token: 0x04000934 RID: 2356
		Diplo_Chains_Of_Avarice = 97,
		// Token: 0x04000935 RID: 2357
		Diplo_Vile_Calumny,
		// Token: 0x04000936 RID: 2358
		Diplo_Vile_LureOfExcess = 103,
		// Token: 0x04000937 RID: 2359
		March_Legion_Attack = 4,
		// Token: 0x04000938 RID: 2360
		March_Legion_Retreat,
		// Token: 0x04000939 RID: 2361
		March_Legion_Threaten = 88,
		// Token: 0x0400093A RID: 2362
		March_Legion_Flank = 90,
		// Token: 0x0400093B RID: 2363
		March_Legion_Heal = 93,
		// Token: 0x0400093C RID: 2364
		March_Legion_Reinforce_Stronghold,
		// Token: 0x0400093D RID: 2365
		Cast_Damage_Gamepiece_Destruction = 7,
		// Token: 0x0400093E RID: 2366
		Cast_Damage_Gamepiece_Destruction_Permanent = 111,
		// Token: 0x0400093F RID: 2367
		Cast_Anointed_Of_Ash = 31,
		// Token: 0x04000940 RID: 2368
		Cast_Banish_Praetor,
		// Token: 0x04000941 RID: 2369
		Cast_Legion_Sacrifice = 48,
		// Token: 0x04000942 RID: 2370
		Cast_Hells_Maw_Dark_Art,
		// Token: 0x04000943 RID: 2371
		Cast_Steal_Artifact = 51,
		// Token: 0x04000944 RID: 2372
		Cast_Increase_Tribute_Quality,
		// Token: 0x04000945 RID: 2373
		Cast_Produce_Prestige,
		// Token: 0x04000946 RID: 2374
		Cast_Increase_Prestige_Production = 106,
		// Token: 0x04000947 RID: 2375
		Cast_Reveal_Schemes = 54,
		// Token: 0x04000948 RID: 2376
		Cast_Remove_Schemes,
		// Token: 0x04000949 RID: 2377
		Cast_Reveal_Vault,
		// Token: 0x0400094A RID: 2378
		Cast_BlockRituals,
		// Token: 0x0400094B RID: 2379
		Cast_Steal_Tribute = 113,
		// Token: 0x0400094C RID: 2380
		Cast_Destroy_Tribute,
		// Token: 0x0400094D RID: 2381
		Cast_Steal_Canton = 62,
		// Token: 0x0400094E RID: 2382
		Cast_Steal_Legion,
		// Token: 0x0400094F RID: 2383
		Cast_Buff_Legion_Wrath,
		// Token: 0x04000950 RID: 2384
		Cast_Wrath_Pathfinding,
		// Token: 0x04000951 RID: 2385
		Cast_Heal,
		// Token: 0x04000952 RID: 2386
		Cast_Danse_Macabre_Dark_Art,
		// Token: 0x04000953 RID: 2387
		Cast_Hell_Sculpt = 69,
		// Token: 0x04000954 RID: 2388
		Cast_Dimensional_Gate,
		// Token: 0x04000955 RID: 2389
		Cast_Summon_Devourer,
		// Token: 0x04000956 RID: 2390
		Cast_Debuff_Gamepiece_Destruction,
		// Token: 0x04000957 RID: 2391
		Cast_Summon_Strider_Near_Weak,
		// Token: 0x04000958 RID: 2392
		Cast_BlockOrders,
		// Token: 0x04000959 RID: 2393
		Cast_BlockEvents,
		// Token: 0x0400095A RID: 2394
		Cast_Remove_Scheme,
		// Token: 0x0400095B RID: 2395
		Cast_Sabotage_Legion_Movement_Deceit,
		// Token: 0x0400095C RID: 2396
		Cast_Steal_Praetor = 81,
		// Token: 0x0400095D RID: 2397
		Cast_Vanitys_Anointed = 95,
		// Token: 0x0400095E RID: 2398
		Cast_Baleful_Gaze,
		// Token: 0x0400095F RID: 2399
		Cast_Raise_Dark_Pylon = 109,
		// Token: 0x04000960 RID: 2400
		Cast_Steal_Manuscripts = 116,
		// Token: 0x04000961 RID: 2401
		Cast_Destroy_Manuscripts,
		// Token: 0x04000962 RID: 2402
		Draw_New_Scheme = 58,
		// Token: 0x04000963 RID: 2403
		Maintain_Baleful_Gaze_Dark_Art,
		// Token: 0x04000964 RID: 2404
		Maintain_Anti_Destruction,
		// Token: 0x04000965 RID: 2405
		Maintain_Anti_Deceit,
		// Token: 0x04000966 RID: 2406
		Maintain_Vanitys_Annointed = 82,
		// Token: 0x04000967 RID: 2407
		Attach_Praetor_Legion = 8,
		// Token: 0x04000968 RID: 2408
		Attach_Artifact_Legion,
		// Token: 0x04000969 RID: 2409
		Attach_Prestige_Artifact_To_PoP = 15,
		// Token: 0x0400096A RID: 2410
		Attach_Praetor_To_PoP = 50,
		// Token: 0x0400096B RID: 2411
		Attach_Artifact_To_Ritual_Table = 68,
		// Token: 0x0400096C RID: 2412
		Attach_Send_Item_To_Vault = 110,
		// Token: 0x0400096D RID: 2413
		Forge_Combat_Card = 10,
		// Token: 0x0400096E RID: 2414
		March_Expand_Borders = 16,
		// Token: 0x0400096F RID: 2415
		March_Legion_Attack_PoP = 19,
		// Token: 0x04000970 RID: 2416
		March_Capture_Players_Territory = 86,
		// Token: 0x04000971 RID: 2417
		March_On_Pandaemonium = 91,
		// Token: 0x04000972 RID: 2418
		March_Support_Battle,
		// Token: 0x04000973 RID: 2419
		Bid_Bazaar = 6,
		// Token: 0x04000974 RID: 2420
		Bid_On_Legion = 17,
		// Token: 0x04000975 RID: 2421
		Bid_On_Praetor,
		// Token: 0x04000976 RID: 2422
		Bid_On_Artifact = 20,
		// Token: 0x04000977 RID: 2423
		Bid_On_Manuscript = 105,
		// Token: 0x04000978 RID: 2424
		Levelup_Archfiend = 21,
		// Token: 0x04000979 RID: 2425
		Levelup_Conclave_Rank,
		// Token: 0x0400097A RID: 2426
		Levelup_Archfiend_Destruction = 33,
		// Token: 0x0400097B RID: 2427
		Levelup_Archfiend_Deceit,
		// Token: 0x0400097C RID: 2428
		Levelup_Archfiend_Wrath,
		// Token: 0x0400097D RID: 2429
		Levelup_Archfiend_Prophecy,
		// Token: 0x0400097E RID: 2430
		Levelup_Archfiend_Charisma,
		// Token: 0x0400097F RID: 2431
		Invoke_Manuscript_Manual,
		// Token: 0x04000980 RID: 2432
		Invoke_Manuscript_Schematic,
		// Token: 0x04000981 RID: 2433
		Invoke_Manuscript_Treatise,
		// Token: 0x04000982 RID: 2434
		Invoke_Manuscript_Primer,
		// Token: 0x04000983 RID: 2435
		Purchase_Infernal_Rank = 24,
		// Token: 0x04000984 RID: 2436
		Pay_Upkeep = 27,
		// Token: 0x04000985 RID: 2437
		Goal_Conquer_Pandaemonium = 112,
		// Token: 0x04000986 RID: 2438
		Goal_Hunt_Personal_Guard = 12,
		// Token: 0x04000987 RID: 2439
		Goal_Expand_Territory,
		// Token: 0x04000988 RID: 2440
		Goal_Increase_Prestige_Production,
		// Token: 0x04000989 RID: 2441
		Goal_Increase_Tribute_Production = 25,
		// Token: 0x0400098A RID: 2442
		Goal_Increase_Military_Power,
		// Token: 0x0400098B RID: 2443
		Goal_Attack_Player = 28,
		// Token: 0x0400098C RID: 2444
		Goal_Increase_Duelling = 42,
		// Token: 0x0400098D RID: 2445
		Goal_Avoid_Elimination,
		// Token: 0x0400098E RID: 2446
		Goal_Eliminate_Opponent,
		// Token: 0x0400098F RID: 2447
		Goal_Undermine,
		// Token: 0x04000990 RID: 2448
		Goal_Vendetta_Capture_Cantons = 83,
		// Token: 0x04000991 RID: 2449
		Goal_Vendetta_Capture_PoPs,
		// Token: 0x04000992 RID: 2450
		Goal_Vendetta_Destroy_Legions,
		// Token: 0x04000993 RID: 2451
		Goal_Capture_Specific_PoP = 87,
		// Token: 0x04000994 RID: 2452
		Goal_Prepare_Invasion = 104,
		// Token: 0x04000995 RID: 2453
		Goal_Pursue_Scheme = 115,
		// Token: 0x04000996 RID: 2454
		Decision_RespondToDemand = 99,
		// Token: 0x04000997 RID: 2455
		Decision_RespondToInsult,
		// Token: 0x04000998 RID: 2456
		Decision_RespondToTributeSelect,
		// Token: 0x04000999 RID: 2457
		Decision_RespondToGrievanceChoice,
		// Token: 0x0400099A RID: 2458
		Decision_RespondToManuscriptSelect = 107
	}
}
