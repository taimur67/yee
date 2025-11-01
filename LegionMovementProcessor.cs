using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003EB RID: 1003
	public static class LegionMovementProcessor
	{
		// Token: 0x060013C6 RID: 5062 RVA: 0x0004B4A6 File Offset: 0x000496A6
		public static DiplomaticState GetDiplomaticRelationship(TurnContext context, GamePiece piece, HexCoord coord, HexBoard hexBoard)
		{
			return LegionMovementProcessor.GetDiplomaticRelationship(context, piece.ControllingPlayerId, coord, hexBoard);
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0004B4B8 File Offset: 0x000496B8
		public static DiplomaticState GetDiplomaticRelationship(TurnContext context, int playerId, HexCoord coord, HexBoard hexBoard)
		{
			int controllingPlayerID = hexBoard[coord].ControllingPlayerID;
			return context.Diplomacy.GetDiplomaticStatus(playerId, controllingPlayerID).DiplomaticState;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0004B4E4 File Offset: 0x000496E4
		public static LegionMoveEvent TeleportMove(TurnProcessContext context, Identifier gamePieceId, HexCoord dest, bool systemMove = false, AttackOutcomeIntent attackOutcomeIntent = AttackOutcomeIntent.Default)
		{
			return LegionMovementProcessor.MoveInternal(context, context.CurrentTurn.FetchGameItem<GamePiece>(gamePieceId), attackOutcomeIntent, IEnumerableExtensions.ToList<HexCoord>(IEnumerableExtensions.ToEnumerable<HexCoord>(dest)), PathMode.Teleport, systemMove);
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0004B507 File Offset: 0x00049707
		public static LegionMoveEvent GroundMove(TurnProcessContext context, Identifier piece, params HexCoord[] movePath)
		{
			return LegionMovementProcessor.GroundMove(context, piece, IEnumerableExtensions.ToList<HexCoord>(movePath), AttackOutcomeIntent.Default);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0004B517 File Offset: 0x00049717
		public static LegionMoveEvent GroundMove(TurnProcessContext context, GamePiece piece, params HexCoord[] movePath)
		{
			return LegionMovementProcessor.GroundMove(context, piece, IEnumerableExtensions.ToList<HexCoord>(movePath), AttackOutcomeIntent.Default);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0004B527 File Offset: 0x00049727
		public static LegionMoveEvent GroundMove(TurnProcessContext context, Identifier piece, AttackOutcomeIntent attackOutcomeIntent, params HexCoord[] movePath)
		{
			return LegionMovementProcessor.GroundMove(context, piece, IEnumerableExtensions.ToList<HexCoord>(movePath), attackOutcomeIntent);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0004B537 File Offset: 0x00049737
		public static LegionMoveEvent GroundMove(TurnProcessContext context, GamePiece piece, AttackOutcomeIntent attackOutcomeIntent, params HexCoord[] movePath)
		{
			return LegionMovementProcessor.GroundMove(context, piece, IEnumerableExtensions.ToList<HexCoord>(movePath), attackOutcomeIntent);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0004B547 File Offset: 0x00049747
		public static LegionMoveEvent GroundMove(TurnProcessContext context, Identifier id, List<HexCoord> movePath, AttackOutcomeIntent attackOutcomeIntent = AttackOutcomeIntent.Default)
		{
			return LegionMovementProcessor.MoveInternal(context, context.CurrentTurn.FetchGameItem<GamePiece>(id), attackOutcomeIntent, movePath, PathMode.March, false);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0004B55F File Offset: 0x0004975F
		public static LegionMoveEvent GroundMove(TurnProcessContext context, GamePiece gamePiece, List<HexCoord> movePath, AttackOutcomeIntent attackOutcomeIntent = AttackOutcomeIntent.Default)
		{
			return LegionMovementProcessor.MoveInternal(context, gamePiece, attackOutcomeIntent, movePath, PathMode.March, false);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0004B56C File Offset: 0x0004976C
		public static bool IsOccupied(TurnState turn, HexCoord coord)
		{
			GamePiece gamePiece;
			return LegionMovementProcessor.IsOccupied(turn, coord, out gamePiece);
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0004B584 File Offset: 0x00049784
		public static bool IsOccupied(TurnState turn, HexCoord coord, out GamePiece occupier)
		{
			bool result;
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				occupier = turn.GetGamePieceAt(coord);
				result = (occupier != null);
			}
			return result;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0004B5C8 File Offset: 0x000497C8
		public static GameEvent Place(this TurnProcessContext context, GamePiece piece, HexCoord location)
		{
			return context.Place(piece, location, CaptureExtensions.CanCaptureWith(context, piece, location, context.HexBoard));
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0004B5E0 File Offset: 0x000497E0
		public static GameEvent Place(this TurnProcessContext context, GamePiece piece, HexCoord location, bool captureCanton)
		{
			Hex hex = context.HexBoard[location];
			int? num = (hex != null) ? new int?(hex.ControllingPlayerID) : null;
			int controllingPlayerId = piece.ControllingPlayerId;
			bool flag = num.GetValueOrDefault() == controllingPlayerId & num != null;
			if (LegionMovementProcessor.IsValidRetreatHex(context, piece, location, PathMode.Teleport) && flag)
			{
				piece.LastFriendlyCanton = location;
			}
			TurnState currentTurn = context.CurrentTurn;
			piece.Location = currentTurn.HexBoard.ToRelativeHex(location);
			context.RecalculateModifiersFor(piece.Id);
			GameEvent result = null;
			if (captureCanton)
			{
				result = context.ClaimCanton(location, piece.ControllingPlayerId);
			}
			return result;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0004B67E File Offset: 0x0004987E
		public static bool IsValidRetreatHex(TurnContext context, GamePiece gamePiece, HexCoord coord, PathMode pathMode)
		{
			return LegionMovementProcessor.CanMoveBeExecuted(context, gamePiece, coord, pathMode, null);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0004B690 File Offset: 0x00049890
		private static Result TryGetRetreatHex(this TurnContext turn, GamePiece gamePiece, HexCoord startingCoord, out HexCoord retreatHex, PathMode pathMode = PathMode.Teleport)
		{
			retreatHex = HexCoord.Invalid;
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>(from x in IEnumerableExtensions.ExceptFor<HexCoord>(turn.HexBoard.GetNeighbours(startingCoord, false), new HexCoord[]
			{
				startingCoord
			})
			where LegionMovementProcessor.IsValidRetreatHex(turn, gamePiece, x, pathMode)
			select x);
			if (list.Count > 0)
			{
				retreatHex = list.GetRandom(turn.Random);
				return Result.Success;
			}
			int range = Math.Max(gamePiece.GroundMoveDistance.Value, gamePiece.TeleportDistance);
			List<HexCoord> list2 = IEnumerableExtensions.ToList<HexCoord>(from x in IEnumerableExtensions.ExceptFor<HexCoord>(turn.HexBoard.EnumerateRange(startingCoord, range), new HexCoord[]
			{
				startingCoord
			})
			where LegionMovementProcessor.IsValidRetreatHex(turn, gamePiece, x, pathMode)
			select x);
			if (list2.Count > 0)
			{
				retreatHex = list2.GetRandom(turn.Random);
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0004B7B4 File Offset: 0x000499B4
		public static LegionMoveEvent RetreatTeleport(TurnProcessContext context, GamePiece gamePiece, HexCoord coord)
		{
			HexCoord hexCoord;
			if (context.TryGetRetreatHex(gamePiece, coord, out hexCoord, PathMode.Teleport))
			{
				return LegionMovementProcessor.MoveInternal(context, gamePiece, AttackOutcomeIntent.Default, IEnumerableExtensions.ToList<HexCoord>(IEnumerableExtensions.ToEnumerable<HexCoord>(hexCoord)), PathMode.Teleport, true);
			}
			return null;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0004B7EC File Offset: 0x000499EC
		public static Result RetreatMarchFrom(TurnProcessContext context, GamePiece gamePiece, HexCoord coord)
		{
			HexCoord hexCoord;
			Problem problem = context.TryGetRetreatHex(gamePiece, coord, out hexCoord, PathMode.Teleport) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return LegionMovementProcessor.GroundMove(context, gamePiece, AttackOutcomeIntent.Default, new HexCoord[]
			{
				hexCoord
			}).Result;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0004B82C File Offset: 0x00049A2C
		public static RepatriateLegionEvent RepatriateToStronghold(this GamePiece gamePiece, TurnProcessContext context)
		{
			GamePiece stronghold = context.CurrentTurn.GetStronghold(gamePiece.ControllingPlayerId);
			return gamePiece.Repatriate(context, stronghold.Location);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0004B858 File Offset: 0x00049A58
		public static RepatriateLegionEvent Repatriate(this GamePiece gamePiece, TurnProcessContext context)
		{
			HexCoord retreatDestination = gamePiece.LastFriendlyCanton;
			if (retreatDestination == HexCoord.Invalid)
			{
				GamePiece stronghold = context.CurrentTurn.GetStronghold(gamePiece.ControllingPlayerId);
				if (stronghold != null)
				{
					retreatDestination = stronghold.Location;
				}
			}
			return gamePiece.Repatriate(context, retreatDestination);
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0004B8A0 File Offset: 0x00049AA0
		public static RepatriateLegionEvent Repatriate(this GamePiece gamePiece, TurnProcessContext context, HexCoord retreatDestination)
		{
			RepatriateLegionEvent repatriateLegionEvent = new RepatriateLegionEvent(gamePiece);
			if (retreatDestination == HexCoord.Invalid || !LegionMovementProcessor.IsValidRetreatHex(context, gamePiece, retreatDestination, PathMode.Teleport))
			{
				HexBoard board = context.HexBoard;
				Hex hex = IEnumerableExtensions.FirstOrDefault<Hex>((from x in board.GetHexesControlledByPlayer(gamePiece.ControllingPlayerId)
				where LegionMovementProcessor.IsValidRetreatHex(context, gamePiece, x.HexCoord, PathMode.Teleport)
				select x).OrderBy(delegate(Hex x)
				{
					float num = board.ShortestDistancef(x.HexCoord, gamePiece.Location);
					if (retreatDestination == HexCoord.Invalid)
					{
						return num;
					}
					float num2 = board.ShortestDistancef(x.HexCoord, retreatDestination);
					return num * num2 * num2;
				}));
				retreatDestination = ((hex != null) ? hex.HexCoord : HexCoord.Invalid);
			}
			if (retreatDestination != HexCoord.Invalid)
			{
				LegionMoveEvent legionMoveEvent = LegionMovementProcessor.TeleportMove(context, gamePiece.Id, retreatDestination, true, AttackOutcomeIntent.Default);
				if (legionMoveEvent != null && legionMoveEvent.Result.successful)
				{
					repatriateLegionEvent.AddChildEvent<LegionMoveEvent>(legionMoveEvent);
					return repatriateLegionEvent;
				}
			}
			LegionKilledEvent ev = context.KillGamePiece(gamePiece, -1);
			repatriateLegionEvent.AddChildEvent<LegionKilledEvent>(ev);
			return repatriateLegionEvent;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0004B9F7 File Offset: 0x00049BF7
		public static bool IsTraversable(TurnContext context, GamePiece piece, HexCoord coord, PathMode pathMode = PathMode.March)
		{
			return LegionMovementProcessor.IsTraversable(context, piece, context.HexBoard[coord], pathMode);
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0004BA10 File Offset: 0x00049C10
		public static bool IsTraversable(TurnContext context, GamePiece piece, Hex hex, PathMode pathMode = PathMode.March)
		{
			TerrainStaticData terrainStaticData;
			return context.Database.TryFindTerrainData(hex, out terrainStaticData) && (pathMode != PathMode.Teleport || terrainStaticData.AllowTeleport) && LegionMovementProcessor.IsWalkable(terrainStaticData, piece);
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0004BA44 File Offset: 0x00049C44
		public static bool IsWalkable(TerrainStaticData data, GamePiece piece)
		{
			return LegionMovementProcessor.CalculateMovementCostType(data, piece) > MoveCostType.NonTraversible;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0004BA50 File Offset: 0x00049C50
		public static bool IsCapturableBy(TurnState turn, GamePiece target, PlayerState playerState)
		{
			return turn.IsValidCombatTarget(target, playerState) && target.IsCurrentlyCapturable();
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0004BA64 File Offset: 0x00049C64
		public static bool CanCreateMovementOrder(this GamePiece target)
		{
			return target.CanCreateMovementOrder(null);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0004BA6D File Offset: 0x00049C6D
		public static bool CanCreateMovementOrder(this GamePiece target, PlayerState player)
		{
			return target.Status == GameItemStatus.InPlay && (player == null || player.Id == target.ControllingPlayerId) && target.CanMove;
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0004BA98 File Offset: 0x00049C98
		public static bool IsValidCombatTarget(this GamePiece target, TurnState turn, GamePiece initiator)
		{
			return initiator.IsActive && initiator.CanInitiateCombat && turn.IsValidCombatTarget(target, turn.FindPlayerState(initiator.ControllingPlayerId, null));
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0004BAC8 File Offset: 0x00049CC8
		public static bool IsValidCombatTarget(this TurnState turn, GamePiece target, PlayerState player)
		{
			if (!target.IsActive)
			{
				return false;
			}
			int id = player.Id;
			int controllingPlayerId = target.ControllingPlayerId;
			if (!turn.CombatAuthorizedBetween(id, controllingPlayerId))
			{
				return false;
			}
			DiplomaticState diplomaticState = turn.CurrentDiplomaticTurn.GetDiplomaticStatus(id, controllingPlayerId).DiplomaticState;
			PlayerState playerState = turn.FindPlayerState(controllingPlayerId, null);
			Identifier? identifier = (playerState != null) ? new Identifier?(playerState.StrongholdId) : null;
			Identifier id2 = target.Id;
			Identifier? identifier2 = identifier;
			return !(id2 == identifier2.GetValueOrDefault() & identifier2 != null) || diplomaticState.AllowStrongholdCapture(turn.CurrentDiplomaticTurn, id, controllingPlayerId);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0004BB60 File Offset: 0x00049D60
		public static bool IsOwnedOrFriendly(TurnState state, PlayerState player, HexCoord hex)
		{
			int controllingPlayerID = state.HexBoard[hex].GetControllingPlayerID();
			if (controllingPlayerID == player.Id)
			{
				return true;
			}
			if (controllingPlayerID == -1)
			{
				return false;
			}
			DiplomaticState diplomaticState = state.CurrentDiplomaticTurn.GetDiplomaticStatus(player.Id, controllingPlayerID).DiplomaticState;
			return diplomaticState.AllowMovementIntoTerritory(state.CurrentDiplomaticTurn, player.Id, controllingPlayerID) && !diplomaticState.AllowCantonCapture(state.CurrentDiplomaticTurn, player.Id, controllingPlayerID);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0004BBD5 File Offset: 0x00049DD5
		public static bool HasRightOfEntry(TurnState state, PlayerState player, GamePiece gamePiece, HexCoord hexCoord)
		{
			return LegionMovementProcessor.HasRightOfEntry(state, player.Id, gamePiece, hexCoord);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0004BBE8 File Offset: 0x00049DE8
		public static bool HasRightOfEntry(TurnState state, int playerId, GamePiece gamePiece, HexCoord hexCoord)
		{
			Hex hex = state.HexBoard[hexCoord];
			if (hex.IsUnclaimed())
			{
				return true;
			}
			int controllingPlayerID = hex.GetControllingPlayerID();
			if (controllingPlayerID == -1)
			{
				return true;
			}
			if (gamePiece != null)
			{
				if (gamePiece.CanMoveThroughEnemyTerritory)
				{
					return true;
				}
				if (playerId != gamePiece.ControllingPlayerId && !state.BazaarState.IsForSale(gamePiece))
				{
					playerId = gamePiece.ControllingPlayerId;
				}
			}
			return controllingPlayerID == playerId || playerId == -1 || state.CurrentDiplomaticTurn.GetDiplomaticStatus(playerId, controllingPlayerID).DiplomaticState.AllowMovementIntoTerritory(state.CurrentDiplomaticTurn, playerId, controllingPlayerID);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0004BC7C File Offset: 0x00049E7C
		public static bool TryFindSpawnPointFor(TurnContext context, PlayerState player, GamePiece piece, out HexCoord spawnPoint)
		{
			spawnPoint = HexCoord.Invalid;
			GamePiece stronghold = context.CurrentTurn.GetStronghold(player.Id);
			if (stronghold == null || stronghold.Status != GameItemStatus.InPlay)
			{
				return false;
			}
			HexBoard board = context.HexBoard;
			Hex hex = IEnumerableExtensions.FirstOrDefault<Hex>(from candidate in board.Hexes
			where context.IsValidSpawnPoint(player, candidate.HexCoord, piece)
			orderby (float)board.ShortestDistance(stronghold.Location, candidate.HexCoord) + context.Random.NextFloat(0f, 0.5f)
			select candidate);
			if (hex == null)
			{
				return false;
			}
			spawnPoint = hex.HexCoord;
			return spawnPoint != HexCoord.Invalid;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0004BD4D File Offset: 0x00049F4D
		public static bool IsValidSpawnPoint(this TurnContext context, PlayerState player, HexCoord coord, GamePiece piece = null)
		{
			return context.HexBoard[coord].ControllingPlayerID == player.Id && LegionMovementProcessor.IsTraversable(context, piece, coord, PathMode.March) && !LegionMovementProcessor.IsOccupied(context.CurrentTurn, coord);
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0004BD84 File Offset: 0x00049F84
		public static bool CanOccupyCanton(this GamePiece piece, TurnContext context, HexCoord coord)
		{
			return LegionMovementProcessor.CanEnterCanton(context, piece, coord, PathMode.March, null).successful;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0004BD98 File Offset: 0x00049F98
		public static Result CheckForProblemInMoveSequence(TurnContext context, GamePiece legion, List<HexCoord> movePath)
		{
			if (movePath.Count == 0)
			{
				return Result.Success;
			}
			Result result = LegionMovementProcessor.IsValidMove(context, legion, legion.Location, movePath[0], PathMode.March, null);
			if (!result.successful)
			{
				return result;
			}
			for (int i = 1; i < movePath.Count; i++)
			{
				result = LegionMovementProcessor.IsValidMove(context, legion, movePath[i - 1], movePath[i], PathMode.March, null);
				if (!result.successful)
				{
					return result;
				}
			}
			return Result.Success;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0004BE10 File Offset: 0x0004A010
		public static Result CanMoveBeExecuted(TurnContext context, GamePiece gamePiece, HexCoord relativeDest, PathMode pathMode, List<int> ignoreDiplomacyWithPlayers = null)
		{
			Problem problem = LegionMovementProcessor.CanEnterCanton(context, gamePiece, relativeDest, pathMode, ignoreDiplomacyWithPlayers) as Problem;
			if (problem != null)
			{
				return problem;
			}
			TurnState currentTurn = context.CurrentTurn;
			GamePiece gamePiece2;
			if (LegionMovementProcessor.IsOccupied(currentTurn, relativeDest, out gamePiece2))
			{
				if (!currentTurn.CombatAuthorizedBetween(gamePiece.ControllingPlayerId, gamePiece2.ControllingPlayerId))
				{
					return Result.OccupiedCanton(gamePiece, relativeDest, gamePiece2.Id);
				}
				if (pathMode == PathMode.Teleport)
				{
					return Result.CannotTeleportIntoCombat(gamePiece, relativeDest, gamePiece2.Id);
				}
			}
			return Result.Success;
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0004BE80 File Offset: 0x0004A080
		public static Result IsValidMove(TurnContext context, GamePiece gamePiece, HexCoord location, HexCoord relativeDest, PathMode pathMode, List<int> ignoreDiplomacyWithPlayers = null)
		{
			Result result;
			if (pathMode != PathMode.March)
			{
				if (pathMode != PathMode.Teleport)
				{
					result = null;
				}
				else
				{
					result = LegionMovementProcessor._IsValidTeleportMove(context, gamePiece, location, relativeDest);
				}
			}
			else
			{
				result = LegionMovementProcessor._IsValidGroundMove(context, gamePiece, location, relativeDest);
			}
			Problem problem = result as Problem;
			if (problem != null)
			{
				return problem;
			}
			return LegionMovementProcessor.CanEnterCanton(context, gamePiece, relativeDest, pathMode, ignoreDiplomacyWithPlayers);
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0004BECC File Offset: 0x0004A0CC
		public static Result CanEnterCanton(TurnContext context, GamePiece gamePiece, HexCoord relativeDest, PathMode pathMode, List<int> ignoreDiplomacyWithPlayers = null)
		{
			if (!gamePiece.IsAlive())
			{
				return Result.BanishedBeforeMoving(gamePiece, relativeDest);
			}
			TurnState currentTurn = context.CurrentTurn;
			if (!LegionMovementProcessor.IsTraversable(context, gamePiece, relativeDest, PathMode.March))
			{
				return Result.ImpassibleTerrain(gamePiece, relativeDest, currentTurn.HexBoard[relativeDest].Type);
			}
			if (pathMode == PathMode.Teleport)
			{
				Hex hex = context.HexBoard[relativeDest];
				TerrainStaticData terrainStaticData;
				if (hex == null || !context.Database.TryFindTerrainData(hex, out terrainStaticData))
				{
					return new DebugProblem(string.Format("No TerrainStaticData found for {0}", relativeDest));
				}
				if (!terrainStaticData.AllowTeleport)
				{
					return Result.TerrainForbidsTeleport(gamePiece, relativeDest, hex.Type);
				}
				GamePiece gamePiece2;
				if (LegionMovementProcessor.IsOccupied(currentTurn, relativeDest, out gamePiece2) && gamePiece2.IsFixture())
				{
					return Result.OccupiedCanton(gamePiece, relativeDest, gamePiece2.Id);
				}
			}
			if ((ignoreDiplomacyWithPlayers == null || !ignoreDiplomacyWithPlayers.Contains(currentTurn.HexBoard[relativeDest].ControllingPlayerID)) && !LegionMovementProcessor.HasRightOfEntry(currentTurn, currentTurn.FindPlayerState(gamePiece.ControllingPlayerId, null), gamePiece, relativeDest))
			{
				return Result.NoRightOfEntry(gamePiece, relativeDest, currentTurn.HexBoard.GetOwnership(relativeDest));
			}
			return Result.Success;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0004BFD8 File Offset: 0x0004A1D8
		private static Result _IsValidTeleportMove(TurnContext context, GamePiece gamePiece, HexCoord from, HexCoord to)
		{
			if (!gamePiece.CanTeleport)
			{
				return Result.DoesNotHaveTeleport(gamePiece, to);
			}
			from = context.HexBoard.ToRelativeHex(from);
			to = context.HexBoard.ToRelativeHex(to);
			int num = context.HexBoard.ShortestDistance(from, to);
			if (num > gamePiece.TeleportDistance)
			{
				return Result.OutsideTeleportRange(gamePiece, to, num);
			}
			return Result.Success;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0004C044 File Offset: 0x0004A244
		private static Result _IsValidGroundMove(TurnContext context, GamePiece gamePiece, HexCoord from, HexCoord to)
		{
			from = context.HexBoard.ToRelativeHex(from);
			to = context.HexBoard.ToRelativeHex(to);
			if (!context.HexBoard.AreAdjacent(from, to) && from != to)
			{
				return Result.InvalidTransition(gamePiece, from, to);
			}
			return Result.Success;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0004C098 File Offset: 0x0004A298
		public static int CalculateMovementCost(TurnContext actionContext, LegionMovementProcessor.MoveQuery context, HexBoard hexBoard)
		{
			Hex hex = hexBoard[context.DestinationCoord];
			GamePiece gamePiece;
			if (LegionMovementProcessor.CanRedeployFrom(actionContext, context.GamePiece, context.DestinationCoord, out gamePiece))
			{
				return 0;
			}
			if (LegionMovementProcessor.CanRedeployFrom(actionContext, context.GamePiece, context.StartingCoord, out gamePiece))
			{
				return Math.Max(context.MovementPointsRemaining, 1);
			}
			TerrainStaticData terrainStaticData;
			if (actionContext.Database.TryFindTerrainData(hex, out terrainStaticData))
			{
				MoveCostType moveCostType = LegionMovementProcessor.CalculateMovementCostType(terrainStaticData, context.GamePiece);
				if (moveCostType == MoveCostType.MovePoints)
				{
					return terrainStaticData.MovePoints;
				}
				if (moveCostType == MoveCostType.RemainderOfMovement)
				{
					return Math.Max(context.MovementPointsRemaining, 1);
				}
				if (moveCostType == MoveCostType.AllOfMovement)
				{
					return context.GamePiece.GroundMoveDistance;
				}
			}
			return 1;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0004C13C File Offset: 0x0004A33C
		public static bool HasMovementCostReduction(GamePiece gamePiece, TerrainType terrainType)
		{
			MoveCostType moveCostType = MoveCostType.NonTraversible;
			return LegionMovementProcessor.CalculateMovementCostType(terrainType, moveCostType, gamePiece) != moveCostType;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0004C15C File Offset: 0x0004A35C
		public static MoveCostType CalculateMovementCostType(TerrainType terrainType, MoveCostType moveCost, GamePiece gamePiece)
		{
			EntityTag_TerrainMoveCostOverride entityTag_TerrainMoveCostOverride = null;
			if (gamePiece != null)
			{
				foreach (EntityTag entityTag in gamePiece.TagList)
				{
					EntityTag_TerrainMoveCostOverride entityTag_TerrainMoveCostOverride2 = entityTag as EntityTag_TerrainMoveCostOverride;
					if (entityTag_TerrainMoveCostOverride2 != null && entityTag_TerrainMoveCostOverride2.Type == terrainType && (entityTag_TerrainMoveCostOverride == null || entityTag_TerrainMoveCostOverride2.MoveCostType < entityTag_TerrainMoveCostOverride.MoveCostType))
					{
						entityTag_TerrainMoveCostOverride = entityTag_TerrainMoveCostOverride2;
					}
				}
			}
			if (entityTag_TerrainMoveCostOverride == null)
			{
				return moveCost;
			}
			return entityTag_TerrainMoveCostOverride.MoveCostType;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0004C1DC File Offset: 0x0004A3DC
		public static MoveCostType CalculateMovementCostType(TerrainStaticData data, GamePiece gamePiece)
		{
			return LegionMovementProcessor.CalculateMovementCostType(data.LegacyType, data.MoveCost, gamePiece);
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0004C1F0 File Offset: 0x0004A3F0
		public static bool AllowRedeployThrough(this GamePiece gamePiece)
		{
			return gamePiece.SubCategory.IsFixture();
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0004C1FD File Offset: 0x0004A3FD
		public static bool IsRedeploying(TurnContext context, LegionMovementProcessor.MoveQuery query, out GamePiece deployer)
		{
			return LegionMovementProcessor.CanRedeployFrom(context, query.GamePiece, query.StartingCoord, out deployer);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0004C212 File Offset: 0x0004A412
		public static bool CanRedeploy(this GamePiece gamePiece)
		{
			return gamePiece.SubCategory.IsLegion();
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0004C21F File Offset: 0x0004A41F
		private static bool CanRedeployFrom(TurnContext context, GamePiece gp, HexCoord coord, out GamePiece redeployer)
		{
			redeployer = null;
			if (gp.Location == coord)
			{
				return false;
			}
			redeployer = context.CurrentTurn.GetGamePieceAt(coord);
			return redeployer != null && LegionMovementProcessor.SupportsRedeploy(context.CurrentTurn, gp, redeployer);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0004C258 File Offset: 0x0004A458
		public static bool SupportsRedeploy(TurnState turn, GamePiece actor, GamePiece redeployer)
		{
			return actor.CanRedeploy() && redeployer.AllowRedeployThrough() && turn.GetDiplomaticStatus(actor.ControllingPlayerId, redeployer.ControllingPlayerId).DiplomaticState.AllowRedeployThroughFixtures(turn.CurrentDiplomaticTurn, actor.ControllingPlayerId, redeployer.ControllingPlayerId);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0004C2A8 File Offset: 0x0004A4A8
		[return: TupleElementNames(new string[]
		{
			"allResults",
			"potentialCaptures"
		})]
		public static ValueTuple<List<LegionMovementProcessor.MoveQueryResult>, List<LegionMovementProcessor.MoveQueryResult>> DetermineMoveResults(TurnContext context, List<LegionMovementProcessor.MoveQuery> queries)
		{
			HexBoard hexBoard = context.HexBoard.DeepClone<HexBoard>();
			List<LegionMovementProcessor.MoveQuery> list = new List<LegionMovementProcessor.MoveQuery>();
			List<LegionMovementProcessor.MoveQueryResult> list2 = new List<LegionMovementProcessor.MoveQueryResult>();
			List<LegionMovementProcessor.MoveQueryResult> list3 = new List<LegionMovementProcessor.MoveQueryResult>();
			int num = (from x in queries
			select x.GamePiece).Distinct<GamePiece>().Count<GamePiece>();
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < queries.Count; j++)
				{
					LegionMovementProcessor.MoveQuery moveQuery = queries[j];
					LegionMovementProcessor.MoveQueryResult moveQueryResult = LegionMovementProcessor.DetermineMoveResult(context, moveQuery, hexBoard);
					if (queries.FindLast((LegionMovementProcessor.MoveQuery x) => x.GamePiece.Equals(moveQuery.GamePiece)) == moveQuery)
					{
						if (moveQueryResult.CapturesCantonAtEndOfMovement || moveQueryResult.CapturesCantonDuringMovement)
						{
							list3.Add(moveQueryResult);
							list2.Add(moveQueryResult);
							hexBoard.SetOwnership(moveQuery.DestinationCoord, moveQuery.GamePiece.ControllingPlayerId);
						}
						else if (i + 1 < num)
						{
							list.Add(moveQuery);
						}
						else
						{
							list2.Add(moveQueryResult);
						}
					}
					else
					{
						if (moveQueryResult.CapturesCantonDuringMovement)
						{
							list3.Add(moveQueryResult);
							hexBoard.SetOwnership(moveQuery.DestinationCoord, moveQuery.GamePiece.ControllingPlayerId);
						}
						list2.Add(moveQueryResult);
					}
				}
				queries = new List<LegionMovementProcessor.MoveQuery>(list);
				list.Clear();
			}
			return new ValueTuple<List<LegionMovementProcessor.MoveQueryResult>, List<LegionMovementProcessor.MoveQueryResult>>(list2, list3);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0004C434 File Offset: 0x0004A634
		public static List<LegionMovementProcessor.MoveQuery> GetMoveQueries(this TurnState turnView, OverrideMovementRitualOrder order)
		{
			List<LegionMovementProcessor.MoveQuery> list = new List<LegionMovementProcessor.MoveQuery>();
			AttackOutcomeIntent requestAttackOutcomeIntent = AttackOutcomeIntent.Default;
			GamePiece actor = turnView.FetchGameItem<GamePiece>(order.TargetItemId);
			PathMode mode = PathMode.March;
			foreach (HexCoord coord in order.MovePath)
			{
				list.Add(LegionMovementProcessor.GetMoveQuery(actor, requestAttackOutcomeIntent, mode, false, coord.GetPreviousHexOnPath(order.MovePath, actor), coord, int.MaxValue, null));
			}
			return list;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0004C4C4 File Offset: 0x0004A6C4
		public static List<LegionMovementProcessor.MoveQuery> GetMoveQueries(this TurnState turnView, OrderMoveLegion order)
		{
			List<LegionMovementProcessor.MoveQuery> list = new List<LegionMovementProcessor.MoveQuery>();
			AttackOutcomeIntent requestAttackOutcomeIntent = AttackOutcomeIntent.Default;
			GamePiece gamePiece = turnView.FetchGameItem<GamePiece>(order.GamePieceId);
			OrderMarchLegion orderMarchLegion = order as OrderMarchLegion;
			OrderTeleportLegion orderTeleportLegion;
			PathMode mode;
			if (orderMarchLegion == null)
			{
				orderTeleportLegion = (order as OrderTeleportLegion);
				if (orderTeleportLegion == null)
				{
					return list;
				}
			}
			else
			{
				requestAttackOutcomeIntent = orderMarchLegion.AttackOutcomeIntent;
				mode = PathMode.March;
				using (List<HexCoord>.Enumerator enumerator = orderMarchLegion.MovePath.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						HexCoord coord = enumerator.Current;
						list.Add(LegionMovementProcessor.GetMoveQuery(gamePiece, requestAttackOutcomeIntent, mode, false, coord.GetPreviousHexOnPath(orderMarchLegion.MovePath, gamePiece), coord, int.MaxValue, null));
					}
					return list;
				}
			}
			mode = PathMode.Teleport;
			list.Add(LegionMovementProcessor.GetMoveQuery(gamePiece, requestAttackOutcomeIntent, mode, false, gamePiece.Location, orderTeleportLegion.DestinationHex, int.MaxValue, null));
			return list;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0004C59C File Offset: 0x0004A79C
		public static LegionMovementProcessor.MoveQueryResult DetermineMoveResult(TurnContext context, LegionMovementProcessor.MoveQuery query, HexBoard hexBoard = null)
		{
			if (hexBoard == null)
			{
				hexBoard = context.HexBoard;
			}
			if (query.Territory == null || query.Territory.Count == 0)
			{
				query.Territory = hexBoard.GetHexCoordsControlledByPlayer(query.GamePiece.ControllingPlayerId).ToHashSet<HexCoord>();
			}
			DiplomaticState diplomaticRelationship = LegionMovementProcessor.GetDiplomaticRelationship(context, query.GamePiece, query.DestinationCoord, hexBoard);
			int controllingPlayerID = hexBoard[query.DestinationCoord].ControllingPlayerID;
			LegionMovementProcessor.MoveQueryResult moveQueryResult = new LegionMovementProcessor.MoveQueryResult
			{
				OriginalQuery = query,
				CanCaptureCanton = CaptureExtensions.CanCaptureWith(context, query.GamePiece, query.DestinationCoord, hexBoard),
				CaptureRule = diplomaticRelationship.GetCantonCaptureRules(context.Diplomacy, query.GamePiece.ControllingPlayerId, controllingPlayerID),
				MoveCost = LegionMovementProcessor.CalculateMovementCost(context, query, hexBoard),
				AttackOutcomeIntent = query.AttackOutcomeIntent
			};
			moveQueryResult.MovePromise = LegionMovementProcessor.DetermineMoveResult_Internal(context, query, moveQueryResult);
			return moveQueryResult;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0004C678 File Offset: 0x0004A878
		public static IEnumerable<HexCoord> PerimeterCantons(TurnState turn, PlayerState player)
		{
			return turn.HexBoard.GetBorderCantons(player, true);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0004C687 File Offset: 0x0004A887
		private static int CalculateMoveRange(GamePiece gp, PathMode mode)
		{
			if (mode == PathMode.Teleport)
			{
				return gp.TeleportDistance;
			}
			return 1;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0004C69C File Offset: 0x0004A89C
		private static LegionMovementProcessor.MovePromise DetermineMoveResult_Internal(TurnContext context, LegionMovementProcessor.MoveQuery query, LegionMovementProcessor.MoveQueryResult result)
		{
			GamePiece gamePiece = query.GamePiece;
			if (!gamePiece.IsAlive())
			{
				return new LegionMovementProcessor.MoveQueryResultInvalid(Result.BanishedBeforeMoving(gamePiece, query.DestinationCoord));
			}
			if (query.SystemMove)
			{
				Problem problem = LegionMovementProcessor.CanEnterCanton(context, gamePiece, query.DestinationCoord, query.MovementMode, null) as Problem;
				if (problem != null)
				{
					return new LegionMovementProcessor.MoveQueryResultInvalid(problem);
				}
			}
			else
			{
				Problem problem2 = LegionMovementProcessor.IsValidMove(context, gamePiece, query.StartingCoord, query.DestinationCoord, query.MovementMode, null) as Problem;
				if (problem2 != null)
				{
					return new LegionMovementProcessor.MoveQueryResultInvalid(problem2);
				}
			}
			if (query.MovementPointsRemaining < result.MoveCost && !query.SystemMove)
			{
				return new LegionMovementProcessor.MoveQueryResultInvalid(gamePiece.CanMove ? Result.NotEnoughMovementPoints(gamePiece, query.DestinationCoord) : Result.AlreadyMovedThisTurn(gamePiece, query.DestinationCoord));
			}
			TurnState currentTurn = context.CurrentTurn;
			GamePiece redeployer;
			bool flag = LegionMovementProcessor.IsRedeploying(context, query, out redeployer);
			bool flag2 = query.MovementMode == PathMode.Teleport || flag;
			GamePiece gamePieceAt = currentTurn.GetGamePieceAt(query.DestinationCoord);
			if (gamePieceAt != null && gamePieceAt.IsAlive())
			{
				result.Occupier = gamePieceAt;
				PlayerState playerState = currentTurn.FindPlayerState(query.GamePiece.ControllingPlayerId, null);
				bool flag3 = gamePieceAt.Faction != gamePiece.Faction;
				bool flag4 = flag3 || currentTurn.CombatAuthorizedBetween(playerState.Id, gamePieceAt.ControllingPlayerId);
				if (flag2)
				{
					if (flag4)
					{
						return new LegionMovementProcessor.MoveQueryResultInvalid(Result.CannotTeleportIntoCombat(gamePiece, query.DestinationCoord, gamePieceAt));
					}
					if (!query.IgnoreLegionOccupiedCantons || !gamePieceAt.IsLegionOrTitan())
					{
						return new LegionMovementProcessor.MoveQueryResultInvalid(Result.OccupiedCanton(gamePiece, query.DestinationCoord, gamePieceAt));
					}
				}
				if (flag3 && gamePiece.IsActive && gamePiece.CanInitiateCombat && gamePieceAt.IsActive)
				{
					result.PreventsFutureMovement = false;
					return new LegionMovementProcessor.MoveQueryResultBattle(BattleProcessor.GenerateContext(context, gamePiece, gamePieceAt, query.DestinationCoord, query.AttackOutcomeIntent));
				}
				bool flag5 = LegionMovementProcessor.SupportsRedeploy(context.CurrentTurn, query.GamePiece, gamePieceAt);
				if (gamePieceAt.ControllingPlayerId != playerState.Id)
				{
					PlayerState playerState2 = currentTurn.FindPlayerState(gamePieceAt.ControllingPlayerId, null);
					DiplomaticPairStatus diplomaticStatus = currentTurn.GetDiplomaticStatus(playerState.Id, playerState2.Id);
					if (!diplomaticStatus.DiplomaticState.AllowSupport && !flag5)
					{
						bool flag6 = diplomaticStatus.DiplomaticState.AllowStrongholdCapture(context.Diplomacy, playerState.Id, playerState2.Id);
						if (playerState2.StrongholdId == gamePieceAt.Id && !flag6)
						{
							return new LegionMovementProcessor.MoveQueryResultInvalid(Result.NoRightOfElimination(gamePiece, query.DestinationCoord, playerState2.Id));
						}
						if (!flag4)
						{
							return new LegionMovementProcessor.MoveQueryResultInvalid(Result.NoRightOfCombat(gamePiece, query.DestinationCoord, gamePieceAt));
						}
						if (gamePieceAt.IsValidCombatTarget(currentTurn, gamePiece))
						{
							result.PreventsFutureMovement = gamePieceAt.IsCapturable();
							return new LegionMovementProcessor.MoveQueryResultBattle(BattleProcessor.GenerateContext(context, gamePiece, gamePieceAt, query.DestinationCoord, query.AttackOutcomeIntent));
						}
					}
				}
				if (gamePieceAt.IsCapturable() || flag5)
				{
					result.PreventsFutureMovement = !flag5;
					return new LegionMovementProcessor.MoveQueryResultSecure();
				}
				if (!query.IgnoreLegionOccupiedCantons || !gamePieceAt.IsLegionOrTitan())
				{
					return new LegionMovementProcessor.MoveQueryResultInvalid(Result.OccupiedCanton(gamePiece, query.DestinationCoord, gamePieceAt.Id));
				}
			}
			if (flag)
			{
				result.PreventsFutureMovement = true;
				return new LegionMovementProcessor.MoveQueryResultRedeployTo(redeployer);
			}
			if (!flag2)
			{
				int num = currentTurn.HexBoard.ShortestDistance(query.StartingCoord, query.DestinationCoord);
				if (LegionMovementProcessor.CalculateMoveRange(query.GamePiece, query.MovementMode) < num && !query.SystemMove)
				{
					return new LegionMovementProcessor.MoveQueryResultInvalid(Result.InvalidTransition(gamePiece, query.StartingCoord, query.DestinationCoord));
				}
			}
			return new LegionMovementProcessor.MoveQueryResultMove();
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0004CA2E File Offset: 0x0004AC2E
		private static LegionMoveEvent MoveInternal(TurnProcessContext context, GamePiece actor, AttackOutcomeIntent requestAttackOutcomeIntent, HexCoord location, PathMode mode = PathMode.March)
		{
			return LegionMovementProcessor.MoveInternal(context, actor, requestAttackOutcomeIntent, IEnumerableExtensions.ToList<HexCoord>(IEnumerableExtensions.ToEnumerable<HexCoord>(location)), mode, false);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0004CA48 File Offset: 0x0004AC48
		public static LegionMoveEvent MoveInternal(TurnProcessContext context, GamePiece actor, AttackOutcomeIntent requestAttackOutcomeIntent, List<HexCoord> steps, PathMode mode = PathMode.March, bool systemMove = false)
		{
			HexCoord hexCoord = (steps != null && steps.Count > 0) ? steps.Last<HexCoord>() : HexCoord.Invalid;
			LegionMoveEvent legionMoveEvent = new LegionMoveEvent(actor, actor.Location, hexCoord, actor.ControllingPlayerId);
			if (!systemMove)
			{
				if (actor.LastConvertedTurn == context.CurrentTurn.TurnValue)
				{
					legionMoveEvent.AddChildEvent<BlockedStep>(new BlockedStep(actor.ControllingPlayerId, actor.Id, actor.Location, IEnumerableExtensions.LastOrDefault<HexCoord>(steps, ref HexCoord.Invalid), mode)
					{
						Reason = Result.ConvertedBeforeMoving(actor, hexCoord)
					});
					return legionMoveEvent;
				}
				if (actor.LastMoveTurn == context.CurrentTurn.TurnValue)
				{
					legionMoveEvent.AddChildEvent<BlockedStep>(new BlockedStep(actor.ControllingPlayerId, actor.Id, actor.Location, IEnumerableExtensions.LastOrDefault<HexCoord>(steps, ref HexCoord.Invalid), mode)
					{
						Reason = Result.AlreadyMovedThisTurn(actor, hexCoord)
					});
					return legionMoveEvent;
				}
			}
			int num = actor.GroundMoveDistance.Value;
			HexCoord startCoord = actor.Location;
			HashSet<HexCoord> territory = context.HexBoard.GetHexCoordsControlledByPlayer(actor.ControllingPlayerId).ToHashSet<HexCoord>();
			List<MoveStepEvent> list = new List<MoveStepEvent>();
			foreach (HexCoord hexCoord2 in steps)
			{
				if (!(hexCoord2 == actor.Location))
				{
					LegionMovementProcessor.MoveQuery moveQuery = LegionMovementProcessor.GetMoveQuery(actor, requestAttackOutcomeIntent, mode, systemMove, startCoord, hexCoord2, num, territory);
					LegionMovementProcessor.MoveQueryResult moveQueryResult = LegionMovementProcessor.DetermineMoveResult(context, moveQuery, null);
					num -= moveQueryResult.MoveCost;
					MoveStepEvent moveStepEvent = LegionMovementProcessor.Execute(context, moveQueryResult, moveQueryResult.MovePromise);
					if (moveStepEvent == null)
					{
						break;
					}
					list.Add(moveStepEvent);
					legionMoveEvent.AddChildEvent<MoveStepEvent>(moveStepEvent);
					if (moveQueryResult.PreventsFutureMovement || !moveQueryResult.Result.successful)
					{
						break;
					}
					if (moveStepEvent is BlockedStep)
					{
						break;
					}
					startCoord = hexCoord2;
				}
			}
			int controllingPlayerID = context.HexBoard[actor.Location].ControllingPlayerID;
			CantonClaimedEvent ev;
			if (LegionMovementProcessor.GetDiplomaticRelationship(context, actor, actor.Location, context.HexBoard).GetCantonCaptureRules(context.Diplomacy, actor.ControllingPlayerId, controllingPlayerID) == CantonCaptureRule.OnStop && context.TryCaptureRestedTile(actor, out ev))
			{
				GameEvent gameEvent = null;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					MoveStepEvent moveStepEvent2 = list[i];
					if (moveStepEvent2.Destination == actor.Location)
					{
						gameEvent = moveStepEvent2;
						break;
					}
				}
				if (gameEvent == null)
				{
					gameEvent = legionMoveEvent;
				}
				gameEvent.AddChildEvent<CantonClaimedEvent>(ev);
			}
			return legionMoveEvent;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0004CCD0 File Offset: 0x0004AED0
		public static LegionMovementProcessor.MoveQuery GetMoveQuery(GamePiece actor, AttackOutcomeIntent requestAttackOutcomeIntent, PathMode mode, bool systemMove, HexCoord startCoord, HexCoord coord, int movementPoints, HashSet<HexCoord> territory)
		{
			return new LegionMovementProcessor.MoveQuery
			{
				StartingCoord = startCoord,
				DestinationCoord = coord,
				GamePiece = actor,
				MovementMode = mode,
				MovementPointsRemaining = movementPoints,
				Territory = territory,
				SystemMove = systemMove,
				AttackOutcomeIntent = requestAttackOutcomeIntent
			};
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0004CD20 File Offset: 0x0004AF20
		public static MoveStepEvent Execute(TurnProcessContext context, LegionMovementProcessor.MoveQueryResult result, LegionMovementProcessor.MovePromise promise)
		{
			TurnState currentTurn = context.CurrentTurn;
			GamePiece gamePiece = result.OriginalQuery.GamePiece;
			LegionMovementProcessor.MoveQueryResultBattle moveQueryResultBattle = promise as LegionMovementProcessor.MoveQueryResultBattle;
			if (moveQueryResultBattle != null)
			{
				BattleEvent battleEvent = BattleProcessor.SimulateBattle(context, moveQueryResultBattle.PotentialBattle);
				gamePiece.LastBattleTurn = context.CurrentTurn.TurnValue;
				bool flag = battleEvent.BattleResult.Defender_EndState.Status != GameItemStatus.InPlay;
				MoveStepEvent moveStepEvent;
				if (battleEvent.BattleResult.DidWin(gamePiece) && (!battleEvent.IsVsPoP() || flag))
				{
					LegionMovementProcessor.MoveQueryResult moveQueryResult = LegionMovementProcessor.DetermineMoveResult(context, result.OriginalQuery, null);
					moveStepEvent = LegionMovementProcessor.Execute(context, moveQueryResult, moveQueryResult.MovePromise);
				}
				else if (battleEvent.BattleResult.Defender_StartState.IsFixture())
				{
					moveStepEvent = new EmptyMoveStep(result.OriginalQuery);
				}
				else
				{
					GamePiece attacker_EndState = battleEvent.BattleResult.Attacker_EndState;
					if (attacker_EndState != null && attacker_EndState.IsAlive())
					{
						moveStepEvent = new BlockedStep(result.OriginalQuery, Result.DidNotWinBattle(gamePiece, moveQueryResultBattle.PotentialBattle.Location, moveQueryResultBattle.PotentialBattle.Defender));
					}
					else
					{
						moveStepEvent = new BlockedStep(result.OriginalQuery, Result.BanishedBeforeMoving(gamePiece, moveQueryResultBattle.PotentialBattle.Location));
					}
				}
				moveStepEvent.AddChildEvent<BattleEvent>(battleEvent);
				return moveStepEvent;
			}
			LegionMovementProcessor.MoveQueryResultRedeployTo moveQueryResultRedeployTo = promise as LegionMovementProcessor.MoveQueryResultRedeployTo;
			if (moveQueryResultRedeployTo != null)
			{
				RedeployedEvent redeployedEvent = new RedeployedEvent(result.OriginalQuery.GamePiece.ControllingPlayerId, result.OriginalQuery.GamePiece, result.OriginalQuery.StartingCoord, result.OriginalQuery.DestinationCoord);
				redeployedEvent.Redeployer = moveQueryResultRedeployTo.Redeployer.Id;
				GameEvent ev = context.Place(result.OriginalQuery.GamePiece, result.OriginalQuery.DestinationCoord, result.CapturesCantonDuringMovement);
				redeployedEvent.AddChildEvent(ev);
				result.OriginalQuery.GamePiece.LastMoveTurn = context.CurrentTurn.TurnValue;
				return redeployedEvent;
			}
			if (promise is LegionMovementProcessor.MoveQueryResultMove)
			{
				object obj = (result.OriginalQuery.MovementMode == PathMode.March) ? new MarchedEvent(result.OriginalQuery) : new TeleportedEvent(result.OriginalQuery);
				GameEvent ev2 = context.Place(result.OriginalQuery.GamePiece, result.OriginalQuery.DestinationCoord, result.CapturesCantonDuringMovement);
				object obj2 = obj;
				obj2.AddChildEvent(ev2);
				result.OriginalQuery.GamePiece.LastMoveTurn = context.CurrentTurn.TurnValue;
				return obj2;
			}
			LegionMovementProcessor.MoveQueryResultInvalid moveQueryResultInvalid = promise as LegionMovementProcessor.MoveQueryResultInvalid;
			if (moveQueryResultInvalid != null)
			{
				return new BlockedStep(result.OriginalQuery, moveQueryResultInvalid.Result);
			}
			if (!(promise is LegionMovementProcessor.MoveQueryResultSecure))
			{
				return null;
			}
			return new EmptyMoveStep(result.OriginalQuery);
		}

		// Token: 0x0200096D RID: 2413
		public class MoveQuery
		{
			// Token: 0x04001629 RID: 5673
			public PathMode MovementMode;

			// Token: 0x0400162A RID: 5674
			public GamePiece GamePiece;

			// Token: 0x0400162B RID: 5675
			public HexCoord StartingCoord;

			// Token: 0x0400162C RID: 5676
			public HexCoord DestinationCoord;

			// Token: 0x0400162D RID: 5677
			public int MovementPointsRemaining;

			// Token: 0x0400162E RID: 5678
			public bool SystemMove;

			// Token: 0x0400162F RID: 5679
			public HashSet<HexCoord> Territory = new HashSet<HexCoord>();

			// Token: 0x04001630 RID: 5680
			public AttackOutcomeIntent AttackOutcomeIntent;

			// Token: 0x04001631 RID: 5681
			public bool IgnoreLegionOccupiedCantons;
		}

		// Token: 0x0200096E RID: 2414
		public sealed class MoveQueryResult
		{
			// Token: 0x17000641 RID: 1601
			// (get) Token: 0x06002C56 RID: 11350 RVA: 0x00090C95 File Offset: 0x0008EE95
			public Result Result
			{
				get
				{
					LegionMovementProcessor.MoveQueryResultInvalid moveQueryResultInvalid = this.MovePromise as LegionMovementProcessor.MoveQueryResultInvalid;
					return ((moveQueryResultInvalid != null) ? moveQueryResultInvalid.Result : null) ?? Result.Success;
				}
			}

			// Token: 0x17000642 RID: 1602
			// (get) Token: 0x06002C57 RID: 11351 RVA: 0x00090CB7 File Offset: 0x0008EEB7
			// (set) Token: 0x06002C58 RID: 11352 RVA: 0x00090CBF File Offset: 0x0008EEBF
			public AttackOutcomeIntent AttackOutcomeIntent { get; set; }

			// Token: 0x17000643 RID: 1603
			// (get) Token: 0x06002C59 RID: 11353 RVA: 0x00090CC8 File Offset: 0x0008EEC8
			public bool CapturesCantonDuringMovement
			{
				get
				{
					return this.Result.successful && this.CanCaptureCanton && this.CaptureRule == CantonCaptureRule.DuringMovement;
				}
			}

			// Token: 0x17000644 RID: 1604
			// (get) Token: 0x06002C5A RID: 11354 RVA: 0x00090CEA File Offset: 0x0008EEEA
			public bool CapturesCantonAtEndOfMovement
			{
				get
				{
					return this.Result.successful && this.CanCaptureCanton && this.CaptureRule == CantonCaptureRule.OnStop;
				}
			}

			// Token: 0x04001632 RID: 5682
			public LegionMovementProcessor.MoveQuery OriginalQuery;

			// Token: 0x04001634 RID: 5684
			public bool PreventsFutureMovement;

			// Token: 0x04001635 RID: 5685
			public bool CanEndMovementHere = true;

			// Token: 0x04001636 RID: 5686
			public int MoveCost;

			// Token: 0x04001637 RID: 5687
			public GamePiece Occupier;

			// Token: 0x04001638 RID: 5688
			public bool CanCaptureCanton;

			// Token: 0x04001639 RID: 5689
			public CantonCaptureRule CaptureRule;

			// Token: 0x0400163A RID: 5690
			public LegionMovementProcessor.MovePromise MovePromise;
		}

		// Token: 0x0200096F RID: 2415
		public abstract class MovePromise
		{
		}

		// Token: 0x02000970 RID: 2416
		public class MoveQueryResultMove : LegionMovementProcessor.MovePromise
		{
		}

		// Token: 0x02000971 RID: 2417
		public class MoveQueryResultRedeployTo : LegionMovementProcessor.MovePromise
		{
			// Token: 0x06002C5E RID: 11358 RVA: 0x00090D2B File Offset: 0x0008EF2B
			public MoveQueryResultRedeployTo(GamePiece redeployer)
			{
				this.Redeployer = redeployer;
			}

			// Token: 0x0400163B RID: 5691
			public GamePiece Redeployer;
		}

		// Token: 0x02000972 RID: 2418
		public class MoveQueryResultInvalid : LegionMovementProcessor.MovePromise
		{
			// Token: 0x06002C5F RID: 11359 RVA: 0x00090D3A File Offset: 0x0008EF3A
			public MoveQueryResultInvalid(Problem problem)
			{
				this.Result = problem;
			}

			// Token: 0x0400163C RID: 5692
			public Problem Result;
		}

		// Token: 0x02000973 RID: 2419
		public class MoveQueryResultSecure : LegionMovementProcessor.MovePromise
		{
		}

		// Token: 0x02000974 RID: 2420
		public class MoveQueryResultBattle : LegionMovementProcessor.MovePromise
		{
			// Token: 0x06002C61 RID: 11361 RVA: 0x00090D51 File Offset: 0x0008EF51
			public MoveQueryResultBattle(BattleContext potentialBattle)
			{
				this.PotentialBattle = potentialBattle;
			}

			// Token: 0x0400163D RID: 5693
			public BattleContext PotentialBattle;
		}

		// Token: 0x02000975 RID: 2421
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public abstract class MovementProblem : Problem
		{
			// Token: 0x17000645 RID: 1605
			// (get) Token: 0x06002C62 RID: 11362 RVA: 0x00090D60 File Offset: 0x0008EF60
			public virtual LegionMovementProcessor.MovementProblemType ProblemType
			{
				get
				{
					return LegionMovementProcessor.MovementProblemType.Unknown;
				}
			}

			// Token: 0x17000646 RID: 1606
			// (get) Token: 0x06002C63 RID: 11363 RVA: 0x00090D63 File Offset: 0x0008EF63
			protected override string LocKeyScope
			{
				get
				{
					return "Result.Movement";
				}
			}

			// Token: 0x06002C64 RID: 11364 RVA: 0x00090D6A File Offset: 0x0008EF6A
			[JsonConstructor]
			protected MovementProblem()
			{
			}

			// Token: 0x06002C65 RID: 11365 RVA: 0x00090D72 File Offset: 0x0008EF72
			protected MovementProblem(int blockedPlayerID, Identifier blockedLegionId, HexCoord blockingHexCoord)
			{
				this.BlockedPlayerID = blockedPlayerID;
				this.BlockedLegionId = blockedLegionId;
				this.BlockingHexCoord = blockingHexCoord;
			}

			// Token: 0x17000647 RID: 1607
			// (get) Token: 0x06002C66 RID: 11366 RVA: 0x00090D8F File Offset: 0x0008EF8F
			public override string DebugString
			{
				get
				{
					return string.Format("{0} could not enter {1}", this.BlockedLegionId, this.BlockingHexCoord);
				}
			}

			// Token: 0x0400163E RID: 5694
			[JsonProperty]
			[BindableValue("source_name", BindingOption.IntPlayerId)]
			public int BlockedPlayerID;

			// Token: 0x0400163F RID: 5695
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public Identifier BlockedLegionId;

			// Token: 0x04001640 RID: 5696
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public HexCoord BlockingHexCoord;
		}

		// Token: 0x02000976 RID: 2422
		public enum MovementProblemType
		{
			// Token: 0x04001642 RID: 5698
			Unknown,
			// Token: 0x04001643 RID: 5699
			InvalidMove,
			// Token: 0x04001644 RID: 5700
			Battle,
			// Token: 0x04001645 RID: 5701
			Warning,
			// Token: 0x04001646 RID: 5702
			CantonCapturable,
			// Token: 0x04001647 RID: 5703
			CantonUncapturable,
			// Token: 0x04001648 RID: 5704
			Redeploy,
			// Token: 0x04001649 RID: 5705
			Secure
		}

		// Token: 0x02000977 RID: 2423
		[Serializable]
		public class OccupiedCantonProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x17000648 RID: 1608
			// (get) Token: 0x06002C67 RID: 11367 RVA: 0x00090DB1 File Offset: 0x0008EFB1
			public override LegionMovementProcessor.MovementProblemType ProblemType
			{
				get
				{
					return LegionMovementProcessor.MovementProblemType.Warning;
				}
			}

			// Token: 0x06002C68 RID: 11368 RVA: 0x00090DB4 File Offset: 0x0008EFB4
			[JsonConstructor]
			protected OccupiedCantonProblem()
			{
			}

			// Token: 0x06002C69 RID: 11369 RVA: 0x00090DBC File Offset: 0x0008EFBC
			public OccupiedCantonProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, Identifier blockingGamePieceId) : base(blockedPlayerId, blockedLegionId, blockingHexCoord)
			{
				this.BlockingGamePieceID = blockingGamePieceId;
			}

			// Token: 0x17000649 RID: 1609
			// (get) Token: 0x06002C6A RID: 11370 RVA: 0x00090DCF File Offset: 0x0008EFCF
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because it was blocked by {0}", this.BlockingGamePieceID);
				}
			}

			// Token: 0x1700064A RID: 1610
			// (get) Token: 0x06002C6B RID: 11371 RVA: 0x00090DF1 File Offset: 0x0008EFF1
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".OccupiedCanton";
				}
			}

			// Token: 0x0400164A RID: 5706
			[JsonProperty]
			[BindableValue("blocker", BindingOption.None)]
			public Identifier BlockingGamePieceID;
		}

		// Token: 0x02000978 RID: 2424
		[Serializable]
		public class CannotTeleportIntoCombatProblem : LegionMovementProcessor.OccupiedCantonProblem
		{
			// Token: 0x06002C6C RID: 11372 RVA: 0x00090E03 File Offset: 0x0008F003
			[JsonConstructor]
			protected CannotTeleportIntoCombatProblem()
			{
			}

			// Token: 0x06002C6D RID: 11373 RVA: 0x00090E0B File Offset: 0x0008F00B
			public CannotTeleportIntoCombatProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, Identifier blockingGamePieceId) : base(blockedPlayerId, blockedLegionId, blockingHexCoord, blockingGamePieceId)
			{
			}

			// Token: 0x1700064B RID: 1611
			// (get) Token: 0x06002C6E RID: 11374 RVA: 0x00090E18 File Offset: 0x0008F018
			public override string DebugString
			{
				get
				{
					return base.DebugString + " and teleporting into combat is not possible";
				}
			}

			// Token: 0x1700064C RID: 1612
			// (get) Token: 0x06002C6F RID: 11375 RVA: 0x00090E2A File Offset: 0x0008F02A
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CannotTeleportIntoCombat";
				}
			}
		}

		// Token: 0x02000979 RID: 2425
		[Serializable]
		public class DidNotWinBattleProblem : LegionMovementProcessor.OccupiedCantonProblem
		{
			// Token: 0x06002C70 RID: 11376 RVA: 0x00090E3C File Offset: 0x0008F03C
			[JsonConstructor]
			protected DidNotWinBattleProblem()
			{
			}

			// Token: 0x06002C71 RID: 11377 RVA: 0x00090E44 File Offset: 0x0008F044
			public DidNotWinBattleProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, Identifier blockingGamePieceId) : base(blockedPlayerId, blockedLegionId, blockingHexCoord, blockingGamePieceId)
			{
			}

			// Token: 0x1700064D RID: 1613
			// (get) Token: 0x06002C72 RID: 11378 RVA: 0x00090E51 File Offset: 0x0008F051
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".DidNotWinBattle";
				}
			}

			// Token: 0x1700064E RID: 1614
			// (get) Token: 0x06002C73 RID: 11379 RVA: 0x00090E63 File Offset: 0x0008F063
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because its battle ended in a stalemate";
				}
			}
		}

		// Token: 0x0200097A RID: 2426
		[Serializable]
		public class NotEnoughMovementPointsProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x06002C74 RID: 11380 RVA: 0x00090E75 File Offset: 0x0008F075
			[JsonConstructor]
			protected NotEnoughMovementPointsProblem()
			{
			}

			// Token: 0x06002C75 RID: 11381 RVA: 0x00090E7D File Offset: 0x0008F07D
			public NotEnoughMovementPointsProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord) : base(blockedPlayerId, blockedLegionId, blockingHexCoord)
			{
			}

			// Token: 0x1700064F RID: 1615
			// (get) Token: 0x06002C76 RID: 11382 RVA: 0x00090E88 File Offset: 0x0008F088
			public override string DebugString
			{
				get
				{
					return string.Format("{0} didn't have enough movement points to enter {1}", this.BlockedLegionId, this.BlockingHexCoord);
				}
			}

			// Token: 0x17000650 RID: 1616
			// (get) Token: 0x06002C77 RID: 11383 RVA: 0x00090EAA File Offset: 0x0008F0AA
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NotEnoughMovementPoints";
				}
			}
		}

		// Token: 0x0200097B RID: 2427
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class DiplomaticMovementProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x17000651 RID: 1617
			// (get) Token: 0x06002C78 RID: 11384 RVA: 0x00090EBC File Offset: 0x0008F0BC
			public override LegionMovementProcessor.MovementProblemType ProblemType
			{
				get
				{
					return LegionMovementProcessor.MovementProblemType.InvalidMove;
				}
			}

			// Token: 0x06002C79 RID: 11385 RVA: 0x00090EBF File Offset: 0x0008F0BF
			[JsonConstructor]
			protected DiplomaticMovementProblem()
			{
			}

			// Token: 0x06002C7A RID: 11386 RVA: 0x00090EC7 File Offset: 0x0008F0C7
			public DiplomaticMovementProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, int blockingPlayerID) : base(blockedPlayerId, blockedLegionId, blockingHexCoord)
			{
				this.BlockingPlayerID = blockingPlayerID;
			}

			// Token: 0x17000652 RID: 1618
			// (get) Token: 0x06002C7B RID: 11387 RVA: 0x00090EDA File Offset: 0x0008F0DA
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because of your relationship with {0}", this.BlockingPlayerID);
				}
			}

			// Token: 0x0400164B RID: 5707
			[JsonProperty]
			[BindableValue("affected_name", BindingOption.IntPlayerId)]
			public int BlockingPlayerID;
		}

		// Token: 0x0200097C RID: 2428
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class NoRightOfEntryProblem : LegionMovementProcessor.DiplomaticMovementProblem
		{
			// Token: 0x06002C7C RID: 11388 RVA: 0x00090EFC File Offset: 0x0008F0FC
			[JsonConstructor]
			protected NoRightOfEntryProblem()
			{
			}

			// Token: 0x06002C7D RID: 11389 RVA: 0x00090F04 File Offset: 0x0008F104
			public NoRightOfEntryProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, int blockingPlayerID) : base(blockedPlayerId, blockedLegionId, blockingHexCoord, blockingPlayerID)
			{
			}

			// Token: 0x17000653 RID: 1619
			// (get) Token: 0x06002C7E RID: 11390 RVA: 0x00090F11 File Offset: 0x0008F111
			public override string DebugString
			{
				get
				{
					return base.DebugString + " which forbids trespassing";
				}
			}

			// Token: 0x17000654 RID: 1620
			// (get) Token: 0x06002C7F RID: 11391 RVA: 0x00090F23 File Offset: 0x0008F123
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NoRightOfEntry";
				}
			}
		}

		// Token: 0x0200097D RID: 2429
		public class NoRightOfCombatProblem : LegionMovementProcessor.DiplomaticMovementProblem
		{
			// Token: 0x06002C80 RID: 11392 RVA: 0x00090F35 File Offset: 0x0008F135
			[JsonConstructor]
			protected NoRightOfCombatProblem()
			{
			}

			// Token: 0x06002C81 RID: 11393 RVA: 0x00090F3D File Offset: 0x0008F13D
			public NoRightOfCombatProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, Identifier blockingGamePieceID, int blockingPlayerID) : base(blockedPlayerId, blockedLegionId, blockingHexCoord, blockingPlayerID)
			{
				this.BlockingGamePieceID = blockingGamePieceID;
			}

			// Token: 0x17000655 RID: 1621
			// (get) Token: 0x06002C82 RID: 11394 RVA: 0x00090F52 File Offset: 0x0008F152
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" which forbids fighting {0}", this.BlockingGamePieceID);
				}
			}

			// Token: 0x17000656 RID: 1622
			// (get) Token: 0x06002C83 RID: 11395 RVA: 0x00090F74 File Offset: 0x0008F174
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NoRightOfCombat";
				}
			}

			// Token: 0x0400164C RID: 5708
			[JsonProperty]
			[BindableValue("blocker", BindingOption.None)]
			public Identifier BlockingGamePieceID;
		}

		// Token: 0x0200097E RID: 2430
		public class NoRightOfEliminationProblem : LegionMovementProcessor.DiplomaticMovementProblem
		{
			// Token: 0x06002C84 RID: 11396 RVA: 0x00090F86 File Offset: 0x0008F186
			[JsonConstructor]
			protected NoRightOfEliminationProblem()
			{
			}

			// Token: 0x06002C85 RID: 11397 RVA: 0x00090F8E File Offset: 0x0008F18E
			public NoRightOfEliminationProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, int blockingPlayerID) : base(blockedPlayerId, blockedLegionId, blockingHexCoord, blockingPlayerID)
			{
			}

			// Token: 0x17000657 RID: 1623
			// (get) Token: 0x06002C86 RID: 11398 RVA: 0x00090F9B File Offset: 0x0008F19B
			public override string DebugString
			{
				get
				{
					return base.DebugString + " which forbids elimination";
				}
			}

			// Token: 0x17000658 RID: 1624
			// (get) Token: 0x06002C87 RID: 11399 RVA: 0x00090FAD File Offset: 0x0008F1AD
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".NoRightOfElimination";
				}
			}
		}

		// Token: 0x0200097F RID: 2431
		[Serializable]
		public class AlreadyMovedThisTurnProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x06002C88 RID: 11400 RVA: 0x00090FBF File Offset: 0x0008F1BF
			[JsonConstructor]
			protected AlreadyMovedThisTurnProblem()
			{
			}

			// Token: 0x06002C89 RID: 11401 RVA: 0x00090FC7 File Offset: 0x0008F1C7
			public AlreadyMovedThisTurnProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord destination) : base(blockedPlayerId, blockedLegionId, destination)
			{
			}

			// Token: 0x17000659 RID: 1625
			// (get) Token: 0x06002C8A RID: 11402 RVA: 0x00090FD2 File Offset: 0x0008F1D2
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it has already moved this turn";
				}
			}

			// Token: 0x1700065A RID: 1626
			// (get) Token: 0x06002C8B RID: 11403 RVA: 0x00090FE4 File Offset: 0x0008F1E4
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".AlreadyMovedThisTurn";
				}
			}
		}

		// Token: 0x02000980 RID: 2432
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class ImpassableTerrainProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x1700065B RID: 1627
			// (get) Token: 0x06002C8C RID: 11404 RVA: 0x00090FF6 File Offset: 0x0008F1F6
			public override LegionMovementProcessor.MovementProblemType ProblemType
			{
				get
				{
					return LegionMovementProcessor.MovementProblemType.InvalidMove;
				}
			}

			// Token: 0x06002C8D RID: 11405 RVA: 0x00090FF9 File Offset: 0x0008F1F9
			[JsonConstructor]
			protected ImpassableTerrainProblem()
			{
			}

			// Token: 0x06002C8E RID: 11406 RVA: 0x00091001 File Offset: 0x0008F201
			public ImpassableTerrainProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, TerrainType blockingTerrainType) : base(blockedPlayerId, blockedLegionId, blockingHexCoord)
			{
				this.BlockingTerrainType = blockingTerrainType;
			}

			// Token: 0x1700065C RID: 1628
			// (get) Token: 0x06002C8F RID: 11407 RVA: 0x00091014 File Offset: 0x0008F214
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".ImpassableTerrain";
				}
			}

			// Token: 0x1700065D RID: 1629
			// (get) Token: 0x06002C90 RID: 11408 RVA: 0x00091026 File Offset: 0x0008F226
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because {0} is impassible", this.BlockingTerrainType);
				}
			}

			// Token: 0x0400164D RID: 5709
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public TerrainType BlockingTerrainType;
		}

		// Token: 0x02000981 RID: 2433
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class TerrainForbidsTeleportProblem : LegionMovementProcessor.ImpassableTerrainProblem
		{
			// Token: 0x06002C91 RID: 11409 RVA: 0x00091048 File Offset: 0x0008F248
			[JsonConstructor]
			protected TerrainForbidsTeleportProblem()
			{
			}

			// Token: 0x06002C92 RID: 11410 RVA: 0x00091050 File Offset: 0x0008F250
			public TerrainForbidsTeleportProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord blockingHexCoord, TerrainType blockingTerrainType) : base(blockedPlayerId, blockedLegionId, blockingHexCoord, blockingTerrainType)
			{
			}

			// Token: 0x1700065E RID: 1630
			// (get) Token: 0x06002C93 RID: 11411 RVA: 0x0009105D File Offset: 0x0008F25D
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".TerrainForbidsTeleport";
				}
			}

			// Token: 0x1700065F RID: 1631
			// (get) Token: 0x06002C94 RID: 11412 RVA: 0x0009106F File Offset: 0x0008F26F
			public override string DebugString
			{
				get
				{
					return base.DebugString + " to teleport moves";
				}
			}
		}

		// Token: 0x02000982 RID: 2434
		[Serializable]
		public class InvalidTransitionProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x06002C95 RID: 11413 RVA: 0x00091081 File Offset: 0x0008F281
			[JsonConstructor]
			protected InvalidTransitionProblem()
			{
			}

			// Token: 0x06002C96 RID: 11414 RVA: 0x00091089 File Offset: 0x0008F289
			public InvalidTransitionProblem(int blockedPlayerId, Identifier blockedLegion, HexCoord startingHex, HexCoord destinationHex) : base(blockedPlayerId, blockedLegion, destinationHex)
			{
				this.StartingHex = startingHex;
			}

			// Token: 0x17000660 RID: 1632
			// (get) Token: 0x06002C97 RID: 11415 RVA: 0x0009109C File Offset: 0x0008F29C
			public override string DebugString
			{
				get
				{
					return base.ToString() + string.Format(" because {0} is too far away", this.StartingHex);
				}
			}

			// Token: 0x0400164E RID: 5710
			[JsonProperty]
			public HexCoord StartingHex;
		}

		// Token: 0x02000983 RID: 2435
		[Serializable]
		public class BanishedBeforeMovingProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x06002C98 RID: 11416 RVA: 0x000910BE File Offset: 0x0008F2BE
			[JsonConstructor]
			protected BanishedBeforeMovingProblem()
			{
			}

			// Token: 0x06002C99 RID: 11417 RVA: 0x000910C6 File Offset: 0x0008F2C6
			public BanishedBeforeMovingProblem(int blockedPlayerId, Identifier destroyedLegionId, HexCoord blockingHexCoord) : base(blockedPlayerId, destroyedLegionId, blockingHexCoord)
			{
			}

			// Token: 0x17000661 RID: 1633
			// (get) Token: 0x06002C9A RID: 11418 RVA: 0x000910D1 File Offset: 0x0008F2D1
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".BanishedBeforeMoving";
				}
			}

			// Token: 0x17000662 RID: 1634
			// (get) Token: 0x06002C9B RID: 11419 RVA: 0x000910E3 File Offset: 0x0008F2E3
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because it was banished";
				}
			}
		}

		// Token: 0x02000984 RID: 2436
		[Serializable]
		public class ConvertedBeforeMovingProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x06002C9C RID: 11420 RVA: 0x000910F5 File Offset: 0x0008F2F5
			[JsonConstructor]
			protected ConvertedBeforeMovingProblem()
			{
			}

			// Token: 0x06002C9D RID: 11421 RVA: 0x000910FD File Offset: 0x0008F2FD
			public ConvertedBeforeMovingProblem(int blockedPlayerId, Identifier convertedLegionId, HexCoord blockingHexCoord) : base(blockedPlayerId, convertedLegionId, blockingHexCoord)
			{
			}

			// Token: 0x17000663 RID: 1635
			// (get) Token: 0x06002C9E RID: 11422 RVA: 0x00091108 File Offset: 0x0008F308
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".ConvertedBeforeMoving";
				}
			}

			// Token: 0x17000664 RID: 1636
			// (get) Token: 0x06002C9F RID: 11423 RVA: 0x0009111A File Offset: 0x0008F31A
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because a different player had assumed control of it";
				}
			}
		}

		// Token: 0x02000985 RID: 2437
		[Serializable]
		public class OutsideTeleportRangeProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x06002CA0 RID: 11424 RVA: 0x0009112C File Offset: 0x0008F32C
			[JsonConstructor]
			protected OutsideTeleportRangeProblem()
			{
			}

			// Token: 0x06002CA1 RID: 11425 RVA: 0x00091134 File Offset: 0x0008F334
			public OutsideTeleportRangeProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord destination, int attemptedTeleportDistance, int maxTeleportRange) : base(blockedPlayerId, blockedLegionId, destination)
			{
				this.AttemptedTeleportDistance = attemptedTeleportDistance;
				this.MaximumTeleportRange = maxTeleportRange;
			}

			// Token: 0x17000665 RID: 1637
			// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x0009114F File Offset: 0x0008F34F
			public override string DebugString
			{
				get
				{
					return base.DebugString + string.Format(" because it is beyond its teleport range ({0} > {1}", this.AttemptedTeleportDistance, this.MaximumTeleportRange);
				}
			}

			// Token: 0x17000666 RID: 1638
			// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x0009117C File Offset: 0x0008F37C
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".OutsideTeleportRange";
				}
			}

			// Token: 0x0400164F RID: 5711
			[JsonProperty]
			[BindableValue("distance", BindingOption.None)]
			public int AttemptedTeleportDistance;

			// Token: 0x04001650 RID: 5712
			[JsonProperty]
			[BindableValue(null, BindingOption.None)]
			public int MaximumTeleportRange;
		}

		// Token: 0x02000986 RID: 2438
		[Serializable]
		public class DoesNotHaveTeleportProblem : LegionMovementProcessor.MovementProblem
		{
			// Token: 0x06002CA4 RID: 11428 RVA: 0x0009118E File Offset: 0x0008F38E
			[JsonConstructor]
			protected DoesNotHaveTeleportProblem()
			{
			}

			// Token: 0x06002CA5 RID: 11429 RVA: 0x00091196 File Offset: 0x0008F396
			public DoesNotHaveTeleportProblem(int blockedPlayerId, Identifier blockedLegionId, HexCoord destination) : base(blockedPlayerId, blockedLegionId, destination)
			{
			}

			// Token: 0x17000667 RID: 1639
			// (get) Token: 0x06002CA6 RID: 11430 RVA: 0x000911A1 File Offset: 0x0008F3A1
			public override string DebugString
			{
				get
				{
					return base.DebugString + " because does not have the ability to teleport";
				}
			}

			// Token: 0x17000668 RID: 1640
			// (get) Token: 0x06002CA7 RID: 11431 RVA: 0x000911B3 File Offset: 0x0008F3B3
			public override string LocKey
			{
				get
				{
					return base.LocKeyScope + ".DoesNotHaveTeleport";
				}
			}
		}
	}
}
