using System;

namespace LoG
{
	// Token: 0x02000158 RID: 344
	public class WPCanEliminate : WorldProperty<WPCanEliminate>
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0002181F File Offset: 0x0001FA1F
		public override bool MustBeSuccessfullFulfilledAsPrecondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00021822 File Offset: 0x0001FA22
		public bool Anybody
		{
			get
			{
				return this._playerId == int.MinValue;
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00021831 File Offset: 0x0001FA31
		public WPCanEliminate(int playerID = -2147483648)
		{
			this._playerId = playerID;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00021840 File Offset: 0x0001FA40
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			if (playerState.Excommunicated || this.OwningPlanner.PlayerState.Excommunicated)
			{
				return true;
			}
			TurnState currentTurn = viewContext.CurrentTurn;
			if (!this.Anybody)
			{
				return currentTurn.StrongholdCaptureAuthorizedBetween(playerState.Id, this._playerId);
			}
			return currentTurn.StrongholdCaptureAuthorizedWithAny(playerState.Id);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00021897 File Offset: 0x0001FA97
		public override WPProvidesEffect ProvidesEffectInternal(WPCanEliminate eliminatePrecondition)
		{
			if (!this.Anybody && !eliminatePrecondition.Anybody && this._playerId != eliminatePrecondition._playerId)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400030F RID: 783
		private int _playerId;
	}
}
