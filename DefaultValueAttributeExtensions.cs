using System;
using System.ComponentModel;

namespace LoG
{
	// Token: 0x02000377 RID: 887
	public static class DefaultValueAttributeExtensions
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x00041E80 File Offset: 0x00040080
		public static float FloatValue(this DefaultValueAttribute attribute, float @default = 0f)
		{
			object value = attribute.Value;
			float result;
			if (value is float)
			{
				float num = (float)value;
				result = num;
			}
			else if (value is int)
			{
				int num2 = (int)value;
				result = (float)num2;
			}
			else if (value is double)
			{
				double num3 = (double)value;
				result = (float)num3;
			}
			else if (value is byte)
			{
				byte b = (byte)value;
				result = (float)b;
			}
			else if (value is short)
			{
				short num4 = (short)value;
				result = (float)num4;
			}
			else if (value is long)
			{
				long num5 = (long)value;
				result = (float)num5;
			}
			else
			{
				result = @default;
			}
			return result;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00041F34 File Offset: 0x00040134
		public static int IntValue(this DefaultValueAttribute attribute, int @default = 0)
		{
			object value = attribute.Value;
			int result;
			if (value is float)
			{
				float num = (float)value;
				result = (int)num;
			}
			else if (value is int)
			{
				int num2 = (int)value;
				result = num2;
			}
			else if (value is double)
			{
				double num3 = (double)value;
				result = (int)num3;
			}
			else if (value is byte)
			{
				byte b = (byte)value;
				result = (int)b;
			}
			else if (value is short)
			{
				short num4 = (short)value;
				result = (int)num4;
			}
			else if (value is long)
			{
				long num5 = (long)value;
				result = (int)num5;
			}
			else
			{
				result = @default;
			}
			return result;
		}
	}
}
