using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000391 RID: 913
	public static class ModifierExtensions
	{
		// Token: 0x06001158 RID: 4440 RVA: 0x00042E0C File Offset: 0x0004100C
		public static bool CanApplyModifierTo(this TurnContext context, ModifierStaticData modData, GameEntity entity)
		{
			IEnumerable<GameEntityFilter> enumerable = (modData != null) ? modData.Conditions : null;
			return (enumerable ?? Enumerable.Empty<GameEntityFilter>()).All((GameEntityFilter t) => t.Filter(context, entity));
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00042E55 File Offset: 0x00041055
		public static IEnumerable<ModifierStaticData> GetModifiers(this GameItem item, GameDatabase db)
		{
			StaticDataEntity staticDataEntity = db.Fetch<StaticDataEntity>(item.StaticDataId);
			return ((staticDataEntity != null) ? staticDataEntity.GetModifiers() : null) ?? Enumerable.Empty<ModifierStaticData>();
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00042E78 File Offset: 0x00041078
		public static IEnumerable<ModifierStaticData> GetModifiers(this GameDatabase db, GameItem item)
		{
			StaticDataEntity staticDataEntity = db.Fetch<StaticDataEntity>(item.StaticDataId);
			return ((staticDataEntity != null) ? staticDataEntity.GetModifiers() : null) ?? Enumerable.Empty<ModifierStaticData>();
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00042E9B File Offset: 0x0004109B
		public static IEnumerable<ModifierStaticData> GetModifiers(this StaticDataEntity entity)
		{
			return entity.Components.OfType<ModifierStaticData>();
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00042EA8 File Offset: 0x000410A8
		public static void RecalculateSupportModifiers(this TurnContext context, int playerId)
		{
			foreach (GamePiece piece in IEnumerableExtensions.ToList<GamePiece>(context.CurrentTurn.GetPiecesControlledBy(playerId)))
			{
				context.RecalculateSupportModifiers(piece);
			}
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00042F08 File Offset: 0x00041108
		public static void RecalculateSupportModifiers(this TurnContext context, Identifier id, HexCoord calculationOrigin)
		{
			context.RecalculateSupportModifiers(context.CurrentTurn.FetchGameItem<GamePiece>(id), calculationOrigin);
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00042F1D File Offset: 0x0004111D
		public static void RecalculateSupportModifiers(this TurnContext context, GamePiece piece)
		{
			context.RecalculateSupportModifiers(piece, piece.Location);
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00042F2C File Offset: 0x0004112C
		public static void RecalculateSupportModifiers(this TurnContext context, GamePiece piece, HexCoord calculatingOrigin)
		{
			List<Identifier> list;
			context.RecalculateSupportModifiers(piece, calculatingOrigin, out list);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00042F44 File Offset: 0x00041144
		public static void RecalculateSupportModifiers(this TurnContext context, GamePiece piece, HexCoord calculatingOrigin, out List<Identifier> supporters)
		{
			supporters = new List<Identifier>();
			if (piece == null)
			{
				return;
			}
			piece.SupportStats.ClearModifiers();
			foreach (IModifier modifier in context.CurrentTurn.GenerateSupportModifiers(piece, calculatingOrigin))
			{
				modifier.ApplyTo(context, piece);
				SupportContext supportContext = modifier.Source as SupportContext;
				if (supportContext != null)
				{
					supporters.Add(supportContext.GameItemId);
				}
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00042FCC File Offset: 0x000411CC
		public static void RecalculateSupportModifiers(this TurnContext context, GamePiece piece, List<Identifier> forceUseTheseSupportersOnly)
		{
			if (piece == null)
			{
				return;
			}
			piece.SupportStats.ClearModifiers();
			List<GamePiece> list = new List<GamePiece>();
			foreach (Identifier id in forceUseTheseSupportersOnly)
			{
				GamePiece item;
				if (context.CurrentTurn.TryFetchGameItem<GamePiece>(id, out item))
				{
					list.Add(item);
				}
			}
			foreach (IModifier modifier in ModifierExtensions.GenerateSupportModifiersSupportsProvided(context.CurrentTurn, piece, list))
			{
				modifier.ApplyTo(context, piece);
			}
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00043088 File Offset: 0x00041288
		public static IEnumerable<IModifier> GenerateSupportModifiers(this TurnState turn, GamePiece piece)
		{
			return turn.GenerateSupportModifiers(piece, piece.Location);
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00043098 File Offset: 0x00041298
		public static IEnumerable<IModifier> GenerateSupportModifiers(this TurnState turn, GamePiece piece, HexCoord calculationOrigin)
		{
			List<GamePiece> supporters = IEnumerableExtensions.ToList<GamePiece>(turn.GetSupportingGamePieces(piece, calculationOrigin));
			return ModifierExtensions.GenerateSupportModifiersSupportsProvided(turn, piece, supporters);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x000430BB File Offset: 0x000412BB
		public static IEnumerable<IModifier> GenerateSupportModifiersSupportsProvided(TurnState turn, GamePiece piece, List<GamePiece> supporters)
		{
			foreach (GamePiece gamePiece in supporters)
			{
				GamePieceSupportModifier gamePieceSupportModifier = new GamePieceSupportModifier
				{
					Ranged = (int)Math.Floor((double)((float)gamePiece.CombatStats.Ranged * ((float)gamePiece.SupportStrength / 100f))),
					Melee = (int)Math.Floor((double)((float)gamePiece.CombatStats.Melee * ((float)gamePiece.SupportStrength / 100f))),
					Infernal = (int)Math.Floor((double)((float)gamePiece.CombatStats.Infernal * ((float)gamePiece.SupportStrength / 100f)))
				};
				gamePieceSupportModifier.Source = new SupportContext(gamePiece.Id);
				yield return gamePieceSupportModifier;
			}
			List<GamePiece>.Enumerator enumerator = default(List<GamePiece>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x000430CB File Offset: 0x000412CB
		public static IEnumerable<GamePiece> GetSupportingGamePieces(this TurnState turn, GamePiece piece)
		{
			return turn.GetSupportingGamePieces(piece, piece.Location);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000430DA File Offset: 0x000412DA
		public static IEnumerable<GamePiece> GetSupportingGamePieces(this TurnState turn, GamePiece piece, HexCoord targetCoord)
		{
			if (piece.Status != GameItemStatus.InPlay)
			{
				yield break;
			}
			if (piece.Location == HexCoord.Invalid)
			{
				yield break;
			}
			foreach (GamePiece gamePiece in turn.GetActiveGamePieces())
			{
				if (piece.CanBeSupportedBy(turn, gamePiece) && gamePiece.SupportRange != 0 && turn.HexBoard.ShortestDistance(targetCoord, gamePiece.Location) <= gamePiece.SupportRange)
				{
					yield return gamePiece;
				}
			}
			IEnumerator<GamePiece> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x000430F8 File Offset: 0x000412F8
		public static IEnumerable<GamePiece> GetPotentialSupportingGamePiecesForLocation(this TurnState turn, HexCoord targetCoord)
		{
			foreach (GamePiece gamePiece in turn.GetActiveGamePieces())
			{
				if (gamePiece.SupportRange != 0 && turn.HexBoard.ShortestDistance(targetCoord, gamePiece.Location) <= gamePiece.SupportRange)
				{
					yield return gamePiece;
				}
			}
			IEnumerator<GamePiece> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0004310F File Offset: 0x0004130F
		public static IEnumerable<GamePiece> GetHealingGamePieces(this TurnState turn, GamePiece piece)
		{
			return turn.GetHealingGamePieces(piece, piece.Location);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0004311E File Offset: 0x0004131E
		public static IEnumerable<GamePiece> GetHealingGamePieces(this TurnState turn, GamePiece piece, HexCoord targetCoord)
		{
			foreach (GamePiece gamePiece in turn.GetActiveGamePieces())
			{
				if (piece.CanBeHealedBy(turn, gamePiece) && gamePiece.IsFixture() && gamePiece.HealingBonus != 0 && turn.HexBoard.AreAdjacent(targetCoord, gamePiece.Location))
				{
					yield return gamePiece;
				}
			}
			IEnumerator<GamePiece> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0004313C File Offset: 0x0004133C
		public static IEnumerable<GamePiece> GetPotentialHealingGamePiecesForLocation(this TurnState turn, HexCoord targetCoord)
		{
			foreach (GamePiece gamePiece in turn.GetActiveGamePieces())
			{
				if (gamePiece.IsFixture() && gamePiece.HealingBonus != 0 && turn.HexBoard.AreAdjacent(targetCoord, gamePiece.Location))
				{
					yield return gamePiece;
				}
			}
			IEnumerator<GamePiece> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00043154 File Offset: 0x00041354
		public static void RecalculateAllPlayerModifiers(this TurnContext context)
		{
			foreach (PlayerState player in context.CurrentTurn.EnumeratePlayerStates(true, false))
			{
				context.RecalculateAllModifiersFor(player);
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000431A8 File Offset: 0x000413A8
		public static void RecalculateAllModifiersFor(this TurnContext context, PlayerState player)
		{
			context.RecalculateModifiers(player);
			foreach (IModifiable modifiable in context.CurrentTurn.GetFieldedGameItemsControlledBy(player.Id).OfType<IModifiable>())
			{
				context.RecalculateModifiers(modifiable);
			}
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0004320C File Offset: 0x0004140C
		public static void RecalculateModifiers(this TurnContext context, IModifiable modifiable)
		{
			if (modifiable == null)
			{
				return;
			}
			modifiable.ClearModifiers();
			IEnumerable<IModifier> modifiers = context.CurrentTurn.GenerateAllModifiersFor(context.Database, modifiable);
			ModifierExtensions.ApplyAll(context, modifiable, modifiers);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00043240 File Offset: 0x00041440
		public static void RecalculateModifiersFor(this TurnContext context, Identifier id)
		{
			IModifiable modifiable = context.CurrentTurn.FetchGameItem(id);
			if (modifiable == null)
			{
				return;
			}
			modifiable.ClearModifiers();
			IEnumerable<IModifier> modifiers = context.CurrentTurn.GenerateAllModifiersFor(context.Database, modifiable);
			ModifierExtensions.ApplyAll(context, modifiable, modifiers);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00043280 File Offset: 0x00041480
		public static IEnumerable<IModifier> GenerateAllModifiersFor(this TurnState turn, GameDatabase db, BattlePhaseResult battlePhaseResult)
		{
			IEnumerable<IModifier> first = turn.GenerateAllModifiersFor(db, battlePhaseResult.WinningLegionId);
			IEnumerable<IModifier> second = turn.GenerateAllModifiersFor(db, battlePhaseResult.LosingLegionId);
			return first.Concat(second);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x000432B0 File Offset: 0x000414B0
		private static IEnumerable<IModifier> GenerateAllModifiersFor(this TurnState turn, GameDatabase db, Identifier id)
		{
			IModifiable modifiable = turn.FetchGameItem(id);
			if (modifiable == null)
			{
				return Enumerable.Empty<IModifier>();
			}
			return turn.GenerateAllModifiersFor(db, modifiable);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000432D8 File Offset: 0x000414D8
		private static IEnumerable<IModifier> GenerateAllModifiersFor(this TurnState turn, GameDatabase db, IModifiable modifiable)
		{
			BattlePhaseResult battlePhaseResult = modifiable as BattlePhaseResult;
			IEnumerable<IModifier> result;
			if (battlePhaseResult == null)
			{
				GamePiece gamePiece = modifiable as GamePiece;
				if (gamePiece == null)
				{
					PlayerState playerState = modifiable as PlayerState;
					if (playerState == null)
					{
						Praetor praetor = modifiable as Praetor;
						if (praetor == null)
						{
							result = Enumerable.Empty<IModifier>();
						}
						else
						{
							result = turn.GenerateAllModifiersFor(db, praetor);
						}
					}
					else
					{
						result = turn.GenerateAllModifiersFor(db, playerState);
					}
				}
				else
				{
					result = turn.GenerateAllModifiersFor(db, gamePiece);
				}
			}
			else
			{
				result = turn.GenerateAllModifiersFor(db, battlePhaseResult);
			}
			return result;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00043348 File Offset: 0x00041548
		public static IEnumerable<IModifier> GenerateAllModifiersFor(this TurnState turn, GameDatabase db, GamePiece piece)
		{
			PlayerState playerState = turn.FindPlayerState(piece.ControllingPlayerId, null);
			foreach (IModifier modifier in turn.GenerateTerrainModifiers(db, piece.Location))
			{
				yield return modifier;
			}
			IEnumerator<IModifier> enumerator = null;
			foreach (IModifier modifier2 in turn.GenerateLocalArchfiendModifiers(db, playerState))
			{
				yield return modifier2;
			}
			enumerator = null;
			foreach (IModifier modifier3 in turn.GenerateGameItemModifiers(db, piece))
			{
				yield return modifier3;
			}
			enumerator = null;
			foreach (IModifier modifier4 in turn.GenerateAttachedGameItemModifiers(db, piece))
			{
				yield return modifier4;
			}
			enumerator = null;
			foreach (IModifier modifier5 in ModifierExtensions.CreateModifiersFromAuraAbilities(turn, db, piece))
			{
				yield return modifier5;
			}
			enumerator = null;
			foreach (IModifier modifier6 in turn.GlobalModifierStack.GetGamePieceModifiers(piece))
			{
				yield return modifier6;
			}
			enumerator = null;
			foreach (IModifier modifier7 in turn.GlobalModifierStack.GetPlayerModifiers(playerState))
			{
				yield return modifier7;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00043366 File Offset: 0x00041566
		public static IEnumerable<IModifier> GenerateAllModifiersFor(this TurnState turn, GameDatabase db, PlayerState player)
		{
			foreach (IModifier modifier in turn.GlobalModifierStack.GetPlayerModifiers(player))
			{
				yield return modifier;
			}
			IEnumerator<IModifier> enumerator = null;
			foreach (IModifier modifier2 in turn.GenerateGameItemModifiers(db, player))
			{
				yield return modifier2;
			}
			enumerator = null;
			foreach (IModifier modifier3 in turn.GenerateLocalArchfiendModifiers(db, player))
			{
				yield return modifier3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00043384 File Offset: 0x00041584
		public static IEnumerable<IModifier> GenerateAllModifiersFor(this TurnState turn, GameDatabase db, Praetor praetorId)
		{
			PlayerState player = turn.FindControllingPlayer(praetorId);
			if (player == null || player.Id == -2147483648 || player.Id == -1)
			{
				yield break;
			}
			foreach (IModifier modifier in turn.GenerateLocalArchfiendModifiers(db, player))
			{
				yield return modifier;
			}
			IEnumerator<IModifier> enumerator = null;
			foreach (IModifier modifier2 in turn.GenerateGameItemModifiers(db, praetorId))
			{
				yield return modifier2;
			}
			enumerator = null;
			foreach (IModifier modifier3 in turn.GlobalModifierStack.GetGamePieceModifiers(praetorId))
			{
				yield return modifier3;
			}
			enumerator = null;
			foreach (IModifier modifier4 in turn.GlobalModifierStack.GetPlayerModifiers(player))
			{
				yield return modifier4;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x000433A2 File Offset: 0x000415A2
		private static IEnumerable<IModifier> GenerateLocalArchfiendModifiers(this TurnState turn, GameDatabase db, PlayerState player)
		{
			foreach (IModifier modifier in turn.GenerateDifficultyModifiers(db, player))
			{
				yield return modifier;
			}
			IEnumerator<IModifier> enumerator = null;
			foreach (IModifier modifier2 in turn.GenerateArchfiendModifiers(db, player))
			{
				yield return modifier2;
			}
			enumerator = null;
			foreach (IModifier modifier3 in turn.GenerateRelicModifiers(db, player))
			{
				yield return modifier3;
			}
			enumerator = null;
			foreach (IModifier modifier4 in turn.GeneratePowerModifiers(db, player))
			{
				yield return modifier4;
			}
			enumerator = null;
			foreach (IModifier modifier5 in turn.GenerateRankModifiers(db, player))
			{
				yield return modifier5;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x000433C0 File Offset: 0x000415C0
		private static IEnumerable<IModifier> GenerateTerrainModifiers(this TurnState turn, GameDatabase db, HexCoord coord)
		{
			if (coord == HexCoord.Invalid)
			{
				return Enumerable.Empty<IModifier>();
			}
			Hex hex = turn.HexBoard[coord];
			TerrainStaticData terrainStaticData;
			if (!db.TryFindTerrainData(hex, out terrainStaticData))
			{
				return Enumerable.Empty<IModifier>();
			}
			return terrainStaticData.CreateModifiers(db, new TerrainContext
			{
				TerrainType = terrainStaticData.LegacyType
			});
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00043417 File Offset: 0x00041617
		private static IEnumerable<IModifier> GenerateRankModifiers(this TurnState turn, GameDatabase db, PlayerState player)
		{
			return db.GetArchfiendRank(player.RankValue).CreateModifiers(db, new PlayerContext(player.Id, player.ArchfiendId));
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00043441 File Offset: 0x00041641
		private static IEnumerable<IModifier> GenerateDifficultyModifiers(this TurnState turn, GameDatabase db, PlayerState player)
		{
			if (player.Role != PlayerRole.Human)
			{
				return turn.GenerateDifficultyModifiers(db, player.AIDifficulty);
			}
			return Enumerable.Empty<IModifier>();
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00043460 File Offset: 0x00041660
		private static IEnumerable<IModifier> GenerateDifficultyModifiers(this TurnState turn, GameDatabase db, AIDifficulty difficulty)
		{
			AIDifficultyStaticData entity;
			if (db.TryGetDifficultyData(difficulty, out entity))
			{
				return entity.CreateModifiers(db, new AIDifficultyContext(difficulty));
			}
			return Enumerable.Empty<IModifier>();
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0004348B File Offset: 0x0004168B
		private static IEnumerable<IModifier> GenerateArchfiendModifiers(this TurnState turn, GameDatabase db, PlayerState player)
		{
			return db.Fetch<ArchFiendStaticData>(player.ArchfiendId).CreateModifiers(db, new PlayerContext(player.Id, player.ArchfiendId));
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x000434B0 File Offset: 0x000416B0
		private static IEnumerable<IModifier> GeneratePowerModifiers(this TurnState turn, GameDatabase db, PlayerState player)
		{
			return db.GetUnlockedAndChosenPowers(player).SelectMany((PowerBaseStaticData power) => power.CreateModifiers(db, new ArchfiendPowerContext()));
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000434E8 File Offset: 0x000416E8
		private static IEnumerable<IModifier> GenerateRelicModifiers(this TurnState turn, GameDatabase db, PlayerState player)
		{
			return player.ActiveRelics.SelectMany((Identifier id) => turn.GenerateGameItemModifiers(db, turn.FetchGameItem(id)));
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00043520 File Offset: 0x00041720
		private static IEnumerable<IModifier> GenerateGameItemModifiers(this TurnState turn, GameDatabase db, PlayerState player)
		{
			return turn.GetFieldedGameItemsControlledBy(player.Id).SelectMany((GameItem t) => turn.GenerateGameItemModifiers(db, t));
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00043564 File Offset: 0x00041764
		private static IEnumerable<IModifier> GenerateAttachedGameItemModifiers(this TurnState turn, GameDatabase db, GamePiece item)
		{
			return item.Slots.Select(new Func<Identifier, GameItem>(turn.FetchGameItem)).ExcludeNull<GameItem>().SelectMany((GameItem t) => turn.GenerateGameItemModifiers(db, t));
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x000435B8 File Offset: 0x000417B8
		private static IEnumerable<IModifier> GenerateGameItemModifiers(this TurnState turn, GameDatabase db, GameItem item)
		{
			if (item is AbilityPlaceholder)
			{
				return Enumerable.Empty<IModifier>();
			}
			GameItemContext context = new GameItemContext(item.StaticDataId);
			StaticDataEntity staticDataEntity = db.Fetch<StaticDataEntity>(item.StaticDataId);
			IEnumerable<IModifier> second = Enumerable.Empty<IModifier>();
			IEnumerable<IModifier> first = Enumerable.Empty<IModifier>();
			IUnlockProvider unlockProvider = staticDataEntity as IUnlockProvider;
			if (unlockProvider != null)
			{
				second = unlockProvider.CreateModifiersFromUnlocks(db, context);
			}
			if (staticDataEntity != null)
			{
				first = ModifierExtensions.CreateModifiers(context, staticDataEntity.GetComponents<ModifierStaticData>());
			}
			IEnumerable<IModifier> second2 = ModifierExtensions.CreateModifiersFromLocalAbilities(turn, db, item);
			return first.Concat(second).Concat(second2);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00043638 File Offset: 0x00041838
		private static IEnumerable<IModifier> CreateModifiersFromLocalAbilities(TurnState turn, GameDatabase db, GameItem item)
		{
			IEnumerable<Ability> localAbilities = db.GetLocalAbilities(item);
			return ModifierExtensions.CreateModifiersFromAbilities(db, item, localAbilities);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00043658 File Offset: 0x00041858
		private static IEnumerable<IModifier> CreateModifiersFromAuraAbilities(TurnState turn, GameDatabase db, GamePiece item)
		{
			IEnumerable<Ability> uniqueAuraAbilities = db.GetUniqueAuraAbilities(turn, item);
			return ModifierExtensions.CreateModifiersFromAbilities(db, item, uniqueAuraAbilities);
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00043678 File Offset: 0x00041878
		private static IEnumerable<IModifier> CreateModifiersFromAbilities(GameDatabase db, GameItem item, IEnumerable<Ability> abilities)
		{
			GameItemContext context = new GameItemContext(item.StaticDataId);
			List<IModifier> list = new List<IModifier>();
			foreach (Ability ability in abilities)
			{
				ItemAbilityStaticData itemAbilityStaticData = db.Fetch<ItemAbilityStaticData>(ability.SourceId);
				if (itemAbilityStaticData != null)
				{
					IEnumerable<IModifier> collection = ModifierExtensions.CreateModifiers(context, itemAbilityStaticData.GetComponents<ModifierStaticData>());
					list.AddRange(collection);
				}
			}
			return list;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000436F8 File Offset: 0x000418F8
		private static IEnumerable<IModifier> CreateModifiersFromProvidedAbilities(this GameItemStaticData entity, GameDatabase db, ModifierContext context)
		{
			foreach (string key in entity.ProvidedAbilities)
			{
				ItemAbilityStaticData itemAbilityStaticData = db.Fetch<ItemAbilityStaticData>(key);
				if (itemAbilityStaticData != null)
				{
					foreach (IModifier modifier in ModifierExtensions.CreateModifiers(context, itemAbilityStaticData.GetComponents<ModifierStaticData>()))
					{
						yield return modifier;
					}
					IEnumerator<IModifier> enumerator2 = null;
				}
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00043716 File Offset: 0x00041916
		private static IEnumerable<IModifier> CreateModifiersFromUnlocks(this IUnlockProvider entity, GameDatabase db, ModifierContext context)
		{
			foreach (ConfigRef<AbilityStaticData> configRef in entity.Unlocks)
			{
				ItemAbilityStaticData itemAbilityStaticData = db.Fetch<ItemAbilityStaticData>(configRef.Id);
				if (itemAbilityStaticData != null)
				{
					foreach (IModifier modifier in ModifierExtensions.CreateModifiers(context, itemAbilityStaticData.GetComponents<ModifierStaticData>()))
					{
						yield return modifier;
					}
					IEnumerator<IModifier> enumerator2 = null;
				}
			}
			IEnumerator<ConfigRef<AbilityStaticData>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00043734 File Offset: 0x00041934
		private static IEnumerable<IModifier> CreateModifiers(this StaticDataEntity entity, GameDatabase db, ModifierContext context)
		{
			if (entity == null)
			{
				yield break;
			}
			GameItemStaticData gameItemStaticData = entity as GameItemStaticData;
			IEnumerator<IModifier> enumerator;
			if (gameItemStaticData != null)
			{
				foreach (IModifier modifier in gameItemStaticData.CreateModifiersFromProvidedAbilities(db, context))
				{
					yield return modifier;
				}
				enumerator = null;
			}
			IUnlockProvider unlockProvider = entity as IUnlockProvider;
			if (unlockProvider != null)
			{
				foreach (IModifier modifier2 in unlockProvider.CreateModifiersFromUnlocks(db, context))
				{
					yield return modifier2;
				}
				enumerator = null;
			}
			foreach (IModifier modifier3 in ModifierExtensions.CreateModifiers(context, entity.GetComponents<ModifierStaticData>()))
			{
				yield return modifier3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00043754 File Offset: 0x00041954
		private static IEnumerable<IModifier> CreateModifiers(ModifierContext context, IEnumerable<ModifierStaticData> modifiers)
		{
			return (from t in modifiers
			select ModifierExtensions.CreateModifier(context, t)).ExcludeNull<IModifier>();
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00043788 File Offset: 0x00041988
		public static IModifier CreateModifier(ModifierContext context, ModifierStaticData data)
		{
			ArchfiendModifierStaticData archfiendModifierStaticData = data as ArchfiendModifierStaticData;
			IModifier modifier;
			if (archfiendModifierStaticData == null)
			{
				GamePieceModifierStaticData gamePieceModifierStaticData = data as GamePieceModifierStaticData;
				if (gamePieceModifierStaticData == null)
				{
					StatTransferModiferStaticData statTransferModiferStaticData = data as StatTransferModiferStaticData;
					if (statTransferModiferStaticData == null)
					{
						StatBoostMultiplierModiferStaticData statBoostMultiplierModiferStaticData = data as StatBoostMultiplierModiferStaticData;
						if (statBoostMultiplierModiferStaticData == null)
						{
							StatBoostPerLevelModifierStaticData statBoostPerLevelModifierStaticData = data as StatBoostPerLevelModifierStaticData;
							if (statBoostPerLevelModifierStaticData == null)
							{
								modifier = null;
							}
							else
							{
								modifier = new StatBoostPerLevelModifier(statBoostPerLevelModifierStaticData);
							}
						}
						else
						{
							modifier = new StatBoostMultiplierModifier(statBoostMultiplierModiferStaticData);
						}
					}
					else
					{
						modifier = new StatTransferModifier(statTransferModiferStaticData);
					}
				}
				else
				{
					modifier = new GamePieceModifier(gamePieceModifierStaticData);
				}
			}
			else
			{
				modifier = new ArchfiendModifier(archfiendModifierStaticData);
			}
			IModifier modifier2 = modifier;
			if (modifier2 == null)
			{
				return null;
			}
			modifier2.Source = context;
			return modifier2;
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00043814 File Offset: 0x00041A14
		private static void ApplyAll(TurnContext context, IModifiable modifiable, IEnumerable<IModifier> modifiers)
		{
			List<IModifier> list = new List<IModifier>();
			foreach (IModifier modifier in modifiers)
			{
				modifier.ApplyTo(context, modifiable);
				list.Add(modifier);
			}
			foreach (IModifier modifier2 in list)
			{
				modifier2.PostApplyTo(context, modifiable);
			}
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000438A8 File Offset: 0x00041AA8
		public static void ClearStatModifiers(this IModifiable modifiable)
		{
			IEnumerable<FieldInfo> source = from t in modifiable.GetType().AllMemberFields()
			where typeof(IModifiableField).IsAssignableFrom(t.FieldType)
			select t;
			Func<FieldInfo, IModifiableField> <>9__1;
			Func<FieldInfo, IModifiableField> selector;
			if ((selector = <>9__1) == null)
			{
				selector = (<>9__1 = ((FieldInfo t) => (IModifiableField)t.GetValue(modifiable)));
			}
			foreach (IModifiableField modifiableField in source.Select(selector))
			{
				modifiableField.ClearModifiers();
			}
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00043954 File Offset: 0x00041B54
		public static IEnumerable<ItemBanishedEvent> DestroyStratagems(this TurnProcessContext context, GamePiece gamePiece)
		{
			foreach (Stratagem stratagem in IEnumerableExtensions.ToList<Stratagem>(gamePiece.GetAttachedItems(context.CurrentTurn)))
			{
				if (gamePiece.TryRemoveItem(stratagem))
				{
					yield return context.BanishGameItem(stratagem.Id, int.MinValue);
				}
			}
			List<Stratagem>.Enumerator enumerator = default(List<Stratagem>.Enumerator);
			yield break;
			yield break;
		}
	}
}
