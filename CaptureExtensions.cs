using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020003E1 RID: 993
	public static class CaptureExtensions
	{
		// Token: 0x06001397 RID: 5015 RVA: 0x0004AB53 File Offset: 0x00048D53
		public static bool TryClaimCanton(this TurnProcessContext context, HexCoord hex, int playerId, out CantonClaimedEvent evt)
		{
			evt = context.ClaimCanton(hex, playerId);
			return evt != null;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0004AB64 File Offset: 0x00048D64
		public static CantonClaimedEvent ClaimCanton(this TurnProcessContext context, HexCoord hex, int playerId)
		{
			TurnState currentTurn = context.CurrentTurn;
			int ownership = currentTurn.HexBoard.GetOwnership(hex);
			if (ownership == playerId)
			{
				return null;
			}
			currentTurn.HexBoard.SetOwnership(hex, playerId);
			CantonClaimedEvent cantonClaimedEvent = new CantonClaimedEvent(hex, playerId, ownership);
			cantonClaimedEvent.AddChildEvent(CaptureExtensions.DaisyChainCantonCapture(context, playerId, hex));
			return cantonClaimedEvent;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0004ABB0 File Offset: 0x00048DB0
		public static bool TryCaptureRestedTile(this TurnProcessContext context, GamePiece actor, out CantonClaimedEvent evt)
		{
			HexCoord location = actor.Location;
			if (CaptureExtensions.CanCaptureWith(context, actor, location, context.HexBoard) && context.TryClaimCanton(location, actor.ControllingPlayerId, out evt))
			{
				return true;
			}
			evt = null;
			return false;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0004ABEA File Offset: 0x00048DEA
		public static IEnumerable<CantonClaimedEvent> DaisyChainCantonCapture(TurnProcessContext context, int playerId, HexCoord coord)
		{
			foreach (HexCoord coord2 in context.HexBoard.EnumerateNeighboursNormalized(coord))
			{
				GamePiece activeGamePieceAt = context.CurrentTurn.GetActiveGamePieceAt(coord2);
				CantonClaimedEvent cantonClaimedEvent;
				if (activeGamePieceAt != null && activeGamePieceAt.ControllingPlayerId == playerId && context.TryCaptureRestedTile(activeGamePieceAt, out cantonClaimedEvent))
				{
					yield return cantonClaimedEvent;
				}
			}
			IEnumerator<HexCoord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0004AC08 File Offset: 0x00048E08
		public static bool IsCapturableTileType(this TurnContext context, HexCoord coord)
		{
			return context.IsCapturableTileType(context.HexBoard[coord]);
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0004AC1C File Offset: 0x00048E1C
		public static bool IsCapturableTileType(this TurnContext context, Hex hex)
		{
			TerrainStaticData terrainStaticData;
			return context.Database.TryFindTerrainData(hex, out terrainStaticData) && terrainStaticData.Capturable;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0004AC41 File Offset: 0x00048E41
		public static bool IsCapturableBy(TurnContext context, PlayerState player, HexCoord coord, HexBoard hexBoard)
		{
			return CaptureExtensions.IsCapturableBy(context, player, hexBoard[coord]);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0004AC54 File Offset: 0x00048E54
		public static bool IsCapturableBy(TurnContext context, PlayerState player, Hex hex)
		{
			return hex.ControllingPlayerID != player.Id && player.Id != -1 && context.IsCapturableTileType(hex) && (hex.IsUnclaimed() || context.Diplomacy.GetDiplomaticStatus(player.Id, hex.ControllingPlayerID).DiplomaticState.AllowCantonCapture(context.Diplomacy, player.Id, hex.ControllingPlayerID));
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0004ACC7 File Offset: 0x00048EC7
		public static bool CanCaptureWith(TurnContext context, GamePiece piece, HexCoord coord, HexBoard hexBoard)
		{
			return CaptureExtensions.CanCapture(context, context.CurrentTurn.FindPlayerState(piece.ControllingPlayerId, null), piece, coord, hexBoard);
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0004ACE4 File Offset: 0x00048EE4
		public static bool CanCapture(TurnContext context, PlayerState player, GamePiece piece, HexCoord coord, HexBoard hexBoard)
		{
			return piece.CanCaptureCantons && piece.IsAlive() && CaptureExtensions.CanCapture(context, player, coord, hexBoard);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0004AD0C File Offset: 0x00048F0C
		public static bool CanCapture(TurnContext context, PlayerState player, HexCoord coord, HexBoard hexBoard)
		{
			if (!CaptureExtensions.IsCapturableBy(context, player, coord, hexBoard))
			{
				return false;
			}
			DiplomaticState diplomaticRelationship = LegionMovementProcessor.GetDiplomaticRelationship(context, player.Id, coord, hexBoard);
			int controllingPlayerID = hexBoard[coord].ControllingPlayerID;
			CantonCaptureRule cantonCaptureRules = diplomaticRelationship.GetCantonCaptureRules(context.Diplomacy, player.Id, controllingPlayerID);
			CantonCaptureRestrictions cantonCaptureRestrictions = diplomaticRelationship.GetCantonCaptureRestrictions(context.Diplomacy, player.Id, controllingPlayerID);
			return cantonCaptureRules != CantonCaptureRule.CannotCapture && (cantonCaptureRestrictions == CantonCaptureRestrictions.None || CaptureExtensions.DetermineMetCantonCaptureRestrictions(player, coord, cantonCaptureRestrictions, hexBoard) == cantonCaptureRestrictions) && diplomaticRelationship.AllowCantonCapture(context.Diplomacy, player.Id, controllingPlayerID);
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0004AD94 File Offset: 0x00048F94
		public static CantonCaptureRestrictions DetermineMetCantonCaptureRestrictions(PlayerState player, HexCoord coord, CantonCaptureRestrictions restrictions, HexBoard hexBoard)
		{
			CantonCaptureRestrictions cantonCaptureRestrictions = CantonCaptureRestrictions.None;
			if (restrictions.HasFlag(CantonCaptureRestrictions.SharedPerimeter) && hexBoard.CantonBordersPlayersRealm(player.Id, coord, false))
			{
				cantonCaptureRestrictions |= CantonCaptureRestrictions.SharedPerimeter;
			}
			return cantonCaptureRestrictions;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0004ADCC File Offset: 0x00048FCC
		public static IEnumerable<HexCoord> GetClaimableBorderCantons(this TurnContext context, PlayerState player, bool uniqueOnly = true, int ownedByPlayer = -2147483648)
		{
			IEnumerable<HexCoord> enumerable = from t in context.HexBoard.GetBorderCantons(player, uniqueOnly)
			where CaptureExtensions.IsCapturableBy(context, player, t, context.HexBoard)
			select t;
			if (ownedByPlayer != -2147483648)
			{
				enumerable = from t in enumerable
				where context.HexBoard[t].GetControllingPlayerID() == ownedByPlayer
				select t;
			}
			return enumerable;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0004AE3E File Offset: 0x0004903E
		public static int CalcFrontierCount(this TurnContext context, int playerID)
		{
			return context.CalcFrontierCount(context.HexBoard.GetHexesControlledByPlayer(playerID).ToHashSet<Hex>());
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0004AE58 File Offset: 0x00049058
		public static int CalcFrontierCount(this TurnContext context, HashSet<Hex> ownedHexes)
		{
			return (from t in ownedHexes.SelectMany((Hex t) => context.HexBoard.EnumerateNeighbours(t.HexCoord))
			where !ownedHexes.Contains(context.HexBoard[t])
			select t).Count(new Func<HexCoord, bool>(context.IsCapturableTileType));
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0004AEB8 File Offset: 0x000490B8
		public static int CalcFrontierCount(this TurnContext context, HashSet<HexCoord> ownedHexes)
		{
			return (from t in ownedHexes.SelectMany((HexCoord t) => context.HexBoard.EnumerateNeighboursNormalized(t))
			where !ownedHexes.Contains(t)
			select t).Count(new Func<HexCoord, bool>(context.IsCapturableTileType));
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0004AF18 File Offset: 0x00049118
		public static Task<float> CalculateTerritorialCohesionAsync(this TurnContext context, int playerID)
		{
			HashSet<Hex> ownedCantons = context.HexBoard.GetHexesControlledByPlayer(playerID).ToHashSet<Hex>();
			return context.CalculateTerritorialCohesionAsync(ownedCantons);
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0004AF40 File Offset: 0x00049140
		public static Task<float> CalculateTerritorialCohesionAsync(this TurnContext context, HashSet<Hex> ownedCantons)
		{
			int count = ownedCantons.Count;
			if (count == 0)
			{
				return Task.FromResult<float>(0f);
			}
			int enclaves = context.HexBoard.CalculateEnclaveCount(ownedCantons);
			int frontiers = context.CalcFrontierCount(ownedCantons);
			return Task.FromResult<float>(CaptureExtensions.CalculateTerritorialCohesion(count, frontiers, enclaves));
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x0004AF84 File Offset: 0x00049184
		public static float CalculateTerritorialCohesion(int cantons, int frontiers, int enclaves)
		{
			return (float)Math.Clamp(Math.Sqrt((double)((float)cantons / (float)frontiers / 2f / (float)enclaves)), 0.0, 1.0);
		}
	}
}
