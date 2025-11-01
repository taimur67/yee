using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003CA RID: 970
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveEffectData_DamageModifier : PraetorCombatMoveEffectData
	{
		// Token: 0x06001302 RID: 4866 RVA: 0x00048601 File Offset: 0x00046801
		public override bool ModifyPower(TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source, ref int workingValue)
		{
			workingValue = (int)Math.Round((double)this.Mode.CalculateEffect((float)workingValue, this.Value));
			return true;
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x00048623 File Offset: 0x00046823
		[JsonIgnore]
		public override string DebugDescription
		{
			get
			{
				return string.Format("{0} {1}({2})", base.DebugDescription, this.Mode, this.Value);
			}
		}

		// Token: 0x040008CF RID: 2255
		[JsonProperty]
		[DefaultValue(PraetorDamageModifierMode.Multiply)]
		public PraetorDamageModifierMode Mode = PraetorDamageModifierMode.Multiply;

		// Token: 0x040008D0 RID: 2256
		[JsonProperty]
		[DefaultValue(1f)]
		public float Value = 1f;
	}
}
