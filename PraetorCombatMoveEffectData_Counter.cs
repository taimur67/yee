using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003D2 RID: 978
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveEffectData_Counter : PraetorCombatMoveEffectData
	{
		// Token: 0x06001311 RID: 4881 RVA: 0x0004880C File Offset: 0x00046A0C
		public override bool PreDamage(GameEvent ev, TurnProcessContext context, DuelProcessContext duel, DuelParticipantInstance source)
		{
			DuelParticipantInstance duelParticipantInstance;
			if (!duel.TryGetOther(source, out duelParticipantInstance))
			{
				return false;
			}
			duelParticipantInstance.MoveCountered = true;
			return true;
		}
	}
}
