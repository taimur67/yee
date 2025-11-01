using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002CA RID: 714
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveInstance : IDeepClone<PraetorCombatMoveInstance>
	{
		// Token: 0x06000DCF RID: 3535 RVA: 0x00036957 File Offset: 0x00034B57
		public void DeepClone(out PraetorCombatMoveInstance clone)
		{
			clone = new PraetorCombatMoveInstance
			{
				Power = this.Power,
				CombatMoveReference = this.CombatMoveReference.DeepClone<PraetorCombatMoveStaticData>()
			};
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0003697D File Offset: 0x00034B7D
		public string DebugName()
		{
			return string.Format("{0}_{1}", StringExtensions.RemoveStart(this.CombatMoveReference.Id, "PraetorCombatMove_"), this.Power);
		}

		// Token: 0x0400062B RID: 1579
		[JsonProperty]
		public int Power;

		// Token: 0x0400062C RID: 1580
		[JsonProperty]
		public ConfigRef<PraetorCombatMoveStaticData> CombatMoveReference;
	}
}
