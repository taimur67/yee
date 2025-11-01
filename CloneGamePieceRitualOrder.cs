using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200065E RID: 1630
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class CloneGamePieceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E1B RID: 7707 RVA: 0x00067CD8 File Offset: 0x00065ED8
		public CloneGamePieceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x00067CE5 File Offset: 0x00065EE5
		public CloneGamePieceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x00067CF9 File Offset: 0x00065EF9
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			CloneGamePieceRitualData data = database.Fetch<CloneGamePieceRitualData>(base.RitualId);
			CloneCountEntry cloneCountEntry = data.CloneCount.Find((CloneCountEntry x) => x.PowerLevel == player.PowersLevels[data.Category]);
			int cloneCount = cloneCountEntry.CloneCount;
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			Action<HexCoord> <>9__2;
			int num;
			for (int i = 0; i < cloneCount; i = num + 1)
			{
				Action<HexCoord> setTarget;
				if ((setTarget = <>9__2) == null)
				{
					setTarget = (<>9__2 = delegate(HexCoord x)
					{
						this.CloneHexes.Add(x);
					});
				}
				yield return new ActionPhase_TargetHex(setTarget, new ActionPhase_Target<HexCoord>.IsValidFunc(this.IsValidCloneHex), 1);
				num = i;
			}
			yield break;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x00067D18 File Offset: 0x00065F18
		private Result IsValidCloneHex(TurnContext context, List<HexCoord> selected, HexCoord coord, int castingPlayerId)
		{
			TurnState currentTurn = context.CurrentTurn;
			CloneGamePieceRitualData cloneGamePieceRitualData = context.Database.Fetch<CloneGamePieceRitualData>(base.RitualId);
			GamePiece gamePiece = currentTurn.FetchGameItem<GamePiece>(base.TargetItemId);
			coord = context.HexBoard.ToRelativeHex(coord);
			if (!context.IsValidSpawnPoint(currentTurn.FindPlayerState(castingPlayerId, null), coord, null))
			{
				return Result.Failure;
			}
			if (this.CloneHexes.Contains(coord))
			{
				return Result.Failure;
			}
			if (coord.HexDistance(gamePiece.Location) > cloneGamePieceRitualData.CloneDeploymentDistance)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x04000CCD RID: 3277
		[JsonProperty]
		public List<HexCoord> CloneHexes = new List<HexCoord>();
	}
}
