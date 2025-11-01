using System;

namespace LoG
{
	// Token: 0x02000592 RID: 1426
	[Serializable]
	public class ObjectiveCondition_InsultPlayer : ObjectiveCondition_EventFilter<InsultHurledEvent>
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x0005E593 File Offset: 0x0005C793
		public override string LocalizationKey
		{
			get
			{
				if (!this.UniquePlayers)
				{
					return "InsultPlayers";
				}
				return "InsultPlayers.Unique";
			}
		}
	}
}
