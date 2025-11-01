using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006D7 RID: 1751
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UnholyCrusadeTurnModuleInstance : TurnModuleInstance
	{
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x0006E191 File Offset: 0x0006C391
		public IEnumerable<Identifier> UnProcessedLegions
		{
			get
			{
				return this.SubmittedLegions.Values.Except(this.VictoriousLegions).Except(this.DefeatedLegions);
			}
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x0006E1B4 File Offset: 0x0006C3B4
		public void SubmitLegion(int forPlayer, Identifier identifier)
		{
			this.SubmittedLegions.Add(forPlayer, identifier);
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x0006E1C3 File Offset: 0x0006C3C3
		public void SetLegionDefeated(Identifier identifier)
		{
			this.DefeatedLegions.Add(identifier);
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x0006E1D1 File Offset: 0x0006C3D1
		public void SetLegionVictorious(Identifier identifier)
		{
			this.VictoriousLegions.Add(identifier);
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0006E1DF File Offset: 0x0006C3DF
		public void SetNoSubmission(int playerId)
		{
			this.NoSubmissionPlayers.Add(playerId);
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0006E1F0 File Offset: 0x0006C3F0
		public override void DeepClone(out TurnModuleInstance clone)
		{
			UnholyCrusadeTurnModuleInstance unholyCrusadeTurnModuleInstance = new UnholyCrusadeTurnModuleInstance
			{
				NoSubmissionPlayers = this.NoSubmissionPlayers.DeepClone(),
				SubmittedLegions = this.SubmittedLegions.DeepClone(CloneFunction.FastClone),
				VictoriousLegions = this.VictoriousLegions.DeepClone(),
				DefeatedLegions = this.DefeatedLegions.DeepClone(),
				Duration = this.Duration,
				StartTurn = this.StartTurn
			};
			base.DeepCloneTurnModuleInstanceParts(unholyCrusadeTurnModuleInstance);
			clone = unholyCrusadeTurnModuleInstance;
		}

		// Token: 0x04000E3A RID: 3642
		[JsonProperty]
		public List<int> NoSubmissionPlayers = new List<int>();

		// Token: 0x04000E3B RID: 3643
		[JsonProperty]
		public Dictionary<int, Identifier> SubmittedLegions = new Dictionary<int, Identifier>();

		// Token: 0x04000E3C RID: 3644
		[JsonProperty]
		public List<Identifier> VictoriousLegions = new List<Identifier>();

		// Token: 0x04000E3D RID: 3645
		[JsonProperty]
		public List<Identifier> DefeatedLegions = new List<Identifier>();

		// Token: 0x04000E3E RID: 3646
		[JsonProperty]
		public int Duration;

		// Token: 0x04000E3F RID: 3647
		[JsonProperty]
		public int StartTurn;
	}
}
