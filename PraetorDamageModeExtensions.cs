using System;

namespace LoG
{
	// Token: 0x020003C8 RID: 968
	public static class PraetorDamageModeExtensions
	{
		// Token: 0x060012F7 RID: 4855 RVA: 0x0004849C File Offset: 0x0004669C
		public static float CalculateEffect(this PraetorDamageModifierMode modifierMode, float value, float effect)
		{
			float result;
			switch (modifierMode)
			{
			case PraetorDamageModifierMode.Offset:
				result = value + effect;
				break;
			case PraetorDamageModifierMode.MultiplyOffset:
				result = value + value * effect;
				break;
			case PraetorDamageModifierMode.Multiply:
				result = value * effect;
				break;
			default:
				result = value;
				break;
			}
			return result;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000484D4 File Offset: 0x000466D4
		public static string GetSymbol(this PraetorDamageModifierMode modifierMode)
		{
			string result;
			switch (modifierMode)
			{
			case PraetorDamageModifierMode.Offset:
				result = "+";
				break;
			case PraetorDamageModifierMode.MultiplyOffset:
				result = "*+";
				break;
			case PraetorDamageModifierMode.Multiply:
				result = "*";
				break;
			default:
				result = "?";
				break;
			}
			return result;
		}
	}
}
