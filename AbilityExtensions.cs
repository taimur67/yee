using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000355 RID: 853
	public static class AbilityExtensions
	{
		// Token: 0x0600103B RID: 4155 RVA: 0x000401D8 File Offset: 0x0003E3D8
		public static IEnumerable<string> GetTacticsFromAbilities([TupleElementNames(new string[]
		{
			"Ability",
			"Effect"
		})] this IEnumerable<ValueTuple<Ability, CombatAbilityEffect>> abilities)
		{
			return from x in abilities
			where x.Item2 is StratagemTactic
			select ((StratagemTactic)x.Item2).StaticDataId;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x00040230 File Offset: 0x0003E430
		[return: TupleElementNames(new string[]
		{
			"Ability",
			"Effect"
		})]
		public static IEnumerable<ValueTuple<Ability, T>> GetAbilityEffects<T>(this TurnProcessContext context, GamePiece piece) where T : AbilityEffect
		{
			bool flag = typeof(CombatAbilityEffect).IsAssignableFrom(typeof(T));
			bool cancelAbilities = !piece.CanUseCombatAbilities && flag;
			return context.GetAllAbilitiesFor(piece).SelectMany((Ability ability) => ability.GetEffects<T>(), delegate(Ability ability, T effect)
			{
				effect.SourceId = ability.SourceId;
				return new ValueTuple<Ability, T>(ability, effect);
			}).Where(delegate([TupleElementNames(new string[]
			{
				"ability",
				"effect"
			})] ValueTuple<Ability, T> x)
			{
				if (!cancelAbilities)
				{
					return true;
				}
				CombatAbilityEffect combatAbilityEffect = x.Item2 as CombatAbilityEffect;
				return combatAbilityEffect == null || !combatAbilityEffect.CanBeCancelled;
			});
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x000402CE File Offset: 0x0003E4CE
		public static IEnumerable<Ability> GetAllAbilitiesFor(this TurnContext context, GameItem item)
		{
			if (item == null)
			{
				yield break;
			}
			foreach (Ability ability in context.Database.GetLocalAbilities(item))
			{
				yield return ability;
			}
			IEnumerator<Ability> enumerator = null;
			foreach (Ability ability2 in context.CurrentTurn.GlobalModifierStack.GetGameItemAbilities(item))
			{
				yield return ability2;
			}
			enumerator = null;
			GamePiece gamePiece = item as GamePiece;
			if (gamePiece != null)
			{
				foreach (Ability ability3 in context.GetAcquiredAbilities(gamePiece))
				{
					yield return ability3;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000402E5 File Offset: 0x0003E4E5
		private static IEnumerable<Ability> FilterUniqueAbilities(this IEnumerable<Ability> collection)
		{
			return collection.Distinct(new UniqueAbilityComparer());
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000402F2 File Offset: 0x0003E4F2
		public static IEnumerable<Ability> GetTerrainAbilities(this GameDatabase database, TurnState turn, GamePiece piece)
		{
			if (piece.Location == HexCoord.Invalid)
			{
				yield break;
			}
			Hex hex = turn.HexBoard[piece.Location];
			TerrainStaticData terrainStaticData;
			if (!database.TryFindTerrainData(hex, out terrainStaticData))
			{
				yield break;
			}
			foreach (ConfigRef<ItemAbilityStaticData> cref in terrainStaticData.ProvidedAbilities)
			{
				ItemAbilityStaticData itemAbilityStaticData = database.Fetch(cref);
				if (itemAbilityStaticData != null)
				{
					yield return new Ability(itemAbilityStaticData)
					{
						ProviderId = piece.Id,
						SourceId = itemAbilityStaticData.Id,
						Name = itemAbilityStaticData.Id
					};
				}
			}
			List<ConfigRef<ItemAbilityStaticData>>.Enumerator enumerator = default(List<ConfigRef<ItemAbilityStaticData>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x00040310 File Offset: 0x0003E510
		public static IEnumerable<Ability> GetLocalAbilities(this GameDatabase database, GameItem item)
		{
			foreach (Ability ability in item.Abilities)
			{
				yield return ability;
			}
			IEnumerator<Ability> enumerator = null;
			GameItemStaticData gameItemStaticData = database.Fetch<GameItemStaticData>(item.StaticDataId);
			if (gameItemStaticData == null)
			{
				yield break;
			}
			foreach (string key in gameItemStaticData.ProvidedAbilities)
			{
				ItemAbilityStaticData itemAbilityStaticData = database.Fetch<ItemAbilityStaticData>(key);
				if (itemAbilityStaticData != null)
				{
					yield return new Ability(itemAbilityStaticData)
					{
						ProviderId = item.Id,
						SourceId = itemAbilityStaticData.Id,
						Name = itemAbilityStaticData.Id
					};
				}
			}
			IEnumerator<string> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00040328 File Offset: 0x0003E528
		public static IEnumerable<string> GetProvidedUnlockIds(this GameDatabase database, GameItem item)
		{
			GameItemStaticData gameItemStaticData = database.Fetch<GameItemStaticData>(item.StaticDataId);
			if (gameItemStaticData == null)
			{
				return Enumerable.Empty<string>();
			}
			return from x in gameItemStaticData.Unlocks
			select x.Id;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00040378 File Offset: 0x0003E578
		public static IEnumerable<ConfigRef<AbilityStaticData>> GetProvidedUnlocks(this GameDatabase database, GameItem item)
		{
			GameItemStaticData gameItemStaticData = database.Fetch<GameItemStaticData>(item.StaticDataId);
			if (gameItemStaticData == null)
			{
				return Enumerable.Empty<ConfigRef<AbilityStaticData>>();
			}
			return gameItemStaticData.Unlocks;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x000403A1 File Offset: 0x0003E5A1
		public static IEnumerable<Ability> GetAcquiredAbilities(this TurnContext context, GamePiece piece)
		{
			return context.Database.GetAcquiredAbilities(context.CurrentTurn, piece);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x000403B5 File Offset: 0x0003E5B5
		public static IEnumerable<Ability> GetAcquiredAbilities(this GameDatabase db, TurnState turn, GamePiece piece)
		{
			IEnumerator<Ability> enumerator2;
			if (piece.Slots.Count > 0)
			{
				foreach (Identifier identifier in piece.Slots)
				{
					GameItem gameItem = turn.FetchGameItem(identifier);
					if (gameItem == null)
					{
						SimLogger logger = SimLogger.Logger;
						if (logger != null)
						{
							logger.Error(string.Format("GamePiece {0} slotIdentifier {1} returned null item on fetch", piece, identifier));
						}
					}
					else if (gameItem.Status == GameItemStatus.InPlay && !(gameItem is AbilityPlaceholder))
					{
						foreach (Ability ability in db.GetLocalAbilities(gameItem))
						{
							if (ability.CanBeAttachedTo(piece))
							{
								yield return ability;
							}
						}
						enumerator2 = null;
					}
				}
				List<Identifier>.Enumerator enumerator = default(List<Identifier>.Enumerator);
			}
			foreach (Ability ability2 in db.GetTerrainAbilities(turn, piece))
			{
				if (ability2.CanBeAttachedTo(piece))
				{
					yield return ability2;
				}
			}
			enumerator2 = null;
			foreach (Ability ability3 in db.GetUniqueAuraAbilities(turn, piece))
			{
				yield return ability3;
			}
			enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x000403D3 File Offset: 0x0003E5D3
		public static IEnumerable<Ability> GetUniqueAuraAbilities(this GameDatabase db, TurnState turn, GamePiece piece)
		{
			return db.GetAuraAbilities(turn, piece).FilterUniqueAbilities();
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x000403E2 File Offset: 0x0003E5E2
		public static IEnumerable<Ability> GetAuraAbilities(this GameDatabase db, TurnState turn, GamePiece piece)
		{
			foreach (Aura aura in turn.GetAurasAffecting(piece))
			{
				foreach (ConfigRef<ItemAbilityStaticData> cref in aura.Abilities)
				{
					ItemAbilityStaticData itemAbilityStaticData = db.Fetch(cref);
					if (itemAbilityStaticData != null)
					{
						yield return new Ability(itemAbilityStaticData)
						{
							ProviderId = aura.ProviderId,
							SourceId = itemAbilityStaticData.Id,
							Name = itemAbilityStaticData.Id
						};
					}
				}
				IEnumerator<ConfigRef<ItemAbilityStaticData>> enumerator2 = null;
				aura = null;
			}
			IEnumerator<Aura> enumerator = null;
			yield break;
			yield break;
		}
	}
}
