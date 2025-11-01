using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200036F RID: 879
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class IModifier
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x000419D7 File Offset: 0x0003FBD7
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x000419DF File Offset: 0x0003FBDF
		[JsonProperty]
		public ModifierContext Source { get; set; }

		// Token: 0x060010B2 RID: 4274
		public abstract void ApplyTo(TurnContext context, IModifiable modifiable);

		// Token: 0x060010B3 RID: 4275
		public abstract void PostApplyTo(TurnContext context, IModifiable modifiable);

		// Token: 0x060010B4 RID: 4276 RVA: 0x000419E8 File Offset: 0x0003FBE8
		protected bool ApplyStatModifier(ModifiableValue modifiable, int value, ModifierTarget targetType)
		{
			if (targetType == ModifierTarget.ValueOffset && value == 0)
			{
				return false;
			}
			if (targetType == ModifierTarget.ValueScalar && value == 1)
			{
				return false;
			}
			if (targetType == ModifierTarget.ValuePercentagePointScalar && value == 100)
			{
				return false;
			}
			modifiable.AddModifier(new StatModifier(value, this.Source, targetType));
			return true;
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00041A1B File Offset: 0x0003FC1B
		protected void ApplyLossPreventionModifier(ModifiableValue modifiable)
		{
			modifiable.AddModifier(new StatModifierPreventLoss
			{
				Provider = this.Source
			});
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00041A34 File Offset: 0x0003FC34
		protected bool InstallStatModifier(ModifiableValue modifiable, int value, ModifierTarget targetType, bool baseAdjust = false)
		{
			if (value == 0)
			{
				return false;
			}
			if (baseAdjust)
			{
				modifiable.AdjustBase((float)value);
			}
			else
			{
				modifiable.AddInstalledModifier(new StatModifier(value, this.Source, targetType));
			}
			return true;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00041A5D File Offset: 0x0003FC5D
		protected bool ApplyBooleanModifier(ModifiableBool modifiable, bool value)
		{
			modifiable.AddModifier(new BooleanModifier(value, this.Source));
			return true;
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00041A72 File Offset: 0x0003FC72
		protected bool ApplyBooleanOverrideModifier(ModifiableBool modifiable, bool @override, ModifierContext provider = null)
		{
			modifiable.AddModifier(new BooleanOverrideModifier(@override, provider ?? this.Source));
			return true;
		}
	}
}
