using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000210 RID: 528
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class CandidateVoteResult : IDeepClone<CandidateVoteResult>
	{
		// Token: 0x06000A4E RID: 2638 RVA: 0x0002DFFC File Offset: 0x0002C1FC
		public Dictionary<int, int> GetTotals()
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (KeyValuePair<int, Dictionary<int, int>> keyValuePair in this.VotesToPlayer)
			{
				int num;
				Dictionary<int, int> dictionary2;
				keyValuePair.Deconstruct(out num, out dictionary2);
				int key = num;
				Dictionary<int, int> source = dictionary2;
				dictionary.AddOrSetValue(key, source.Sum((KeyValuePair<int, int> x) => x.Value));
			}
			return dictionary;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0002E090 File Offset: 0x0002C290
		public int GetTotal(int playerId)
		{
			Dictionary<int, int> dictionary;
			if (!this.VotesToPlayer.TryGetValue(playerId, out dictionary))
			{
				return 0;
			}
			return dictionary.Values.Sum();
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0002E0BA File Offset: 0x0002C2BA
		public void DeepClone(out CandidateVoteResult clone)
		{
			clone = new CandidateVoteResult
			{
				VotesToPlayer = this.VotesToPlayer.DeepClone()
			};
		}

		// Token: 0x040004CF RID: 1231
		[JsonProperty]
		public Dictionary<int, Dictionary<int, int>> VotesToPlayer = new Dictionary<int, Dictionary<int, int>>();
	}
}
