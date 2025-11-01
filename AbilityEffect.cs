using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000354 RID: 852
	[Serializable]
	public class AbilityEffect : IDeepClone<AbilityEffect>
	{
		// Token: 0x06001037 RID: 4151 RVA: 0x000400C0 File Offset: 0x0003E2C0
		public bool IsNullified(TurnContext context, GameItem abilityOwner)
		{
			if (this.NullifiedByOwnAbilities.Count > 0)
			{
				foreach (Ability ability in context.GetAllAbilitiesFor(abilityOwner))
				{
					foreach (ConfigRef<ItemAbilityStaticData> configRef in this.NullifiedByOwnAbilities)
					{
						if (!configRef.IsEmpty() && configRef.Id == ability.SourceId)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00040174 File Offset: 0x0003E374
		protected void DeepCloneAbilityEffectParts(AbilityEffect abilityEffect)
		{
			abilityEffect.SourceId = this.SourceId.DeepClone();
			abilityEffect.AffectsCategory = this.AffectsCategory.DeepClone();
			abilityEffect.NullifiedByOwnAbilities = this.NullifiedByOwnAbilities.DeepClone<ItemAbilityStaticData>();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x000401A9 File Offset: 0x0003E3A9
		public virtual void DeepClone(out AbilityEffect clone)
		{
			clone = new AbilityEffect();
			this.DeepCloneAbilityEffectParts(clone);
		}

		// Token: 0x04000786 RID: 1926
		[JsonProperty]
		[BindableValue("ability", BindingOption.StaticDataId)]
		public string SourceId;

		// Token: 0x04000787 RID: 1927
		[JsonProperty]
		public List<GamePieceCategory> AffectsCategory = new List<GamePieceCategory>();

		// Token: 0x04000788 RID: 1928
		[JsonProperty]
		public List<ConfigRef<ItemAbilityStaticData>> NullifiedByOwnAbilities = new List<ConfigRef<ItemAbilityStaticData>>();
	}
}
