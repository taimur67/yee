using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000607 RID: 1543
	public class NeutraliseGamePieceEventProcessor : GrandEventActionProcessor<NeutraliseGamePieceEventOrder, NeutraliseGamePieceEventStaticData>
	{
		// Token: 0x06001CB9 RID: 7353 RVA: 0x00063184 File Offset: 0x00061384
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(from x in base._currentTurn.GetActiveGamePieces()
			where x.SubCategory == GamePieceCategory.PoP
			where x.IsOwned()
			where x.ControllingPlayerId != this._player.Id
			where !x.HasTag<EntityTag_CannotBeNeutral>()
			select x);
			if (!IEnumerableExtensions.Any<GamePiece>(list))
			{
				return new NoValidTargetsProblem();
			}
			GamePiece random = list.GetRandom(base._random);
			GameItemOwnershipChanged ev = new GameItemOwnershipChanged(random.ControllingPlayerId, -1, random.Id, random.Category);
			base.GameEvent.AddChildEvent<GameItemOwnershipChanged>(ev);
			base._currentTurn.SetNeutral(random);
			return Result.Success;
		}
	}
}
