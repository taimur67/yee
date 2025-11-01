using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003CB RID: 971
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveEffectData_RandomDamageModifier : PraetorCombatMoveEffectData
	{
		// Token: 0x06001305 RID: 4869 RVA: 0x00048668 File Offset: 0x00046868
		[return: TupleElementNames(new string[]
		{
			"min",
			"max"
		})]
		private ValueTuple<float, float> CalculateMinMax()
		{
			if (this.Options.Count == 0)
			{
				return new ValueTuple<float, float>(0f, 0f);
			}
			float num = float.MaxValue;
			float num2 = float.MinValue;
			foreach (WeightedValue<float> weightedValue in this.Options)
			{
				num = Math.Min(num, weightedValue.Value);
				num2 = Math.Max(num2, weightedValue.Value);
			}
			return new ValueTuple<float, float>(num, num2);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x00048700 File Offset: 0x00046900
		public float CalculateExpectedMultiplier()
		{
			ValueTuple<float, float> valueTuple = this.CalculateMinMax();
			float item = valueTuple.Item1;
			float item2 = valueTuple.Item2;
			return (item + item2) / 2f;
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x00048729 File Offset: 0x00046929
		[JsonIgnore]
		public override string DebugDescription
		{
			get
			{
				return string.Format("{0} {1}{2}", base.DebugDescription, this.Mode.GetSymbol(), this.CalculateMinMax());
			}
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00048754 File Offset: 0x00046954
		public override bool ModifyPower(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source, ref int workingValue)
		{
			WeightedValue<float> weightedValue;
			if (!this.Options.SelectRandom(context.Random, out weightedValue))
			{
				return false;
			}
			workingValue = (int)Math.Round((double)this.Mode.CalculateEffect((float)workingValue, weightedValue.Value));
			return true;
		}

		// Token: 0x040008D1 RID: 2257
		[JsonProperty]
		public List<WeightedValue<float>> Options = new List<WeightedValue<float>>();

		// Token: 0x040008D2 RID: 2258
		[JsonProperty]
		[DefaultValue(PraetorDamageModifierMode.Multiply)]
		public PraetorDamageModifierMode Mode = PraetorDamageModifierMode.Multiply;
	}
}
