using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000353 RID: 851
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Ability : IDeepClone<Ability>
	{
		// Token: 0x17000273 RID: 627
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0003FDC9 File Offset: 0x0003DFC9
		// (set) Token: 0x0600102C RID: 4140 RVA: 0x0003FDD1 File Offset: 0x0003DFD1
		[JsonProperty]
		public string Name { get; set; }

		// Token: 0x0600102D RID: 4141 RVA: 0x0003FDDA File Offset: 0x0003DFDA
		public Ability()
		{
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0003FE08 File Offset: 0x0003E008
		public Ability(ItemAbilityStaticData data)
		{
			this.Unique = data.Unique;
			this.ValidTargets = data.ValidTargets;
			this._blockedByTags = data.BlockedByTags;
			foreach (AbilityEffect effect in data.Effects)
			{
				this.AddEffect(effect);
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0003FEAC File Offset: 0x0003E0AC
		public IEnumerable<T> GetEffects<T>() where T : AbilityEffect
		{
			return this.Effects.OfType<T>();
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x0003FEBC File Offset: 0x0003E0BC
		public IList<AbilityEffect> Effects
		{
			get
			{
				List<AbilityEffect> result;
				if ((result = this._effects) == null)
				{
					result = (this._effects = new List<AbilityEffect>());
				}
				return result;
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0003FEE1 File Offset: 0x0003E0E1
		public void AddEffect(AbilityEffect effect)
		{
			this._effects.Add(effect);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0003FEF0 File Offset: 0x0003E0F0
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.Name))
			{
				return this.Name;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (AbilityEffect abilityEffect in this.Effects)
			{
				stringBuilder.AppendLine(abilityEffect.GetType().Name);
			}
			return StringBuilderExtensions.Trim(stringBuilder).ToString();
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0003FF70 File Offset: 0x0003E170
		public static implicit operator AbilityContext(Ability ability)
		{
			return new AbilityContext
			{
				SourceId = ability.SourceId
			};
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0003FF84 File Offset: 0x0003E184
		public bool CanBeAttachedTo(GamePiece piece)
		{
			if (this._blockedByTags != null)
			{
				foreach (EntityTag tag in this._blockedByTags)
				{
					if (piece.HasTag(tag))
					{
						return false;
					}
				}
			}
			return this.ValidTargets.ValidGamePieceTypes.IsSet((int)piece.SubCategory);
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00040000 File Offset: 0x0003E200
		protected virtual T DeepCloneParts<T>(T clone) where T : Ability
		{
			clone.Name = this.Name.DeepClone();
			clone.SourceId = this.SourceId.DeepClone();
			clone.ProviderId = this.ProviderId;
			clone.Unique = this.Unique;
			clone._effects = this._effects.DeepClone<AbilityEffect>();
			clone._blockedByTags = this._blockedByTags.DeepClone<EntityTag>();
			clone.ValidTargets = this.ValidTargets.DeepClone(CloneFunction.FastClone);
			clone.ModifierGroupId = this.ModifierGroupId;
			return clone;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x000400B0 File Offset: 0x0003E2B0
		public virtual void DeepClone(out Ability clone)
		{
			clone = this.DeepCloneParts<Ability>(new Ability());
		}

		// Token: 0x0400077F RID: 1919
		[JsonProperty]
		public string SourceId;

		// Token: 0x04000780 RID: 1920
		[JsonProperty]
		public bool Unique;

		// Token: 0x04000781 RID: 1921
		[JsonProperty]
		public Identifier ProviderId;

		// Token: 0x04000782 RID: 1922
		[JsonProperty]
		private List<AbilityEffect> _effects = new List<AbilityEffect>();

		// Token: 0x04000783 RID: 1923
		[JsonProperty]
		private List<EntityTag> _blockedByTags = new List<EntityTag>();

		// Token: 0x04000784 RID: 1924
		[JsonProperty]
		public GamePieceTargetSettings ValidTargets = new GamePieceTargetSettings(BitMask.All);

		// Token: 0x04000785 RID: 1925
		[JsonProperty]
		public Guid ModifierGroupId;
	}
}
