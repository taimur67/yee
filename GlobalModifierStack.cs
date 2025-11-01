using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000368 RID: 872
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GlobalModifierStack : IDeepClone<GlobalModifierStack>
	{
		// Token: 0x0600109B RID: 4251 RVA: 0x0004181E File Offset: 0x0003FA1E
		public IEnumerable<IModifier> GetPlayerModifiers(PlayerState player)
		{
			foreach (PlayerTargetGroup playerTargetGroup in this.PlayerModifiers)
			{
				if (playerTargetGroup.Targets.Contains(player.Id) && playerTargetGroup.Modifiers != null)
				{
					foreach (IModifier modifier in playerTargetGroup.Modifiers)
					{
						yield return modifier;
					}
					List<IModifier>.Enumerator enumerator2 = default(List<IModifier>.Enumerator);
				}
			}
			List<PlayerTargetGroup>.Enumerator enumerator = default(List<PlayerTargetGroup>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00041835 File Offset: 0x0003FA35
		public IEnumerable<IModifier> GetGamePieceModifiers(Identifier gamePieceId)
		{
			foreach (GameItemTargetGroup gameItemTargetGroup in this.GameItemModifiers)
			{
				if (gameItemTargetGroup.Targets.Contains(gamePieceId) && gameItemTargetGroup.Modifiers != null)
				{
					foreach (IModifier modifier in gameItemTargetGroup.Modifiers)
					{
						yield return modifier;
					}
					List<IModifier>.Enumerator enumerator2 = default(List<IModifier>.Enumerator);
				}
			}
			List<GameItemTargetGroup>.Enumerator enumerator = default(List<GameItemTargetGroup>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0004184C File Offset: 0x0003FA4C
		public IEnumerable<Ability> GetGameItemAbilities(GameItem gameItem)
		{
			GamePiece gamePiece = gameItem as GamePiece;
			if (gamePiece != null)
			{
				foreach (Ability ability in this.GetGamePieceAbilities(gamePiece))
				{
					yield return ability;
				}
				IEnumerator<Ability> enumerator = null;
				yield break;
			}
			foreach (GameItemTargetGroup gameItemTargetGroup in this.GameItemModifiers)
			{
				if (gameItemTargetGroup.Targets.Contains(gameItem.Id))
				{
					foreach (Ability ability2 in gameItemTargetGroup.Abilities)
					{
						yield return ability2;
					}
					List<Ability>.Enumerator enumerator3 = default(List<Ability>.Enumerator);
				}
			}
			List<GameItemTargetGroup>.Enumerator enumerator2 = default(List<GameItemTargetGroup>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00041863 File Offset: 0x0003FA63
		public IEnumerable<Ability> GetGamePieceAbilities(GamePiece gamePiece)
		{
			foreach (PlayerTargetGroup playerTargetGroup in this.PlayerModifiers)
			{
				if (playerTargetGroup.Targets.Contains(gamePiece.ControllingPlayerId))
				{
					foreach (Ability ability in playerTargetGroup.Abilities)
					{
						if (ability.CanBeAttachedTo(gamePiece))
						{
							yield return ability;
						}
					}
					List<Ability>.Enumerator enumerator2 = default(List<Ability>.Enumerator);
				}
			}
			List<PlayerTargetGroup>.Enumerator enumerator = default(List<PlayerTargetGroup>.Enumerator);
			foreach (GameItemTargetGroup gameItemTargetGroup in this.GameItemModifiers)
			{
				if (gameItemTargetGroup.Targets.Contains(gamePiece.Id))
				{
					foreach (Ability ability2 in gameItemTargetGroup.Abilities)
					{
						if (ability2.CanBeAttachedTo(gamePiece))
						{
							yield return ability2;
						}
					}
					List<Ability>.Enumerator enumerator2 = default(List<Ability>.Enumerator);
				}
			}
			List<GameItemTargetGroup>.Enumerator enumerator3 = default(List<GameItemTargetGroup>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0004187A File Offset: 0x0003FA7A
		public void Push(PlayerTargetGroup playerTarget)
		{
			this.PlayerModifiers.Add(playerTarget);
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00041888 File Offset: 0x0003FA88
		public void Push(GameItemTargetGroup gameItemTarget)
		{
			this.GameItemModifiers.Add(gameItemTarget);
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00041898 File Offset: 0x0003FA98
		public void Pop(Guid id)
		{
			this.PlayerModifiers.RemoveAll((PlayerTargetGroup x) => x.Id == id);
			this.GameItemModifiers.RemoveAll((GameItemTargetGroup x) => x.Id == id);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x000418E2 File Offset: 0x0003FAE2
		public void DeepClone(out GlobalModifierStack clone)
		{
			clone = new GlobalModifierStack
			{
				GameItemModifiers = this.GameItemModifiers.DeepClone(CloneFunction.FastClone),
				PlayerModifiers = this.PlayerModifiers.DeepClone(CloneFunction.FastClone)
			};
		}

		// Token: 0x040007BB RID: 1979
		[JsonProperty]
		public List<GameItemTargetGroup> GameItemModifiers = new List<GameItemTargetGroup>();

		// Token: 0x040007BC RID: 1980
		[JsonProperty]
		public List<PlayerTargetGroup> PlayerModifiers = new List<PlayerTargetGroup>();
	}
}
