using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200040B RID: 1035
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class AbyssalStriderTurnModuleInstance : NeutralForceTurnModuleInstance
	{
		// Token: 0x0600148F RID: 5263 RVA: 0x0004E82C File Offset: 0x0004CA2C
		public override void DeepClone(out TurnModuleInstance clone)
		{
			AbyssalStriderTurnModuleInstance abyssalStriderTurnModuleInstance = new AbyssalStriderTurnModuleInstance
			{
				LastSpawnTurn = this.LastSpawnTurn
			};
			base.DeepCloneNeutralForceTurnModuleInstanceParts(abyssalStriderTurnModuleInstance);
			clone = abyssalStriderTurnModuleInstance;
		}

		// Token: 0x04000919 RID: 2329
		[JsonProperty]
		public int LastSpawnTurn;
	}
}
