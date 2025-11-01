using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006CB RID: 1739
	[Serializable]
	public abstract class TargetedActionableOrder : ActionableOrder, ITarget
	{
		// Token: 0x06001FC6 RID: 8134 RVA: 0x0006D3FF File Offset: 0x0006B5FF
		protected TargetedActionableOrder()
		{
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0006D412 File Offset: 0x0006B612
		protected TargetedActionableOrder(Guid id) : base(id)
		{
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x0006D426 File Offset: 0x0006B626
		[JsonIgnore]
		public int TargetPlayerId
		{
			get
			{
				return this.TargetContext.PlayerId;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x0006D433 File Offset: 0x0006B633
		[JsonIgnore]
		public Identifier TargetItemId
		{
			get
			{
				return this.TargetContext.ItemId;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x0006D440 File Offset: 0x0006B640
		[JsonIgnore]
		public HexCoord TargetHex
		{
			get
			{
				return this.TargetContext.Location;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x0006D44D File Offset: 0x0006B64D
		[JsonIgnore]
		public TargetContext Target
		{
			get
			{
				return this.TargetContext;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x0006D455 File Offset: 0x0006B655
		[JsonIgnore]
		public virtual bool UseBloodLordDiplomacy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0006D458 File Offset: 0x0006B658
		public virtual Result TargetSelf(ConfigRef orderAbility, int selfId, bool selfTargetingRequired = false)
		{
			return Result.Failure;
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0006D45F File Offset: 0x0006B65F
		public virtual Result TargetItemHasWrongType(ConfigRef orderAbility, GameItem invalidItem)
		{
			return Result.Failure;
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0006D466 File Offset: 0x0006B666
		public virtual Result TargetArchfiendHasNoItemToTarget(ConfigRef orderAbility, int targetPlayerID, GameItemCategory desiredCategory)
		{
			return Result.Failure;
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0006D46D File Offset: 0x0006B66D
		public virtual Result TargetHexWrongTerrain(ConfigRef orderAbility, HexCoord hexCoord, TerrainStaticData terrainType)
		{
			return Result.Failure;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0006D474 File Offset: 0x0006B674
		public virtual Result TargetHexOccupied(ConfigRef orderAbility, HexCoord hexCoord, GamePiece occupant)
		{
			return Result.Failure;
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0006D47B File Offset: 0x0006B67B
		public virtual Result TargetHexNotNextToFixture(ConfigRef orderAbility, HexCoord hexCoord)
		{
			return Result.Failure;
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0006D482 File Offset: 0x0006B682
		public virtual Result TargetHexHasInvalidOwner(ConfigRef orderAbility, Problem invalidOwnerProblem, HexCoord hexCoord)
		{
			return invalidOwnerProblem;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0006D485 File Offset: 0x0006B685
		public virtual Result TargetArchfiendInvalidDiplomacy(ConfigRef orderAbility, int targetPlayerID, DiplomaticStateValue stateType)
		{
			return Result.Failure;
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0006D48C File Offset: 0x0006B68C
		public override Result IsValidHex(TurnContext context, List<HexCoord> selected, HexCoord hexCoord, int triggerPlayerId)
		{
			Problem problem = base.IsValidHex(context, selected, hexCoord, triggerPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			IHexTargetAbility hexTargetAbility = dataForRequest as IHexTargetAbility;
			if (hexTargetAbility == null)
			{
				return new SimulationError("Expected IHexTargetAbility data but could not find it.");
			}
			int controllingPlayerID = context.HexBoard[hexCoord].GetControllingPlayerID();
			if (hexTargetAbility.HexTargetSettings.HexOwnerMustBeValidArchfiendTarget)
			{
				Problem problem2 = this.IsValidArchfiend(context, controllingPlayerID, triggerPlayerId) as Problem;
				if (problem2 != null)
				{
					return this.TargetHexHasInvalidOwner(dataForRequest.ConfigRef, problem2, hexCoord);
				}
			}
			TerrainType type = context.HexBoard[hexCoord].Type;
			if (!hexTargetAbility.HexTargetSettings.ValidTerrainTypes.IsSet((int)type))
			{
				TerrainStaticData terrainData = context.Database.GetTerrainData(context.HexBoard[hexCoord]);
				if (terrainData == null)
				{
					return this.TargetHexInvalid(dataForRequest.ConfigRef, hexCoord);
				}
				return this.TargetHexWrongTerrain(dataForRequest.ConfigRef, hexCoord, terrainData);
			}
			else
			{
				if (!hexTargetAbility.HexTargetSettings.CanSelectOccupied)
				{
					GamePiece gamePieceAt = context.CurrentTurn.GetGamePieceAt(hexCoord);
					if (gamePieceAt != null)
					{
						return this.TargetHexOccupied(dataForRequest.ConfigRef, hexCoord, gamePieceAt);
					}
				}
				if (hexTargetAbility.HexTargetSettings.MustSelectAdjacentToFriendlyFixture && !context.CurrentTurn.AnyAdjacentFixture(hexCoord, triggerPlayerId))
				{
					return this.TargetHexNotNextToFixture(dataForRequest.ConfigRef, hexCoord);
				}
				if (!hexTargetAbility.HexTargetSettings.CanTargetAdjacentToPoP && context.CurrentTurn.AnyAdjacentFixture(hexCoord, -2147483648))
				{
					return new Result.CastRitualOnHexNextToFixtureProblem(dataForRequest.ConfigRef, hexCoord);
				}
				return Result.Success;
			}
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x0006D600 File Offset: 0x0006B800
		private int GetOwnOrLordId(DiplomaticTurnState diplomaticTurn, int playerId)
		{
			int result;
			if (!this.UseBloodLordDiplomacy || !diplomaticTurn.IsVassalOfAny(playerId, out result))
			{
				return playerId;
			}
			return result;
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0006D624 File Offset: 0x0006B824
		public override Result IsValidArchfiend(TurnContext context, int targetPlayerId, int triggeringPlayerId)
		{
			Problem problem = base.IsValidArchfiend(context, targetPlayerId, triggeringPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			IPlayerTargetAbility playerTargetAbility = dataForRequest as IPlayerTargetAbility;
			if (playerTargetAbility == null)
			{
				return new SimulationError("Expected IPlayerTargetAbility data but could not find it.");
			}
			if (targetPlayerId == triggeringPlayerId)
			{
				if (!playerTargetAbility.PlayerTargetSettings.SelfTarget)
				{
					return this.TargetSelf(dataForRequest.ConfigRef, targetPlayerId, false);
				}
				return Result.Success;
			}
			else
			{
				if (playerTargetAbility.PlayerTargetSettings.MustSelfTarget)
				{
					return this.TargetSelf(dataForRequest.ConfigRef, targetPlayerId, true);
				}
				DiplomaticTurnState currentDiplomaticTurn = context.CurrentTurn.CurrentDiplomaticTurn;
				int ownOrLordId = this.GetOwnOrLordId(currentDiplomaticTurn, triggeringPlayerId);
				int ownOrLordId2 = this.GetOwnOrLordId(currentDiplomaticTurn, targetPlayerId);
				DiplomaticState diplomaticState = currentDiplomaticTurn.GetDiplomaticStatus(ownOrLordId, ownOrLordId2).DiplomaticState;
				if (!playerTargetAbility.PlayerTargetSettings.ValidDiplomaticStates.IsSet((int)diplomaticState.Type))
				{
					return this.TargetArchfiendInvalidDiplomacy(dataForRequest.ConfigRef, targetPlayerId, diplomaticState.Type);
				}
				return Result.Success;
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0006D708 File Offset: 0x0006B908
		public override Result IsValidGamePiece(TurnContext context, List<GamePiece> selected, GamePiece gamePiece, int triggeringPlayerId)
		{
			return this.IsValidGamePiece(context, selected, gamePiece, triggeringPlayerId, null);
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0006D718 File Offset: 0x0006B918
		protected Result IsValidGamePiece(TurnContext context, List<GamePiece> selected, GamePiece gamePiece, int triggeringPlayerId, GamePieceTargetSettings targetSettings)
		{
			Problem problem = base.IsValidGamePiece(context, selected, gamePiece, triggeringPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			IGamePieceTargetAbility gamePieceTargetAbility = dataForRequest as IGamePieceTargetAbility;
			if (gamePieceTargetAbility == null)
			{
				return new SimulationError("Expected IGamePieceTargetAbility data but could not find it.");
			}
			if (targetSettings == null)
			{
				targetSettings = gamePieceTargetAbility.GamePieceTargetSettings;
			}
			if (this.TargetContext.PlayerId != -2147483648 && this.TargetContext.PlayerId != gamePiece.ControllingPlayerId)
			{
				return this.TargetItemStolen(dataForRequest.ConfigRef, this.TargetContext.PlayerId, gamePiece);
			}
			return this.FilterGamePiece(context, context.CurrentTurn.FindPlayerState(triggeringPlayerId, null), gamePiece, dataForRequest, targetSettings);
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x0006D7BC File Offset: 0x0006B9BC
		protected Result FilterGamePiece(TurnContext context, PlayerState player, GamePiece candidate)
		{
			AbilityStaticData data;
			GamePieceTargetSettings targetSettings;
			Problem problem = this.TryGetGamePieceTargetingData(context, out data, out targetSettings) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return this.FilterGamePiece(context, player, candidate, data, targetSettings);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0006D7EA File Offset: 0x0006B9EA
		protected virtual Result FilterGamePiece(TurnContext context, PlayerState player, GamePiece candidate, AbilityStaticData data, GamePieceTargetSettings targetSettings)
		{
			if (!targetSettings.ValidGamePieceTypes.IsSet((int)candidate.SubCategory))
			{
				return this.TargetItemHasWrongType(data.ConfigRef, candidate);
			}
			return Result.Success;
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0006D814 File Offset: 0x0006BA14
		private Result TryGetGamePieceTargetingData(TurnContext context, out AbilityStaticData targetData, out GamePieceTargetSettings targetSettings)
		{
			targetData = context.GetDataForRequest(this);
			IGamePieceTargetAbility gamePieceTargetAbility = targetData as IGamePieceTargetAbility;
			targetSettings = ((gamePieceTargetAbility != null) ? gamePieceTargetAbility.GamePieceTargetSettings : null);
			if (targetData == null)
			{
				return new SimulationError("Expected IGamePieceTargetAbility data but could not find it.");
			}
			return Result.Success;
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x0006D848 File Offset: 0x0006BA48
		public override Result IsValidGameItem(TurnContext context, List<Identifier> selected, Identifier gameItemId, int triggeringPlayerId)
		{
			Problem problem = base.IsValidGameItem(context, selected, gameItemId, triggeringPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			TurnState currentTurn = context.CurrentTurn;
			GameItem gameItem;
			if (!currentTurn.TryFetchGameItem<GameItem>(gameItemId, out gameItem))
			{
				return Result.InvalidItem(gameItemId);
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			IGameItemTargetAbility gameItemTargetAbility = dataForRequest as IGameItemTargetAbility;
			if (gameItemTargetAbility == null)
			{
				return new SimulationError("Expected IGameItemTargetAbility data but could not find it.");
			}
			PlayerState playerState = currentTurn.FindControllingPlayer(gameItem);
			if (playerState == null || playerState.Id == -2147483648)
			{
				return new SimulationError(string.Format("Could not find controlling player of item {0}", gameItem));
			}
			if (this.TargetContext.PlayerId != -2147483648 && this.TargetContext.PlayerId != playerState.Id)
			{
				return this.TargetItemStolen(dataForRequest.ConfigRef, this.TargetContext.PlayerId, gameItem);
			}
			if (!gameItemTargetAbility.GameItemTargetSettings.ValidTargetTypes.IsSet((int)gameItem.Category))
			{
				return this.TargetItemHasWrongType(dataForRequest.ConfigRef, gameItem);
			}
			return Result.Success;
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x0006D93C File Offset: 0x0006BB3C
		protected Result FilterGameItem(TurnContext context, PlayerState player, GameItem candidate)
		{
			AbilityStaticData data;
			GameItemTargetSettings targetSettings;
			Problem problem = this.TryGetGameItemTargetingData(context, out data, out targetSettings) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return this.FilterGameItem(context, player, candidate, data, targetSettings);
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x0006D96A File Offset: 0x0006BB6A
		protected virtual Result FilterGameItem(TurnContext context, PlayerState player, GameItem candidate, AbilityStaticData data, GameItemTargetSettings targetSettings)
		{
			if (!targetSettings.ValidTargetTypes.IsSet((int)candidate.Category))
			{
				return this.TargetItemHasWrongType(data.ConfigRef, candidate);
			}
			return Result.Success;
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x0006D994 File Offset: 0x0006BB94
		private Result TryGetGameItemTargetingData(TurnContext context, out AbilityStaticData targetData, out GameItemTargetSettings targetSettings)
		{
			targetData = context.GetDataForRequest(this);
			IGameItemTargetAbility gameItemTargetAbility = targetData as IGameItemTargetAbility;
			targetSettings = ((gameItemTargetAbility != null) ? gameItemTargetAbility.GameItemTargetSettings : null);
			if (targetData == null)
			{
				return new SimulationError("Expected IGameItemTargetAbility data but could not find it.");
			}
			return Result.Success;
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x0006D9C8 File Offset: 0x0006BBC8
		protected Result IsValidArchfiendWithValidGameItem(TurnContext context, int targetPlayerId, int triggeringPlayerId)
		{
			Problem problem = this.IsValidArchfiend(context, targetPlayerId, triggeringPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			TurnState currentTurn = context.CurrentTurn;
			PlayerState playerState = currentTurn.FindPlayerState(targetPlayerId, null);
			if (currentTurn.GetGameItemsControlledBy(playerState.Id).Any((GameItem item) => this.IsValidGameItem(context, null, item, triggeringPlayerId)))
			{
				return Result.Success;
			}
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			IGameItemTargetAbility gameItemTargetAbility = dataForRequest as IGameItemTargetAbility;
			if (gameItemTargetAbility == null)
			{
				return new SimulationError("Expected IGameItemTargetAbility data but could not find it.");
			}
			return this.TargetArchfiendHasNoItemToTarget(dataForRequest.ConfigRef, targetPlayerId, gameItemTargetAbility.GameItemTargetSettings.GetFirstValidType());
		}

		// Token: 0x04000D1A RID: 3354
		[JsonProperty]
		public TargetContext TargetContext = new TargetContext();
	}
}
