using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003B5 RID: 949
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuelData : IDeepClone<PraetorDuelData>
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x00046BF0 File Offset: 0x00044DF0
		[JsonIgnore]
		public PlayerPair Players
		{
			get
			{
				return new PlayerPair(this.Challenger.PlayerId, this.Defender.PlayerId);
			}
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00046C0D File Offset: 0x00044E0D
		[JsonConstructor]
		protected PraetorDuelData()
		{
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00046C15 File Offset: 0x00044E15
		public PraetorDuelData(int challengerId, int defenderId, Identifier challengerPraetor = Identifier.Invalid)
		{
			this.Challenger = new PraetorDuelParticipantData
			{
				PlayerId = challengerId,
				Praetor = challengerPraetor
			};
			this.Defender = new PraetorDuelParticipantData
			{
				PlayerId = defenderId
			};
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00046C48 File Offset: 0x00044E48
		public bool TryGetAssociated(int playerId, out PraetorDuelParticipantData data)
		{
			data = null;
			if (playerId == this.Challenger.PlayerId)
			{
				data = this.Challenger;
			}
			else if (playerId == this.Defender.PlayerId)
			{
				data = this.Defender;
			}
			return data != null;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00046C80 File Offset: 0x00044E80
		public bool TryGetOther(int playerId, out PraetorDuelParticipantData data)
		{
			data = null;
			if (playerId == this.Challenger.PlayerId)
			{
				data = this.Defender;
			}
			else if (playerId == this.Defender.PlayerId)
			{
				data = this.Challenger;
			}
			return data != null;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00046CB8 File Offset: 0x00044EB8
		public void DeepClone(out PraetorDuelData clone)
		{
			clone = new PraetorDuelData
			{
				BaseWager = this.BaseWager,
				PrestigeReward = this.PrestigeReward.DeepClone<ModifiableValue>(),
				Challenger = this.Challenger.DeepClone<PraetorDuelParticipantData>(),
				Defender = this.Defender.DeepClone<PraetorDuelParticipantData>()
			};
		}

		// Token: 0x040008A5 RID: 2213
		[JsonProperty]
		public int BaseWager;

		// Token: 0x040008A6 RID: 2214
		[JsonProperty]
		public ModifiableValue PrestigeReward;

		// Token: 0x040008A7 RID: 2215
		[JsonProperty]
		public PraetorDuelParticipantData Challenger;

		// Token: 0x040008A8 RID: 2216
		[JsonProperty]
		public PraetorDuelParticipantData Defender;
	}
}
