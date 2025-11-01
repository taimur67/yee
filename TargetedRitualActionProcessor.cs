using System;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020005C7 RID: 1479
	public abstract class TargetedRitualActionProcessor<T, Q, TEvent> : RitualActionProcessor<T, Q> where T : CastTargetedRitualOrder, new() where Q : RitualStaticData where TEvent : RitualCastEvent, new()
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x00060010 File Offset: 0x0005E210
		private RitualSuccessTable RitualSuccessTable
		{
			get
			{
				RitualSuccessTable result;
				if ((result = this._ritualSuccessTable) == null)
				{
					result = (this._ritualSuccessTable = base._database.FetchSingle<RitualSuccessTable>());
				}
				return result;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x0006003C File Offset: 0x0005E23C
		private RitualMaskingSuccessTable RitualMaskingSuccessTable
		{
			get
			{
				RitualMaskingSuccessTable result;
				if ((result = this._ritualMaskingSuccessTable) == null)
				{
					result = (this._ritualMaskingSuccessTable = base._database.FetchSingle<RitualMaskingSuccessTable>());
				}
				return result;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001BBD RID: 7101 RVA: 0x00060067 File Offset: 0x0005E267
		// (set) Token: 0x06001BBE RID: 7102 RVA: 0x0006006F File Offset: 0x0005E26F
		public TEvent GameEvent { get; private set; }

		// Token: 0x06001BBF RID: 7103 RVA: 0x00060078 File Offset: 0x0005E278
		public override Result Validate()
		{
			Problem problem = base.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (!base.request.TargetContext.IsAnyTargetSet)
			{
				return Result.Failure;
			}
			if (base.request.TargetContext.ItemId != Identifier.Invalid)
			{
				GamePiece gamePiece = base._currentTurn.TryFetchGameItem<GamePiece>(base.request.TargetContext.ItemId);
				if (gamePiece != null)
				{
					Problem problem2 = base.request.IsValidGamePiece(this.TurnProcessContext, null, gamePiece, this._player.Id) as Problem;
					if (problem2 != null)
					{
						return problem2;
					}
				}
				else
				{
					Problem problem3 = base.request.IsValidGameItem(this.TurnProcessContext, null, base.request.TargetContext.ItemId, this._player.Id) as Problem;
					if (problem3 != null)
					{
						return problem3;
					}
				}
			}
			if (base.request.TargetPlayerId != -2147483648)
			{
				Problem problem4 = base.request.IsValidArchfiend(this.TurnProcessContext, base.request.TargetPlayerId, this._player.Id) as Problem;
				if (problem4 != null)
				{
					return problem4;
				}
			}
			if (base.request.TargetHex != HexCoord.Invalid)
			{
				Problem problem5 = base.request.IsValidHex(this.TurnProcessContext, null, base.request.TargetHex, this._player.Id) as Problem;
				if (problem5 != null)
				{
					return problem5;
				}
			}
			return Result.Success;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0006021C File Offset: 0x0005E41C
		public sealed override Result Process(ActionProcessContext context)
		{
			Problem problem = base.Process(context) as Problem;
			if (problem != null)
			{
				return problem;
			}
			Result result = this.ProcessInternal(context);
			if (result is Result.RitualResistedProblem)
			{
				return Result.Success;
			}
			if (result is Result.CannotBeAffectedByRitualsProblem)
			{
				return Result.Success;
			}
			Problem problem2 = result as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			this.HandleDestructionReward();
			this.HandleExcommunication(result);
			return result;
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x0006027C File Offset: 0x0005E47C
		private void HandleExcommunication(Result processResult)
		{
			GamePiece pandaemonium = base._currentTurn.GetPandaemonium();
			if (pandaemonium.ControllingPlayerId != -1)
			{
				return;
			}
			if (base.request.TargetItemId != pandaemonium.Id)
			{
				return;
			}
			bool flag = processResult is Result.RitualResistedProblem;
			if (processResult.successful && base.data.PandaemoniumSuccessExcommunicates)
			{
				this.ExcommunicateRitualSource();
				return;
			}
			if (flag && base.data.PandaemoniumResistExcommunicates)
			{
				this.ExcommunicateRitualSource();
				return;
			}
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x00060300 File Offset: 0x0005E500
		private PlayerState GetPlayerToExcommunicate()
		{
			PlayerState result;
			switch (base.request.RitualMaskingSettings.MaskingMode)
			{
			case RitualMaskingMode.NoMasking:
				result = this._player;
				break;
			case RitualMaskingMode.Masked:
				result = (this.GameEvent.MaskingContext.MaskingSuccessful ? null : this._player);
				break;
			case RitualMaskingMode.Framed:
				result = ((base._rules.FramedRitualsCanExcommunicate && this.GameEvent.MaskingContext.MaskingSuccessful) ? base._currentTurn.FindPlayerState(base.request.RitualMaskingSettings.CurrentMaskingTargetId, null) : this._player);
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000603B8 File Offset: 0x0005E5B8
		private void ExcommunicateRitualSource()
		{
			PlayerState playerToExcommunicate = this.GetPlayerToExcommunicate();
			if (playerToExcommunicate == null)
			{
				return;
			}
			this.TurnProcessContext.Diplomacy.SetPlayerAsExcommunicated(this.TurnProcessContext, playerToExcommunicate, ExcommunicationReason.CastRitualOnPandaemonium, this._player.Id);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000603F4 File Offset: 0x0005E5F4
		private void HandleDestructionReward()
		{
			if (base.request.RitualMaskingSettings.MaskingMode != RitualMaskingMode.NoMasking)
			{
				return;
			}
			int num = this.GetPrestigeReward() * base.data.PrestigeRewardMultiplier;
			if (num != 0)
			{
				PaymentReceivedEvent ev = this.TurnProcessContext.GivePrestige(this._player, num);
				this.GameEvent.AddChildEvent<PaymentReceivedEvent>(ev);
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x00060459 File Offset: 0x0005E659
		protected virtual int GetPrestigeReward()
		{
			if (base.request.TargetItemId == Identifier.Invalid)
			{
				return 0;
			}
			return base._currentTurn.FetchGameItem(base.request.TargetItemId).Level;
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x00060490 File Offset: 0x0005E690
		protected virtual Result ProcessInternal(ActionProcessContext context)
		{
			return Result.Success;
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x00060497 File Offset: 0x0005E697
		protected Result CheckPlayerRitualResistance(PlayerState player, out TEvent gameEvent)
		{
			return this.CheckPlayerRitualResistance(player.Id, out gameEvent);
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000604A6 File Offset: 0x0005E6A6
		protected Result CheckGamePieceRitualResistance(Identifier gamePieceId, out TEvent gameEvent)
		{
			return this.CheckGamePieceRitualResistance(base._currentTurn.FetchGameItem<GamePiece>(gamePieceId), out gameEvent);
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000604BB File Offset: 0x0005E6BB
		protected Result CheckGamePieceRitualResistance(GamePiece gamePiece, out TEvent gameEvent)
		{
			return this.CheckGameItemRitualResistance((gamePiece != null) ? gamePiece.Id : Identifier.Invalid, (gamePiece != null) ? gamePiece.ControllingPlayerId : -1, out gameEvent);
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000604DC File Offset: 0x0005E6DC
		protected Result CheckGameItemRitualResistance(Identifier gamePieceId, int targetPlayerId, out TEvent gameEvent)
		{
			GameItem target = base._currentTurn.FetchGameItem(gamePieceId);
			PlayerState targetPlayer = base._currentTurn.FindPlayerState(targetPlayerId, null);
			TargetContext targetContext = new TargetContext();
			targetContext.SetTargetGameItem(gamePieceId, targetPlayerId);
			gameEvent = this.CreateRitualEvent(targetPlayerId);
			gameEvent.SetTargetContext(targetContext);
			int gameItemRitualResistance = this.GetGameItemRitualResistance(this._player, targetPlayer, target);
			return this.ProcessRitualResistance(gameEvent, gameItemRitualResistance);
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x00060550 File Offset: 0x0005E750
		protected Result CheckPlayerRitualResistance(int playerId, out TEvent gameEvent)
		{
			gameEvent = this.CreateRitualEvent(playerId);
			PlayerState target = base._currentTurn.FindPlayerState(playerId, null);
			int playerRitualResistance = this.GetPlayerRitualResistance(this._player, target);
			return this.ProcessRitualResistance(gameEvent, playerRitualResistance);
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x00060598 File Offset: 0x0005E798
		protected Result ProcessRitualResistance(RitualCastEvent gameEvent, int resistanceValue)
		{
			this.IdentityCheck(gameEvent, resistanceValue);
			Result result = this.ResistanceCheck(gameEvent, resistanceValue);
			base._currentTurn.AddGameEvent<RitualCastEvent>(gameEvent);
			return result;
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000605B7 File Offset: 0x0005E7B7
		public int GetRitualStrength(PlayerState targetPlayer)
		{
			return base._currentTurn.CalculateRitualStrength(base.data.Category, this._player, null);
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000605E0 File Offset: 0x0005E7E0
		public int GetRitualStrength(PlayerState targetPlayer, GameItem targetItem)
		{
			return base._currentTurn.CalculateRitualStrength(base.data.Category, this._player, targetItem);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x00060609 File Offset: 0x0005E809
		private int GetPlayerRitualResistance(PlayerState currentPlayer, PlayerState target)
		{
			return base._currentTurn.CalculateRitualResistance(currentPlayer, target, base.data.Category);
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0006062D File Offset: 0x0005E82D
		private int GetGameItemRitualResistance(PlayerState currentPlayer, PlayerState targetPlayer, GameItem target)
		{
			return base._currentTurn.CalculateRitualResistance(currentPlayer, targetPlayer, target, base.data.Category);
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x00060654 File Offset: 0x0005E854
		public Result ResistanceCheck(RitualCastEvent gameEvent, int resistanceValue)
		{
			GameItem gameItem = base._currentTurn.FetchGameItem(base.request.TargetItemId);
			GamePiece gamePiece = gameItem as GamePiece;
			if (gamePiece != null && !gamePiece.CanBeAffectedByRituals)
			{
				BooleanModifierBase booleanModifierBase = gamePiece.CanBeAffectedByRituals.ActiveModifiers.Find((BooleanModifierBase x) => x.Provider is TerrainContext);
				gameEvent.Result = ((booleanModifierBase == null) ? RitualCastResult.AutomaticFailure : RitualCastResult.ResistedByTerrain);
				return new Result.CannotBeAffectedByRitualsProblem(base.data.ConfigRef, gameItem);
			}
			GamePiece controllingPiece = base._currentTurn.GetControllingPiece(gameItem);
			if (controllingPiece != null && !controllingPiece.CanBeAffectedByRituals)
			{
				BooleanModifierBase booleanModifierBase2 = controllingPiece.CanBeAffectedByRituals.ActiveModifiers.Find((BooleanModifierBase x) => x.Provider is TerrainContext);
				gameEvent.Result = ((booleanModifierBase2 == null) ? RitualCastResult.AutomaticFailure : RitualCastResult.ResistedByTerrain);
				return new Result.CannotBeAffectedByRitualsProblem(base.data.ConfigRef, gameItem);
			}
			if (this._player.Id == base.request.TargetPlayerId || !base._rules.RitualResitanceEnabled || !base.data.CanBeResisted)
			{
				gameEvent.Result = RitualCastResult.AutomaticSuccess;
				return Result.Success;
			}
			int num = 0;
			if (controllingPiece != null)
			{
				foreach (EntityTag_LocalRitualResistance entityTag_LocalRitualResistance in controllingPiece.EnumerateTags<EntityTag_LocalRitualResistance>())
				{
					num += entityTag_LocalRitualResistance.PowerTypeToResistanceValue(gameEvent.RitualCategory);
				}
			}
			PlayerState targetPlayer = base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null);
			int casterAdvantage = this.GetRitualStrength(targetPlayer, gameItem) - (resistanceValue + num);
			float num2 = this.LookupSuccessChance(casterAdvantage);
			gameEvent.SuccessChance = num2;
			float num3 = base.Random.NextFloat();
			gameEvent.ResistanceRoll = num3;
			if (num3 > num2)
			{
				gameEvent.Result = RitualCastResult.ResistedByTarget;
				return new Result.RitualResistedProblem(num3, num2);
			}
			gameEvent.Result = RitualCastResult.TargetResistanceOvercome;
			return Result.Success;
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x00060878 File Offset: 0x0005EA78
		private float LookupSuccessChance(int casterAdvantage)
		{
			return this.RitualSuccessTable.SuccessChances.SelectClosestUnderOrEqual((RitualSuccessChance x) => x.CasterAdvantage, casterAdvantage).SuccessChance;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x000608AF File Offset: 0x0005EAAF
		private float LookupMaskingSuccessChance(int casterAdvantage)
		{
			return this.RitualMaskingSuccessTable.SuccessChances.SelectClosestUnderOrEqual((RitualSuccessChance x) => x.CasterAdvantage, casterAdvantage).SuccessChance;
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000608E8 File Offset: 0x0005EAE8
		protected virtual TEvent CreateRitualEvent(int targetId)
		{
			TEvent tevent = Activator.CreateInstance<TEvent>();
			tevent.TriggeringPlayerID = base.PlayerId;
			tevent.RitualId = base.request.RitualId;
			tevent.AffectedPlayerIds.Add(targetId);
			tevent.RitualCategory = base.data.Category;
			tevent.VisibilityType = base.data.VisibilityType;
			tevent.SetTargetContext(base.request.TargetContext);
			this.GameEvent = tevent;
			return tevent;
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x00060994 File Offset: 0x0005EB94
		public override RitualMaskingMode GetRitualMaskingMode()
		{
			RitualMaskingSettings ritualMaskingSettings = base.request.RitualMaskingSettings;
			RitualMaskingMode ritualMaskingMode = (ritualMaskingSettings != null) ? ritualMaskingSettings.MaskingMode : RitualMaskingMode.NoMasking;
			RitualMaskingMode ritualMaskingMode2 = ritualMaskingMode;
			if (ritualMaskingMode2 != RitualMaskingMode.Masked)
			{
				if (ritualMaskingMode2 != RitualMaskingMode.Framed)
				{
					goto IL_73;
				}
				if (this._player.IsRitualFramingAvailable() && base.data.Maskable)
				{
					goto IL_73;
				}
			}
			else if (this._player.IsRitualMaskingAvailable() && base.data.Maskable)
			{
				goto IL_73;
			}
			ritualMaskingMode = RitualMaskingMode.NoMasking;
			IL_73:
			if (base.request.RitualMaskingSettings != null)
			{
				base.request.RitualMaskingSettings.MaskingMode = ritualMaskingMode;
			}
			return ritualMaskingMode;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x00060A40 File Offset: 0x0005EC40
		public void IdentityCheck(RitualCastEvent gameEvent, int resistanceValue)
		{
			RitualMaskingMode ritualMaskingMode = this.GetRitualMaskingMode();
			RitualMaskingContext ritualMaskingContext = new RitualMaskingContext
			{
				MaskingMode = ritualMaskingMode
			};
			gameEvent.TrueSourceId = this._player.Id;
			if (ritualMaskingMode == RitualMaskingMode.Framed)
			{
				ritualMaskingContext.FramedPlayerId = base.request.RitualMaskingSettings.CurrentMaskingTargetId;
				ritualMaskingContext.FramingReceiver = (ritualMaskingContext.FramedPlayerId == base.request.TargetPlayerId);
			}
			if (ritualMaskingMode != RitualMaskingMode.NoMasking)
			{
				PlayerState targetPlayer = base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null);
				GameItem gameItem = base._currentTurn.FetchGameItem(base.request.TargetItemId);
				int casterAdvantage = this.GetRitualStrength(targetPlayer, gameItem) - resistanceValue;
				float num = this.LookupMaskingSuccessChance(casterAdvantage);
				if (gameItem != null)
				{
					foreach (EntityTag_ImprovedRitualDetection entityTag_ImprovedRitualDetection in gameItem.EnumerateTags<EntityTag_ImprovedRitualDetection>())
					{
						num = Math.Clamp(num, 0f, entityTag_ImprovedRitualDetection.MaskingRollLimit);
					}
				}
				ritualMaskingContext.MaskingSuccessChance = num;
				float num2 = base.Random.NextFloat();
				ritualMaskingContext.DetectionRoll = num2;
				ritualMaskingContext.MaskingSuccessful = (num2 <= num);
			}
			gameEvent.MaskingContext = ritualMaskingContext;
		}

		// Token: 0x04000C66 RID: 3174
		private RitualSuccessTable _ritualSuccessTable;

		// Token: 0x04000C67 RID: 3175
		private RitualMaskingSuccessTable _ritualMaskingSuccessTable;
	}
}
