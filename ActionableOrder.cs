using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004A0 RID: 1184
	[Serializable]
	public abstract class ActionableOrder : ISteppedFlowControl
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600161B RID: 5659
		[JsonProperty]
		public abstract OrderTypes OrderType { get; }

		// Token: 0x0600161C RID: 5660 RVA: 0x00052392 File Offset: 0x00050592
		protected ActionableOrder() : this(Guid.NewGuid())
		{
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0005239F File Offset: 0x0005059F
		public ActionableOrder(Guid id)
		{
			this.ActionInstanceId = id;
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600161E RID: 5662 RVA: 0x000523C0 File Offset: 0x000505C0
		public virtual bool NeedsTargetContext
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x000523C3 File Offset: 0x000505C3
		public virtual IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			return Enumerable.Empty<ActionConflict>();
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x000523CA File Offset: 0x000505CA
		public virtual Result ConflictProblem(ActionConflict conflict, ActionableOrder conflictingOrder)
		{
			return new OrderConflictProblem(conflict, this, conflictingOrder);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x000523D4 File Offset: 0x000505D4
		public virtual Result AbilityLockedProblem(PlayerState instigator, AbilityStaticData data)
		{
			return Result.AbilityLocked(data);
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x000523DC File Offset: 0x000505DC
		public virtual Problem PaymentFailedProblem(ConfigRef orderData, Problem paymentFailure)
		{
			return paymentFailure;
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x000523DF File Offset: 0x000505DF
		public virtual int GetCommandRatingCost(PlayerState user, TurnState turn, AbilityStaticData data, GameDatabase database)
		{
			return 0;
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x000523E2 File Offset: 0x000505E2
		public virtual IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x000523E9 File Offset: 0x000505E9
		public override string ToString()
		{
			return this.AbilityId + "(" + base.ToString() + ")";
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00052406 File Offset: 0x00050606
		public virtual Result TargetItemStolen(ConfigRef orderAbility, int originalOwner, GameItem stolenItem)
		{
			return Result.Failure;
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0005240D File Offset: 0x0005060D
		public virtual Result TargetItemBanished(ConfigRef orderAbility, GameItem banishedItem)
		{
			return Result.Failure;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00052414 File Offset: 0x00050614
		public virtual Result TargetItemInvalid(ConfigRef orderAbility, GameItem invalidItem)
		{
			return Result.Failure;
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0005241B File Offset: 0x0005061B
		public virtual Result TargetItemHasInvalidOwner(ConfigRef orderAbility, Problem invalidOwnerProblem, GameItem invalidItem)
		{
			return invalidOwnerProblem;
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0005241E File Offset: 0x0005061E
		public virtual Result TargetArchfiendInvalid(ConfigRef orderAbility, int targetPlayerID)
		{
			return Result.Failure;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00052425 File Offset: 0x00050625
		public virtual Result TargetArchfiendEliminated(ConfigRef orderAbility, int targetPlayerID)
		{
			return Result.Failure;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x0005242C File Offset: 0x0005062C
		public virtual Result TargetHexInvalid(ConfigRef orderAbility, HexCoord hexCoord)
		{
			return Result.Failure;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00052434 File Offset: 0x00050634
		public Result IsValidTarget(TurnContext context, int triggeringPlayer, TargetContext targetContext)
		{
			if (!targetContext.IsAnyTargetSet)
			{
				return Result.Failure;
			}
			HexCoord hexCoord;
			if (targetContext.GetTargetHex(out hexCoord))
			{
				Problem problem = this.IsValidHex(context, null, hexCoord, triggeringPlayer) as Problem;
				if (problem != null)
				{
					return problem;
				}
			}
			Identifier identifier;
			if (targetContext.GetTargetItem(out identifier))
			{
				GamePiece gamePiece;
				if (context.CurrentTurn.TryFetchGameItem<GamePiece>(identifier, out gamePiece))
				{
					Problem problem2 = this.IsValidGamePiece(context, null, gamePiece, triggeringPlayer) as Problem;
					if (problem2 != null)
					{
						return problem2;
					}
				}
				else
				{
					Problem problem3 = this.IsValidGameItem(context, null, identifier, triggeringPlayer) as Problem;
					if (problem3 != null)
					{
						return problem3;
					}
				}
			}
			PlayerIndex targetPlayerId;
			if (targetContext.GetTargetPlayer(out targetPlayerId))
			{
				Problem problem4 = this.IsValidArchfiend(context, (int)targetPlayerId, triggeringPlayer) as Problem;
				if (problem4 != null)
				{
					return problem4;
				}
			}
			return Result.Success;
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x000524E0 File Offset: 0x000506E0
		public virtual Result IsValidHex(TurnContext context, List<HexCoord> selected, HexCoord hexCoord, int triggerPlayerId)
		{
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (dataForRequest == null)
			{
				return new SimulationError("Expected AbilityStaticData data but could not find it.");
			}
			if (hexCoord == HexCoord.Invalid)
			{
				return this.TargetHexInvalid(dataForRequest.ConfigRef, hexCoord);
			}
			return Result.Success;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x00052524 File Offset: 0x00050724
		public virtual Result IsValidArchfiend(TurnContext context, int targetPlayerId, int triggeringPlayerId)
		{
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (dataForRequest == null)
			{
				return new SimulationError("Expected AbilityStaticData data but could not find it.");
			}
			PlayerState playerState = context.CurrentTurn.FindPlayerState(targetPlayerId, null);
			if (playerState == null)
			{
				return this.TargetArchfiendInvalid(dataForRequest.ConfigRef, targetPlayerId);
			}
			if (playerState.Eliminated)
			{
				return this.TargetArchfiendEliminated(dataForRequest.ConfigRef, targetPlayerId);
			}
			return Result.Success;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00052584 File Offset: 0x00050784
		public virtual Result IsValidGamePiece(TurnContext context, List<GamePiece> selected, GamePiece gamePiece, int triggeringPlayerId)
		{
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (dataForRequest == null)
			{
				return new SimulationError("Expected AbilityStaticData data but could not find it.");
			}
			switch (gamePiece.Status)
			{
			case GameItemStatus.InPlay:
			{
				Problem problem = this.IsValidArchfiend(context, gamePiece.ControllingPlayerId, triggeringPlayerId) as Problem;
				if (problem != null)
				{
					return this.TargetItemHasInvalidOwner(dataForRequest.ConfigRef, problem, gamePiece);
				}
				return Result.Success;
			}
			case GameItemStatus.Banished:
				return this.TargetItemBanished(dataForRequest.ConfigRef, gamePiece);
			}
			return this.TargetItemInvalid(dataForRequest.ConfigRef, gamePiece);
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001631 RID: 5681 RVA: 0x00052614 File Offset: 0x00050814
		protected virtual bool GameItemValidationRequiresInPlay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00052618 File Offset: 0x00050818
		public virtual Result IsValidGameItem(TurnContext context, List<Identifier> selected, Identifier gameItemId, int triggeringPlayerId)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameItem gameItem;
			if (!currentTurn.TryFetchGameItem<GameItem>(gameItemId, out gameItem))
			{
				return Result.InvalidItem(gameItemId);
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (dataForRequest == null)
			{
				return new SimulationError("Expected AbilityStaticData data but could not find it.");
			}
			if (gameItem.Status == GameItemStatus.Banished)
			{
				return this.TargetItemBanished(dataForRequest.ConfigRef, gameItem);
			}
			if (this.GameItemValidationRequiresInPlay && gameItem.Status != GameItemStatus.InPlay)
			{
				return this.TargetItemInvalid(dataForRequest.ConfigRef, gameItem);
			}
			if (!(gameItem is AbilityPlaceholder) && !(gameItem is Stratagem))
			{
				PlayerState playerState = currentTurn.FindControllingPlayer(gameItem);
				if (playerState == null || playerState.Id == -2147483648)
				{
					return new SimulationError(string.Format("Could not find controlling player of item {0}", gameItem));
				}
				Problem problem = this.IsValidArchfiend(context, playerState.Id, triggeringPlayerId) as Problem;
				if (problem != null)
				{
					return this.TargetItemHasInvalidOwner(dataForRequest.ConfigRef, problem, gameItem);
				}
			}
			return Result.Success;
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x000526F2 File Offset: 0x000508F2
		public virtual IEnumerable<ResourceNFT> GetReservedResources()
		{
			return this.Payment.Resources;
		}

		// Token: 0x04000B0C RID: 2828
		[JsonProperty]
		public Guid ActionInstanceId;

		// Token: 0x04000B0D RID: 2829
		[JsonProperty]
		public string AbilityId;

		// Token: 0x04000B0E RID: 2830
		[JsonProperty]
		public Payment Payment = new Payment();

		// Token: 0x04000B0F RID: 2831
		[JsonProperty]
		public ActionOrderPriority Priority = ActionOrderPriority.Normal_DontCare;
	}
}
