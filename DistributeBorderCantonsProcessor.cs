using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000713 RID: 1811
	public class DistributeBorderCantonsProcessor : EdictEffectModuleProcessor<DistributeBorderCantonsInstance, DistributeBorderCantonsEffectStaticData>
	{
		// Token: 0x06002292 RID: 8850 RVA: 0x00078511 File Offset: 0x00076711
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x0007852C File Offset: 0x0007672C
		public void OnTurnEnd()
		{
			HexBoard board = base._currentTurn.HexBoard;
			Func<HexCoord, int> <>9__1;
			Func<int, bool> <>9__2;
			foreach (Hex hex2 in IEnumerableExtensions.ToList<Hex>(from hex in board.GetHexesControlledByPlayer(base.Instance.TargetPlayerId)
			where board.IsBorderCanton(hex.HexCoord) && this._currentTurn.GetGamePieceAt(hex.HexCoord) == null
			select hex).SelectRandom(base._random, this._staticData.CantonCount))
			{
				IEnumerable<HexCoord> neighbours = base._currentTurn.HexBoard.GetNeighbours(hex2.HexCoord, false);
				Func<HexCoord, int> selector;
				if ((selector = <>9__1) == null)
				{
					selector = (<>9__1 = ((HexCoord x) => this._currentTurn.HexBoard.GetOwnership(x)));
				}
				IEnumerable<int> source = neighbours.Select(selector);
				Func<int, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((int x) => x != this.Instance.TargetPlayerId && x != -1));
				}
				int playerID;
				if (IEnumerableExtensions.ToList<int>(source.Where(predicate)).TryGetRandom(base._random, out playerID))
				{
					base._currentTurn.HexBoard.SetOwnership(hex2.HexCoord, playerID);
				}
			}
			base.RemoveSelf();
		}
	}
}
