using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003FE RID: 1022
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RitualMaskingSettings : IDeepClone<RitualMaskingSettings>
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x0004D9D8 File Offset: 0x0004BBD8
		public static RitualMaskingSettings NoMasking
		{
			get
			{
				return new RitualMaskingSettings(RitualMaskingMode.NoMasking);
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x0004D9E0 File Offset: 0x0004BBE0
		public static RitualMaskingSettings Masked
		{
			get
			{
				return new RitualMaskingSettings(RitualMaskingMode.Masked);
			}
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0004D9E8 File Offset: 0x0004BBE8
		public static RitualMaskingSettings Framing(int maskingTargetId)
		{
			return new RitualMaskingSettings(RitualMaskingMode.Framed)
			{
				CurrentMaskingTargetId = maskingTargetId
			};
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0004D9F7 File Offset: 0x0004BBF7
		public RitualMaskingSettings()
		{
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0004DA0A File Offset: 0x0004BC0A
		public RitualMaskingSettings(RitualMaskingMode mode)
		{
			this.MaskingMode = mode;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0004DA24 File Offset: 0x0004BC24
		public void DeepClone(out RitualMaskingSettings clone)
		{
			clone = new RitualMaskingSettings
			{
				MaskingMode = this.MaskingMode,
				CurrentMaskingTargetId = this.CurrentMaskingTargetId
			};
		}

		// Token: 0x04000908 RID: 2312
		[JsonProperty]
		[DefaultValue(RitualMaskingMode.NoMasking)]
		public RitualMaskingMode MaskingMode;

		// Token: 0x04000909 RID: 2313
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int CurrentMaskingTargetId = int.MinValue;
	}
}
