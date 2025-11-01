using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000132 RID: 306
	[Serializable]
	public class Result
	{
		// Token: 0x060005D9 RID: 1497 RVA: 0x0001D5BB File Offset: 0x0001B7BB
		public static Problem AwardFailure(Result internalProblem)
		{
			return new DebugProblem("Could not award the item: " + internalProblem.DebugString);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001D5D2 File Offset: 0x0001B7D2
		public static Problem DeadLock()
		{
			return new DebugProblem("The bidding reached a deadlock, and the item was not sold");
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001D5DE File Offset: 0x0001B7DE
		public static Problem InvalidItem(Identifier id)
		{
			return new SimulationError(string.Format("Item {0} does not exist", id));
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001D5F5 File Offset: 0x0001B7F5
		public static Problem InvalidTransition(GamePiece blockedLegion, HexCoord from, HexCoord to)
		{
			return new LegionMovementProcessor.InvalidTransitionProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, from, to);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001D60A File Offset: 0x0001B80A
		public static Problem InvalidTransition(GamePiece blockedLegion, IEnumerable<HexCoord> path)
		{
			return new LegionMovementProcessor.InvalidTransitionProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, IEnumerableExtensions.First<HexCoord>(path), path.Last<HexCoord>());
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001D629 File Offset: 0x0001B829
		public static Problem OccupiedCanton(GamePiece blockedLegion, HexCoord blockingHexCoord, Identifier blockingLegionID)
		{
			return new LegionMovementProcessor.OccupiedCantonProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, blockingLegionID);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001D63E File Offset: 0x0001B83E
		public static Problem CannotTeleportIntoCombat(GamePiece blockedLegion, HexCoord blockingHexCoord, Identifier blockingLegionID)
		{
			return new LegionMovementProcessor.CannotTeleportIntoCombatProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, blockingLegionID);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001D653 File Offset: 0x0001B853
		public static Problem ImpassibleTerrain(GamePiece blockedLegion, HexCoord blockingHexCoord, TerrainType blockingTerrainType)
		{
			return new LegionMovementProcessor.ImpassableTerrainProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, blockingTerrainType);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001D668 File Offset: 0x0001B868
		public static Problem TerrainForbidsTeleport(GamePiece blockedLegion, HexCoord blockingHexCoord, TerrainType blockingTerrainType)
		{
			return new LegionMovementProcessor.TerrainForbidsTeleportProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, blockingTerrainType);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001D67D File Offset: 0x0001B87D
		public static Problem NoRightOfEntry(GamePiece blockedLegion, HexCoord blockingHexCoord, int hexOwner)
		{
			return new LegionMovementProcessor.NoRightOfEntryProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, hexOwner);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001D692 File Offset: 0x0001B892
		public static Problem NoRightOfCombat(GamePiece blockedLegion, HexCoord blockingHexCoord, GamePiece blockingLegion)
		{
			return new LegionMovementProcessor.NoRightOfCombatProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, blockingLegion.Id, blockingLegion.ControllingPlayerId);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001D6B2 File Offset: 0x0001B8B2
		public static Problem NoRightOfElimination(GamePiece blockedLegion, HexCoord blockingHexCoord, int blockingPlayerID)
		{
			return new LegionMovementProcessor.NoRightOfEliminationProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, blockingPlayerID);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001D6C7 File Offset: 0x0001B8C7
		public static Problem NotEnoughMovementPoints(GamePiece blockedLegion, HexCoord blockingHexCoord)
		{
			return new LegionMovementProcessor.NotEnoughMovementPointsProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001D6DB File Offset: 0x0001B8DB
		public static Problem BanishedBeforeMoving(GamePiece blockedLegion, HexCoord blockingHexCoord)
		{
			return new LegionMovementProcessor.BanishedBeforeMovingProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001D6EF File Offset: 0x0001B8EF
		public static Problem ConvertedBeforeMoving(GamePiece blockedLegion, HexCoord blockingHexCoord)
		{
			return new LegionMovementProcessor.ConvertedBeforeMovingProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001D703 File Offset: 0x0001B903
		public static Problem DidNotWinBattle(GamePiece blockedLegion, HexCoord blockingHexCoord, GamePiece blockingLegion)
		{
			return new LegionMovementProcessor.DidNotWinBattleProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, blockingLegion.Id);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001D71D File Offset: 0x0001B91D
		public static Problem AlreadyMovedThisTurn(GamePiece blockedLegion, HexCoord blockingHexCoord)
		{
			return new LegionMovementProcessor.AlreadyMovedThisTurnProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001D731 File Offset: 0x0001B931
		public static Problem OutsideTeleportRange(GamePiece blockedLegion, HexCoord blockingHexCoord, int attemptedTeleportDistance)
		{
			return new LegionMovementProcessor.OutsideTeleportRangeProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord, attemptedTeleportDistance, blockedLegion.TeleportDistance);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001D751 File Offset: 0x0001B951
		public static Problem DoesNotHaveTeleport(GamePiece blockedLegion, HexCoord blockingHexCoord)
		{
			return new LegionMovementProcessor.DoesNotHaveTeleportProblem(blockedLegion.ControllingPlayerId, blockedLegion.Id, blockingHexCoord);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001D765 File Offset: 0x0001B965
		public static Problem CannotAfford(Cost cost, Payment payment = null)
		{
			return new PaymentProcessor.CannotAfford("Cannot afford");
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001D771 File Offset: 0x0001B971
		public static Problem CounterfeitTribute(int playerId, params int[] uuids)
		{
			return Result.CounterfeitTribute(playerId, uuids.AsEnumerable<int>());
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001D77F File Offset: 0x0001B97F
		public static Problem CounterfeitTribute(int playerId, IEnumerable<int> uuids)
		{
			return new PaymentProcessor.CounterfeitTributeProblem();
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001D786 File Offset: 0x0001B986
		public static Problem NotEnoughSlots()
		{
			return new OrderSlotsFullProblem();
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0001D78D File Offset: 0x0001B98D
		public virtual bool successful
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001D790 File Offset: 0x0001B990
		public static Result Success
		{
			get
			{
				return new Result();
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0001D797 File Offset: 0x0001B997
		public static Problem Failure
		{
			get
			{
				return new Problem();
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0001D79E File Offset: 0x0001B99E
		public virtual string DebugString
		{
			get
			{
				if (!this.successful)
				{
					return "Failure";
				}
				return "Success";
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0001D7B3 File Offset: 0x0001B9B3
		public virtual string LocKey
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0001D7BA File Offset: 0x0001B9BA
		public virtual string PreviewLocKey
		{
			get
			{
				return this.LocKey + ".Preview";
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0001D7CC File Offset: 0x0001B9CC
		protected virtual string LocKeyScope
		{
			get
			{
				return "Result";
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001D7D3 File Offset: 0x0001B9D3
		public virtual string ResolutionHintKey
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001D7DA File Offset: 0x0001B9DA
		public static implicit operator bool(Result result)
		{
			return result.successful;
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0001D7E2 File Offset: 0x0001B9E2
		public static Problem UnsetResult
		{
			get
			{
				return new DebugProblem("Unset Result");
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001D7EE File Offset: 0x0001B9EE
		public static Problem SimulationError(string error)
		{
			return new SimulationError(error);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001D7F6 File Offset: 0x0001B9F6
		public static Result AbilityLocked(AbilityStaticData ability)
		{
			return new AbilityLockedProblem();
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x0001D7FD File Offset: 0x0001B9FD
		public static Result SelectedTooManyOptions
		{
			get
			{
				return new DebugProblem("Selected Too Many Options");
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001D809 File Offset: 0x0001BA09
		public static Result SelectedTooFewOptions
		{
			get
			{
				return new DebugProblem("Selected Too Few Options");
			}
		}

		// Token: 0x020007EF RID: 2031
		[Serializable]
		public class PlanningProblem : DebugProblem
		{
			// Token: 0x060025DE RID: 9694 RVA: 0x00081797 File Offset: 0x0007F997
			public PlanningProblem(string problemDescription, GOAPNode problematicNode) : base(string.Format("{0}: {1}", problematicNode, problemDescription))
			{
				this.ProblematicNode = problematicNode;
			}

			// Token: 0x04001178 RID: 4472
			[JsonProperty]
			public GOAPNode ProblematicNode;
		}

		// Token: 0x020007F0 RID: 2032
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class BazaarProblem : Problem
		{
			// Token: 0x060025DF RID: 9695 RVA: 0x000817B2 File Offset: 0x0007F9B2
			[JsonConstructor]
			protected BazaarProblem()
			{
			}

			// Token: 0x060025E0 RID: 9696 RVA: 0x000817BA File Offset: 0x0007F9BA
			public BazaarProblem(Identifier identifier)
			{
				this.DesiredItem = identifier;
			}

			// Token: 0x170004B7 RID: 1207
			// (get) Token: 0x060025E1 RID: 9697 RVA: 0x000817C9 File Offset: 0x0007F9C9
			protected override string LocKeyScope
			{
				get
				{
					return "Result.Bazaar";
				}
			}

			// Token: 0x170004B8 RID: 1208
			// (get) Token: 0x060025E2 RID: 9698 RVA: 0x000817D0 File Offset: 0x0007F9D0
			public override string DebugString
			{
				get
				{
					return string.Format("{0} could not be purchased", this.DesiredItem);
				}
			}

			// Token: 0x04001179 RID: 4473
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public Identifier DesiredItem;
		}

		// Token: 0x020007F1 RID: 2033
		public class OutbidProblem : Result.BazaarProblem
		{
			// Token: 0x060025E3 RID: 9699 RVA: 0x000817E7 File Offset: 0x0007F9E7
			[JsonConstructor]
			protected OutbidProblem()
			{
			}

			// Token: 0x060025E4 RID: 9700 RVA: 0x000817EF File Offset: 0x0007F9EF
			public OutbidProblem(Identifier item, int winningPlayerID) : base(item)
			{
				this.WinningPlayerID = winningPlayerID;
			}

			// Token: 0x170004B9 RID: 1209
			// (get) Token: 0x060025E5 RID: 9701 RVA: 0x000817FF File Offset: 0x0007F9FF
			public override string DebugString
			{
				get
				{
					return string.Format(" because {0} paid more", this.WinningPlayerID);
				}
			}

			// Token: 0x0400117A RID: 4474
			[JsonProperty]
			[BindableValue("affected_name", BindingOption.IntPlayerId)]
			public int WinningPlayerID;
		}

		// Token: 0x020007F2 RID: 2034
		public class BazaarUnavailableProblem : Result.BazaarProblem
		{
			// Token: 0x060025E6 RID: 9702 RVA: 0x00081816 File Offset: 0x0007FA16
			[JsonConstructor]
			protected BazaarUnavailableProblem()
			{
			}

			// Token: 0x060025E7 RID: 9703 RVA: 0x0008181E File Offset: 0x0007FA1E
			public BazaarUnavailableProblem(Identifier item) : base(item)
			{
			}

			// Token: 0x170004BA RID: 1210
			// (get) Token: 0x060025E8 RID: 9704 RVA: 0x00081827 File Offset: 0x0007FA27
			public override string DebugString
			{
				get
				{
					return " because the bazaar is closed";
				}
			}
		}

		// Token: 0x020007F3 RID: 2035
		public class ItemUnavailableProblem : Result.BazaarProblem
		{
			// Token: 0x060025E9 RID: 9705 RVA: 0x0008182E File Offset: 0x0007FA2E
			[JsonConstructor]
			protected ItemUnavailableProblem()
			{
			}

			// Token: 0x060025EA RID: 9706 RVA: 0x00081836 File Offset: 0x0007FA36
			public ItemUnavailableProblem(Identifier item) : base(item)
			{
			}

			// Token: 0x170004BB RID: 1211
			// (get) Token: 0x060025EB RID: 9707 RVA: 0x0008183F File Offset: 0x0007FA3F
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".ItemUnavailable";
				}
			}

			// Token: 0x170004BC RID: 1212
			// (get) Token: 0x060025EC RID: 9708 RVA: 0x00081851 File Offset: 0x0007FA51
			public override string DebugString
			{
				get
				{
					return " because it is no longer on the market";
				}
			}
		}

		// Token: 0x020007F4 RID: 2036
		public class CommandRatingTooLowForBidProblem : Result.BazaarProblem
		{
			// Token: 0x060025ED RID: 9709 RVA: 0x00081858 File Offset: 0x0007FA58
			[JsonConstructor]
			protected CommandRatingTooLowForBidProblem()
			{
			}

			// Token: 0x060025EE RID: 9710 RVA: 0x00081860 File Offset: 0x0007FA60
			public CommandRatingTooLowForBidProblem(Identifier item, int legionCount, int commandRating) : base(item)
			{
				this.LegionCount = legionCount;
				this.CommandRating = commandRating;
			}

			// Token: 0x170004BD RID: 1213
			// (get) Token: 0x060025EF RID: 9711 RVA: 0x00081877 File Offset: 0x0007FA77
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CommandRatingTooLow";
				}
			}

			// Token: 0x170004BE RID: 1214
			// (get) Token: 0x060025F0 RID: 9712 RVA: 0x00081889 File Offset: 0x0007FA89
			public override string DebugString
			{
				get
				{
					return string.Format(" because Command Rating ({0}) can't support {1} legions", this.LegionCount, this.LegionCount + 1);
				}
			}

			// Token: 0x0400117B RID: 4475
			[JsonProperty]
			[BindableValue("value", BindingOption.None)]
			public int LegionCount;

			// Token: 0x0400117C RID: 4476
			[JsonProperty]
			[BindableValue("max_value", BindingOption.None)]
			public int CommandRating;
		}

		// Token: 0x020007F5 RID: 2037
		public class NoRoomToSpawnPurchaseProblem : Result.BazaarProblem
		{
			// Token: 0x060025F1 RID: 9713 RVA: 0x000818AD File Offset: 0x0007FAAD
			[JsonConstructor]
			protected NoRoomToSpawnPurchaseProblem()
			{
			}

			// Token: 0x060025F2 RID: 9714 RVA: 0x000818B5 File Offset: 0x0007FAB5
			public NoRoomToSpawnPurchaseProblem(Identifier item) : base(item)
			{
			}

			// Token: 0x170004BF RID: 1215
			// (get) Token: 0x060025F3 RID: 9715 RVA: 0x000818BE File Offset: 0x0007FABE
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NoRoomToSpawnPurchase";
				}
			}

			// Token: 0x170004C0 RID: 1216
			// (get) Token: 0x060025F4 RID: 9716 RVA: 0x000818D0 File Offset: 0x0007FAD0
			public override string DebugString
			{
				get
				{
					return " there is no room to spawn it";
				}
			}
		}

		// Token: 0x020007F6 RID: 2038
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class DiplomacyProblem : Problem
		{
			// Token: 0x170004C1 RID: 1217
			// (get) Token: 0x060025F5 RID: 9717 RVA: 0x000818D7 File Offset: 0x0007FAD7
			[JsonIgnore]
			[BindableValue("affected_name", BindingOption.IntPlayerId)]
			public int TargetPlayerId
			{
				get
				{
					return this._targetPlayerID;
				}
			}

			// Token: 0x170004C2 RID: 1218
			// (get) Token: 0x060025F6 RID: 9718 RVA: 0x000818DF File Offset: 0x0007FADF
			[JsonIgnore]
			[BindableValue(null, BindingOption.None)]
			public OrderTypes OrderType
			{
				get
				{
					return this._orderType;
				}
			}

			// Token: 0x060025F7 RID: 9719 RVA: 0x000818E7 File Offset: 0x0007FAE7
			public DiplomacyProblem(int targetPlayerID, OrderTypes orderType)
			{
				this._targetPlayerID = targetPlayerID;
				this._orderType = orderType;
			}

			// Token: 0x060025F8 RID: 9720 RVA: 0x000818FD File Offset: 0x0007FAFD
			[JsonConstructor]
			protected DiplomacyProblem()
			{
			}

			// Token: 0x170004C3 RID: 1219
			// (get) Token: 0x060025F9 RID: 9721 RVA: 0x00081905 File Offset: 0x0007FB05
			public override string DebugString
			{
				get
				{
					return string.Format("Could not perform {0} on {1}", this._orderType, this._targetPlayerID);
				}
			}

			// Token: 0x170004C4 RID: 1220
			// (get) Token: 0x060025FA RID: 9722 RVA: 0x00081927 File Offset: 0x0007FB27
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".Diplomacy";
				}
			}

			// Token: 0x170004C5 RID: 1221
			// (get) Token: 0x060025FB RID: 9723 RVA: 0x00081939 File Offset: 0x0007FB39
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x0400117D RID: 4477
			[JsonProperty]
			private int _targetPlayerID;

			// Token: 0x0400117E RID: 4478
			[JsonProperty]
			private OrderTypes _orderType;

			// Token: 0x0400117F RID: 4479
			[JsonProperty]
			public bool IsDecisionResponse;
		}

		// Token: 0x020007F7 RID: 2039
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RankTooLowForDiplomacyProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004C6 RID: 1222
			// (get) Token: 0x060025FC RID: 9724 RVA: 0x0008194B File Offset: 0x0007FB4B
			[JsonIgnore]
			[BindableValue(null, BindingOption.None)]
			public Rank RequiredRank
			{
				get
				{
					return this._requiredRank;
				}
			}

			// Token: 0x170004C7 RID: 1223
			// (get) Token: 0x060025FD RID: 9725 RVA: 0x00081953 File Offset: 0x0007FB53
			[JsonIgnore]
			[BindableValue("source_name", BindingOption.IntPlayerId)]
			public int SelfId
			{
				get
				{
					return this._selfId;
				}
			}

			// Token: 0x060025FE RID: 9726 RVA: 0x0008195B File Offset: 0x0007FB5B
			public RankTooLowForDiplomacyProblem(int targetPlayerID, OrderTypes orderType, Rank requiredRank, int selfId) : base(targetPlayerID, orderType)
			{
				this._requiredRank = requiredRank;
				this._selfId = selfId;
			}

			// Token: 0x060025FF RID: 9727 RVA: 0x00081974 File Offset: 0x0007FB74
			[JsonConstructor]
			public RankTooLowForDiplomacyProblem()
			{
			}

			// Token: 0x170004C8 RID: 1224
			// (get) Token: 0x06002600 RID: 9728 RVA: 0x0008197C File Offset: 0x0007FB7C
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because your Rank is too low";
				}
			}

			// Token: 0x170004C9 RID: 1225
			// (get) Token: 0x06002601 RID: 9729 RVA: 0x0008198E File Offset: 0x0007FB8E
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".RankTooLow";
				}
			}

			// Token: 0x04001180 RID: 4480
			[JsonProperty]
			private Rank _requiredRank;

			// Token: 0x04001181 RID: 4481
			[JsonProperty]
			private int _selfId;
		}

		// Token: 0x020007F8 RID: 2040
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ScapegoatProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004CA RID: 1226
			// (get) Token: 0x06002602 RID: 9730 RVA: 0x000819A0 File Offset: 0x0007FBA0
			[JsonIgnore]
			[BindableValue("scapegoat_name", BindingOption.IntPlayerId)]
			public int ScapegoatId
			{
				get
				{
					return this._scapegoatId;
				}
			}

			// Token: 0x170004CB RID: 1227
			// (get) Token: 0x06002603 RID: 9731 RVA: 0x000819A8 File Offset: 0x0007FBA8
			[JsonIgnore]
			[BindableValue("scapegoat_self", BindingOption.None)]
			public bool ScapegoatSelf
			{
				get
				{
					return base.TargetPlayerId != this._scapegoatId;
				}
			}

			// Token: 0x06002604 RID: 9732 RVA: 0x000819BB File Offset: 0x0007FBBB
			[JsonConstructor]
			public ScapegoatProblem()
			{
			}

			// Token: 0x06002605 RID: 9733 RVA: 0x000819C3 File Offset: 0x0007FBC3
			public ScapegoatProblem(int targetPlayerID, OrderTypes orderType, int scapegoatId) : base(targetPlayerID, orderType)
			{
				this._scapegoatId = scapegoatId;
			}

			// Token: 0x170004CC RID: 1228
			// (get) Token: 0x06002606 RID: 9734 RVA: 0x000819D4 File Offset: 0x0007FBD4
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" as {0} cannot be accused of insulting {1}", this._scapegoatId, base.TargetPlayerId);
				}
			}

			// Token: 0x170004CD RID: 1229
			// (get) Token: 0x06002607 RID: 9735 RVA: 0x00081A01 File Offset: 0x0007FC01
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".Scapegoat";
				}
			}

			// Token: 0x170004CE RID: 1230
			// (get) Token: 0x06002608 RID: 9736 RVA: 0x00081A13 File Offset: 0x0007FC13
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x04001182 RID: 4482
			[JsonProperty]
			private int _scapegoatId;
		}

		// Token: 0x020007F9 RID: 2041
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ScapegoatRelationshipProblemProblem : Result.ScapegoatProblem
		{
			// Token: 0x170004CF RID: 1231
			// (get) Token: 0x06002609 RID: 9737 RVA: 0x00081A25 File Offset: 0x0007FC25
			[JsonIgnore]
			[BindableValue(null, BindingOption.None)]
			public DiplomaticStateValue RequiredThirdPartyState
			{
				get
				{
					return this._requiredThirdPartyState;
				}
			}

			// Token: 0x0600260A RID: 9738 RVA: 0x00081A2D File Offset: 0x0007FC2D
			[JsonConstructor]
			public ScapegoatRelationshipProblemProblem()
			{
			}

			// Token: 0x0600260B RID: 9739 RVA: 0x00081A35 File Offset: 0x0007FC35
			public ScapegoatRelationshipProblemProblem(int targetPlayerID, OrderTypes orderType, int scapegoatId, DiplomaticStateValue requiredThirdPartyState) : base(targetPlayerID, orderType, scapegoatId)
			{
				this._requiredThirdPartyState = requiredThirdPartyState;
			}

			// Token: 0x170004D0 RID: 1232
			// (get) Token: 0x0600260C RID: 9740 RVA: 0x00081A48 File Offset: 0x0007FC48
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because the state between them is not not {0}", this._requiredThirdPartyState);
				}
			}

			// Token: 0x170004D1 RID: 1233
			// (get) Token: 0x0600260D RID: 9741 RVA: 0x00081A6A File Offset: 0x0007FC6A
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".ThirdPartyRelationship";
				}
			}

			// Token: 0x04001183 RID: 4483
			[JsonProperty]
			private DiplomaticStateValue _requiredThirdPartyState;
		}

		// Token: 0x020007FA RID: 2042
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvalidDiplomaticStateProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004D2 RID: 1234
			// (get) Token: 0x0600260E RID: 9742 RVA: 0x00081A7C File Offset: 0x0007FC7C
			[JsonIgnore]
			[BindableValue(null, BindingOption.None)]
			public DiplomaticStateValue CurrentState
			{
				get
				{
					return this._currentState;
				}
			}

			// Token: 0x0600260F RID: 9743 RVA: 0x00081A84 File Offset: 0x0007FC84
			[JsonConstructor]
			public InvalidDiplomaticStateProblem()
			{
			}

			// Token: 0x06002610 RID: 9744 RVA: 0x00081A8C File Offset: 0x0007FC8C
			public InvalidDiplomaticStateProblem(int targetPlayerID, OrderTypes orderType, DiplomaticStateValue currentState) : base(targetPlayerID, orderType)
			{
				this._currentState = currentState;
			}

			// Token: 0x170004D3 RID: 1235
			// (get) Token: 0x06002611 RID: 9745 RVA: 0x00081A9D File Offset: 0x0007FC9D
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the state precludes this initiatives";
				}
			}

			// Token: 0x170004D4 RID: 1236
			// (get) Token: 0x06002612 RID: 9746 RVA: 0x00081AAF File Offset: 0x0007FCAF
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".InvalidDiplomaticState";
				}
			}

			// Token: 0x04001184 RID: 4484
			[JsonProperty]
			private DiplomaticStateValue _currentState;
		}

		// Token: 0x020007FB RID: 2043
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class QueuedDiplomacyActionOfSameTypeProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004D5 RID: 1237
			// (get) Token: 0x06002613 RID: 9747 RVA: 0x00081AC1 File Offset: 0x0007FCC1
			[JsonIgnore]
			[BindableValue("archfiend_name", BindingOption.IntPlayerId)]
			public int OtherPlayerId
			{
				get
				{
					return this._otherPlayerID;
				}
			}

			// Token: 0x06002614 RID: 9748 RVA: 0x00081AC9 File Offset: 0x0007FCC9
			[JsonConstructor]
			public QueuedDiplomacyActionOfSameTypeProblem()
			{
			}

			// Token: 0x06002615 RID: 9749 RVA: 0x00081AD1 File Offset: 0x0007FCD1
			public QueuedDiplomacyActionOfSameTypeProblem(int targetPlayerID, OrderTypes orderType, int otherPlayerID) : base(targetPlayerID, orderType)
			{
				this._otherPlayerID = otherPlayerID;
			}

			// Token: 0x170004D6 RID: 1238
			// (get) Token: 0x06002616 RID: 9750 RVA: 0x00081AE2 File Offset: 0x0007FCE2
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because it is already queued against {0}", this._otherPlayerID);
				}
			}

			// Token: 0x170004D7 RID: 1239
			// (get) Token: 0x06002617 RID: 9751 RVA: 0x00081B04 File Offset: 0x0007FD04
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".QueuedDiplomacyActionOfSameType";
				}
			}

			// Token: 0x04001185 RID: 4485
			[JsonProperty]
			private int _otherPlayerID;
		}

		// Token: 0x020007FC RID: 2044
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class QueuedDiplomacyActionAgainstTargetProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004D8 RID: 1240
			// (get) Token: 0x06002618 RID: 9752 RVA: 0x00081B16 File Offset: 0x0007FD16
			[JsonIgnore]
			[BindableValue("conflicting_order", BindingOption.None)]
			public OrderTypes ConflictingOrderType
			{
				get
				{
					return this._conflictingOrderType;
				}
			}

			// Token: 0x06002619 RID: 9753 RVA: 0x00081B1E File Offset: 0x0007FD1E
			[JsonConstructor]
			public QueuedDiplomacyActionAgainstTargetProblem()
			{
			}

			// Token: 0x0600261A RID: 9754 RVA: 0x00081B26 File Offset: 0x0007FD26
			public QueuedDiplomacyActionAgainstTargetProblem(int targetPlayerID, OrderTypes orderType, OrderTypes conflictingOrderType) : base(targetPlayerID, orderType)
			{
				this._conflictingOrderType = conflictingOrderType;
			}

			// Token: 0x170004D9 RID: 1241
			// (get) Token: 0x0600261B RID: 9755 RVA: 0x00081B37 File Offset: 0x0007FD37
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because a {0} is already queued against this target", this._conflictingOrderType);
				}
			}

			// Token: 0x170004DA RID: 1242
			// (get) Token: 0x0600261C RID: 9756 RVA: 0x00081B59 File Offset: 0x0007FD59
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".QueuedDiplomacyActionAgainstTarget";
				}
			}

			// Token: 0x04001186 RID: 4486
			[JsonProperty]
			private OrderTypes _conflictingOrderType;
		}

		// Token: 0x020007FD RID: 2045
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class NotEnoughDemandsAcceptedProblem : Result.DiplomacyProblem
		{
			// Token: 0x0600261D RID: 9757 RVA: 0x00081B6B File Offset: 0x0007FD6B
			[JsonConstructor]
			public NotEnoughDemandsAcceptedProblem()
			{
			}

			// Token: 0x170004DB RID: 1243
			// (get) Token: 0x0600261E RID: 9758 RVA: 0x00081B73 File Offset: 0x0007FD73
			[JsonIgnore]
			[BindableValue("value", BindingOption.None)]
			public int CurrentConcessions
			{
				get
				{
					return this._currentConcessions;
				}
			}

			// Token: 0x170004DC RID: 1244
			// (get) Token: 0x0600261F RID: 9759 RVA: 0x00081B7B File Offset: 0x0007FD7B
			[JsonIgnore]
			[BindableValue("target", BindingOption.None)]
			public int RequiredConcessions
			{
				get
				{
					return this._requiredConcessions;
				}
			}

			// Token: 0x06002620 RID: 9760 RVA: 0x00081B83 File Offset: 0x0007FD83
			public NotEnoughDemandsAcceptedProblem(int targetPlayerID, OrderTypes orderType, int currentConcessions, int requiredConcessions) : base(targetPlayerID, orderType)
			{
				this._currentConcessions = currentConcessions;
				this._requiredConcessions = requiredConcessions;
			}

			// Token: 0x170004DD RID: 1245
			// (get) Token: 0x06002621 RID: 9761 RVA: 0x00081B9C File Offset: 0x0007FD9C
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because only {0}/{1} consecutive demands had been accepted", this._currentConcessions, this._requiredConcessions);
				}
			}

			// Token: 0x170004DE RID: 1246
			// (get) Token: 0x06002622 RID: 9762 RVA: 0x00081BC9 File Offset: 0x0007FDC9
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NotEnoughDemandsAccepted";
				}
			}

			// Token: 0x04001187 RID: 4487
			[JsonProperty]
			private int _currentConcessions;

			// Token: 0x04001188 RID: 4488
			[JsonProperty]
			private int _requiredConcessions;
		}

		// Token: 0x020007FE RID: 2046
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class NotEnoughVendettasWonProblem : Result.DiplomacyProblem
		{
			// Token: 0x06002623 RID: 9763 RVA: 0x00081BDB File Offset: 0x0007FDDB
			[JsonConstructor]
			public NotEnoughVendettasWonProblem()
			{
			}

			// Token: 0x170004DF RID: 1247
			// (get) Token: 0x06002624 RID: 9764 RVA: 0x00081BE3 File Offset: 0x0007FDE3
			[JsonIgnore]
			[BindableValue("value", BindingOption.None)]
			public int CurrentVictories
			{
				get
				{
					return this._currentVictories;
				}
			}

			// Token: 0x170004E0 RID: 1248
			// (get) Token: 0x06002625 RID: 9765 RVA: 0x00081BEB File Offset: 0x0007FDEB
			[JsonIgnore]
			[BindableValue("target", BindingOption.None)]
			public int RequiredVictories
			{
				get
				{
					return this._requiredVictories;
				}
			}

			// Token: 0x06002626 RID: 9766 RVA: 0x00081BF3 File Offset: 0x0007FDF3
			public NotEnoughVendettasWonProblem(int targetPlayerID, OrderTypes orderType, int currentVictories, int requiredVictories) : base(targetPlayerID, orderType)
			{
				this._currentVictories = currentVictories;
				this._requiredVictories = requiredVictories;
			}

			// Token: 0x170004E1 RID: 1249
			// (get) Token: 0x06002627 RID: 9767 RVA: 0x00081C0C File Offset: 0x0007FE0C
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because only {0}/{1} vendettas have been won", this._currentVictories, this._requiredVictories);
				}
			}

			// Token: 0x170004E2 RID: 1250
			// (get) Token: 0x06002628 RID: 9768 RVA: 0x00081C39 File Offset: 0x0007FE39
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NotEnoughVendettasWon";
				}
			}

			// Token: 0x04001189 RID: 4489
			[JsonProperty]
			private int _currentVictories;

			// Token: 0x0400118A RID: 4490
			[JsonProperty]
			private int _requiredVictories;
		}

		// Token: 0x020007FF RID: 2047
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class OutInfluencedProblem : Result.DiplomacyProblem
		{
			// Token: 0x06002629 RID: 9769 RVA: 0x00081C4B File Offset: 0x0007FE4B
			[JsonConstructor]
			public OutInfluencedProblem()
			{
			}

			// Token: 0x0600262A RID: 9770 RVA: 0x00081C53 File Offset: 0x0007FE53
			public OutInfluencedProblem(int targetPlayerID, OrderTypes orderType) : base(targetPlayerID, orderType)
			{
			}

			// Token: 0x170004E3 RID: 1251
			// (get) Token: 0x0600262B RID: 9771 RVA: 0x00081C5D File Offset: 0x0007FE5D
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the target's own action preempted yours";
				}
			}

			// Token: 0x170004E4 RID: 1252
			// (get) Token: 0x0600262C RID: 9772 RVA: 0x00081C6F File Offset: 0x0007FE6F
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".OutInfluenced";
				}
			}
		}

		// Token: 0x02000800 RID: 2048
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class OutRankedProblem : Result.DiplomacyProblem
		{
			// Token: 0x0600262D RID: 9773 RVA: 0x00081C81 File Offset: 0x0007FE81
			[JsonConstructor]
			public OutRankedProblem()
			{
			}

			// Token: 0x0600262E RID: 9774 RVA: 0x00081C89 File Offset: 0x0007FE89
			public OutRankedProblem(int targetPlayerID, OrderTypes orderType) : base(targetPlayerID, orderType)
			{
			}

			// Token: 0x170004E5 RID: 1253
			// (get) Token: 0x0600262F RID: 9775 RVA: 0x00081C93 File Offset: 0x0007FE93
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the target's own action took precedence due to higher rank";
				}
			}

			// Token: 0x170004E6 RID: 1254
			// (get) Token: 0x06002630 RID: 9776 RVA: 0x00081CA5 File Offset: 0x0007FEA5
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".OutRanked";
				}
			}
		}

		// Token: 0x02000801 RID: 2049
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class VassalageQueuedProblem : Result.QueuedDiplomacyActionAgainstTargetProblem
		{
			// Token: 0x06002631 RID: 9777 RVA: 0x00081CB7 File Offset: 0x0007FEB7
			public VassalageQueuedProblem(int targetID, OrderTypes order, OrderTypes otherOrder) : base(targetID, order, otherOrder)
			{
			}
		}

		// Token: 0x02000802 RID: 2050
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class DiplomacyPendingProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004E7 RID: 1255
			// (get) Token: 0x06002632 RID: 9778 RVA: 0x00081CC2 File Offset: 0x0007FEC2
			[JsonIgnore]
			[BindableValue(null, BindingOption.None)]
			public DiplomaticPendingValue PendingOrder
			{
				get
				{
					return this._pendingOrder;
				}
			}

			// Token: 0x06002633 RID: 9779 RVA: 0x00081CCA File Offset: 0x0007FECA
			[JsonConstructor]
			public DiplomacyPendingProblem()
			{
			}

			// Token: 0x06002634 RID: 9780 RVA: 0x00081CD2 File Offset: 0x0007FED2
			public DiplomacyPendingProblem(int targetPlayerID, OrderTypes orderType, DiplomaticPendingValue pendingOrder) : base(targetPlayerID, orderType)
			{
				this._pendingOrder = pendingOrder;
			}

			// Token: 0x170004E8 RID: 1256
			// (get) Token: 0x06002635 RID: 9781 RVA: 0x00081CE3 File Offset: 0x0007FEE3
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" as pending {0} must be resolved first", this._pendingOrder);
				}
			}

			// Token: 0x170004E9 RID: 1257
			// (get) Token: 0x06002636 RID: 9782 RVA: 0x00081D05 File Offset: 0x0007FF05
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DiplomacyPending";
				}
			}

			// Token: 0x0400118B RID: 4491
			[JsonProperty]
			private DiplomaticPendingValue _pendingOrder;
		}

		// Token: 0x02000803 RID: 2051
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ExcommunicatedProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004EA RID: 1258
			// (get) Token: 0x06002637 RID: 9783 RVA: 0x00081D17 File Offset: 0x0007FF17
			[JsonIgnore]
			[BindableValue("archfiend_name", BindingOption.IntPlayerId)]
			public int ExcommunicatedPlayerId
			{
				get
				{
					return this._excommunicatedPlayerId;
				}
			}

			// Token: 0x170004EB RID: 1259
			// (get) Token: 0x06002638 RID: 9784 RVA: 0x00081D1F File Offset: 0x0007FF1F
			[JsonIgnore]
			[BindableValue("excommunicated_self", BindingOption.None)]
			public bool IsSelfExcommunicated
			{
				get
				{
					return this._selfId == this._excommunicatedPlayerId;
				}
			}

			// Token: 0x06002639 RID: 9785 RVA: 0x00081D2F File Offset: 0x0007FF2F
			[JsonConstructor]
			public ExcommunicatedProblem()
			{
			}

			// Token: 0x0600263A RID: 9786 RVA: 0x00081D37 File Offset: 0x0007FF37
			public ExcommunicatedProblem(int targetPlayerID, OrderTypes orderType, int excommunicatedPlayerId, int selfId) : base(targetPlayerID, orderType)
			{
				this._excommunicatedPlayerId = excommunicatedPlayerId;
				this._selfId = selfId;
			}

			// Token: 0x170004EC RID: 1260
			// (get) Token: 0x0600263B RID: 9787 RVA: 0x00081D50 File Offset: 0x0007FF50
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because {0} is Excommunicated", this._excommunicatedPlayerId);
				}
			}

			// Token: 0x170004ED RID: 1261
			// (get) Token: 0x0600263C RID: 9788 RVA: 0x00081D72 File Offset: 0x0007FF72
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x170004EE RID: 1262
			// (get) Token: 0x0600263D RID: 9789 RVA: 0x00081D84 File Offset: 0x0007FF84
			public override string PreviewLocKey
			{
				get
				{
					return this.LocKeyScope + ".Excommunicated.Preview";
				}
			}

			// Token: 0x0400118C RID: 4492
			[JsonProperty]
			private int _excommunicatedPlayerId;

			// Token: 0x0400118D RID: 4493
			[JsonProperty]
			private int _selfId;
		}

		// Token: 0x02000804 RID: 2052
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CannotWhileEliminatedProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004EF RID: 1263
			// (get) Token: 0x0600263E RID: 9790 RVA: 0x00081D96 File Offset: 0x0007FF96
			[JsonIgnore]
			[BindableValue("archfiend_name", BindingOption.IntPlayerId)]
			public int EliminatedPlayerId
			{
				get
				{
					return this._eliminatedPlayerId;
				}
			}

			// Token: 0x0600263F RID: 9791 RVA: 0x00081D9E File Offset: 0x0007FF9E
			[JsonConstructor]
			public CannotWhileEliminatedProblem()
			{
			}

			// Token: 0x06002640 RID: 9792 RVA: 0x00081DA6 File Offset: 0x0007FFA6
			public CannotWhileEliminatedProblem(int targetPlayerID, OrderTypes orderType, int eliminatedPlayerId) : base(targetPlayerID, orderType)
			{
				this._eliminatedPlayerId = eliminatedPlayerId;
			}

			// Token: 0x170004F0 RID: 1264
			// (get) Token: 0x06002641 RID: 9793 RVA: 0x00081DB7 File Offset: 0x0007FFB7
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because {0} is Eliminated", this._eliminatedPlayerId);
				}
			}

			// Token: 0x170004F1 RID: 1265
			// (get) Token: 0x06002642 RID: 9794 RVA: 0x00081DD9 File Offset: 0x0007FFD9
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CannotWhileEliminated";
				}
			}

			// Token: 0x0400118E RID: 4494
			[JsonProperty]
			private int _eliminatedPlayerId;
		}

		// Token: 0x02000805 RID: 2053
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvalidGameStateForDiplomacyProblem : Result.DiplomacyProblem
		{
			// Token: 0x170004F2 RID: 1266
			// (get) Token: 0x06002643 RID: 9795 RVA: 0x00081DEB File Offset: 0x0007FFEB
			[JsonIgnore]
			[BindableValue(null, BindingOption.None)]
			public TurnPhase CurrentPhase
			{
				get
				{
					return this._currentPhase;
				}
			}

			// Token: 0x06002644 RID: 9796 RVA: 0x00081DF3 File Offset: 0x0007FFF3
			[JsonConstructor]
			public InvalidGameStateForDiplomacyProblem()
			{
			}

			// Token: 0x06002645 RID: 9797 RVA: 0x00081DFB File Offset: 0x0007FFFB
			public InvalidGameStateForDiplomacyProblem(int targetPlayerID, OrderTypes orderType, TurnPhase currentPhase) : base(targetPlayerID, orderType)
			{
				this._currentPhase = currentPhase;
			}

			// Token: 0x170004F3 RID: 1267
			// (get) Token: 0x06002646 RID: 9798 RVA: 0x00081E0C File Offset: 0x0008000C
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because turn phase is {0}", this._currentPhase);
				}
			}

			// Token: 0x170004F4 RID: 1268
			// (get) Token: 0x06002647 RID: 9799 RVA: 0x00081E2E File Offset: 0x0008002E
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".InvalidGameState";
				}
			}

			// Token: 0x0400118F RID: 4495
			[JsonProperty]
			private TurnPhase _currentPhase;
		}

		// Token: 0x02000806 RID: 2054
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class VassalageDiplomacyProblem : Result.InvalidDiplomaticStateProblem
		{
			// Token: 0x170004F5 RID: 1269
			// (get) Token: 0x06002648 RID: 9800 RVA: 0x00081E40 File Offset: 0x00080040
			[JsonIgnore]
			[BindableValue("liege_name", BindingOption.IntPlayerId)]
			public int BloodLordPlayerId
			{
				get
				{
					return this._bloodLordPlayerId;
				}
			}

			// Token: 0x170004F6 RID: 1270
			// (get) Token: 0x06002649 RID: 9801 RVA: 0x00081E48 File Offset: 0x00080048
			[JsonIgnore]
			[BindableValue("vassal_name", BindingOption.IntPlayerId)]
			public int BloodVassalPlayerId
			{
				get
				{
					return this._bloodVassalPlayerId;
				}
			}

			// Token: 0x170004F7 RID: 1271
			// (get) Token: 0x0600264A RID: 9802 RVA: 0x00081E50 File Offset: 0x00080050
			[JsonIgnore]
			[BindableValue("liege_self", BindingOption.None)]
			public bool LiegeSelf
			{
				get
				{
					return this._selfId == this._bloodLordPlayerId;
				}
			}

			// Token: 0x170004F8 RID: 1272
			// (get) Token: 0x0600264B RID: 9803 RVA: 0x00081E60 File Offset: 0x00080060
			[JsonIgnore]
			[BindableValue("vassal_self", BindingOption.None)]
			public bool VassalSelf
			{
				get
				{
					return this._selfId == this._bloodVassalPlayerId;
				}
			}

			// Token: 0x0600264C RID: 9804 RVA: 0x00081E70 File Offset: 0x00080070
			[JsonConstructor]
			public VassalageDiplomacyProblem()
			{
			}

			// Token: 0x0600264D RID: 9805 RVA: 0x00081E78 File Offset: 0x00080078
			public VassalageDiplomacyProblem(int targetPlayerID, OrderTypes orderType, DiplomaticStateValue currentState, int bloodLordPlayerId, int bloodVassalPlayerId, int selfId) : base(targetPlayerID, orderType, currentState)
			{
				this._bloodLordPlayerId = bloodLordPlayerId;
				this._bloodVassalPlayerId = bloodVassalPlayerId;
				this._selfId = selfId;
			}

			// Token: 0x170004F9 RID: 1273
			// (get) Token: 0x0600264E RID: 9806 RVA: 0x00081E9B File Offset: 0x0008009B
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because {0} is a vassal of {1}", this._bloodVassalPlayerId, this._bloodLordPlayerId);
				}
			}

			// Token: 0x170004FA RID: 1274
			// (get) Token: 0x0600264F RID: 9807 RVA: 0x00081EC8 File Offset: 0x000800C8
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".Vassalage";
				}
			}

			// Token: 0x170004FB RID: 1275
			// (get) Token: 0x06002650 RID: 9808 RVA: 0x00081EDA File Offset: 0x000800DA
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x04001190 RID: 4496
			[JsonProperty]
			private int _bloodLordPlayerId;

			// Token: 0x04001191 RID: 4497
			[JsonProperty]
			private int _bloodVassalPlayerId;

			// Token: 0x04001192 RID: 4498
			[JsonProperty]
			private int _selfId;
		}

		// Token: 0x02000807 RID: 2055
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CannotWhileVassalProblem : Result.VassalageDiplomacyProblem
		{
			// Token: 0x06002651 RID: 9809 RVA: 0x00081EEC File Offset: 0x000800EC
			[JsonConstructor]
			public CannotWhileVassalProblem()
			{
			}

			// Token: 0x06002652 RID: 9810 RVA: 0x00081EF4 File Offset: 0x000800F4
			public CannotWhileVassalProblem(int targetPlayerID, OrderTypes orderType, int bloodLordPlayerId, int bloodVassalPlayerId) : base(targetPlayerID, orderType, DiplomaticStateValue.Vassalised, bloodLordPlayerId, bloodVassalPlayerId, bloodVassalPlayerId)
			{
			}

			// Token: 0x170004FC RID: 1276
			// (get) Token: 0x06002653 RID: 9811 RVA: 0x00081F05 File Offset: 0x00080105
			public override string DebugString
			{
				get
				{
					return base.DebugString + " (because you are a vassal)";
				}
			}

			// Token: 0x170004FD RID: 1277
			// (get) Token: 0x06002654 RID: 9812 RVA: 0x00081F17 File Offset: 0x00080117
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CannotWhileVassal";
				}
			}
		}

		// Token: 0x02000808 RID: 2056
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CannotTargetVassalProblem : Result.VassalageDiplomacyProblem
		{
			// Token: 0x06002655 RID: 9813 RVA: 0x00081F29 File Offset: 0x00080129
			[JsonConstructor]
			public CannotTargetVassalProblem()
			{
			}

			// Token: 0x06002656 RID: 9814 RVA: 0x00081F31 File Offset: 0x00080131
			public CannotTargetVassalProblem(int targetPlayerID, OrderTypes orderType, int bloodLordPlayerId, int bloodVassalId, int selfId) : base(targetPlayerID, orderType, DiplomaticStateValue.Vassalised, bloodLordPlayerId, bloodVassalId, selfId)
			{
			}

			// Token: 0x170004FE RID: 1278
			// (get) Token: 0x06002657 RID: 9815 RVA: 0x00081F42 File Offset: 0x00080142
			public override string DebugString
			{
				get
				{
					return base.DebugString + " (because they are a vassal)";
				}
			}

			// Token: 0x170004FF RID: 1279
			// (get) Token: 0x06002658 RID: 9816 RVA: 0x00081F54 File Offset: 0x00080154
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CannotTargetVassal";
				}
			}
		}

		// Token: 0x02000809 RID: 2057
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CannotWhileLiegeProblem : Result.VassalageDiplomacyProblem
		{
			// Token: 0x06002659 RID: 9817 RVA: 0x00081F66 File Offset: 0x00080166
			[JsonConstructor]
			public CannotWhileLiegeProblem()
			{
			}

			// Token: 0x0600265A RID: 9818 RVA: 0x00081F6E File Offset: 0x0008016E
			public CannotWhileLiegeProblem(int targetPlayerID, OrderTypes orderType, DiplomaticStateValue currentState, int bloodLordPlayerId, int bloodVassalPlayerId) : base(targetPlayerID, orderType, currentState, bloodLordPlayerId, bloodVassalPlayerId, bloodLordPlayerId)
			{
			}

			// Token: 0x17000500 RID: 1280
			// (get) Token: 0x0600265B RID: 9819 RVA: 0x00081F7F File Offset: 0x0008017F
			public override string DebugString
			{
				get
				{
					return base.DebugString + " (because you are a liege)";
				}
			}

			// Token: 0x17000501 RID: 1281
			// (get) Token: 0x0600265C RID: 9820 RVA: 0x00081F91 File Offset: 0x00080191
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CannotWhileLiege";
				}
			}
		}

		// Token: 0x0200080A RID: 2058
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CannotTargetLiegeProblem : Result.VassalageDiplomacyProblem
		{
			// Token: 0x0600265D RID: 9821 RVA: 0x00081FA3 File Offset: 0x000801A3
			[JsonConstructor]
			public CannotTargetLiegeProblem()
			{
			}

			// Token: 0x0600265E RID: 9822 RVA: 0x00081FAB File Offset: 0x000801AB
			public CannotTargetLiegeProblem(int targetPlayerID, OrderTypes orderType, DiplomaticStateValue currentState, int bloodLordId, int bloodVassalPlayerId, int selfId) : base(targetPlayerID, orderType, currentState, bloodLordId, bloodVassalPlayerId, selfId)
			{
			}

			// Token: 0x17000502 RID: 1282
			// (get) Token: 0x0600265F RID: 9823 RVA: 0x00081FBC File Offset: 0x000801BC
			public override string DebugString
			{
				get
				{
					return base.DebugString + " (because they are a liege)";
				}
			}

			// Token: 0x17000503 RID: 1283
			// (get) Token: 0x06002660 RID: 9824 RVA: 0x00081FCE File Offset: 0x000801CE
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CannotTargetLiege";
				}
			}
		}

		// Token: 0x0200080B RID: 2059
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class TooMuchPrestigeProblem : Result.VassalageDiplomacyProblem
		{
			// Token: 0x06002661 RID: 9825 RVA: 0x00081FE0 File Offset: 0x000801E0
			[JsonConstructor]
			public TooMuchPrestigeProblem()
			{
			}

			// Token: 0x17000504 RID: 1284
			// (get) Token: 0x06002662 RID: 9826 RVA: 0x00081FE8 File Offset: 0x000801E8
			[JsonIgnore]
			[BindableValue("value", BindingOption.None)]
			public int CurrentPrestige
			{
				get
				{
					return this._currentPrestige;
				}
			}

			// Token: 0x17000505 RID: 1285
			// (get) Token: 0x06002663 RID: 9827 RVA: 0x00081FF0 File Offset: 0x000801F0
			[JsonIgnore]
			[BindableValue("max_value", BindingOption.None)]
			public int MaximumPrestige
			{
				get
				{
					return this._maximumPrestige;
				}
			}

			// Token: 0x06002664 RID: 9828 RVA: 0x00081FF8 File Offset: 0x000801F8
			public TooMuchPrestigeProblem(int targetPlayerID, OrderTypes orderType, DiplomaticStateValue currentState, int currentPrestige, int maximumPrestige, int prospectiveLiegeID, int prospectiveVassalID, int selfId) : base(targetPlayerID, orderType, currentState, prospectiveLiegeID, prospectiveVassalID, selfId)
			{
				this._currentPrestige = currentPrestige;
				this._maximumPrestige = maximumPrestige;
			}

			// Token: 0x17000506 RID: 1286
			// (get) Token: 0x06002665 RID: 9829 RVA: 0x00082019 File Offset: 0x00080219
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because the prospective vassal has {0}>{1} prestige", this._currentPrestige, this._maximumPrestige);
				}
			}

			// Token: 0x17000507 RID: 1287
			// (get) Token: 0x06002666 RID: 9830 RVA: 0x00082046 File Offset: 0x00080246
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".TooMuchPrestige";
				}
			}

			// Token: 0x04001193 RID: 4499
			[JsonProperty]
			private int _currentPrestige;

			// Token: 0x04001194 RID: 4500
			[JsonProperty]
			private int _maximumPrestige;
		}

		// Token: 0x0200080C RID: 2060
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class TargetHasDiplomaticImmunityProblem : Result.DiplomacyProblem
		{
			// Token: 0x06002667 RID: 9831 RVA: 0x00082058 File Offset: 0x00080258
			[JsonConstructor]
			public TargetHasDiplomaticImmunityProblem()
			{
			}

			// Token: 0x06002668 RID: 9832 RVA: 0x00082060 File Offset: 0x00080260
			public TargetHasDiplomaticImmunityProblem(int targetPlayerID, OrderTypes orderType) : base(targetPlayerID, orderType)
			{
			}

			// Token: 0x17000508 RID: 1288
			// (get) Token: 0x06002669 RID: 9833 RVA: 0x0008206A File Offset: 0x0008026A
			public override string DebugString
			{
				get
				{
					return base.DebugString + " while the target's Diplomatic Immunity last";
				}
			}

			// Token: 0x17000509 RID: 1289
			// (get) Token: 0x0600266A RID: 9834 RVA: 0x0008207C File Offset: 0x0008027C
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".TargetHasDiplomaticImmunity";
				}
			}
		}

		// Token: 0x0200080D RID: 2061
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class DiplomacyCooldownProblem : Result.DiplomacyProblem
		{
			// Token: 0x0600266B RID: 9835 RVA: 0x0008208E File Offset: 0x0008028E
			[JsonConstructor]
			public DiplomacyCooldownProblem()
			{
			}

			// Token: 0x1700050A RID: 1290
			// (get) Token: 0x0600266C RID: 9836 RVA: 0x00082096 File Offset: 0x00080296
			[JsonIgnore]
			[BindableValue("duration", BindingOption.None)]
			[JsonProperty]
			public int TurnsRemaining
			{
				get
				{
					return this._turnsRemaining;
				}
			}

			// Token: 0x0600266D RID: 9837 RVA: 0x0008209E File Offset: 0x0008029E
			public DiplomacyCooldownProblem(int targetPlayerID, OrderTypes orderType, int turnsRemaining) : base(targetPlayerID, orderType)
			{
				this._turnsRemaining = turnsRemaining;
			}

			// Token: 0x1700050B RID: 1291
			// (get) Token: 0x0600266E RID: 9838 RVA: 0x000820AF File Offset: 0x000802AF
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".ActionOnCooldown";
				}
			}

			// Token: 0x1700050C RID: 1292
			// (get) Token: 0x0600266F RID: 9839 RVA: 0x000820C1 File Offset: 0x000802C1
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because the action can't be used for another {0} turns", this.TurnsRemaining);
				}
			}

			// Token: 0x04001195 RID: 4501
			[JsonProperty]
			private int _turnsRemaining;
		}

		// Token: 0x0200080E RID: 2062
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class TemporaryStateProblem : Result.InvalidDiplomaticStateProblem
		{
			// Token: 0x06002670 RID: 9840 RVA: 0x000820E3 File Offset: 0x000802E3
			[JsonConstructor]
			public TemporaryStateProblem()
			{
			}

			// Token: 0x1700050D RID: 1293
			// (get) Token: 0x06002671 RID: 9841 RVA: 0x000820EB File Offset: 0x000802EB
			[JsonIgnore]
			[BindableValue("duration", BindingOption.None)]
			[JsonProperty]
			public int TurnsRemaining
			{
				get
				{
					return this._turnsRemaining;
				}
			}

			// Token: 0x06002672 RID: 9842 RVA: 0x000820F3 File Offset: 0x000802F3
			public TemporaryStateProblem(int targetPlayerID, OrderTypes orderType, DiplomaticStateValue currentState, int turnsRemaining) : base(targetPlayerID, orderType, currentState)
			{
				this._turnsRemaining = turnsRemaining;
			}

			// Token: 0x1700050E RID: 1294
			// (get) Token: 0x06002673 RID: 9843 RVA: 0x00082106 File Offset: 0x00080306
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" but this state will end in {0} turns", this._turnsRemaining);
				}
			}

			// Token: 0x1700050F RID: 1295
			// (get) Token: 0x06002674 RID: 9844 RVA: 0x00082128 File Offset: 0x00080328
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".TemporaryState";
				}
			}

			// Token: 0x04001196 RID: 4502
			[JsonProperty]
			private int _turnsRemaining;
		}

		// Token: 0x0200080F RID: 2063
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualProblem : Problem
		{
			// Token: 0x06002675 RID: 9845 RVA: 0x0008213A File Offset: 0x0008033A
			[JsonConstructor]
			protected CastRitualProblem()
			{
			}

			// Token: 0x06002676 RID: 9846 RVA: 0x00082142 File Offset: 0x00080342
			public CastRitualProblem(ConfigRef ritual)
			{
				this.Ritual = ritual;
			}

			// Token: 0x17000510 RID: 1296
			// (get) Token: 0x06002677 RID: 9847 RVA: 0x00082151 File Offset: 0x00080351
			protected override string LocKeyScope
			{
				get
				{
					return "Result.Ritual";
				}
			}

			// Token: 0x17000511 RID: 1297
			// (get) Token: 0x06002678 RID: 9848 RVA: 0x00082158 File Offset: 0x00080358
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000512 RID: 1298
			// (get) Token: 0x06002679 RID: 9849 RVA: 0x0008216A File Offset: 0x0008036A
			public override string DebugString
			{
				get
				{
					return string.Format("{0} could not be cast", this.Ritual);
				}
			}

			// Token: 0x04001197 RID: 4503
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public ConfigRef Ritual;
		}

		// Token: 0x02000810 RID: 2064
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RitualTableBlockedProblem : Result.CastRitualProblem
		{
			// Token: 0x0600267A RID: 9850 RVA: 0x0008217C File Offset: 0x0008037C
			[JsonConstructor]
			protected RitualTableBlockedProblem()
			{
			}

			// Token: 0x0600267B RID: 9851 RVA: 0x00082184 File Offset: 0x00080384
			public RitualTableBlockedProblem(ConfigRef ritual) : base(ritual)
			{
			}

			// Token: 0x17000513 RID: 1299
			// (get) Token: 0x0600267C RID: 9852 RVA: 0x0008218D File Offset: 0x0008038D
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".RitualTableBlocked";
				}
			}

			// Token: 0x17000514 RID: 1300
			// (get) Token: 0x0600267D RID: 9853 RVA: 0x0008219F File Offset: 0x0008039F
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the ritual table is unavailable";
				}
			}
		}

		// Token: 0x02000811 RID: 2065
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CannotAffordRitualProblem : Result.CastRitualProblem
		{
			// Token: 0x0600267E RID: 9854 RVA: 0x000821B1 File Offset: 0x000803B1
			[JsonConstructor]
			protected CannotAffordRitualProblem()
			{
			}

			// Token: 0x0600267F RID: 9855 RVA: 0x000821B9 File Offset: 0x000803B9
			public CannotAffordRitualProblem(ConfigRef ritual) : base(ritual)
			{
			}

			// Token: 0x17000515 RID: 1301
			// (get) Token: 0x06002680 RID: 9856 RVA: 0x000821C2 File Offset: 0x000803C2
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NotEnoughResources";
				}
			}

			// Token: 0x17000516 RID: 1302
			// (get) Token: 0x06002681 RID: 9857 RVA: 0x000821D4 File Offset: 0x000803D4
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because not enough resources were provided";
				}
			}
		}

		// Token: 0x02000812 RID: 2066
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class NoFreeRitualSlotProblem : Result.CastRitualProblem
		{
			// Token: 0x06002682 RID: 9858 RVA: 0x000821E6 File Offset: 0x000803E6
			[JsonConstructor]
			protected NoFreeRitualSlotProblem()
			{
			}

			// Token: 0x06002683 RID: 9859 RVA: 0x000821EE File Offset: 0x000803EE
			public NoFreeRitualSlotProblem(ConfigRef ritual) : base(ritual)
			{
			}

			// Token: 0x17000517 RID: 1303
			// (get) Token: 0x06002684 RID: 9860 RVA: 0x000821F7 File Offset: 0x000803F7
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NoFreeRitualSlot";
				}
			}

			// Token: 0x17000518 RID: 1304
			// (get) Token: 0x06002685 RID: 9861 RVA: 0x00082209 File Offset: 0x00080409
			public override string DebugString
			{
				get
				{
					return base.DebugString + " due to insufficient ritual slots";
				}
			}
		}

		// Token: 0x02000813 RID: 2067
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RitualMaskingUnavailableProblem : Result.CastRitualProblem
		{
			// Token: 0x06002686 RID: 9862 RVA: 0x0008221B File Offset: 0x0008041B
			[JsonConstructor]
			protected RitualMaskingUnavailableProblem()
			{
			}

			// Token: 0x06002687 RID: 9863 RVA: 0x00082223 File Offset: 0x00080423
			public RitualMaskingUnavailableProblem(ConfigRef ritual) : base(ritual)
			{
			}

			// Token: 0x17000519 RID: 1305
			// (get) Token: 0x06002688 RID: 9864 RVA: 0x0008222C File Offset: 0x0008042C
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".MaskingUnavailable";
				}
			}

			// Token: 0x1700051A RID: 1306
			// (get) Token: 0x06002689 RID: 9865 RVA: 0x0008223E File Offset: 0x0008043E
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because they can no longer use masking";
				}
			}
		}

		// Token: 0x02000814 RID: 2068
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RitualMaskingExcommunicatedProblem : Problem
		{
			// Token: 0x0600268A RID: 9866 RVA: 0x00082250 File Offset: 0x00080450
			[JsonConstructor]
			public RitualMaskingExcommunicatedProblem()
			{
			}

			// Token: 0x1700051B RID: 1307
			// (get) Token: 0x0600268B RID: 9867 RVA: 0x00082258 File Offset: 0x00080458
			public override string LocKey
			{
				get
				{
					return "Rituals.Masking.MirrorLocked.Excommunication";
				}
			}

			// Token: 0x1700051C RID: 1308
			// (get) Token: 0x0600268C RID: 9868 RVA: 0x0008225F File Offset: 0x0008045F
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because they can no longer use masking due to excommunication";
				}
			}
		}

		// Token: 0x02000815 RID: 2069
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RitualMaskingLockedProblem : Problem
		{
			// Token: 0x0600268D RID: 9869 RVA: 0x00082271 File Offset: 0x00080471
			[JsonConstructor]
			public RitualMaskingLockedProblem()
			{
			}

			// Token: 0x1700051D RID: 1309
			// (get) Token: 0x0600268E RID: 9870 RVA: 0x00082279 File Offset: 0x00080479
			public override string LocKey
			{
				get
				{
					return "Rituals.Masking.MirrorLocked.Prerequisite";
				}
			}

			// Token: 0x1700051E RID: 1310
			// (get) Token: 0x0600268F RID: 9871 RVA: 0x00082280 File Offset: 0x00080480
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because they can no longer use masking";
				}
			}
		}

		// Token: 0x02000816 RID: 2070
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RitualFramingUnavailableProblem : Result.CastRitualProblem
		{
			// Token: 0x06002690 RID: 9872 RVA: 0x00082292 File Offset: 0x00080492
			[JsonConstructor]
			protected RitualFramingUnavailableProblem()
			{
			}

			// Token: 0x06002691 RID: 9873 RVA: 0x0008229A File Offset: 0x0008049A
			public RitualFramingUnavailableProblem(ConfigRef ritual) : base(ritual)
			{
			}

			// Token: 0x1700051F RID: 1311
			// (get) Token: 0x06002692 RID: 9874 RVA: 0x000822A3 File Offset: 0x000804A3
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".FramingUnavailable";
				}
			}

			// Token: 0x17000520 RID: 1312
			// (get) Token: 0x06002693 RID: 9875 RVA: 0x000822B5 File Offset: 0x000804B5
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because they can no longer use framing";
				}
			}
		}

		// Token: 0x02000817 RID: 2071
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ArtifactNotEquippedProblem : Result.CastRitualProblem
		{
			// Token: 0x06002694 RID: 9876 RVA: 0x000822C7 File Offset: 0x000804C7
			[JsonConstructor]
			protected ArtifactNotEquippedProblem()
			{
			}

			// Token: 0x06002695 RID: 9877 RVA: 0x000822CF File Offset: 0x000804CF
			public ArtifactNotEquippedProblem(ConfigRef ritual) : base(ritual)
			{
			}

			// Token: 0x17000521 RID: 1313
			// (get) Token: 0x06002696 RID: 9878 RVA: 0x000822D8 File Offset: 0x000804D8
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".ArtifactNotEquipped";
				}
			}

			// Token: 0x17000522 RID: 1314
			// (get) Token: 0x06002697 RID: 9879 RVA: 0x000822EA File Offset: 0x000804EA
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the required artifact is not equipped";
				}
			}
		}

		// Token: 0x02000818 RID: 2072
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class PowerTooLowForRitualProblem : Result.CastRitualProblem
		{
			// Token: 0x06002698 RID: 9880 RVA: 0x000822FC File Offset: 0x000804FC
			[JsonConstructor]
			protected PowerTooLowForRitualProblem()
			{
			}

			// Token: 0x06002699 RID: 9881 RVA: 0x00082304 File Offset: 0x00080504
			public PowerTooLowForRitualProblem(ConfigRef ritual, PowerType offendingPower) : base(ritual)
			{
				this.OffendingPower = offendingPower;
			}

			// Token: 0x17000523 RID: 1315
			// (get) Token: 0x0600269A RID: 9882 RVA: 0x00082314 File Offset: 0x00080514
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".PowerTooLow";
				}
			}

			// Token: 0x17000524 RID: 1316
			// (get) Token: 0x0600269B RID: 9883 RVA: 0x00082326 File Offset: 0x00080526
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because {0} is too low", this.OffendingPower);
				}
			}

			// Token: 0x04001198 RID: 4504
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public PowerType OffendingPower;
		}

		// Token: 0x02000819 RID: 2073
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CommandRatingTooLowToSummonProblem : Result.CastRitualProblem
		{
			// Token: 0x0600269C RID: 9884 RVA: 0x00082348 File Offset: 0x00080548
			[JsonConstructor]
			protected CommandRatingTooLowToSummonProblem()
			{
			}

			// Token: 0x0600269D RID: 9885 RVA: 0x00082350 File Offset: 0x00080550
			public CommandRatingTooLowToSummonProblem(ConfigRef ritual, int legionCount, int commandRating) : base(ritual)
			{
				this.LegionCount = legionCount;
				this.CommandRating = commandRating;
			}

			// Token: 0x17000525 RID: 1317
			// (get) Token: 0x0600269E RID: 9886 RVA: 0x00082367 File Offset: 0x00080567
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CommandRatingTooLow";
				}
			}

			// Token: 0x17000526 RID: 1318
			// (get) Token: 0x0600269F RID: 9887 RVA: 0x00082379 File Offset: 0x00080579
			public override string DebugString
			{
				get
				{
					return string.Format(" because Command Rating ({0}) can't support {1} legions", this.LegionCount, this.LegionCount + 1);
				}
			}

			// Token: 0x04001199 RID: 4505
			[JsonProperty]
			[BindableValue("value", BindingOption.None)]
			public int LegionCount;

			// Token: 0x0400119A RID: 4506
			[JsonProperty]
			[BindableValue("max_value", BindingOption.None)]
			public int CommandRating;
		}

		// Token: 0x0200081A RID: 2074
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnPlayerProblem : Result.CastRitualProblem
		{
			// Token: 0x060026A0 RID: 9888 RVA: 0x0008239D File Offset: 0x0008059D
			[JsonConstructor]
			protected CastRitualOnPlayerProblem()
			{
			}

			// Token: 0x060026A1 RID: 9889 RVA: 0x000823A5 File Offset: 0x000805A5
			public CastRitualOnPlayerProblem(ConfigRef ritual, int targetPlayerID) : base(ritual)
			{
				this.TargetPlayerID = targetPlayerID;
			}

			// Token: 0x17000527 RID: 1319
			// (get) Token: 0x060026A2 RID: 9890 RVA: 0x000823B5 File Offset: 0x000805B5
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".Archfiend";
				}
			}

			// Token: 0x17000528 RID: 1320
			// (get) Token: 0x060026A3 RID: 9891 RVA: 0x000823C7 File Offset: 0x000805C7
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000529 RID: 1321
			// (get) Token: 0x060026A4 RID: 9892 RVA: 0x000823D9 File Offset: 0x000805D9
			public override string DebugString
			{
				get
				{
					return string.Format(" on player {0}", this.TargetPlayerID);
				}
			}

			// Token: 0x0400119B RID: 4507
			[JsonProperty]
			[BindableValue("affected_name", BindingOption.IntPlayerId)]
			[DefaultValue(-2147483648)]
			public int TargetPlayerID;
		}

		// Token: 0x0200081B RID: 2075
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnSelfProblem : Result.CastRitualOnPlayerProblem
		{
			// Token: 0x060026A5 RID: 9893 RVA: 0x000823F0 File Offset: 0x000805F0
			[JsonConstructor]
			protected CastRitualOnSelfProblem()
			{
			}

			// Token: 0x060026A6 RID: 9894 RVA: 0x000823F8 File Offset: 0x000805F8
			public CastRitualOnSelfProblem(ConfigRef ritual, int selfId, bool selfTargetingRequired = false) : base(ritual, selfId)
			{
				this.SelfTargetingRequired = selfTargetingRequired;
			}

			// Token: 0x1700052A RID: 1322
			// (get) Token: 0x060026A7 RID: 9895 RVA: 0x00082409 File Offset: 0x00080609
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + (this.SelfTargetingRequired ? ".SelfTargeting.Required" : ".SelfTargeting.Forbidden");
				}
			}

			// Token: 0x1700052B RID: 1323
			// (get) Token: 0x060026A8 RID: 9896 RVA: 0x0008242A File Offset: 0x0008062A
			public override string DebugString
			{
				get
				{
					return base.DebugString + (this.SelfTargetingRequired ? " because it must be self-cast" : " because it cannot be self-cast");
				}
			}

			// Token: 0x0400119C RID: 4508
			[JsonProperty]
			public bool SelfTargetingRequired;
		}

		// Token: 0x0200081C RID: 2076
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualNoAdjacentHexOwnedProblem : Result.CastRitualOnPlayerProblem
		{
			// Token: 0x060026A9 RID: 9897 RVA: 0x0008244B File Offset: 0x0008064B
			[JsonConstructor]
			protected CastRitualNoAdjacentHexOwnedProblem()
			{
			}

			// Token: 0x060026AA RID: 9898 RVA: 0x00082453 File Offset: 0x00080653
			public CastRitualNoAdjacentHexOwnedProblem(ConfigRef ritual, int selfId, HexCoord targetHex) : base(ritual, selfId)
			{
				this.TargetHex = targetHex;
			}

			// Token: 0x1700052C RID: 1324
			// (get) Token: 0x060026AB RID: 9899 RVA: 0x00082464 File Offset: 0x00080664
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NoAdjacentHexOwned";
				}
			}

			// Token: 0x1700052D RID: 1325
			// (get) Token: 0x060026AC RID: 9900 RVA: 0x00082476 File Offset: 0x00080676
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because they do not own a tile adjacent to {0}", this.TargetHex);
				}
			}

			// Token: 0x0400119D RID: 4509
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public HexCoord TargetHex;
		}

		// Token: 0x0200081D RID: 2077
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class PlayerHasNoItemRitualCanTargetProblem : Result.CastRitualOnPlayerProblem
		{
			// Token: 0x060026AD RID: 9901 RVA: 0x00082498 File Offset: 0x00080698
			[JsonConstructor]
			protected PlayerHasNoItemRitualCanTargetProblem()
			{
			}

			// Token: 0x060026AE RID: 9902 RVA: 0x000824A0 File Offset: 0x000806A0
			public PlayerHasNoItemRitualCanTargetProblem(ConfigRef ritual, int targetPlayerID, GameItemCategory itemCategory) : base(ritual, targetPlayerID)
			{
				this.DesiredItemCategory = itemCategory;
			}

			// Token: 0x1700052E RID: 1326
			// (get) Token: 0x060026AF RID: 9903 RVA: 0x000824B1 File Offset: 0x000806B1
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NoItemToTarget";
				}
			}

			// Token: 0x1700052F RID: 1327
			// (get) Token: 0x060026B0 RID: 9904 RVA: 0x000824C3 File Offset: 0x000806C3
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because they have no targetable {0}", this.DesiredItemCategory);
				}
			}

			// Token: 0x0400119E RID: 4510
			[JsonProperty]
			[BindableValue("gameitem_category", BindingOption.None)]
			public GameItemCategory DesiredItemCategory;
		}

		// Token: 0x0200081E RID: 2078
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnEliminatedPlayerProblem : Result.CastRitualOnPlayerProblem
		{
			// Token: 0x060026B1 RID: 9905 RVA: 0x000824E5 File Offset: 0x000806E5
			[JsonConstructor]
			protected CastRitualOnEliminatedPlayerProblem()
			{
			}

			// Token: 0x060026B2 RID: 9906 RVA: 0x000824ED File Offset: 0x000806ED
			public CastRitualOnEliminatedPlayerProblem(ConfigRef ritual, int targetPlayerID) : base(ritual, targetPlayerID)
			{
			}

			// Token: 0x17000530 RID: 1328
			// (get) Token: 0x060026B3 RID: 9907 RVA: 0x000824F7 File Offset: 0x000806F7
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Eliminated";
				}
			}

			// Token: 0x17000531 RID: 1329
			// (get) Token: 0x060026B4 RID: 9908 RVA: 0x00082509 File Offset: 0x00080709
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the target is eliminated";
				}
			}
		}

		// Token: 0x0200081F RID: 2079
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvalidDiplomacyForRitualProblem : Result.CastRitualOnPlayerProblem
		{
			// Token: 0x060026B5 RID: 9909 RVA: 0x0008251B File Offset: 0x0008071B
			[JsonConstructor]
			protected InvalidDiplomacyForRitualProblem()
			{
			}

			// Token: 0x060026B6 RID: 9910 RVA: 0x00082523 File Offset: 0x00080723
			public InvalidDiplomacyForRitualProblem(ConfigRef ritual, int targetPlayerID, DiplomaticStateValue state) : base(ritual, targetPlayerID)
			{
				this.State = state;
			}

			// Token: 0x17000532 RID: 1330
			// (get) Token: 0x060026B7 RID: 9911 RVA: 0x00082534 File Offset: 0x00080734
			public override string LocKey
			{
				get
				{
					string locKeyScope = this.LocKeyScope;
					DiplomaticStateValue state = this.State;
					string str = ".InvalidDiplomacy";
					return locKeyScope + str;
				}
			}

			// Token: 0x17000533 RID: 1331
			// (get) Token: 0x060026B8 RID: 9912 RVA: 0x0008255A File Offset: 0x0008075A
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because in is not possible in {0}", this.State);
				}
			}

			// Token: 0x0400119F RID: 4511
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public DiplomaticStateValue State;
		}

		// Token: 0x02000820 RID: 2080
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RitualResistedProblem : Result.CastRitualOnPlayerProblem
		{
			// Token: 0x060026B9 RID: 9913 RVA: 0x0008257C File Offset: 0x0008077C
			[JsonConstructor]
			protected RitualResistedProblem()
			{
			}

			// Token: 0x060026BA RID: 9914 RVA: 0x00082584 File Offset: 0x00080784
			public RitualResistedProblem(float strengthRoll, float chance)
			{
				this.StrengthRoll = strengthRoll;
				this.Chance = chance;
			}

			// Token: 0x17000534 RID: 1332
			// (get) Token: 0x060026BB RID: 9915 RVA: 0x0008259A File Offset: 0x0008079A
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because it was resisted (needed to roll below [Chance]{0} but rolled [Roll]{1:F3})", this.Chance, this.StrengthRoll);
				}
			}

			// Token: 0x040011A0 RID: 4512
			[JsonProperty]
			private float StrengthRoll;

			// Token: 0x040011A1 RID: 4513
			[JsonProperty]
			private float Chance;
		}

		// Token: 0x02000821 RID: 2081
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnGameItemProblem : Result.CastRitualProblem
		{
			// Token: 0x060026BC RID: 9916 RVA: 0x000825C7 File Offset: 0x000807C7
			[JsonConstructor]
			protected CastRitualOnGameItemProblem()
			{
			}

			// Token: 0x060026BD RID: 9917 RVA: 0x000825CF File Offset: 0x000807CF
			public CastRitualOnGameItemProblem(ConfigRef ritual, GameItem gameItem) : base(ritual)
			{
				this.GameItemId = gameItem.Id;
			}

			// Token: 0x17000535 RID: 1333
			// (get) Token: 0x060026BE RID: 9918 RVA: 0x000825E4 File Offset: 0x000807E4
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".GameItem";
				}
			}

			// Token: 0x17000536 RID: 1334
			// (get) Token: 0x060026BF RID: 9919 RVA: 0x000825F6 File Offset: 0x000807F6
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000537 RID: 1335
			// (get) Token: 0x060026C0 RID: 9920 RVA: 0x00082608 File Offset: 0x00080808
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" on {0}", this.GameItemId);
				}
			}

			// Token: 0x040011A2 RID: 4514
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public Identifier GameItemId;
		}

		// Token: 0x02000822 RID: 2082
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnWrongTypeOfItemProblem : Result.CastRitualOnGameItemProblem
		{
			// Token: 0x060026C1 RID: 9921 RVA: 0x0008262A File Offset: 0x0008082A
			[JsonConstructor]
			protected CastRitualOnWrongTypeOfItemProblem()
			{
			}

			// Token: 0x060026C2 RID: 9922 RVA: 0x00082632 File Offset: 0x00080832
			public CastRitualOnWrongTypeOfItemProblem(ConfigRef ritual, GameItem gameItem) : base(ritual, gameItem)
			{
			}

			// Token: 0x17000538 RID: 1336
			// (get) Token: 0x060026C3 RID: 9923 RVA: 0x0008263C File Offset: 0x0008083C
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".WrongType";
				}
			}

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x060026C4 RID: 9924 RVA: 0x0008264E File Offset: 0x0008084E
			public override string DebugString
			{
				get
				{
					return base.DebugString + " it is the wrong type of item";
				}
			}
		}

		// Token: 0x02000823 RID: 2083
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnBanishedItemProblem : Result.CastRitualOnGameItemProblem
		{
			// Token: 0x060026C5 RID: 9925 RVA: 0x00082660 File Offset: 0x00080860
			[JsonConstructor]
			protected CastRitualOnBanishedItemProblem()
			{
			}

			// Token: 0x060026C6 RID: 9926 RVA: 0x00082668 File Offset: 0x00080868
			public CastRitualOnBanishedItemProblem(ConfigRef ritual, GameItem gameItem) : base(ritual, gameItem)
			{
			}

			// Token: 0x1700053A RID: 1338
			// (get) Token: 0x060026C7 RID: 9927 RVA: 0x00082672 File Offset: 0x00080872
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Banished";
				}
			}

			// Token: 0x1700053B RID: 1339
			// (get) Token: 0x060026C8 RID: 9928 RVA: 0x00082684 File Offset: 0x00080884
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it has been banished";
				}
			}
		}

		// Token: 0x02000824 RID: 2084
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CannotBeAffectedByRitualsProblem : Result.CastRitualOnGameItemProblem
		{
			// Token: 0x060026C9 RID: 9929 RVA: 0x00082696 File Offset: 0x00080896
			[JsonConstructor]
			protected CannotBeAffectedByRitualsProblem()
			{
			}

			// Token: 0x060026CA RID: 9930 RVA: 0x0008269E File Offset: 0x0008089E
			public CannotBeAffectedByRitualsProblem(ConfigRef ritual, GameItem gameItem) : base(ritual, gameItem)
			{
			}

			// Token: 0x1700053C RID: 1340
			// (get) Token: 0x060026CB RID: 9931 RVA: 0x000826A8 File Offset: 0x000808A8
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it cannot be affected by rituals";
				}
			}
		}

		// Token: 0x02000825 RID: 2085
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CannotBeStolenByRitualsProblem : Result.CastRitualOnGameItemProblem
		{
			// Token: 0x060026CC RID: 9932 RVA: 0x000826BA File Offset: 0x000808BA
			[JsonConstructor]
			protected CannotBeStolenByRitualsProblem()
			{
			}

			// Token: 0x060026CD RID: 9933 RVA: 0x000826C2 File Offset: 0x000808C2
			public CannotBeStolenByRitualsProblem(ConfigRef ritual, GameItem gameItem) : base(ritual, gameItem)
			{
			}

			// Token: 0x1700053D RID: 1341
			// (get) Token: 0x060026CE RID: 9934 RVA: 0x000826CC File Offset: 0x000808CC
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it cannot be stolen by rituals";
				}
			}
		}

		// Token: 0x02000826 RID: 2086
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class AlreadyAffectedByRitualProblem : Result.CastRitualOnGameItemProblem
		{
			// Token: 0x060026CF RID: 9935 RVA: 0x000826DE File Offset: 0x000808DE
			[JsonConstructor]
			protected AlreadyAffectedByRitualProblem()
			{
			}

			// Token: 0x060026D0 RID: 9936 RVA: 0x000826E6 File Offset: 0x000808E6
			public AlreadyAffectedByRitualProblem(ConfigRef ritual, GameItem gameItem) : base(ritual, gameItem)
			{
			}

			// Token: 0x1700053E RID: 1342
			// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000826F0 File Offset: 0x000808F0
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".AlreadyAffected";
				}
			}

			// Token: 0x1700053F RID: 1343
			// (get) Token: 0x060026D2 RID: 9938 RVA: 0x00082702 File Offset: 0x00080902
			public override string PreviewLocKey
			{
				get
				{
					return this.LocKey;
				}
			}

			// Token: 0x17000540 RID: 1344
			// (get) Token: 0x060026D3 RID: 9939 RVA: 0x0008270A File Offset: 0x0008090A
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because is already being affected";
				}
			}
		}

		// Token: 0x02000827 RID: 2087
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class TargetBlocksRitualMaskingProblem : Result.CastRitualOnGameItemProblem
		{
			// Token: 0x060026D4 RID: 9940 RVA: 0x0008271C File Offset: 0x0008091C
			[JsonConstructor]
			protected TargetBlocksRitualMaskingProblem()
			{
			}

			// Token: 0x060026D5 RID: 9941 RVA: 0x00082724 File Offset: 0x00080924
			public TargetBlocksRitualMaskingProblem(ConfigRef ritual, GameItem gameItem) : base(ritual, gameItem)
			{
			}

			// Token: 0x17000541 RID: 1345
			// (get) Token: 0x060026D6 RID: 9942 RVA: 0x0008272E File Offset: 0x0008092E
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".TargetBlocksRitualMasking";
				}
			}

			// Token: 0x17000542 RID: 1346
			// (get) Token: 0x060026D7 RID: 9943 RVA: 0x00082740 File Offset: 0x00080940
			public override string PreviewLocKey
			{
				get
				{
					return this.LocKey;
				}
			}

			// Token: 0x17000543 RID: 1347
			// (get) Token: 0x060026D8 RID: 9944 RVA: 0x00082748 File Offset: 0x00080948
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it blocks ritual masking";
				}
			}
		}

		// Token: 0x02000828 RID: 2088
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnPlayerItemProblem : Result.CastRitualOnPlayerProblem
		{
			// Token: 0x060026D9 RID: 9945 RVA: 0x0008275A File Offset: 0x0008095A
			[JsonConstructor]
			protected CastRitualOnPlayerItemProblem()
			{
			}

			// Token: 0x060026DA RID: 9946 RVA: 0x00082762 File Offset: 0x00080962
			public CastRitualOnPlayerItemProblem(ConfigRef ritual, int targetPlayerId, GameItem gameItem) : base(ritual, targetPlayerId)
			{
				this.GameItemId = gameItem.Id;
			}

			// Token: 0x17000544 RID: 1348
			// (get) Token: 0x060026DB RID: 9947 RVA: 0x00082778 File Offset: 0x00080978
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".GameItem";
				}
			}

			// Token: 0x17000545 RID: 1349
			// (get) Token: 0x060026DC RID: 9948 RVA: 0x0008278A File Offset: 0x0008098A
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000546 RID: 1350
			// (get) Token: 0x060026DD RID: 9949 RVA: 0x0008279C File Offset: 0x0008099C
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format("'s {0}", this.GameItemId);
				}
			}

			// Token: 0x040011A3 RID: 4515
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public Identifier GameItemId;
		}

		// Token: 0x02000829 RID: 2089
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnStolenItemProblem : Result.CastRitualOnPlayerItemProblem
		{
			// Token: 0x060026DE RID: 9950 RVA: 0x000827BE File Offset: 0x000809BE
			[JsonConstructor]
			protected CastRitualOnStolenItemProblem()
			{
			}

			// Token: 0x060026DF RID: 9951 RVA: 0x000827C6 File Offset: 0x000809C6
			public CastRitualOnStolenItemProblem(ConfigRef ritual, int targetPlayerID, GameItem gameItem) : base(ritual, targetPlayerID, gameItem)
			{
			}

			// Token: 0x17000547 RID: 1351
			// (get) Token: 0x060026E0 RID: 9952 RVA: 0x000827D1 File Offset: 0x000809D1
			public override string LocKey
			{
				get
				{
					if (this.TargetPlayerID != -1)
					{
						return this.LocKeyScope + ".Stolen";
					}
					return this.LocKeyScope + ".Stolen.Neutral";
				}
			}

			// Token: 0x17000548 RID: 1352
			// (get) Token: 0x060026E1 RID: 9953 RVA: 0x000827FD File Offset: 0x000809FD
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it has changed hands";
				}
			}
		}

		// Token: 0x0200082A RID: 2090
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RitualSchemeAlreadyCompletedProblem : Result.CastRitualOnPlayerItemProblem
		{
			// Token: 0x060026E2 RID: 9954 RVA: 0x0008280F File Offset: 0x00080A0F
			[JsonConstructor]
			protected RitualSchemeAlreadyCompletedProblem()
			{
			}

			// Token: 0x060026E3 RID: 9955 RVA: 0x00082817 File Offset: 0x00080A17
			public RitualSchemeAlreadyCompletedProblem(ConfigRef ritual, int targetPlayerID, GameItem gameItem) : base(ritual, targetPlayerID, gameItem)
			{
			}

			// Token: 0x17000549 RID: 1353
			// (get) Token: 0x060026E4 RID: 9956 RVA: 0x00082822 File Offset: 0x00080A22
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Scheme.Completed";
				}
			}

			// Token: 0x1700054A RID: 1354
			// (get) Token: 0x060026E5 RID: 9957 RVA: 0x00082834 File Offset: 0x00080A34
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the scheme is already completed";
				}
			}
		}

		// Token: 0x0200082B RID: 2091
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class RitualSchemeAlreadyDiscardedProblem : Result.CastRitualOnPlayerItemProblem
		{
			// Token: 0x060026E6 RID: 9958 RVA: 0x00082846 File Offset: 0x00080A46
			[JsonConstructor]
			protected RitualSchemeAlreadyDiscardedProblem()
			{
			}

			// Token: 0x060026E7 RID: 9959 RVA: 0x0008284E File Offset: 0x00080A4E
			public RitualSchemeAlreadyDiscardedProblem(ConfigRef ritual, int targetPlayerID, GameItem gameItem) : base(ritual, targetPlayerID, gameItem)
			{
			}

			// Token: 0x1700054B RID: 1355
			// (get) Token: 0x060026E8 RID: 9960 RVA: 0x00082859 File Offset: 0x00080A59
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Scheme.Discarded";
				}
			}

			// Token: 0x1700054C RID: 1356
			// (get) Token: 0x060026E9 RID: 9961 RVA: 0x0008286B File Offset: 0x00080A6B
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the scheme is already discarded";
				}
			}
		}

		// Token: 0x0200082C RID: 2092
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnItemOwnershipProblem : Result.CastRitualOnPlayerItemProblem
		{
			// Token: 0x060026EA RID: 9962 RVA: 0x0008287D File Offset: 0x00080A7D
			[JsonConstructor]
			protected CastRitualOnItemOwnershipProblem()
			{
			}

			// Token: 0x060026EB RID: 9963 RVA: 0x00082885 File Offset: 0x00080A85
			public CastRitualOnItemOwnershipProblem(ConfigRef ritual, int itemOwnerPlayerID, GameItem item) : base(ritual, itemOwnerPlayerID, item)
			{
			}

			// Token: 0x1700054D RID: 1357
			// (get) Token: 0x060026EC RID: 9964 RVA: 0x00082890 File Offset: 0x00080A90
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ((this.TargetPlayerID == -1) ? ".InvalidOwnership.Neutral" : ".InvalidOwnership");
				}
			}

			// Token: 0x1700054E RID: 1358
			// (get) Token: 0x060026ED RID: 9965 RVA: 0x000828B2 File Offset: 0x00080AB2
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" it is owned by {0}", this.TargetPlayerID);
				}
			}
		}

		// Token: 0x0200082D RID: 2093
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualItemControllingGamePieceProblem : Result.CastRitualOnPlayerItemProblem
		{
			// Token: 0x060026EE RID: 9966 RVA: 0x000828D4 File Offset: 0x00080AD4
			[JsonConstructor]
			protected CastRitualItemControllingGamePieceProblem()
			{
			}

			// Token: 0x060026EF RID: 9967 RVA: 0x000828E3 File Offset: 0x00080AE3
			public CastRitualItemControllingGamePieceProblem(ConfigRef ritual, int targetPlayerID, GameItem gameItem, GamePieceCategory desiredOwnerCategory = GamePieceCategory.None) : base(ritual, targetPlayerID, gameItem)
			{
				this.DesiredOwnerCategory = desiredOwnerCategory;
			}

			// Token: 0x1700054F RID: 1359
			// (get) Token: 0x060026F0 RID: 9968 RVA: 0x000828FD File Offset: 0x00080AFD
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ((this.DesiredOwnerCategory == GamePieceCategory.None) ? ".MustBeInVault" : ".MustBeAttached");
				}
			}

			// Token: 0x17000550 RID: 1360
			// (get) Token: 0x060026F1 RID: 9969 RVA: 0x0008291F File Offset: 0x00080B1F
			public override string DebugString
			{
				get
				{
					return base.DebugString + ((this.DesiredOwnerCategory == GamePieceCategory.None) ? " because it is not in the vault" : string.Format(" because it is not attached to a {0}", this.DesiredOwnerCategory));
				}
			}

			// Token: 0x040011A4 RID: 4516
			[JsonProperty]
			[DefaultValue(GamePieceCategory.None)]
			[BindableValue("gamepiece_category", BindingOption.None)]
			public GamePieceCategory DesiredOwnerCategory = GamePieceCategory.None;
		}

		// Token: 0x0200082E RID: 2094
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnInvalidHexProblem : Result.CastRitualProblem
		{
			// Token: 0x060026F2 RID: 9970 RVA: 0x00082951 File Offset: 0x00080B51
			[JsonConstructor]
			protected CastRitualOnInvalidHexProblem()
			{
			}

			// Token: 0x060026F3 RID: 9971 RVA: 0x00082959 File Offset: 0x00080B59
			public CastRitualOnInvalidHexProblem(ConfigRef ritual, HexCoord targetHex) : base(ritual)
			{
				this.TargetHex = targetHex;
			}

			// Token: 0x17000551 RID: 1361
			// (get) Token: 0x060026F4 RID: 9972 RVA: 0x00082969 File Offset: 0x00080B69
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".Hex";
				}
			}

			// Token: 0x17000552 RID: 1362
			// (get) Token: 0x060026F5 RID: 9973 RVA: 0x0008297B File Offset: 0x00080B7B
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000553 RID: 1363
			// (get) Token: 0x060026F6 RID: 9974 RVA: 0x0008298D File Offset: 0x00080B8D
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" on hex {0}", this.TargetHex);
				}
			}

			// Token: 0x040011A5 RID: 4517
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public HexCoord TargetHex;
		}

		// Token: 0x0200082F RID: 2095
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnWrongTerrainProblem : Result.CastRitualOnInvalidHexProblem
		{
			// Token: 0x060026F7 RID: 9975 RVA: 0x000829AF File Offset: 0x00080BAF
			[JsonConstructor]
			protected CastRitualOnWrongTerrainProblem()
			{
			}

			// Token: 0x060026F8 RID: 9976 RVA: 0x000829B7 File Offset: 0x00080BB7
			public CastRitualOnWrongTerrainProblem(ConfigRef ritual, HexCoord targetHex, TerrainStaticData terrainType) : base(ritual, targetHex)
			{
				this.TerrainTypeConfig = terrainType.ConfigRef;
			}

			// Token: 0x17000554 RID: 1364
			// (get) Token: 0x060026F9 RID: 9977 RVA: 0x000829CD File Offset: 0x00080BCD
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".WrongTerrain";
				}
			}

			// Token: 0x17000555 RID: 1365
			// (get) Token: 0x060026FA RID: 9978 RVA: 0x000829DF File Offset: 0x00080BDF
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because {0} is not a valid terrain type", this.TerrainTypeConfig);
				}
			}

			// Token: 0x040011A6 RID: 4518
			[JsonProperty]
			[BindableValue("terrain_type", BindingOption.None)]
			public ConfigRef TerrainTypeConfig;
		}

		// Token: 0x02000830 RID: 2096
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualWithinAuraProblem : Result.CastRitualOnInvalidHexProblem
		{
			// Token: 0x060026FB RID: 9979 RVA: 0x000829FC File Offset: 0x00080BFC
			[JsonConstructor]
			protected CastRitualWithinAuraProblem()
			{
			}

			// Token: 0x060026FC RID: 9980 RVA: 0x00082A04 File Offset: 0x00080C04
			public CastRitualWithinAuraProblem(ConfigRef ritual, HexCoord targetHex, string aura) : base(ritual, targetHex)
			{
				this.AuraId = aura;
			}

			// Token: 0x17000556 RID: 1366
			// (get) Token: 0x060026FD RID: 9981 RVA: 0x00082A15 File Offset: 0x00080C15
			public override string LocKey
			{
				get
				{
					string locKeyScope = this.LocKeyScope;
					string str = ".";
					string auraId = this.AuraId;
					return locKeyScope + str + ((auraId != null) ? auraId.Replace(" ", string.Empty) : null);
				}
			}

			// Token: 0x17000557 RID: 1367
			// (get) Token: 0x060026FE RID: 9982 RVA: 0x00082A43 File Offset: 0x00080C43
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is within the " + this.AuraId + " aura";
				}
			}

			// Token: 0x040011A7 RID: 4519
			[JsonProperty]
			public string AuraId;
		}

		// Token: 0x02000831 RID: 2097
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnOccupiedHexProblem : Result.CastRitualOnInvalidHexProblem
		{
			// Token: 0x060026FF RID: 9983 RVA: 0x00082A60 File Offset: 0x00080C60
			[JsonConstructor]
			protected CastRitualOnOccupiedHexProblem()
			{
			}

			// Token: 0x06002700 RID: 9984 RVA: 0x00082A68 File Offset: 0x00080C68
			public CastRitualOnOccupiedHexProblem(ConfigRef ritual, HexCoord targetHex, Identifier occupant) : base(ritual, targetHex)
			{
				this.Occupant = occupant;
			}

			// Token: 0x17000558 RID: 1368
			// (get) Token: 0x06002701 RID: 9985 RVA: 0x00082A79 File Offset: 0x00080C79
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Occupied";
				}
			}

			// Token: 0x17000559 RID: 1369
			// (get) Token: 0x06002702 RID: 9986 RVA: 0x00082A8B File Offset: 0x00080C8B
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because {0} is in the way", this.Occupant);
				}
			}

			// Token: 0x040011A8 RID: 4520
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public Identifier Occupant;
		}

		// Token: 0x02000832 RID: 2098
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnHexNotNextToFixtureProblem : Result.CastRitualOnInvalidHexProblem
		{
			// Token: 0x06002703 RID: 9987 RVA: 0x00082AAD File Offset: 0x00080CAD
			[JsonConstructor]
			protected CastRitualOnHexNotNextToFixtureProblem()
			{
			}

			// Token: 0x06002704 RID: 9988 RVA: 0x00082AB5 File Offset: 0x00080CB5
			public CastRitualOnHexNotNextToFixtureProblem(ConfigRef ritual, HexCoord targetHex) : base(ritual, targetHex)
			{
			}

			// Token: 0x1700055A RID: 1370
			// (get) Token: 0x06002705 RID: 9989 RVA: 0x00082ABF File Offset: 0x00080CBF
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NotNextToFixture";
				}
			}

			// Token: 0x1700055B RID: 1371
			// (get) Token: 0x06002706 RID: 9990 RVA: 0x00082AD1 File Offset: 0x00080CD1
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is not next to a friendly fixture";
				}
			}
		}

		// Token: 0x02000833 RID: 2099
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnHexNextToFixtureProblem : Result.CastRitualOnInvalidHexProblem
		{
			// Token: 0x06002707 RID: 9991 RVA: 0x00082AE3 File Offset: 0x00080CE3
			[JsonConstructor]
			protected CastRitualOnHexNextToFixtureProblem()
			{
			}

			// Token: 0x06002708 RID: 9992 RVA: 0x00082AEB File Offset: 0x00080CEB
			public CastRitualOnHexNextToFixtureProblem(ConfigRef ritual, HexCoord targetHex) : base(ritual, targetHex)
			{
			}

			// Token: 0x1700055C RID: 1372
			// (get) Token: 0x06002709 RID: 9993 RVA: 0x00082AF5 File Offset: 0x00080CF5
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NextToFixture";
				}
			}

			// Token: 0x1700055D RID: 1373
			// (get) Token: 0x0600270A RID: 9994 RVA: 0x00082B07 File Offset: 0x00080D07
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is next to a fixture";
				}
			}
		}

		// Token: 0x02000834 RID: 2100
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualTargetIsBorderProblem : Result.CastRitualOnInvalidHexProblem
		{
			// Token: 0x0600270B RID: 9995 RVA: 0x00082B19 File Offset: 0x00080D19
			[JsonConstructor]
			protected CastRitualTargetIsBorderProblem()
			{
			}

			// Token: 0x0600270C RID: 9996 RVA: 0x00082B21 File Offset: 0x00080D21
			public CastRitualTargetIsBorderProblem(ConfigRef ritual, HexCoord targetHex, int borderPlayerID, bool borderRequired = true) : base(ritual, targetHex)
			{
				this.BorderRequired = borderRequired;
				this.BorderPlayerID = borderPlayerID;
			}

			// Token: 0x1700055E RID: 1374
			// (get) Token: 0x0600270D RID: 9997 RVA: 0x00082B3A File Offset: 0x00080D3A
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + (this.BorderRequired ? ".Border.Required" : ".Border.Forbidden");
				}
			}

			// Token: 0x1700055F RID: 1375
			// (get) Token: 0x0600270E RID: 9998 RVA: 0x00082B5C File Offset: 0x00080D5C
			public override string DebugString
			{
				get
				{
					return base.DebugString + (this.BorderRequired ? string.Format(" because it must border {0}", this.BorderPlayerID) : string.Format(" because it cannot border {0}", this.BorderPlayerID));
				}
			}

			// Token: 0x040011A9 RID: 4521
			[JsonProperty]
			public bool BorderRequired;

			// Token: 0x040011AA RID: 4522
			[JsonProperty]
			[BindableValue("affected_name", BindingOption.IntPlayerId)]
			[DefaultValue(-2147483648)]
			public int BorderPlayerID;
		}

		// Token: 0x02000835 RID: 2101
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CastRitualOnHexOwnershipProblem : Result.CastRitualOnInvalidHexProblem
		{
			// Token: 0x0600270F RID: 9999 RVA: 0x00082BA8 File Offset: 0x00080DA8
			[JsonConstructor]
			protected CastRitualOnHexOwnershipProblem()
			{
			}

			// Token: 0x06002710 RID: 10000 RVA: 0x00082BB0 File Offset: 0x00080DB0
			public CastRitualOnHexOwnershipProblem(ConfigRef ritual, HexCoord targetHex, int hexOwnerPlayerID) : base(ritual, targetHex)
			{
				this.HexOwnerPlayerID = hexOwnerPlayerID;
			}

			// Token: 0x17000560 RID: 1376
			// (get) Token: 0x06002711 RID: 10001 RVA: 0x00082BC1 File Offset: 0x00080DC1
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ((this.HexOwnerPlayerID == -1) ? ".InvalidOwnership.Neutral" : ".InvalidOwnership");
				}
			}

			// Token: 0x17000561 RID: 1377
			// (get) Token: 0x06002712 RID: 10002 RVA: 0x00082BE3 File Offset: 0x00080DE3
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" it is owned by {0}", this.HexOwnerPlayerID);
				}
			}

			// Token: 0x040011AB RID: 4523
			[JsonProperty]
			[BindableValue("affected_name", BindingOption.IntPlayerId)]
			[DefaultValue(-2147483648)]
			public int HexOwnerPlayerID;
		}

		// Token: 0x02000836 RID: 2102
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class CreateStratagemProblem : Problem
		{
			// Token: 0x06002713 RID: 10003 RVA: 0x00082C05 File Offset: 0x00080E05
			[JsonConstructor]
			protected CreateStratagemProblem()
			{
			}

			// Token: 0x06002714 RID: 10004 RVA: 0x00082C0D File Offset: 0x00080E0D
			public CreateStratagemProblem(ConfigRef createStratagemOrder, Identifier destinationGamePiece)
			{
				this.CreateStratagemOrder = createStratagemOrder;
				this.DestinationGamePiece = destinationGamePiece;
			}

			// Token: 0x17000562 RID: 1378
			// (get) Token: 0x06002715 RID: 10005 RVA: 0x00082C23 File Offset: 0x00080E23
			protected override string LocKeyScope
			{
				get
				{
					return "Result.CreateStratagem";
				}
			}

			// Token: 0x17000563 RID: 1379
			// (get) Token: 0x06002716 RID: 10006 RVA: 0x00082C2A File Offset: 0x00080E2A
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000564 RID: 1380
			// (get) Token: 0x06002717 RID: 10007 RVA: 0x00082C3C File Offset: 0x00080E3C
			public override string DebugString
			{
				get
				{
					return string.Format("Stratagem could not be created for {0}", this.DestinationGamePiece);
				}
			}

			// Token: 0x040011AC RID: 4524
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public ConfigRef CreateStratagemOrder;

			// Token: 0x040011AD RID: 4525
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public Identifier DestinationGamePiece;
		}

		// Token: 0x02000837 RID: 2103
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class StratagemsNotUnlockedProblem : Result.CreateStratagemProblem
		{
			// Token: 0x06002718 RID: 10008 RVA: 0x00082C53 File Offset: 0x00080E53
			public StratagemsNotUnlockedProblem(ConfigRef createStratagemOrder, Identifier destinationGamePiece) : base(createStratagemOrder, destinationGamePiece)
			{
			}

			// Token: 0x17000565 RID: 1381
			// (get) Token: 0x06002719 RID: 10009 RVA: 0x00082C5D File Offset: 0x00080E5D
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".StratagemsUnavailable";
				}
			}

			// Token: 0x17000566 RID: 1382
			// (get) Token: 0x0600271A RID: 10010 RVA: 0x00082C6F File Offset: 0x00080E6F
			public override string PreviewLocKey
			{
				get
				{
					return "Orders.CreateStratagem.Locked";
				}
			}

			// Token: 0x17000567 RID: 1383
			// (get) Token: 0x0600271B RID: 10011 RVA: 0x00082C76 File Offset: 0x00080E76
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because stratagems have not been unlocked";
				}
			}
		}

		// Token: 0x02000838 RID: 2104
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class StratagemsBlockedProblem : Result.CreateStratagemProblem
		{
			// Token: 0x0600271C RID: 10012 RVA: 0x00082C88 File Offset: 0x00080E88
			public StratagemsBlockedProblem(ConfigRef createStratagemOrder, Identifier destinationGamePiece) : base(createStratagemOrder, destinationGamePiece)
			{
			}

			// Token: 0x17000568 RID: 1384
			// (get) Token: 0x0600271D RID: 10013 RVA: 0x00082C92 File Offset: 0x00080E92
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".StratagemsBlockedProblem";
				}
			}

			// Token: 0x17000569 RID: 1385
			// (get) Token: 0x0600271E RID: 10014 RVA: 0x00082CA4 File Offset: 0x00080EA4
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the use of stratagems is blocked";
				}
			}
		}

		// Token: 0x02000839 RID: 2105
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class TacticUnavailableProblem : Result.CreateStratagemProblem
		{
			// Token: 0x0600271F RID: 10015 RVA: 0x00082CB6 File Offset: 0x00080EB6
			public TacticUnavailableProblem(ConfigRef createStratagemOrder, Identifier destinationGamePiece) : base(createStratagemOrder, destinationGamePiece)
			{
			}

			// Token: 0x1700056A RID: 1386
			// (get) Token: 0x06002720 RID: 10016 RVA: 0x00082CC0 File Offset: 0x00080EC0
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".TacticUnavailable";
				}
			}

			// Token: 0x1700056B RID: 1387
			// (get) Token: 0x06002721 RID: 10017 RVA: 0x00082CD2 File Offset: 0x00080ED2
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because one of the chosen tactics has not been unlocked";
				}
			}
		}

		// Token: 0x0200083A RID: 2106
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class NotEnoughTacticsSlotsProblem : Result.CreateStratagemProblem
		{
			// Token: 0x06002722 RID: 10018 RVA: 0x00082CE4 File Offset: 0x00080EE4
			public NotEnoughTacticsSlotsProblem(ConfigRef createStratagemOrder, Identifier destinationGamePiece) : base(createStratagemOrder, destinationGamePiece)
			{
			}

			// Token: 0x1700056C RID: 1388
			// (get) Token: 0x06002723 RID: 10019 RVA: 0x00082CEE File Offset: 0x00080EEE
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NotEnoughTacticsSlots";
				}
			}

			// Token: 0x1700056D RID: 1389
			// (get) Token: 0x06002724 RID: 10020 RVA: 0x00082D00 File Offset: 0x00080F00
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because not enough tactic slots have been unlocked";
				}
			}
		}

		// Token: 0x0200083B RID: 2107
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class StratagemTargetBanishedProblem : Result.CreateStratagemProblem
		{
			// Token: 0x06002725 RID: 10021 RVA: 0x00082D12 File Offset: 0x00080F12
			public StratagemTargetBanishedProblem(ConfigRef createStratagemOrder, Identifier destinationGamePiece) : base(createStratagemOrder, destinationGamePiece)
			{
			}

			// Token: 0x1700056E RID: 1390
			// (get) Token: 0x06002726 RID: 10022 RVA: 0x00082D1C File Offset: 0x00080F1C
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".GamePieceBanished";
				}
			}

			// Token: 0x1700056F RID: 1391
			// (get) Token: 0x06002727 RID: 10023 RVA: 0x00082D2E File Offset: 0x00080F2E
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the target Game Piece is banished";
				}
			}
		}

		// Token: 0x0200083C RID: 2108
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class StratagemTargetStolenProblem : Result.CreateStratagemProblem
		{
			// Token: 0x06002728 RID: 10024 RVA: 0x00082D40 File Offset: 0x00080F40
			public StratagemTargetStolenProblem(ConfigRef createStratagemOrder, Identifier destinationGamePiece) : base(createStratagemOrder, destinationGamePiece)
			{
			}

			// Token: 0x17000570 RID: 1392
			// (get) Token: 0x06002729 RID: 10025 RVA: 0x00082D4A File Offset: 0x00080F4A
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".GamePieceStolen";
				}
			}

			// Token: 0x17000571 RID: 1393
			// (get) Token: 0x0600272A RID: 10026 RVA: 0x00082D5C File Offset: 0x00080F5C
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the target Game Piece belong to another player";
				}
			}
		}

		// Token: 0x0200083D RID: 2109
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvokeManuscriptProblem : Problem
		{
			// Token: 0x0600272B RID: 10027 RVA: 0x00082D6E File Offset: 0x00080F6E
			[JsonConstructor]
			protected InvokeManuscriptProblem()
			{
			}

			// Token: 0x0600272C RID: 10028 RVA: 0x00082D76 File Offset: 0x00080F76
			public InvokeManuscriptProblem(Identifier manuscriptId)
			{
				this.ManuscriptId = manuscriptId;
			}

			// Token: 0x17000572 RID: 1394
			// (get) Token: 0x0600272D RID: 10029 RVA: 0x00082D85 File Offset: 0x00080F85
			protected override string LocKeyScope
			{
				get
				{
					return "Result.InvokeManuscript";
				}
			}

			// Token: 0x17000573 RID: 1395
			// (get) Token: 0x0600272E RID: 10030 RVA: 0x00082D8C File Offset: 0x00080F8C
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000574 RID: 1396
			// (get) Token: 0x0600272F RID: 10031 RVA: 0x00082D9E File Offset: 0x00080F9E
			public override string DebugString
			{
				get
				{
					return string.Format("Manuscript {0} cannot be invoked", this.ManuscriptId);
				}
			}

			// Token: 0x040011AE RID: 4526
			[JsonProperty]
			[BindableValue("manuscript", BindingOption.None)]
			public Identifier ManuscriptId;
		}

		// Token: 0x0200083E RID: 2110
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvokeIncompleteManuscriptProblem : Result.InvokeManuscriptProblem
		{
			// Token: 0x06002730 RID: 10032 RVA: 0x00082DB5 File Offset: 0x00080FB5
			[JsonConstructor]
			protected InvokeIncompleteManuscriptProblem()
			{
			}

			// Token: 0x06002731 RID: 10033 RVA: 0x00082DBD File Offset: 0x00080FBD
			public InvokeIncompleteManuscriptProblem(Identifier manuscriptId) : base(manuscriptId)
			{
			}

			// Token: 0x17000575 RID: 1397
			// (get) Token: 0x06002732 RID: 10034 RVA: 0x00082DC6 File Offset: 0x00080FC6
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Incomplete";
				}
			}

			// Token: 0x17000576 RID: 1398
			// (get) Token: 0x06002733 RID: 10035 RVA: 0x00082DD8 File Offset: 0x00080FD8
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is incomplete";
				}
			}
		}

		// Token: 0x0200083F RID: 2111
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvokeManuscriptOnGameItemProblem : Result.InvokeManuscriptProblem
		{
			// Token: 0x06002734 RID: 10036 RVA: 0x00082DEA File Offset: 0x00080FEA
			[JsonConstructor]
			protected InvokeManuscriptOnGameItemProblem()
			{
			}

			// Token: 0x06002735 RID: 10037 RVA: 0x00082DF2 File Offset: 0x00080FF2
			public InvokeManuscriptOnGameItemProblem(Identifier manuscriptId, Identifier destinationGameItem) : base(manuscriptId)
			{
				this.DestinationGameItem = destinationGameItem;
			}

			// Token: 0x17000577 RID: 1399
			// (get) Token: 0x06002736 RID: 10038 RVA: 0x00082E02 File Offset: 0x00081002
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".OnGameItem";
				}
			}

			// Token: 0x17000578 RID: 1400
			// (get) Token: 0x06002737 RID: 10039 RVA: 0x00082E14 File Offset: 0x00081014
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000579 RID: 1401
			// (get) Token: 0x06002738 RID: 10040 RVA: 0x00082E26 File Offset: 0x00081026
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" on GameItem {0}", this.DestinationGameItem);
				}
			}

			// Token: 0x040011AF RID: 4527
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public Identifier DestinationGameItem;
		}

		// Token: 0x02000840 RID: 2112
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvokeManuscriptOnBanishedGameItemProblem : Result.InvokeManuscriptOnGameItemProblem
		{
			// Token: 0x06002739 RID: 10041 RVA: 0x00082E48 File Offset: 0x00081048
			[JsonConstructor]
			protected InvokeManuscriptOnBanishedGameItemProblem()
			{
			}

			// Token: 0x0600273A RID: 10042 RVA: 0x00082E50 File Offset: 0x00081050
			public InvokeManuscriptOnBanishedGameItemProblem(Identifier manuscriptId, Identifier destinationGameItem) : base(manuscriptId, destinationGameItem)
			{
			}

			// Token: 0x1700057A RID: 1402
			// (get) Token: 0x0600273B RID: 10043 RVA: 0x00082E5A File Offset: 0x0008105A
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Banished";
				}
			}

			// Token: 0x1700057B RID: 1403
			// (get) Token: 0x0600273C RID: 10044 RVA: 0x00082E6C File Offset: 0x0008106C
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is banished";
				}
			}
		}

		// Token: 0x02000841 RID: 2113
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvokeManuscriptOnStolenGameItemProblem : Result.InvokeManuscriptOnGameItemProblem
		{
			// Token: 0x0600273D RID: 10045 RVA: 0x00082E7E File Offset: 0x0008107E
			[JsonConstructor]
			protected InvokeManuscriptOnStolenGameItemProblem()
			{
			}

			// Token: 0x0600273E RID: 10046 RVA: 0x00082E86 File Offset: 0x00081086
			public InvokeManuscriptOnStolenGameItemProblem(Identifier manuscriptId, Identifier destinationGameItem) : base(manuscriptId, destinationGameItem)
			{
			}

			// Token: 0x1700057C RID: 1404
			// (get) Token: 0x0600273F RID: 10047 RVA: 0x00082E90 File Offset: 0x00081090
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Stolen";
				}
			}

			// Token: 0x1700057D RID: 1405
			// (get) Token: 0x06002740 RID: 10048 RVA: 0x00082EA2 File Offset: 0x000810A2
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it has been stolen";
				}
			}
		}

		// Token: 0x02000842 RID: 2114
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvokeManuscriptOnGameItemAboveCapacityProblem : Result.InvokeManuscriptOnGameItemProblem
		{
			// Token: 0x06002741 RID: 10049 RVA: 0x00082EB4 File Offset: 0x000810B4
			[JsonConstructor]
			protected InvokeManuscriptOnGameItemAboveCapacityProblem()
			{
			}

			// Token: 0x06002742 RID: 10050 RVA: 0x00082EBC File Offset: 0x000810BC
			public InvokeManuscriptOnGameItemAboveCapacityProblem(Identifier manuscriptId, Identifier destinationGameItem, int previouslyAppliedCount, int maxCapacity) : base(manuscriptId, destinationGameItem)
			{
				this.PreviouslyAppliedCount = previouslyAppliedCount;
				this.MaxCapacity = maxCapacity;
			}

			// Token: 0x1700057E RID: 1406
			// (get) Token: 0x06002743 RID: 10051 RVA: 0x00082ED5 File Offset: 0x000810D5
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".AboveCapacity";
				}
			}

			// Token: 0x1700057F RID: 1407
			// (get) Token: 0x06002744 RID: 10052 RVA: 0x00082EE7 File Offset: 0x000810E7
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because has already had {0} applied to it", this.PreviouslyAppliedCount);
				}
			}

			// Token: 0x040011B0 RID: 4528
			[JsonProperty]
			[BindableValue("value", BindingOption.None)]
			public int PreviouslyAppliedCount;

			// Token: 0x040011B1 RID: 4529
			[JsonProperty]
			[BindableValue("max_value", BindingOption.None)]
			public int MaxCapacity;
		}

		// Token: 0x02000843 RID: 2115
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ChangePraetorTechniqueViaManuscriptProblem : Result.InvokeManuscriptOnGameItemProblem
		{
			// Token: 0x06002745 RID: 10053 RVA: 0x00082F09 File Offset: 0x00081109
			[JsonConstructor]
			protected ChangePraetorTechniqueViaManuscriptProblem()
			{
			}

			// Token: 0x06002746 RID: 10054 RVA: 0x00082F11 File Offset: 0x00081111
			public ChangePraetorTechniqueViaManuscriptProblem(Identifier manuscriptId, Identifier destinationPraetor, ConfigRef<PraetorCombatMoveStaticData> targetTechnique) : base(manuscriptId, destinationPraetor)
			{
				this.TargetTechnique = targetTechnique;
			}

			// Token: 0x17000580 RID: 1408
			// (get) Token: 0x06002747 RID: 10055 RVA: 0x00082F22 File Offset: 0x00081122
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".Technique";
				}
			}

			// Token: 0x17000581 RID: 1409
			// (get) Token: 0x06002748 RID: 10056 RVA: 0x00082F34 File Offset: 0x00081134
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" to replace {0}", this.TargetTechnique);
				}
			}

			// Token: 0x040011B2 RID: 4530
			[JsonProperty]
			[BindableValue("technique", BindingOption.None)]
			public ConfigRef<PraetorCombatMoveStaticData> TargetTechnique;
		}

		// Token: 0x02000844 RID: 2116
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class DuplicateTechniqueOnPraetorProblem : Result.ChangePraetorTechniqueViaManuscriptProblem
		{
			// Token: 0x06002749 RID: 10057 RVA: 0x00082F51 File Offset: 0x00081151
			[JsonConstructor]
			protected DuplicateTechniqueOnPraetorProblem()
			{
			}

			// Token: 0x0600274A RID: 10058 RVA: 0x00082F59 File Offset: 0x00081159
			public DuplicateTechniqueOnPraetorProblem(Identifier manuscriptId, Identifier destinationPraetor, ConfigRef<PraetorCombatMoveStaticData> targetTechnique) : base(manuscriptId, destinationPraetor, targetTechnique)
			{
			}

			// Token: 0x17000582 RID: 1410
			// (get) Token: 0x0600274B RID: 10059 RVA: 0x00082F64 File Offset: 0x00081164
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Duplicate";
				}
			}

			// Token: 0x17000583 RID: 1411
			// (get) Token: 0x0600274C RID: 10060 RVA: 0x00082F76 File Offset: 0x00081176
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the Praetor already knows this technique";
				}
			}
		}

		// Token: 0x02000845 RID: 2117
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ChangePraetorTechniqueTypeProblem : Result.ChangePraetorTechniqueViaManuscriptProblem
		{
			// Token: 0x0600274D RID: 10061 RVA: 0x00082F88 File Offset: 0x00081188
			[JsonConstructor]
			protected ChangePraetorTechniqueTypeProblem()
			{
			}

			// Token: 0x0600274E RID: 10062 RVA: 0x00082F90 File Offset: 0x00081190
			public ChangePraetorTechniqueTypeProblem(Identifier manuscriptId, Identifier destinationPraetor, ConfigRef<PraetorCombatMoveStaticData> targetTechnique, ConfigRef<PraetorCombatMoveStyle> requiredStyle) : base(manuscriptId, destinationPraetor, targetTechnique)
			{
				this.RequiredStyle = requiredStyle;
			}

			// Token: 0x17000584 RID: 1412
			// (get) Token: 0x0600274F RID: 10063 RVA: 0x00082FA3 File Offset: 0x000811A3
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".TypeChanged";
				}
			}

			// Token: 0x17000585 RID: 1413
			// (get) Token: 0x06002750 RID: 10064 RVA: 0x00082FB5 File Offset: 0x000811B5
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because you must replace techniques with new ones of the same type";
				}
			}

			// Token: 0x040011B3 RID: 4531
			[JsonProperty]
			[BindableValue("style", BindingOption.None)]
			public ConfigRef<PraetorCombatMoveStyle> RequiredStyle;
		}

		// Token: 0x02000846 RID: 2118
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class InvalidTargetTechniqueProblem : Result.ChangePraetorTechniqueViaManuscriptProblem
		{
			// Token: 0x06002751 RID: 10065 RVA: 0x00082FC7 File Offset: 0x000811C7
			[JsonConstructor]
			protected InvalidTargetTechniqueProblem()
			{
			}

			// Token: 0x06002752 RID: 10066 RVA: 0x00082FCF File Offset: 0x000811CF
			public InvalidTargetTechniqueProblem(Identifier manuscriptId, Identifier destinationPraetor, ConfigRef<PraetorCombatMoveStaticData> targetTechnique) : base(manuscriptId, destinationPraetor, targetTechnique)
			{
			}

			// Token: 0x17000586 RID: 1414
			// (get) Token: 0x06002753 RID: 10067 RVA: 0x00082FDA File Offset: 0x000811DA
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Missing";
				}
			}

			// Token: 0x17000587 RID: 1415
			// (get) Token: 0x06002754 RID: 10068 RVA: 0x00082FEC File Offset: 0x000811EC
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the praetor no longer knows this technique";
				}
			}
		}

		// Token: 0x02000847 RID: 2119
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class PraetorCannotLearnTechniquesProblem : Result.ChangePraetorTechniqueViaManuscriptProblem
		{
			// Token: 0x06002755 RID: 10069 RVA: 0x00082FFE File Offset: 0x000811FE
			[JsonConstructor]
			protected PraetorCannotLearnTechniquesProblem()
			{
			}

			// Token: 0x06002756 RID: 10070 RVA: 0x00083006 File Offset: 0x00081206
			public PraetorCannotLearnTechniquesProblem(Identifier manuscriptId, Identifier destinationPraetor, ConfigRef<PraetorCombatMoveStaticData> targetTechnique) : base(manuscriptId, destinationPraetor, targetTechnique)
			{
			}

			// Token: 0x17000588 RID: 1416
			// (get) Token: 0x06002757 RID: 10071 RVA: 0x00083011 File Offset: 0x00081211
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the praetor cannot learn any techniques";
				}
			}
		}

		// Token: 0x02000848 RID: 2120
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemProblem : Problem
		{
			// Token: 0x06002758 RID: 10072 RVA: 0x00083023 File Offset: 0x00081223
			[JsonConstructor]
			protected ReassignItemProblem()
			{
			}

			// Token: 0x06002759 RID: 10073 RVA: 0x0008302B File Offset: 0x0008122B
			public ReassignItemProblem(ConfigRef reassignmentOrder, Identifier itemToReassign)
			{
				this.ReassignmentOrder = reassignmentOrder;
				this.ItemToReassign = itemToReassign;
			}

			// Token: 0x17000589 RID: 1417
			// (get) Token: 0x0600275A RID: 10074 RVA: 0x00083041 File Offset: 0x00081241
			protected override string LocKeyScope
			{
				get
				{
					return "Result.ReassignItem";
				}
			}

			// Token: 0x1700058A RID: 1418
			// (get) Token: 0x0600275B RID: 10075 RVA: 0x00083048 File Offset: 0x00081248
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x1700058B RID: 1419
			// (get) Token: 0x0600275C RID: 10076 RVA: 0x0008305A File Offset: 0x0008125A
			public override string DebugString
			{
				get
				{
					return string.Format("{0} could not be reassigned", this.ItemToReassign);
				}
			}

			// Token: 0x040011B4 RID: 4532
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public ConfigRef ReassignmentOrder;

			// Token: 0x040011B5 RID: 4533
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public Identifier ItemToReassign;
		}

		// Token: 0x02000849 RID: 2121
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignBanishedItemProblem : Result.ReassignItemProblem
		{
			// Token: 0x0600275D RID: 10077 RVA: 0x00083071 File Offset: 0x00081271
			[JsonConstructor]
			protected ReassignBanishedItemProblem()
			{
			}

			// Token: 0x0600275E RID: 10078 RVA: 0x00083079 File Offset: 0x00081279
			public ReassignBanishedItemProblem(ConfigRef reassignmentOrder, Identifier itemToReassign) : base(reassignmentOrder, itemToReassign)
			{
			}

			// Token: 0x1700058C RID: 1420
			// (get) Token: 0x0600275F RID: 10079 RVA: 0x00083083 File Offset: 0x00081283
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".Banished";
				}
			}

			// Token: 0x1700058D RID: 1421
			// (get) Token: 0x06002760 RID: 10080 RVA: 0x00083095 File Offset: 0x00081295
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it has been banished";
				}
			}
		}

		// Token: 0x0200084A RID: 2122
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignStolenItemProblem : Result.ReassignItemProblem
		{
			// Token: 0x06002761 RID: 10081 RVA: 0x000830A7 File Offset: 0x000812A7
			[JsonConstructor]
			protected ReassignStolenItemProblem()
			{
			}

			// Token: 0x06002762 RID: 10082 RVA: 0x000830AF File Offset: 0x000812AF
			public ReassignStolenItemProblem(ConfigRef reassignmentOrder, Identifier itemToReassign) : base(reassignmentOrder, itemToReassign)
			{
			}

			// Token: 0x1700058E RID: 1422
			// (get) Token: 0x06002763 RID: 10083 RVA: 0x000830B9 File Offset: 0x000812B9
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".Stolen";
				}
			}

			// Token: 0x1700058F RID: 1423
			// (get) Token: 0x06002764 RID: 10084 RVA: 0x000830CB File Offset: 0x000812CB
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it has been stolen";
				}
			}
		}

		// Token: 0x0200084B RID: 2123
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class AlreadyBeingReassignedProblem : Result.ReassignItemProblem
		{
			// Token: 0x06002765 RID: 10085 RVA: 0x000830DD File Offset: 0x000812DD
			[JsonConstructor]
			protected AlreadyBeingReassignedProblem()
			{
			}

			// Token: 0x06002766 RID: 10086 RVA: 0x000830E5 File Offset: 0x000812E5
			public AlreadyBeingReassignedProblem(ConfigRef reassignmentOrder, Identifier lockedItem) : base(reassignmentOrder, lockedItem)
			{
			}

			// Token: 0x17000590 RID: 1424
			// (get) Token: 0x06002767 RID: 10087 RVA: 0x000830EF File Offset: 0x000812EF
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".AlreadyBeingReassigned";
				}
			}

			// Token: 0x17000591 RID: 1425
			// (get) Token: 0x06002768 RID: 10088 RVA: 0x00083101 File Offset: 0x00081301
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is already being reassigned";
				}
			}
		}

		// Token: 0x0200084C RID: 2124
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignLockedItemProblem : Result.ReassignItemProblem
		{
			// Token: 0x06002769 RID: 10089 RVA: 0x00083113 File Offset: 0x00081313
			[JsonConstructor]
			protected ReassignLockedItemProblem()
			{
			}

			// Token: 0x0600276A RID: 10090 RVA: 0x0008311B File Offset: 0x0008131B
			public ReassignLockedItemProblem(ConfigRef reassignmentOrder, Identifier lockedItem, GamePiece lockingPiece) : base(reassignmentOrder, lockedItem)
			{
				this.LockingPiece = lockingPiece;
			}

			// Token: 0x17000592 RID: 1426
			// (get) Token: 0x0600276B RID: 10091 RVA: 0x00083131 File Offset: 0x00081331
			public override string PreviewLocKey
			{
				get
				{
					return base.LocKeyScope + ".LockedInPlace.Preview";
				}
			}

			// Token: 0x17000593 RID: 1427
			// (get) Token: 0x0600276C RID: 10092 RVA: 0x00083143 File Offset: 0x00081343
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it locked in place by something";
				}
			}

			// Token: 0x040011B6 RID: 4534
			[JsonProperty]
			[BindableValue("target", BindingOption.None)]
			private Identifier LockingPiece;
		}

		// Token: 0x0200084D RID: 2125
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemInVaultProblem : Result.ReassignItemProblem
		{
			// Token: 0x0600276D RID: 10093 RVA: 0x00083155 File Offset: 0x00081355
			[JsonConstructor]
			protected ReassignItemInVaultProblem()
			{
			}

			// Token: 0x0600276E RID: 10094 RVA: 0x0008315D File Offset: 0x0008135D
			public ReassignItemInVaultProblem(ConfigRef reassignmentOrder, Identifier itemToReassign) : base(reassignmentOrder, itemToReassign)
			{
			}

			// Token: 0x17000594 RID: 1428
			// (get) Token: 0x0600276F RID: 10095 RVA: 0x00083167 File Offset: 0x00081367
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".InVault";
				}
			}

			// Token: 0x17000595 RID: 1429
			// (get) Token: 0x06002770 RID: 10096 RVA: 0x00083179 File Offset: 0x00081379
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is in the vault";
				}
			}
		}

		// Token: 0x0200084E RID: 2126
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemCannotBeInVaultProblem : Result.ReassignItemProblem
		{
			// Token: 0x06002771 RID: 10097 RVA: 0x0008318B File Offset: 0x0008138B
			[JsonConstructor]
			protected ReassignItemCannotBeInVaultProblem()
			{
			}

			// Token: 0x06002772 RID: 10098 RVA: 0x00083193 File Offset: 0x00081393
			public ReassignItemCannotBeInVaultProblem(ConfigRef reassignmentOrder, Identifier itemToReassign) : base(reassignmentOrder, itemToReassign)
			{
			}

			// Token: 0x17000596 RID: 1430
			// (get) Token: 0x06002773 RID: 10099 RVA: 0x0008319D File Offset: 0x0008139D
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is not allowed to be in the vault";
				}
			}
		}

		// Token: 0x0200084F RID: 2127
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemToRitualTableProblem : Result.ReassignItemProblem
		{
			// Token: 0x06002774 RID: 10100 RVA: 0x000831AF File Offset: 0x000813AF
			[JsonConstructor]
			protected ReassignItemToRitualTableProblem()
			{
			}

			// Token: 0x06002775 RID: 10101 RVA: 0x000831B7 File Offset: 0x000813B7
			public ReassignItemToRitualTableProblem(ConfigRef reassignmentOrder, Identifier itemToReassign) : base(reassignmentOrder, itemToReassign)
			{
			}

			// Token: 0x17000597 RID: 1431
			// (get) Token: 0x06002776 RID: 10102 RVA: 0x000831C1 File Offset: 0x000813C1
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".RitualTable";
				}
			}

			// Token: 0x17000598 RID: 1432
			// (get) Token: 0x06002777 RID: 10103 RVA: 0x000831D3 File Offset: 0x000813D3
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x17000599 RID: 1433
			// (get) Token: 0x06002778 RID: 10104 RVA: 0x000831E5 File Offset: 0x000813E5
			public override string DebugString
			{
				get
				{
					return base.DebugString + " to the Ritual Table";
				}
			}
		}

		// Token: 0x02000850 RID: 2128
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemToBlockedRitualTableProblem : Result.ReassignItemToRitualTableProblem
		{
			// Token: 0x06002779 RID: 10105 RVA: 0x000831F7 File Offset: 0x000813F7
			[JsonConstructor]
			protected ReassignItemToBlockedRitualTableProblem()
			{
			}

			// Token: 0x0600277A RID: 10106 RVA: 0x000831FF File Offset: 0x000813FF
			public ReassignItemToBlockedRitualTableProblem(ConfigRef reassignmentOrder, Identifier itemToReassign) : base(reassignmentOrder, itemToReassign)
			{
			}

			// Token: 0x1700059A RID: 1434
			// (get) Token: 0x0600277B RID: 10107 RVA: 0x00083209 File Offset: 0x00081409
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".Blocked";
				}
			}

			// Token: 0x1700059B RID: 1435
			// (get) Token: 0x0600277C RID: 10108 RVA: 0x0008321B File Offset: 0x0008141B
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is blocked";
				}
			}
		}

		// Token: 0x02000851 RID: 2129
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemToFullRitualTableProblem : Result.ReassignItemToRitualTableProblem
		{
			// Token: 0x0600277D RID: 10109 RVA: 0x0008322D File Offset: 0x0008142D
			[JsonConstructor]
			protected ReassignItemToFullRitualTableProblem()
			{
			}

			// Token: 0x0600277E RID: 10110 RVA: 0x00083235 File Offset: 0x00081435
			public ReassignItemToFullRitualTableProblem(ConfigRef reassignmentOrder, Identifier itemToReassign) : base(reassignmentOrder, itemToReassign)
			{
			}

			// Token: 0x1700059C RID: 1436
			// (get) Token: 0x0600277F RID: 10111 RVA: 0x0008323F File Offset: 0x0008143F
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".Full";
				}
			}

			// Token: 0x1700059D RID: 1437
			// (get) Token: 0x06002780 RID: 10112 RVA: 0x00083251 File Offset: 0x00081451
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because there are no free slots";
				}
			}
		}

		// Token: 0x02000852 RID: 2130
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemToInvalidGamePieceProblem : Result.ReassignItemProblem
		{
			// Token: 0x06002781 RID: 10113 RVA: 0x00083263 File Offset: 0x00081463
			[JsonConstructor]
			protected ReassignItemToInvalidGamePieceProblem()
			{
			}

			// Token: 0x06002782 RID: 10114 RVA: 0x0008326B File Offset: 0x0008146B
			public ReassignItemToInvalidGamePieceProblem(ConfigRef reassignmentOrder, Identifier itemToReassign, GamePiece destinationGamePiece) : base(reassignmentOrder, itemToReassign)
			{
				this.DestinationGamePiece = destinationGamePiece;
			}

			// Token: 0x1700059E RID: 1438
			// (get) Token: 0x06002783 RID: 10115 RVA: 0x00083281 File Offset: 0x00081481
			protected override string LocKeyScope
			{
				get
				{
					return base.LocKeyScope + ".GamePiece";
				}
			}

			// Token: 0x1700059F RID: 1439
			// (get) Token: 0x06002784 RID: 10116 RVA: 0x00083293 File Offset: 0x00081493
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DefaultProblem";
				}
			}

			// Token: 0x170005A0 RID: 1440
			// (get) Token: 0x06002785 RID: 10117 RVA: 0x000832A5 File Offset: 0x000814A5
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" to game piece {0}", this.DestinationGamePiece);
				}
			}

			// Token: 0x040011B7 RID: 4535
			[JsonProperty]
			[BindableValue("target", BindingOption.None)]
			public Identifier DestinationGamePiece;
		}

		// Token: 0x02000853 RID: 2131
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemToIncorrectGamePieceType : Result.ReassignItemToInvalidGamePieceProblem
		{
			// Token: 0x06002786 RID: 10118 RVA: 0x000832C7 File Offset: 0x000814C7
			[JsonConstructor]
			protected ReassignItemToIncorrectGamePieceType()
			{
			}

			// Token: 0x06002787 RID: 10119 RVA: 0x000832CF File Offset: 0x000814CF
			public ReassignItemToIncorrectGamePieceType(ConfigRef reassignmentOrder, Identifier itemToReassign, GamePiece destinationGamePiece) : base(reassignmentOrder, itemToReassign, destinationGamePiece)
			{
			}

			// Token: 0x170005A1 RID: 1441
			// (get) Token: 0x06002788 RID: 10120 RVA: 0x000832DA File Offset: 0x000814DA
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is the wrong type";
				}
			}
		}

		// Token: 0x02000854 RID: 2132
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemToStolenGamePieceProblem : Result.ReassignItemToInvalidGamePieceProblem
		{
			// Token: 0x06002789 RID: 10121 RVA: 0x000832EC File Offset: 0x000814EC
			[JsonConstructor]
			protected ReassignItemToStolenGamePieceProblem()
			{
			}

			// Token: 0x0600278A RID: 10122 RVA: 0x000832F4 File Offset: 0x000814F4
			public ReassignItemToStolenGamePieceProblem(ConfigRef reassignmentOrder, Identifier itemToReassign, GamePiece destinationGamePiece) : base(reassignmentOrder, itemToReassign, destinationGamePiece)
			{
			}

			// Token: 0x170005A2 RID: 1442
			// (get) Token: 0x0600278B RID: 10123 RVA: 0x000832FF File Offset: 0x000814FF
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Stolen";
				}
			}

			// Token: 0x170005A3 RID: 1443
			// (get) Token: 0x0600278C RID: 10124 RVA: 0x00083311 File Offset: 0x00081511
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the game piece was stolen";
				}
			}
		}

		// Token: 0x02000855 RID: 2133
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemToBanishedGamePieceProblem : Result.ReassignItemToInvalidGamePieceProblem
		{
			// Token: 0x0600278D RID: 10125 RVA: 0x00083323 File Offset: 0x00081523
			[JsonConstructor]
			protected ReassignItemToBanishedGamePieceProblem()
			{
			}

			// Token: 0x0600278E RID: 10126 RVA: 0x0008332B File Offset: 0x0008152B
			public ReassignItemToBanishedGamePieceProblem(ConfigRef reassignmentOrder, Identifier itemToReassign, GamePiece destinationGamePiece) : base(reassignmentOrder, itemToReassign, destinationGamePiece)
			{
			}

			// Token: 0x170005A4 RID: 1444
			// (get) Token: 0x0600278F RID: 10127 RVA: 0x00083336 File Offset: 0x00081536
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".Banished";
				}
			}

			// Token: 0x170005A5 RID: 1445
			// (get) Token: 0x06002790 RID: 10128 RVA: 0x00083348 File Offset: 0x00081548
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because the game piece was destroyed";
				}
			}
		}

		// Token: 0x02000856 RID: 2134
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignItemGamePieceThereCanOnlyBeOneProblem : Result.ReassignItemToInvalidGamePieceProblem
		{
			// Token: 0x06002791 RID: 10129 RVA: 0x0008335A File Offset: 0x0008155A
			[JsonConstructor]
			protected ReassignItemGamePieceThereCanOnlyBeOneProblem()
			{
			}

			// Token: 0x06002792 RID: 10130 RVA: 0x00083362 File Offset: 0x00081562
			public ReassignItemGamePieceThereCanOnlyBeOneProblem(ConfigRef reassignmentOrder, Identifier itemToReassign, GamePiece destinationGamePiece) : base(reassignmentOrder, itemToReassign, destinationGamePiece)
			{
			}

			// Token: 0x170005A6 RID: 1446
			// (get) Token: 0x06002793 RID: 10131 RVA: 0x0008336D File Offset: 0x0008156D
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ((this.ItemToReassign != Identifier.Invalid) ? ".ThereCanOnlyBeOne" : ".ThereCanOnlyBeOne.Praetor");
				}
			}

			// Token: 0x170005A7 RID: 1447
			// (get) Token: 0x06002794 RID: 10132 RVA: 0x0008338F File Offset: 0x0008158F
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because there can only be one!";
				}
			}
		}

		// Token: 0x02000857 RID: 2135
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignAlreadyAttachedItemProblem : Result.ReassignItemToInvalidGamePieceProblem
		{
			// Token: 0x06002795 RID: 10133 RVA: 0x000833A1 File Offset: 0x000815A1
			[JsonConstructor]
			protected ReassignAlreadyAttachedItemProblem()
			{
			}

			// Token: 0x06002796 RID: 10134 RVA: 0x000833A9 File Offset: 0x000815A9
			public ReassignAlreadyAttachedItemProblem(ConfigRef reassignmentOrder, Identifier itemToReassign, GamePiece destinationGamePiece) : base(reassignmentOrder, itemToReassign, destinationGamePiece)
			{
			}

			// Token: 0x170005A8 RID: 1448
			// (get) Token: 0x06002797 RID: 10135 RVA: 0x000833B4 File Offset: 0x000815B4
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it is already attached to this game piece";
				}
			}
		}

		// Token: 0x02000858 RID: 2136
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ReassignToFullGamePieceProblem : Result.ReassignItemToInvalidGamePieceProblem
		{
			// Token: 0x06002798 RID: 10136 RVA: 0x000833C6 File Offset: 0x000815C6
			[JsonConstructor]
			protected ReassignToFullGamePieceProblem()
			{
			}

			// Token: 0x06002799 RID: 10137 RVA: 0x000833CE File Offset: 0x000815CE
			public ReassignToFullGamePieceProblem(ConfigRef reassignmentOrder, Identifier itemToReassign, GamePiece destinationGamePiece) : base(reassignmentOrder, itemToReassign, destinationGamePiece)
			{
			}

			// Token: 0x170005A9 RID: 1449
			// (get) Token: 0x0600279A RID: 10138 RVA: 0x000833D9 File Offset: 0x000815D9
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NoFreeSlot";
				}
			}

			// Token: 0x170005AA RID: 1450
			// (get) Token: 0x0600279B RID: 10139 RVA: 0x000833EB File Offset: 0x000815EB
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it has no attachment slots left";
				}
			}
		}
	}
}
