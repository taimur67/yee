using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020006EB RID: 1771
	public static class ResourceUtils
	{
		// Token: 0x060021BD RID: 8637 RVA: 0x00075B40 File Offset: 0x00073D40
		public static void DestroyAllOwnedBy(this TurnProcessContext context, PlayerState player)
		{
			player.DestroyResources();
			foreach (Identifier itemId in IEnumerableExtensions.ToList<Identifier>(player.VaultedItems))
			{
				context.BanishGameItem(itemId, int.MinValue);
			}
			TurnState currentTurn = context.CurrentTurn;
			foreach (GamePiece gamePiece in currentTurn.GetPiecesControlledBy(player.Id))
			{
				if (gamePiece.IsFixture() && !gamePiece.HasTag<EntityTag_DestructibleFixture>())
				{
					currentTurn.SetNeutral(gamePiece);
				}
				else
				{
					context.KillGamePiece(gamePiece, -1);
				}
			}
			foreach (Identifier itemId2 in IEnumerableExtensions.ToList<Identifier>(player.RitualState.SlottedItems))
			{
				context.BanishGameItem(itemId2, int.MinValue);
			}
			foreach (Hex hex in IEnumerableExtensions.ToList<Hex>(currentTurn.HexBoard.GetHexesControlledByPlayer(player.Id)))
			{
				context.ClaimCanton(hex.HexCoord, -1);
			}
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x00075CC0 File Offset: 0x00073EC0
		public static void DestroyResources(this PlayerState player)
		{
			foreach (ResourceNFT nft in IEnumerableExtensions.ToList<ResourceNFT>(player.Resources))
			{
				player.DestroyResource(nft);
			}
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x00075D18 File Offset: 0x00073F18
		public static void DestroyResources(this PlayerState player, IEnumerable<ResourceNFT> resources)
		{
			foreach (ResourceNFT nft in resources)
			{
				player.DestroyResource(nft);
			}
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00075D60 File Offset: 0x00073F60
		public static void CheatCombinedResources(this PlayerState player, TurnState turn, int numCards = 8, int value = 9)
		{
			for (int i = 0; i < numCards; i++)
			{
				ResourceAccumulation resourceAccumulation = new ResourceAccumulation();
				foreach (ResourceTypes type in ResourceNFT.ResourceKeys)
				{
					resourceAccumulation[type] = value;
				}
				player.Cheat_CreateCard(turn, resourceAccumulation);
			}
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x00075DC8 File Offset: 0x00073FC8
		public static void CheatResourcesSpread(this PlayerState player, TurnState turn)
		{
			foreach (ResourceTypes resource in EnumUtility.GetValues<ResourceTypes>())
			{
				player.CheatResource(turn, resource);
			}
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x00075DF5 File Offset: 0x00073FF5
		public static void CheatResource(this PlayerState player, TurnState turn, ResourceTypes resource)
		{
			if (resource == ResourceTypes.Prestige)
			{
				player.GivePrestige(150);
				return;
			}
			ResourceUtils.Cheat_CreateCards(player, turn, resource, new int[]
			{
				1,
				2,
				3,
				5,
				8
			});
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x00075E20 File Offset: 0x00074020
		public static void CheatResourcesMax(this PlayerState player, TurnState turn, ResourceTypes resource)
		{
			if (resource == ResourceTypes.Prestige)
			{
				player.SpendablePrestige = 999;
				return;
			}
			ResourceUtils.Cheat_CreateCards(player, turn, resource, new int[]
			{
				99,
				99,
				99,
				99,
				99,
				99,
				99,
				99,
				99,
				99,
				99,
				99,
				99,
				99,
				99
			});
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x00075E4C File Offset: 0x0007404C
		private static void Cheat_CreateCards(PlayerState player, TurnState turn, ResourceTypes resource, int[] values)
		{
			foreach (int value in values)
			{
				ResourceAccumulation resourceAccumulation = new ResourceAccumulation();
				resourceAccumulation[resource] = value;
				player.Cheat_CreateCard(turn, resourceAccumulation);
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00075E84 File Offset: 0x00074084
		public static ResourceNFT Cheat_CreateCard(this PlayerState player, TurnState turn, ResourceAccumulation resources)
		{
			ResourceNFT resourceNFT = turn.CreateNFT(new ResourceAccumulation[]
			{
				resources
			});
			resourceNFT.Values[ResourceTypes.Prestige] = 0;
			player.GiveResources(new ResourceNFT[]
			{
				resourceNFT
			});
			return resourceNFT;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00075EC4 File Offset: 0x000740C4
		public static IEnumerable<ResourceTypes> GetAllNonNullResources(this ResourceAccumulation accumulation)
		{
			return from t in ResourceNFT.ResourceKeys
			where accumulation[t] > 0
			select t;
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x00075EF4 File Offset: 0x000740F4
		public static IEnumerable<ResourceTypes> GetAllNonNullNotFullResources(this ResourceAccumulation accumulation, int maxValue)
		{
			return from t in ResourceNFT.ResourceKeys
			where accumulation[t] > 0 && accumulation[t] < maxValue
			select t;
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00075F2C File Offset: 0x0007412C
		public static IEnumerable<ResourceTypes> GetAllNullResources(this ResourceAccumulation accumulation)
		{
			return from t in ResourceNFT.ResourceKeys
			where accumulation[t] == 0
			select t;
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x00075F5C File Offset: 0x0007415C
		public static bool HasSpaceForResourcesOnExistingTypes(this ResourceAccumulation accumulation, int maxValue)
		{
			foreach (ResourceTypes type in ResourceNFT.ResourceKeys)
			{
				if (accumulation[type] > 0 && accumulation[type] < maxValue)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x00075FBC File Offset: 0x000741BC
		public static bool HasSpaceForMoreResources(this ResourceAccumulation accumulation, int maxValue)
		{
			foreach (ResourceTypes type in ResourceNFT.ResourceKeys)
			{
				if (accumulation[type] < maxValue)
				{
					return true;
				}
			}
			return false;
		}
	}
}
