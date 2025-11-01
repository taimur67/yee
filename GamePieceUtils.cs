using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020006E7 RID: 1767
	public static class GamePieceUtils
	{
		// Token: 0x06002192 RID: 8594 RVA: 0x00075177 File Offset: 0x00073377
		public static bool IsPersonalGuard(this GamePiece gamePiece, TurnState turn)
		{
			return turn.FindPlayerState(gamePiece.ControllingPlayerId, null).PersonalGuardId == gamePiece.Id;
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x00075194 File Offset: 0x00073394
		public static bool IsSupported(this GamePiece gamePiece, TurnState turn)
		{
			List<GamePiece> list;
			return gamePiece.IsSupported(turn, out list);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000751AA File Offset: 0x000733AA
		public static bool IsSupported(this GamePiece gamePiece, TurnState turn, out List<GamePiece> supporters)
		{
			supporters = IEnumerableExtensions.ToList<GamePiece>(turn.GetSupportingGamePieces(gamePiece));
			return supporters.Count > 0;
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000751C4 File Offset: 0x000733C4
		public static LegionKilledEvent KillGamePiece(this TurnProcessContext turn, GamePiece gamePiece, int triggeringPlayerId = -1)
		{
			LegionKilledEvent legionKilledEvent = new LegionKilledEvent(gamePiece.Id, triggeringPlayerId, gamePiece.ControllingPlayerId);
			legionKilledEvent.HeldItems = IEnumerableExtensions.ToList<Identifier>(gamePiece.Slots);
			turn.BanishGameItem(gamePiece, triggeringPlayerId);
			return legionKilledEvent;
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000751F7 File Offset: 0x000733F7
		public static GamePiece ReviveGamePiece(this TurnContext context, GamePiece gamePiece, HexCoord coord)
		{
			gamePiece.Status = GameItemStatus.InPlay;
			gamePiece.HP = gamePiece.TotalHP;
			gamePiece.Location = coord;
			context.RecalculateSupportModifiers(gamePiece.ControllingPlayerId);
			return gamePiece;
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x00075225 File Offset: 0x00073425
		public static void KillGamePieceWithEvent(this TurnProcessContext context, int playerId, GamePiece gamePiece)
		{
			context.CurrentTurn.AddGameEvent<LegionKilledEvent>(context.KillGamePiece(gamePiece, -1));
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x0007523B File Offset: 0x0007343B
		public static void KillGamePieceWithEvent(this TurnProcessContext turn, PlayerState player, GamePiece gamePiece)
		{
			turn.KillGamePieceWithEvent(player.Id, gamePiece);
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x0007524A File Offset: 0x0007344A
		public static void KillGamePieceWithEvent(this TurnProcessContext context, int playerId, Identifier gamePieceId)
		{
			context.KillGamePieceWithEvent(playerId, context.CurrentTurn.FetchGameItem<GamePiece>(gamePieceId));
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x0007525F File Offset: 0x0007345F
		public static IEnumerable<GameItem> GetAttachedAndPendingItems(this GamePiece piece, TurnState turn)
		{
			return piece.Slots.Select(new Func<Identifier, GameItem>(turn.FetchGameItem));
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x00075278 File Offset: 0x00073478
		public static IEnumerable<ActionableOrder> GetPendingReassignments(this GameItem item, PlayerState reassigningPlayer)
		{
			return reassigningPlayer.PlayerTurn.Orders.Where(delegate(ActionableOrder order)
			{
				OrderAttachGameItemToGamePiece orderAttachGameItemToGamePiece = order as OrderAttachGameItemToGamePiece;
				bool result;
				if (orderAttachGameItemToGamePiece == null)
				{
					OrderReturnGameItemToVault orderReturnGameItemToVault = order as OrderReturnGameItemToVault;
					if (orderReturnGameItemToVault == null)
					{
						OrderAttachGameItemToRitualSlot orderAttachGameItemToRitualSlot = order as OrderAttachGameItemToRitualSlot;
						result = (orderAttachGameItemToRitualSlot != null && orderAttachGameItemToRitualSlot.GameItemId == item.Id);
					}
					else
					{
						result = (orderReturnGameItemToVault.GameItemId == item.Id);
					}
				}
				else
				{
					result = (orderAttachGameItemToGamePiece.GameItemId == item.Id);
				}
				return result;
			});
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000752AE File Offset: 0x000734AE
		public static IEnumerable<GameItem> GetAttachedItems(this GamePiece piece, TurnState turn)
		{
			return from x in piece.Slots.Select(new Func<Identifier, GameItem>(turn.FetchGameItem))
			where !(x is AbilityPlaceholder)
			select x;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000752EB File Offset: 0x000734EB
		public static IEnumerable<T> GetAttachedItems<T>(this GamePiece piece, TurnState turn) where T : GameItem
		{
			return piece.GetAttachedItems(turn).OfType<T>();
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000752F9 File Offset: 0x000734F9
		public static bool IsPendingPromotion(this GamePiece piece, TurnState turn)
		{
			return piece.GetPendingPromotion(turn) != null;
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x00075308 File Offset: 0x00073508
		public static SelectLevelUpRewardRequest GetPendingPromotion(this GamePiece piece, TurnState turn)
		{
			return turn.FindPlayerState(piece.ControllingPlayerId, null).GetDecisionRequests<SelectLevelUpRewardRequest>().FirstOrDefault((SelectLevelUpRewardRequest x) => x.GamePieceId == piece.Id);
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x0007534C File Offset: 0x0007354C
		public static int LookupMaxSlots(this GamePiece piece)
		{
			switch (piece.Level)
			{
			case 1:
			case 2:
				return 1;
			case 3:
			case 4:
				return 2;
			case 5:
			case 6:
				return 3;
			default:
				return 4;
			}
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00075389 File Offset: 0x00073589
		public static float EvaluateGamePieceStrength(this GamePiece gamePiece)
		{
			return (float)(gamePiece.CombatStats.Ranged + gamePiece.CombatStats.Melee + gamePiece.CombatStats.Infernal);
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000753C0 File Offset: 0x000735C0
		public static void ProcessLevelUp(this GamePiece gamePiece, TurnProcessContext context, LegionLevelTable levelTable, int levelIncrease = 1)
		{
			int level = gamePiece.Level;
			int num = level + levelIncrease;
			LevelUpData dataForLevel = levelTable.GetDataForLevel(num);
			if (gamePiece.LevelExperience < dataForLevel.RequiredExperience)
			{
				return;
			}
			if (gamePiece.Level == gamePiece.HighestLevel)
			{
				int controllingPlayerId = gamePiece.ControllingPlayerId;
				if (controllingPlayerId == -2147483648 || controllingPlayerId == -1)
				{
					gamePiece.TryForceRandomLevelUp(context.Database, context.CurrentTurn, num, false);
				}
				else
				{
					List<GamePieceRewardStaticData> list = new List<GamePieceRewardStaticData>();
					List<ConfigRef<GamePieceRewardStaticData>> list2 = IEnumerableExtensions.ToList<ConfigRef<GamePieceRewardStaticData>>(dataForLevel.BonusPool);
					for (int i = 0; i < dataForLevel.BonusOptionsCount; i++)
					{
						ConfigRef<GamePieceRewardStaticData> configRef = list2.PopRandom(context.Random);
						if (configRef == null)
						{
							break;
						}
						list.Add(context.Database.Fetch(configRef));
					}
					SelectLevelUpRewardRequest decisionRequest = new SelectLevelUpRewardRequest(context.CurrentTurn, gamePiece.Id, list, level, num);
					PlayerState playerState = context.CurrentTurn.FindPlayerState(gamePiece.ControllingPlayerId, null);
					context.CurrentTurn.AddDecisionToAskPlayer(playerState.Id, decisionRequest);
					context.CurrentTurn.AddGameEvent<LegionLevelUpEvent>(new LegionLevelUpEvent(gamePiece.ControllingPlayerId, gamePiece.Id, level, num));
				}
			}
			gamePiece.LevelExperience.SetBase(0f);
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000754F8 File Offset: 0x000736F8
		public static bool TryForceRandomLevelUp(this GamePiece gamePiece, GameDatabase database, TurnState turn, int targetLevel = -1, bool resetHP = true)
		{
			if (targetLevel <= 0)
			{
				targetLevel = gamePiece.Level + 1;
			}
			if (targetLevel <= gamePiece.Level)
			{
				return false;
			}
			GamePropertiesStaticData gamePropertiesStaticData = database.FetchSingle<GamePropertiesStaticData>();
			if (gamePropertiesStaticData == null)
			{
				return false;
			}
			LegionLevelTable legionLevelTable = database.Fetch(gamePropertiesStaticData.LegionLevelTable);
			if (legionLevelTable == null)
			{
				return false;
			}
			for (int i = gamePiece.Level + 1; i <= targetLevel; i++)
			{
				LevelUpData dataForLevel = legionLevelTable.GetDataForLevel(i);
				gamePiece.LevelExperience = dataForLevel.RequiredExperience;
				ConfigRef<GamePieceRewardStaticData> cref = dataForLevel.BonusPool.Random(turn.Random);
				GamePieceRewardStaticData gamePieceRewardStaticData = database.Fetch(cref);
				if (gamePieceRewardStaticData == null)
				{
					return false;
				}
				new GamePieceModifier(gamePieceRewardStaticData.Reward)
				{
					Source = new LevelUpContext()
				}.InstallInto(gamePiece, turn, false);
				gamePiece.LevelUp(true, i);
			}
			if (resetHP)
			{
				gamePiece.HP = gamePiece.TotalHP;
			}
			return true;
		}
	}
}
