using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Game.Simulation.Utils;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000379 RID: 889
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ModifiableValue : IModifiableField, IDontSerializeIfDefault, ICloneable, IDeepClone<ModifiableValue>
	{
		// Token: 0x060010DE RID: 4318 RVA: 0x00041FE8 File Offset: 0x000401E8
		[JsonConstructor]
		public ModifiableValue()
		{
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0004205C File Offset: 0x0004025C
		public ModifiableValue(float baseValue, int lowerBound = 0, int upperBound = 2147483647, RoundingMode roundingMode = RoundingMode.RoundDown)
		{
			this._initialValue = baseValue;
			this._baseValue = baseValue;
			this._lowerBound = lowerBound;
			this._upperBound = upperBound;
			this.RoundingMode = roundingMode;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x000420F1 File Offset: 0x000402F1
		public bool IsDefault(DefaultValueAttribute defaultValueAttribute)
		{
			return defaultValueAttribute != null && this.IsDefault(defaultValueAttribute.FloatValue(0f), 0, int.MaxValue, RoundingMode.RoundDown);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00042110 File Offset: 0x00040310
		public bool IsDefault(float baseValue, int lowerBound = 0, int upperBound = 2147483647, RoundingMode roundingMode = RoundingMode.RoundDown)
		{
			return MathUtils.IsMagnitudinallyApproximate(baseValue, this._initialValue, 1E-05f) && MathUtils.IsMagnitudinallyApproximate(this._baseValue, this._initialValue, 1E-05f) && this._lowerBound == lowerBound && this._upperBound == upperBound && this.RoundingMode == roundingMode && this.InstalledModifiers.Count == 0 && this.TransientModifiers.Count == 0;
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00042181 File Offset: 0x00040381
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x00042189 File Offset: 0x00040389
		[JsonProperty]
		[DefaultValue(RoundingMode.RoundDown)]
		public RoundingMode RoundingMode { get; private set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00042192 File Offset: 0x00040392
		// (set) Token: 0x060010E5 RID: 4325 RVA: 0x0004219A File Offset: 0x0004039A
		[JsonProperty]
		private List<StatModifierBase> TransientModifiers { get; set; } = new List<StatModifierBase>();

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x000421A3 File Offset: 0x000403A3
		// (set) Token: 0x060010E7 RID: 4327 RVA: 0x000421AB File Offset: 0x000403AB
		[JsonProperty]
		private List<StatModifierBase> InstalledModifiers { get; set; } = new List<StatModifierBase>();

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x000421B4 File Offset: 0x000403B4
		[JsonIgnore]
		public bool HasModifiers
		{
			get
			{
				return this.ActiveModifiers.Count > 0;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x000421C4 File Offset: 0x000403C4
		[JsonIgnore]
		public IReadOnlyList<StatModifierBase> ActiveModifiers
		{
			get
			{
				if (this._requiresActiveModifierRecalculation)
				{
					this._activeModifiers.Clear();
					this._activeModifiers.AddRange(this.InstalledModifiers);
					this._activeModifiers.AddRange(this.TransientModifiers);
					this._requiresActiveModifierRecalculation = false;
				}
				return this._activeModifiers;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x00042213 File Offset: 0x00040413
		[JsonIgnore]
		public float InitialValue
		{
			get
			{
				return this._initialValue;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x0004221B File Offset: 0x0004041B
		[JsonIgnore]
		public int BaseValue
		{
			get
			{
				return this.RoundValue(this._baseValue);
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x00042229 File Offset: 0x00040429
		[JsonIgnore]
		public int Value
		{
			get
			{
				return this.RoundValue(this.RawValue);
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x00042237 File Offset: 0x00040437
		[JsonIgnore]
		public int UnclampedRoundedValue
		{
			get
			{
				return this.RoundValue(this.UnclampedModifiedValue);
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x00042245 File Offset: 0x00040445
		[JsonIgnore]
		public float Difference
		{
			get
			{
				return (float)(this.Value - this.BaseValue);
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x00042255 File Offset: 0x00040455
		[JsonIgnore]
		public float RawValue
		{
			get
			{
				return Math.Min((float)this.UpperBound, Math.Max((float)this.LowerBound, this.UnclampedModifiedValue));
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x00042275 File Offset: 0x00040475
		[JsonIgnore]
		public float Modifier
		{
			get
			{
				return this.UnclampedModifiedValue - (float)this.BaseValue;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00042285 File Offset: 0x00040485
		[JsonIgnore]
		public float UnclampedModifiedValue
		{
			get
			{
				return this.RecalculateModifiedValue();
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0004228D File Offset: 0x0004048D
		[JsonIgnore]
		public int LowerBound
		{
			get
			{
				return this.RecalculateLowerBound();
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00042295 File Offset: 0x00040495
		[JsonIgnore]
		public int UpperBound
		{
			get
			{
				return this.RecalculateUpperBound();
			}
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x000422A0 File Offset: 0x000404A0
		private void FetchValueModifiers(out List<StatModifierBase> valueModifiers, out List<StatModifier> valueStatModifiers)
		{
			if (this._requiresValueModifierRecalculation)
			{
				this._valueModifiers.Clear();
				this._valueStatModifiers.Clear();
				this._valueModifiers.AddRange(this.ActiveModifiers);
				this._valueModifiers.Sort((StatModifierBase x, StatModifierBase y) => x.Priority - y.Priority);
				foreach (StatModifierBase statModifierBase in this._valueModifiers)
				{
					StatModifier statModifier = statModifierBase as StatModifier;
					if (statModifier != null)
					{
						this._valueStatModifiers.Add(statModifier);
					}
				}
				this._requiresValueModifierRecalculation = false;
			}
			valueModifiers = this._valueModifiers;
			valueStatModifiers = this._valueStatModifiers;
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00042374 File Offset: 0x00040574
		[JsonIgnore]
		private IEnumerable<StatModifier> LowerModifiers
		{
			get
			{
				return this.GetStatModifiersOfTargetType(ModifierTarget.LowerBound);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0004237D File Offset: 0x0004057D
		[JsonIgnore]
		private IEnumerable<StatModifier> UpperModifiers
		{
			get
			{
				return this.GetStatModifiersOfTargetType(ModifierTarget.UpperBound);
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00042386 File Offset: 0x00040586
		private IEnumerable<StatModifier> GetStatModifiersOfTargetType(ModifierTarget targetType)
		{
			foreach (StatModifierBase statModifierBase in this.ActiveModifiers)
			{
				StatModifier statModifier = statModifierBase as StatModifier;
				if (statModifier != null && targetType == statModifier.TargetType)
				{
					yield return statModifier;
				}
			}
			IEnumerator<StatModifierBase> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0004239D File Offset: 0x0004059D
		private void MarkModifiersForRecalculation()
		{
			this._requiresActiveModifierRecalculation = true;
			this._requiresValueModifierRecalculation = true;
			this._requiresUpperModifierRecalculation = true;
			this._requiresLowerModifierRecalculation = true;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x000423BB File Offset: 0x000405BB
		public void ResetModifiers()
		{
			this.MarkModifiersForRecalculation();
			this.TransientModifiers.Clear();
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000423CE File Offset: 0x000405CE
		public void AdjustBase(float value)
		{
			this.SetBase((float)this.BaseValue + value);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x000423DF File Offset: 0x000405DF
		public void SetBase(float value)
		{
			this._baseValue = value;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x000423E8 File Offset: 0x000405E8
		private int RoundValue(float value)
		{
			RoundingMode roundingMode = this.RoundingMode;
			int result;
			if (roundingMode != RoundingMode.RoundDown)
			{
				if (roundingMode != RoundingMode.RoundUp)
				{
					result = (int)Math.Round((double)value, MidpointRounding.AwayFromZero);
				}
				else
				{
					result = (int)Math.Ceiling((double)value);
				}
			}
			else
			{
				result = (int)Math.Floor((double)value);
			}
			return result;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00042426 File Offset: 0x00040626
		public bool IsTransientModifier(StatModifierBase statModifier)
		{
			return this.TransientModifiers.Contains(statModifier);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00042434 File Offset: 0x00040634
		public static implicit operator int(ModifiableValue val)
		{
			return val.Value;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0004243C File Offset: 0x0004063C
		public static implicit operator ModifiableValue(int val)
		{
			return new ModifiableValue((float)val, 0, int.MaxValue, RoundingMode.RoundDown);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0004244C File Offset: 0x0004064C
		private float RecalculateModifiedValue()
		{
			List<StatModifierBase> list;
			ModifiableValue.<>c__DisplayClass71_0 CS$<>8__locals1;
			this.FetchValueModifiers(out list, out CS$<>8__locals1.statModifiers);
			foreach (StatModifierBase statModifierBase in list)
			{
				statModifierBase.PreModification(this);
			}
			if (list.Any((StatModifierBase t) => t is StatModifierPreventLoss))
			{
				CS$<>8__locals1.statModifiers.RemoveAll((StatModifier t) => t.IsNegativeContribution);
			}
			float num = ModifiableValue.<RecalculateModifiedValue>g__Compound|71_2(ModifierTarget.ValueOffset, this._baseValue, ref CS$<>8__locals1);
			float workingValue = num;
			foreach (StatModifier statModifier in CS$<>8__locals1.statModifiers)
			{
				if (statModifier.TargetType == ModifierTarget.ValueMultiplierOffset)
				{
					num = statModifier.ApplyModification(workingValue);
				}
			}
			num = ModifiableValue.<RecalculateModifiedValue>g__Compound|71_2(ModifierTarget.ValueMultiplierCompound, num, ref CS$<>8__locals1);
			num = ModifiableValue.<RecalculateModifiedValue>g__Compound|71_2(ModifierTarget.ValueScalar, num, ref CS$<>8__locals1);
			num = ModifiableValue.<RecalculateModifiedValue>g__Compound|71_2(ModifierTarget.ValuePercentagePointScalar, num, ref CS$<>8__locals1);
			foreach (StatModifierBase statModifierBase2 in list)
			{
				statModifierBase2.PostModification(this, num);
			}
			return num;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000425B8 File Offset: 0x000407B8
		private int RecalculateLowerBound()
		{
			if (this._requiresLowerModifierRecalculation)
			{
				this._requiresLowerModifierRecalculation = false;
				int val = this.LowerModifiers.SelectMaxValueOrDefault((StatModifier x) => x.Value, this._lowerBound);
				this._calculatedLowerModifier = Math.Max(val, this._lowerBound);
			}
			return this._calculatedLowerModifier;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00042620 File Offset: 0x00040820
		private int RecalculateUpperBound()
		{
			if (this._requiresUpperModifierRecalculation)
			{
				this._requiresUpperModifierRecalculation = false;
				int val = this.UpperModifiers.SelectMinValueOrDefault((StatModifier x) => x.Value, this._upperBound);
				this._calculatedUpperModifier = Math.Min(val, this._upperBound);
			}
			return this._calculatedUpperModifier;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00042685 File Offset: 0x00040885
		public void AddModifier(StatModifierBase modifier)
		{
			this.MarkModifiersForRecalculation();
			this.TransientModifiers.Add(modifier);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00042699 File Offset: 0x00040899
		public void RemoveModifier(StatModifierBase modifier)
		{
			this.MarkModifiersForRecalculation();
			this.TransientModifiers.Remove(modifier);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000426AE File Offset: 0x000408AE
		public void ClearModifiers()
		{
			this.MarkModifiersForRecalculation();
			this.TransientModifiers.Clear();
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000426C4 File Offset: 0x000408C4
		public override string ToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x000426DF File Offset: 0x000408DF
		public void AddInstalledModifier(StatModifierBase modifier)
		{
			this.MarkModifiersForRecalculation();
			this.InstalledModifiers.Add(modifier);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x000426F3 File Offset: 0x000408F3
		public void RemoveInstalledModifiers(Predicate<StatModifierBase> predicate)
		{
			this.MarkModifiersForRecalculation();
			this.InstalledModifiers.RemoveAll(predicate);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00042708 File Offset: 0x00040908
		public void DeepClone(out ModifiableValue clone)
		{
			clone = new ModifiableValue(this._baseValue, this._lowerBound, this._upperBound, this.RoundingMode)
			{
				_initialValue = this._initialValue,
				TransientModifiers = this.TransientModifiers.DeepClone<StatModifierBase>(),
				InstalledModifiers = this.InstalledModifiers.DeepClone<StatModifierBase>(),
				_requiresUpperModifierRecalculation = this._requiresUpperModifierRecalculation,
				_requiresLowerModifierRecalculation = this._requiresLowerModifierRecalculation,
				_calculatedUpperModifier = this._calculatedUpperModifier,
				_calculatedLowerModifier = this._calculatedLowerModifier
			};
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00042794 File Offset: 0x00040994
		public object Clone()
		{
			return new ModifiableValue(this._baseValue, this._lowerBound, this._upperBound, this.RoundingMode)
			{
				_initialValue = this._initialValue,
				TransientModifiers = this.TransientModifiers,
				InstalledModifiers = this.InstalledModifiers,
				_requiresUpperModifierRecalculation = this._requiresUpperModifierRecalculation,
				_requiresLowerModifierRecalculation = this._requiresLowerModifierRecalculation,
				_calculatedUpperModifier = this._calculatedUpperModifier,
				_calculatedLowerModifier = this._calculatedLowerModifier
			};
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00042814 File Offset: 0x00040A14
		[CompilerGenerated]
		internal static float <RecalculateModifiedValue>g__Compound|71_2(ModifierTarget target, float inputValue, ref ModifiableValue.<>c__DisplayClass71_0 A_2)
		{
			foreach (StatModifier statModifier in A_2.statModifiers)
			{
				if (statModifier.TargetType == target)
				{
					inputValue = statModifier.ApplyModification(inputValue);
				}
			}
			return inputValue;
		}

		// Token: 0x040007D3 RID: 2003
		private const int DefaultLowerBound = 0;

		// Token: 0x040007D4 RID: 2004
		private const int DefaultUpperBound = 2147483647;

		// Token: 0x040007D5 RID: 2005
		private const RoundingMode DefaultRoundingMode = RoundingMode.RoundDown;

		// Token: 0x040007D6 RID: 2006
		private const float Precision = 1E-05f;

		// Token: 0x040007DA RID: 2010
		[JsonProperty]
		private float _initialValue;

		// Token: 0x040007DB RID: 2011
		[JsonProperty]
		private float _baseValue;

		// Token: 0x040007DC RID: 2012
		[JsonProperty]
		private int _lowerBound;

		// Token: 0x040007DD RID: 2013
		[JsonProperty]
		[DefaultValue(2147483647)]
		private int _upperBound = int.MaxValue;

		// Token: 0x040007DE RID: 2014
		[JsonIgnore]
		[DefaultValue(true)]
		private bool _requiresActiveModifierRecalculation = true;

		// Token: 0x040007DF RID: 2015
		[JsonIgnore]
		[DefaultValue(true)]
		private bool _requiresValueModifierRecalculation = true;

		// Token: 0x040007E0 RID: 2016
		[JsonIgnore]
		[DefaultValue(true)]
		private bool _requiresUpperModifierRecalculation = true;

		// Token: 0x040007E1 RID: 2017
		[JsonIgnore]
		[DefaultValue(true)]
		private bool _requiresLowerModifierRecalculation = true;

		// Token: 0x040007E2 RID: 2018
		[JsonIgnore]
		private List<StatModifierBase> _activeModifiers = new List<StatModifierBase>();

		// Token: 0x040007E3 RID: 2019
		[JsonIgnore]
		private List<StatModifierBase> _valueModifiers = new List<StatModifierBase>();

		// Token: 0x040007E4 RID: 2020
		[JsonIgnore]
		private List<StatModifier> _valueStatModifiers = new List<StatModifier>();

		// Token: 0x040007E5 RID: 2021
		[JsonIgnore]
		private int _calculatedUpperModifier;

		// Token: 0x040007E6 RID: 2022
		[JsonIgnore]
		private int _calculatedLowerModifier;
	}
}
