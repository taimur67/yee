using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002C0 RID: 704
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class CombatStats : IModifiable, IModifiableField, IDeepClone<CombatStats>
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x000352A0 File Offset: 0x000334A0
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x000352C5 File Offset: 0x000334C5
		[JsonIgnore]
		public ModifiableValue RangedResist
		{
			get
			{
				ModifiableValue result;
				if (this.Resistances.TryGetValue(DamageType.Ranged, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Resistances[DamageType.Ranged] = value;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x000352D4 File Offset: 0x000334D4
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x000352F9 File Offset: 0x000334F9
		[JsonIgnore]
		public ModifiableValue MeleeResist
		{
			get
			{
				ModifiableValue result;
				if (this.Resistances.TryGetValue(DamageType.Melee, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Resistances[DamageType.Melee] = value;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x00035308 File Offset: 0x00033508
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x0003532D File Offset: 0x0003352D
		[JsonIgnore]
		public ModifiableValue InfernalResist
		{
			get
			{
				ModifiableValue result;
				if (this.Resistances.TryGetValue(DamageType.Infernal, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Resistances[DamageType.Infernal] = value;
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0003533C File Offset: 0x0003353C
		public IEnumerable<ModifiableValue> EnumerateStatValues()
		{
			yield return this.Ranged;
			yield return this.Melee;
			yield return this.Infernal;
			yield return this.RangedResist;
			yield return this.MeleeResist;
			yield return this.InfernalResist;
			yield break;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0003534C File Offset: 0x0003354C
		public IEnumerable<ValueTuple<CombatStatType, ModifiableValue>> EnumerateStats()
		{
			yield return new ValueTuple<CombatStatType, ModifiableValue>(CombatStatType.Ranged, this.Ranged);
			yield return new ValueTuple<CombatStatType, ModifiableValue>(CombatStatType.Melee, this.Melee);
			yield return new ValueTuple<CombatStatType, ModifiableValue>(CombatStatType.Infernal, this.Infernal);
			yield break;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003535C File Offset: 0x0003355C
		public ModifiableValue GetStat(DamageType type)
		{
			ModifiableValue result;
			switch (type)
			{
			case DamageType.Ranged:
				result = this.Ranged;
				break;
			case DamageType.Melee:
				result = this.Melee;
				break;
			case DamageType.Infernal:
				result = this.Infernal;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003539C File Offset: 0x0003359C
		public ModifiableValue GetStat(CombatStatType type)
		{
			ModifiableValue result;
			switch (type)
			{
			case CombatStatType.Ranged:
				result = this.Ranged;
				break;
			case CombatStatType.Melee:
				result = this.Melee;
				break;
			case CombatStatType.Infernal:
				result = this.Infernal;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000353DC File Offset: 0x000335DC
		public void Add(CombatStats other)
		{
			this.Ranged += other.Ranged;
			this.Melee += other.Melee;
			this.Infernal += other.Infernal;
			this.RangedResist += other.RangedResist;
			this.MeleeResist += other.MeleeResist;
			this.InfernalResist += other.InfernalResist;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000354B5 File Offset: 0x000336B5
		public void ClearModifiers()
		{
			this.ClearStatModifiers();
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000354BD File Offset: 0x000336BD
		public IEnumerable<string> GetFieldStrings()
		{
			foreach (FieldInfo fieldInfo in base.GetType().GetFields())
			{
				if (!(fieldInfo.FieldType != typeof(ModifiableValue)))
				{
					int num = (ModifiableValue)fieldInfo.GetValue(this);
					if (num != 0)
					{
						yield return string.Format("{0}: {1}", fieldInfo.Name, num);
					}
				}
			}
			FieldInfo[] array = null;
			yield break;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x000354D0 File Offset: 0x000336D0
		public void DeepClone(out CombatStats clone)
		{
			clone = new CombatStats
			{
				Ranged = this.Ranged.DeepClone<ModifiableValue>(),
				Melee = this.Melee.DeepClone<ModifiableValue>(),
				Infernal = this.Infernal.DeepClone<ModifiableValue>(),
				Resistances = this.Resistances.DeepClone<DamageType, ModifiableValue>()
			};
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00035528 File Offset: 0x00033728
		public BattlePhase GetWeakestStat()
		{
			if (this.Ranged < this.Melee && this.Ranged < this.Infernal)
			{
				return BattlePhase.Ranged;
			}
			if (this.Melee < this.Ranged && this.Melee < this.Infernal)
			{
				return BattlePhase.Melee;
			}
			if (this.Infernal < this.Ranged && this.Infernal < this.Melee)
			{
				return BattlePhase.Infernal;
			}
			return BattlePhase.Undefined;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000355CC File Offset: 0x000337CC
		public BattlePhase GetStrongestStat()
		{
			if (this.Ranged > this.Melee && this.Ranged > this.Infernal)
			{
				return BattlePhase.Ranged;
			}
			if (this.Melee > this.Ranged && this.Melee > this.Infernal)
			{
				return BattlePhase.Melee;
			}
			if (this.Infernal > this.Ranged && this.Infernal > this.Melee)
			{
				return BattlePhase.Infernal;
			}
			return BattlePhase.Undefined;
		}

		// Token: 0x040005F4 RID: 1524
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue Ranged = 0;

		// Token: 0x040005F5 RID: 1525
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue Melee = 0;

		// Token: 0x040005F6 RID: 1526
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue Infernal = 0;

		// Token: 0x040005F7 RID: 1527
		[JsonProperty]
		public Dictionary<DamageType, ModifiableValue> Resistances = new Dictionary<DamageType, ModifiableValue>();
	}
}
