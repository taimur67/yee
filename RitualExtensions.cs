using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000697 RID: 1687
	public static class RitualExtensions
	{
		// Token: 0x06001EF1 RID: 7921 RVA: 0x0006A883 File Offset: 0x00068A83
		private static void SideLoad(this GameDatabase database, RitualStaticData data)
		{
			database.SideLoad<RitualStaticData>(data.Id, data);
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0006A894 File Offset: 0x00068A94
		public static IEnumerable<RitualStaticData> FetchRituals(this GameDatabase database, IEnumerable<string> ids)
		{
			return from id in ids
			select database.Fetch<RitualStaticData>(id);
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x0006A8C0 File Offset: 0x00068AC0
		public static HexCoord GetTargetCoordinate(this CastRitualOrder ritualOrder, TurnState turn)
		{
			if (ritualOrder.TargetHex != HexCoord.Invalid && ritualOrder.TargetHex != HexCoord.Invalid)
			{
				return ritualOrder.TargetHex;
			}
			if (ritualOrder.TargetItemId != Identifier.Invalid)
			{
				GameItem gameItem = turn.FetchGameItem(ritualOrder.TargetItemId);
				if (gameItem != null)
				{
					GamePiece gamePiece = gameItem as GamePiece;
					if (gamePiece != null)
					{
						return gamePiece.Location;
					}
					GamePiece gamePiece2;
					if (turn.TryFindControllingPiece(gameItem, out gamePiece2))
					{
						return gamePiece2.Location;
					}
				}
			}
			PlayerState playerState = turn.FindPlayerState(ritualOrder.TargetPlayerId, null);
			if (playerState != null && playerState.Id != -2147483648 && playerState.Id != -1)
			{
				return turn.FetchGameItem<GamePiece>(playerState.StrongholdId).Location;
			}
			return HexCoord.Invalid;
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0006A978 File Offset: 0x00068B78
		public static PowerType ToPowerType(this RitualType ritualType)
		{
			PowerType result;
			switch (ritualType)
			{
			case RitualType.Wrath:
				result = PowerType.Wrath;
				break;
			case RitualType.Deceit:
				result = PowerType.Deceit;
				break;
			case RitualType.Prophecy:
				result = PowerType.Prophecy;
				break;
			case RitualType.Destruction:
				result = PowerType.Destruction;
				break;
			case RitualType.Charisma:
				result = PowerType.Charisma;
				break;
			case RitualType.Artifact:
				result = PowerType.None;
				break;
			case RitualType.All:
				result = PowerType.None;
				break;
			default:
				throw new ArgumentOutOfRangeException("ritualType", ritualType, null);
			}
			return result;
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0006A9D8 File Offset: 0x00068BD8
		public static IEnumerable<IdentifiableStaticData> GetAllControlledStaticDataReferences(this GameDatabase db, TurnState turn, PlayerState player)
		{
			return (from t in turn.GetFieldedGameItemsControlledBy(player.Id)
			select db.Fetch<IdentifiableStaticData>(t.StaticDataId)).ExcludeNull<IdentifiableStaticData>();
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0006AA14 File Offset: 0x00068C14
		public static int CalculateLocalRitualVulnerability(this TurnState turn, PowerType category, PlayerState castingPlayer, GameItem targetItem)
		{
			int num = 0;
			IEnumerable<EntityTag_RitualVulnerability> enumerable = targetItem.EnumerateTags<EntityTag_RitualVulnerability>();
			GamePiece controllingPiece = turn.GetControllingPiece(targetItem.Id);
			if (controllingPiece != null)
			{
				enumerable = enumerable.Concat(controllingPiece.EnumerateTags<EntityTag_RitualVulnerability>());
			}
			foreach (EntityTag_RitualVulnerability entityTag_RitualVulnerability in enumerable)
			{
				if (entityTag_RitualVulnerability.VulnerableToPlayers.IsSet(castingPlayer.Id))
				{
					num += entityTag_RitualVulnerability.Value;
				}
			}
			return num;
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0006AA9C File Offset: 0x00068C9C
		public static int CalculateLocalRitualResistance(this TurnState turn, PowerType category, PlayerState castingPlayer, GameItem targetItem)
		{
			int num = 0;
			IEnumerable<EntityTag_LocalRitualResistance> enumerable = targetItem.EnumerateTags<EntityTag_LocalRitualResistance>();
			GamePiece controllingPiece = turn.GetControllingPiece(targetItem.Id);
			if (controllingPiece != null)
			{
				enumerable = enumerable.Concat(controllingPiece.EnumerateTags<EntityTag_LocalRitualResistance>());
			}
			foreach (EntityTag_LocalRitualResistance entityTag_LocalRitualResistance in enumerable)
			{
				num += entityTag_LocalRitualResistance.PowerTypeToResistanceValue(category);
			}
			return num;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0006AB10 File Offset: 0x00068D10
		public static bool IsResistanceKnown(this TurnState turn, PlayerState currentPlayer, int targetPlayerId)
		{
			if (targetPlayerId == -1)
			{
				return true;
			}
			PlayerKnowledgeContext knowledgeContext = currentPlayer.GetKnowledgeContext(targetPlayerId);
			return knowledgeContext != null && knowledgeContext.LastRevealedRelics >= 0 && turn.TurnValue - knowledgeContext.LastRevealedRelics <= 1;
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0006AB50 File Offset: 0x00068D50
		public static bool IsPowerKnown(this PowerType power, PlayerState currentPlayer, int targetPlayerId)
		{
			if (targetPlayerId == -1)
			{
				return true;
			}
			PlayerKnowledgeContext knowledgeContext = currentPlayer.GetKnowledgeContext(targetPlayerId);
			return knowledgeContext != null && knowledgeContext.GetLastRevealed(KnowledgeCategory.Powers, power) >= 0;
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0006AB80 File Offset: 0x00068D80
		public static ModifiableValue CalculateRitualStrength(this TurnState turn, PowerType category, PlayerState castingPlayer, GameItem targetItem = null)
		{
			if (category == PowerType.None)
			{
				return 0;
			}
			int value = (targetItem != null) ? turn.CalculateLocalRitualVulnerability(category, castingPlayer, targetItem) : 0;
			ModifiableValue strengthBonus = castingPlayer.RitualState.GetStrengthBonus(category);
			ModifiableValue modifiableValue = new ModifiableValue();
			foreach (StatModifierBase modifier in strengthBonus.ActiveModifiers)
			{
				modifiableValue.AddInstalledModifier(modifier);
			}
			modifiableValue.AddInstalledModifier(new StatModifier(castingPlayer.PowersLevels[category].CurrentLevel, new PowerContext(category), ModifierTarget.ValueOffset));
			modifiableValue.AddInstalledModifier(new StatModifier(value, null, ModifierTarget.ValueOffset));
			return modifiableValue;
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0006AC30 File Offset: 0x00068E30
		public static ModifiableValue CalculateRitualResistance(this TurnState turn, PlayerState curPlayer, PlayerState targetPlayer, PowerType category)
		{
			ModifiableValue modifiableValue = turn.CalculateRitualResistance_Internal(curPlayer, targetPlayer, category);
			modifiableValue.AddInstalledModifier(new StatModifier(targetPlayer.RankValue, new RankContext(targetPlayer.Id, targetPlayer.Rank), ModifierTarget.ValueOffset));
			return modifiableValue;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0006AC64 File Offset: 0x00068E64
		public static ModifiableValue CalculateRitualResistance(this TurnState turn, PlayerState curPlayer, PlayerState targetPlayer, GameItem targetGameItem, PowerType category)
		{
			ModifiableValue modifiableValue = turn.CalculateRitualResistance_Internal(curPlayer, targetPlayer, category);
			modifiableValue.AddInstalledModifier(new StatModifier(targetGameItem.Level - 1, new LevelContext(targetGameItem.Id, targetGameItem.Level), ModifierTarget.ValueOffset));
			GamePiece gamePiece = targetGameItem as GamePiece;
			if (gamePiece != null)
			{
				int num = turn.CalculateLocalRitualResistance(category, curPlayer, gamePiece);
				if (num != 0)
				{
					modifiableValue.AddInstalledModifier(new StatModifier(num, new LocalResistanceContext(targetGameItem.Id, num), ModifierTarget.ValueOffset));
				}
			}
			return modifiableValue;
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0006ACD4 File Offset: 0x00068ED4
		public static ModifiableValue CalculateRitualResistance_Internal(this TurnState turn, PlayerState curPlayer, PlayerState targetPlayer, PowerType category)
		{
			if (category == PowerType.None)
			{
				return 0;
			}
			if (targetPlayer == null)
			{
				return 0;
			}
			ModifiableValue resistanceBonus = targetPlayer.RitualState.GetResistanceBonus(category);
			ModifiableValue modifiableValue = new ModifiableValue();
			foreach (StatModifierBase modifier in resistanceBonus.ActiveModifiers)
			{
				modifiableValue.AddInstalledModifier(modifier);
			}
			modifiableValue.AddInstalledModifier(new StatModifier(targetPlayer.PowersLevels[category].CurrentLevel, new PowerContext(category), ModifierTarget.ValueOffset));
			return modifiableValue;
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x0006AD70 File Offset: 0x00068F70
		public static IEnumerable<ActiveRitual> GetActiveRituals(this TurnState turn, int playerId)
		{
			return turn.GetActiveRituals(turn.FindPlayerState(playerId, null));
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x0006AD80 File Offset: 0x00068F80
		public static IEnumerable<ActiveRitual> GetActiveRituals(this TurnState turn, PlayerState player)
		{
			return player.RitualState.SlottedItems.Select(new Func<Identifier, GameItem>(turn.FetchGameItem)).OfType<ActiveRitual>();
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0006ADA3 File Offset: 0x00068FA3
		public static IEnumerable<ActiveRitual> GetRitualsAffecting(this TurnState turn, PlayerState player)
		{
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(true, false))
			{
				if (player.Id != playerState.Id)
				{
					foreach (ActiveRitual activeRitual in turn.GetActiveRituals(playerState))
					{
						if (activeRitual.AffectsPlayer(turn, player.Id))
						{
							yield return activeRitual;
						}
					}
					IEnumerator<ActiveRitual> enumerator2 = null;
				}
			}
			IEnumerator<PlayerState> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0006ADBC File Offset: 0x00068FBC
		public static IEnumerable<GameItem> GetRitualSlotContentsForPlayer(this TurnState turn, PlayerState player, bool removeUnknown)
		{
			IEnumerable<GameItem> enumerable = player.RitualState.SlottedItems.Select(new Func<Identifier, GameItem>(turn.FetchGameItem));
			if (!removeUnknown)
			{
				return enumerable;
			}
			return enumerable.Where(delegate(GameItem x)
			{
				ActiveRitual activeRitual = x as ActiveRitual;
				return activeRitual == null || activeRitual.GetApparentSource(turn) == player;
			});
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0006AE1C File Offset: 0x0006901C
		public static bool TryGetActiveRitual(this TurnState turn, PlayerState player, string ritualId, out ActiveRitual activeRitual)
		{
			foreach (Identifier id in player.RitualState.SlottedItems)
			{
				ActiveRitual activeRitual2 = turn.TryFetchGameItem<ActiveRitual>(id);
				if (!(((activeRitual2 != null) ? activeRitual2.StaticDataId : null) != ritualId))
				{
					activeRitual = activeRitual2;
					return true;
				}
			}
			activeRitual = null;
			return false;
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0006AE98 File Offset: 0x00069098
		public static bool IsRitualActive(this TurnState turn, PlayerState player, string ritualId)
		{
			return player.RitualState.SlottedItems.Any(delegate(Identifier x)
			{
				ActiveRitual activeRitual = turn.TryFetchGameItem<ActiveRitual>(x);
				return ((activeRitual != null) ? activeRitual.StaticDataId : null) == ritualId;
			});
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0006AED8 File Offset: 0x000690D8
		public static bool IsRitualActiveAndTargeting(this TurnState turn, PlayerState player, string ritualId, Identifier itemId)
		{
			GameItem gameItem = turn.FetchGameItem<GameItem>(itemId);
			return gameItem != null && player.RitualState.SlottedItems.Any(delegate(Identifier x)
			{
				ActiveRitual activeRitual = turn.TryFetchGameItem<ActiveRitual>(x);
				return activeRitual != null && activeRitual.StaticDataId != "" && activeRitual.StaticDataId == ritualId && activeRitual.TargetContext.ItemId == gameItem.Id;
			});
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0006AF34 File Offset: 0x00069134
		public static T CreateActiveRitual<T>(this TurnProcessContext context, RitualStaticData ritualData, CastRitualOrder ritualOrder, Cost upkeepCost, RitualMaskingContext masking) where T : ActiveRitual, new()
		{
			T t = context.CurrentTurn.AddGameItem<T>();
			t.UpkeepCost = upkeepCost;
			t.AttachableTo = SlotType.Ritual;
			t.Status = GameItemStatus.InPlay;
			t.StaticDataId = ritualData.Id;
			t.TargetContext = ritualOrder.TargetContext;
			t.MaskingSettings = ritualOrder.RitualMaskingSettings;
			t.MaskingContext = masking;
			if (context.Rules.PrepayRitualUpkeep)
			{
				t.CheatPayUpkeep();
			}
			if (ritualData.FixedDuration > 0)
			{
				t.SetFixedDuration(context.CurrentTurn, ritualData.FixedDuration);
			}
			return t;
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x0006AFEC File Offset: 0x000691EC
		public static HashSet<RitualStaticData> GetRitualInventory(this TurnState turn, GameDatabase database, PlayerState player)
		{
			HashSet<RitualStaticData> hashSet = turn.GetAvailableRituals(database, player, false).ToHashSet<RitualStaticData>();
			foreach (GameItem gameItem in turn.GetRitualSlotContentsForPlayer(player, false))
			{
				ActiveRitual activeRitual = gameItem as ActiveRitual;
				RitualStaticData ritualStaticData;
				if (activeRitual != null && database.TryFetch<RitualStaticData>(activeRitual.StaticDataId, out ritualStaticData))
				{
					List<ConfigRef<StaticDataEntity>> otherLevels = database.GetOtherLevels(ritualStaticData.Id);
					if (otherLevels != null)
					{
						using (List<ConfigRef<StaticDataEntity>>.Enumerator enumerator2 = otherLevels.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ConfigRef<StaticDataEntity> level = enumerator2.Current;
								hashSet.RemoveWhere((RitualStaticData x) => x.Id == level.Id);
							}
						}
					}
					hashSet.Add(ritualStaticData);
				}
			}
			return hashSet;
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0006B0D8 File Offset: 0x000692D8
		public static IEnumerable<RitualStaticData> GetAvailableRituals(this TurnState turn, GameDatabase database, PlayerState player, bool includePreviousLevels = false)
		{
			return turn.GetAvailableAbilities(database, player, includePreviousLevels);
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0006B0E3 File Offset: 0x000692E3
		public static IEnumerable<StratagemTacticStaticData> GetAvailableTactics(this TurnState turn, GameDatabase database, PlayerState player)
		{
			return turn.GetAvailableAbilities(database, player, false);
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0006B0F0 File Offset: 0x000692F0
		public static IEnumerable<T> GetAvailableAbilities<T>(this TurnState turn, GameDatabase database, PlayerState player, bool includePreviousLevels = false) where T : StaticDataEntity
		{
			return from t in database.GetUnlockedAbilities(turn, player, includePreviousLevels)
			where turn.IsAvailable(t, player)
			select t;
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0006B135 File Offset: 0x00069335
		public static bool IsAvailable(this TurnState turn, StaticDataEntity ability, PlayerState player)
		{
			return true;
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x0006B138 File Offset: 0x00069338
		public static bool IsUnlockedAndAvailable<T>(this TurnState turn, GameDatabase database, T ability, PlayerState player) where T : StaticDataEntity
		{
			return turn.GetAvailableAbilities(database, player, false).Any((T x) => x.Id == ability.Id);
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0006B16C File Offset: 0x0006936C
		public static string GetBestUnlockedVariantOfRitual(this PlayerState playerState, TurnContext context, string ritualId, PowerType powerType)
		{
			ScalableAbility scalableAbilityThatUnlocksRitualVariant;
			context.Database.TryFetch<ScalableAbility>(ritualId, out scalableAbilityThatUnlocksRitualVariant);
			if (scalableAbilityThatUnlocksRitualVariant == null)
			{
				scalableAbilityThatUnlocksRitualVariant = context.Database.GetScalableAbilityThatUnlocksRitualVariant(ritualId);
			}
			if (scalableAbilityThatUnlocksRitualVariant != null)
			{
				int value = playerState.PowersLevels[powerType].CurrentLevel.Value;
				foreach (LevelValue levelValue in from level in scalableAbilityThatUnlocksRitualVariant.Levels
				orderby level.Level descending
				select level)
				{
					if (levelValue.Level <= value)
					{
						return levelValue.Ability.Id;
					}
				}
				return null;
			}
			RitualStaticData ritualStaticData = context.Database.Fetch<RitualStaticData>(ritualId);
			if (ritualStaticData == null)
			{
				return null;
			}
			if (!context.CurrentTurn.IsUnlockedAndAvailable(context.Database, ritualStaticData, playerState))
			{
				return null;
			}
			return ritualId;
		}
	}
}
