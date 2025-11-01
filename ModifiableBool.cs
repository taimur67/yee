using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.Utils;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000375 RID: 885
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ModifiableBool : IModifiableField, IDontSerializeIfDefault, IDeepClone<ModifiableBool>
	{
		// Token: 0x060010CA RID: 4298 RVA: 0x00041BC1 File Offset: 0x0003FDC1
		public ModifiableBool()
		{
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00041BD4 File Offset: 0x0003FDD4
		public ModifiableBool(bool baseValue = false, LogicOperation logic = LogicOperation.And)
		{
			this._logic = logic;
			this._initialValue = baseValue;
			this._baseValue = baseValue;
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x00041BFC File Offset: 0x0003FDFC
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x00041C04 File Offset: 0x0003FE04
		[JsonProperty]
		public List<BooleanModifierBase> ActiveModifiers { get; set; } = new List<BooleanModifierBase>();

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x00041C0D File Offset: 0x0003FE0D
		[JsonIgnore]
		public bool InitialValue
		{
			get
			{
				return this._initialValue;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x00041C15 File Offset: 0x0003FE15
		[JsonIgnore]
		public bool BaseValue
		{
			get
			{
				return this._baseValue;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x00041C20 File Offset: 0x0003FE20
		[JsonIgnore]
		public bool Value
		{
			get
			{
				foreach (BooleanModifierBase booleanModifierBase in this.ActiveModifiers)
				{
					BooleanOverrideModifier booleanOverrideModifier = booleanModifierBase as BooleanOverrideModifier;
					if (booleanOverrideModifier != null)
					{
						return booleanOverrideModifier.Value;
					}
				}
				if (this._logic == LogicOperation.And)
				{
					bool flag = this.BaseValue;
					foreach (BooleanModifierBase booleanModifierBase2 in this.ActiveModifiers)
					{
						BooleanModifier booleanModifier = booleanModifierBase2 as BooleanModifier;
						if (booleanModifier != null)
						{
							flag &= booleanModifier.Value;
						}
					}
					return flag;
				}
				bool flag2 = this.BaseValue;
				foreach (BooleanModifierBase booleanModifierBase3 in this.ActiveModifiers)
				{
					BooleanModifier booleanModifier2 = booleanModifierBase3 as BooleanModifier;
					if (booleanModifier2 != null)
					{
						flag2 |= booleanModifier2.Value;
					}
				}
				return flag2;
			}
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00041D40 File Offset: 0x0003FF40
		public bool IsDefault(DefaultValueAttribute defaultValueAttribute)
		{
			int num = 0;
			if (defaultValueAttribute != null)
			{
				num = defaultValueAttribute.IntValue(0);
			}
			ValueTuple<bool, LogicOperation> valueTuple;
			switch (num)
			{
			case 0:
				valueTuple = new ValueTuple<bool, LogicOperation>(false, LogicOperation.And);
				break;
			case 1:
				valueTuple = new ValueTuple<bool, LogicOperation>(false, LogicOperation.Or);
				break;
			case 2:
				valueTuple = new ValueTuple<bool, LogicOperation>(true, LogicOperation.And);
				break;
			case 3:
				valueTuple = new ValueTuple<bool, LogicOperation>(true, LogicOperation.Or);
				break;
			default:
				valueTuple = new ValueTuple<bool, LogicOperation>(false, LogicOperation.And);
				break;
			}
			ValueTuple<bool, LogicOperation> valueTuple2 = valueTuple;
			bool item = valueTuple2.Item1;
			LogicOperation item2 = valueTuple2.Item2;
			return item == this._initialValue && this._logic == item2 && this._initialValue == this._baseValue && this.ActiveModifiers.Count == 0;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00041DE0 File Offset: 0x0003FFE0
		public void ResetModifiers()
		{
			this.ActiveModifiers.Clear();
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00041DED File Offset: 0x0003FFED
		public void SetBase(bool value)
		{
			this._baseValue = value;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00041DF6 File Offset: 0x0003FFF6
		public static implicit operator bool(ModifiableBool val)
		{
			return val.Value;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00041DFE File Offset: 0x0003FFFE
		public static implicit operator ModifiableBool(bool val)
		{
			return new ModifiableBool(val, LogicOperation.And);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00041E07 File Offset: 0x00040007
		public void AddModifier(BooleanModifierBase modifier)
		{
			this.ActiveModifiers.Add(modifier);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00041E15 File Offset: 0x00040015
		public void RemoveModifier(BooleanModifierBase modifier)
		{
			this.ActiveModifiers.Remove(modifier);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00041E24 File Offset: 0x00040024
		public void ClearModifiers()
		{
			this.ActiveModifiers.Clear();
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00041E31 File Offset: 0x00040031
		public void DeepClone(out ModifiableBool clone)
		{
			clone = new ModifiableBool(this._baseValue, this._logic)
			{
				_initialValue = this._initialValue,
				ActiveModifiers = this.ActiveModifiers.DeepClone<BooleanModifierBase>()
			};
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00041E64 File Offset: 0x00040064
		public override string ToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x040007C6 RID: 1990
		public const int DefaultFalseAnd = 0;

		// Token: 0x040007C7 RID: 1991
		public const int DefaultFalseOr = 1;

		// Token: 0x040007C8 RID: 1992
		public const int DefaultTrueAnd = 2;

		// Token: 0x040007C9 RID: 1993
		public const int DefaultTrueOr = 3;

		// Token: 0x040007CA RID: 1994
		private const LogicOperation DefaultLogicOperation = LogicOperation.And;

		// Token: 0x040007CC RID: 1996
		[JsonProperty]
		private bool _initialValue;

		// Token: 0x040007CD RID: 1997
		[JsonProperty]
		private bool _baseValue;

		// Token: 0x040007CE RID: 1998
		[JsonProperty]
		[DefaultValue(LogicOperation.And)]
		private readonly LogicOperation _logic;
	}
}
