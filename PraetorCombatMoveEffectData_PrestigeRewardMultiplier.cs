using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003CC RID: 972
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveEffectData_PrestigeRewardMultiplier : PraetorCombatMoveEffectData
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x000487B2 File Offset: 0x000469B2
		public override string DebugDescription
		{
			get
			{
				return string.Format("{0} ({1})", base.DebugDescription, this.Value);
			}
		}

		// Token: 0x040008D3 RID: 2259
		[JsonProperty]
		[DefaultValue(1f)]
		public float Value = 1f;
	}
}
