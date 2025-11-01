using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200037F RID: 895
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class CombatMoveContext : ModifierContext
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x00042937 File Offset: 0x00040B37
		[JsonConstructor]
		public CombatMoveContext()
		{
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0004293F File Offset: 0x00040B3F
		public CombatMoveContext(string combatMoveId)
		{
			this.CombatMoveId = combatMoveId;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0004294E File Offset: 0x00040B4E
		public override void DeepClone(out ModifierContext modifierContext)
		{
			modifierContext = new CombatMoveContext
			{
				CombatMoveId = this.CombatMoveId.DeepClone()
			};
		}

		// Token: 0x040007EC RID: 2028
		[JsonProperty]
		public string CombatMoveId;
	}
}
