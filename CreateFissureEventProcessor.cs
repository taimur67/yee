using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005F3 RID: 1523
	public class CreateFissureEventProcessor : GrandEventActionProcessor<CreateFissureEventOrder, CreateFissureEventStaticData>
	{
		// Token: 0x06001C87 RID: 7303 RVA: 0x000625C0 File Offset: 0x000607C0
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			foreach (HexCoord coord in base.request.HexTargets)
			{
				base._currentTurn.HexBoard[coord].SetTerrainType(TerrainType.Ravine);
				foreach (GamePiece gamePiece in base._currentTurn.GetGamePiecesAtLocation(base._currentTurn.HexBoard.ToRelativeHex(coord)))
				{
					if (gamePiece.SubCategory == GamePieceCategory.Titan)
					{
						if (LegionMovementProcessor.RetreatMarchFrom(this.TurnProcessContext, gamePiece, gamePiece.Location).successful)
						{
							this.TurnProcessContext.RecalculateSupportModifiers(gamePiece.ControllingPlayerId);
						}
					}
					else
					{
						LegionKilledEvent ev = this.TurnProcessContext.KillGamePiece(gamePiece, this._player.Id);
						base.GameEvent.AddChildEvent<LegionKilledEvent>(ev);
					}
				}
			}
			return Result.Success;
		}
	}
}
