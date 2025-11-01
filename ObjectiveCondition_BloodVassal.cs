using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000550 RID: 1360
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_BloodVassal : ObjectiveCondition
	{
		// Token: 0x06001A51 RID: 6737 RVA: 0x0005BCB4 File Offset: 0x00059EB4
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (isInitialProgress)
			{
				return 0;
			}
			List<PlayerState> potentialCandidates = base.GetPotentialCandidates(context, owner, this.BloodLordRole, this.BloodLordArchfiend, this.BloodLordNegate);
			List<PlayerState> potentialCandidates2 = base.GetPotentialCandidates(context, owner, this.VassalRole, this.VassalArchfiend, this.VassalNegate);
			int num = 0;
			foreach (PlayerState playerState in potentialCandidates)
			{
				foreach (PlayerState playerState2 in potentialCandidates2)
				{
					BloodVassalageState bloodVassalageState = context.CurrentTurn.GetDiplomaticStatus(playerState.Id, playerState2.Id).DiplomaticState as BloodVassalageState;
					if (bloodVassalageState != null && bloodVassalageState.BloodLordId == playerState.Id)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x04000BE7 RID: 3047
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Player)]
		public ObjectivePlayerRole BloodLordRole;

		// Token: 0x04000BE8 RID: 3048
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> BloodLordArchfiend;

		// Token: 0x04000BE9 RID: 3049
		[JsonProperty]
		public bool BloodLordNegate;

		// Token: 0x04000BEA RID: 3050
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Anyone)]
		public ObjectivePlayerRole VassalRole = ObjectivePlayerRole.Anyone;

		// Token: 0x04000BEB RID: 3051
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> VassalArchfiend;

		// Token: 0x04000BEC RID: 3052
		[JsonProperty]
		public bool VassalNegate;
	}
}
