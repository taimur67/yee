using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020000DE RID: 222
	public class ActionBidOnLegion : ActionOrderGOAPNode<OrderMakeBid>
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000E9D6 File Offset: 0x0000CBD6
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000E9D9 File Offset: 0x0000CBD9
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000E9DC File Offset: 0x0000CBDC
		public override string ActionName
		{
			get
			{
				return string.Format("Bid on legion: {0}, titan:{1}", base.Context.DebugName(this.LegionID), this._legionIsTitan);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000EA04 File Offset: 0x0000CC04
		public override ActionID ID
		{
			get
			{
				return ActionID.Bid_On_Legion;
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000EA08 File Offset: 0x0000CC08
		public ActionBidOnLegion(Identifier identifier, int roundsUpkeepSurplus = 5)
		{
			this.LegionID = identifier;
			this.RoundsUpkeepSurplus = roundsUpkeepSurplus;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000EA20 File Offset: 0x0000CC20
		public override void Prepare()
		{
			TurnState turn = this.OwningPlanner.PlayerViewOfTurnState;
			GamePiece gamePiece = turn.FetchGameItem<GamePiece>(this.LegionID);
			if (gamePiece == null)
			{
				base.Disable("Invalid game piece");
				return;
			}
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(from gp in this.OwningPlanner.PlayerViewOfTurnState.GetActiveGamePiecesForPlayer(this.OwningPlanner.PlayerId)
			where gp.SubCategory.IsLegion() && !gp.IsPersonalGuard(turn)
			select gp);
			if (gamePiece.SubCategory != GamePieceCategory.Titan && this.OwningPlanner.GameProgress >= 0.7f && list.Count >= 2)
			{
				if ((from slot in turn.BazaarState.BazaarSlots[GameItemCategory.GamePiece]
				select turn.FetchGameItem<GamePiece>(slot.CurrentItem)).Any((GamePiece gamePieceForSale) => gamePieceForSale != null && gamePieceForSale.SubCategory == GamePieceCategory.Titan))
				{
					base.Disable(string.Format("We already have {0} Legions, it's the end-game, and a Titan is available", list.Count));
					return;
				}
			}
			GamePiece gamePiece2 = turn.FetchGameItem<GamePiece>(this.OwningPlanner.PlayerState.PersonalGuardId);
			int num = (int)Math.Ceiling((double)((float)(list.Sum((GamePiece hireling) => hireling.Level) + ((gamePiece2 != null) ? gamePiece2.Level : 2)) / 2f));
			if (gamePiece.Level < num)
			{
				base.Disable(string.Format("{0} does not have at least level {1} so is too weak to buy", this.LegionID, num));
				return;
			}
			if (!gamePiece.UpkeepCost.IsZero && !this.OwningPlanner.PlayerState.HasTag<EntityTag_CheatNoUpkeep>())
			{
				Cost costSurplus = gamePiece.UpkeepCost * this.RoundsUpkeepSurplus;
				this.CostSurplus = costSurplus;
			}
			base.AddPrecondition(new WPSpawnPoint());
			base.AddPrecondition(new WPCommandRating(gamePiece.CommandCost));
			if (gamePiece.SubCategory == GamePieceCategory.Titan)
			{
				this._legionIsTitan = true;
				base.AddEffect(new WPHasTitan());
			}
			foreach (PlayerState playerState in turn.EnumeratePlayerStatesExcept(this.OwningPlanner.PlayerId, false, false))
			{
				base.AddEffect(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, playerState.Id, 0.5f));
			}
			base.Prepare();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		protected override OrderMakeBid GenerateOrder()
		{
			return new OrderMakeBid(this.LegionID);
		}

		// Token: 0x040001FC RID: 508
		public Identifier LegionID;

		// Token: 0x040001FD RID: 509
		public int RoundsUpkeepSurplus;

		// Token: 0x040001FE RID: 510
		private bool _legionIsTitan;
	}
}
