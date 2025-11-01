using System;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020003D9 RID: 985
	public static class AbilityCooldownExtensions
	{
		// Token: 0x06001328 RID: 4904 RVA: 0x00048D1B File Offset: 0x00046F1B
		public static bool IsOnCooldown(this PlayerState player, ConfigRef abilityId)
		{
			return !abilityId.IsEmpty() && player.AbilityCooldowns.ContainsKey(abilityId.Id);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00048D38 File Offset: 0x00046F38
		public static void SetAbilityCooldown(this PlayerState player, AbilityStaticData ability)
		{
			if (ability == null)
			{
				return;
			}
			player.SetAbilityCooldown(ability.ConfigRef, ability.Cooldown);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00048D50 File Offset: 0x00046F50
		public static void SetAbilityCooldown(this PlayerState player, ConfigRef abilityId, int count)
		{
			if (!abilityId.IsEmpty())
			{
				player.AbilityCooldowns[abilityId.Id] = count;
			}
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00048D6C File Offset: 0x00046F6C
		public static bool TryGetCooldown(this PlayerState player, AbilityStaticData ability, out int count)
		{
			return player.TryGetCooldown((ability != null) ? ability.ConfigRef : null, out count);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00048D81 File Offset: 0x00046F81
		public static bool TryGetCooldown(this PlayerState player, ConfigRef abilityId, out int count)
		{
			if (abilityId.IsEmpty())
			{
				count = 0;
				return false;
			}
			return player.AbilityCooldowns.TryGetValue(abilityId.Id, out count);
		}
	}
}
