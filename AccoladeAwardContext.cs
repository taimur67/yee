using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020001C4 RID: 452
	public class AccoladeAwardContext
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00027E05 File Offset: 0x00026005
		public IReadOnlyDictionary<PlayerState, List<AccoladeStaticData>> Awards
		{
			get
			{
				return this._awards;
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00027E10 File Offset: 0x00026010
		public IReadOnlyList<AccoladeStaticData> GetAccoladesForPlayer(PlayerState player)
		{
			List<AccoladeStaticData> result;
			if (!this._awards.TryGetValue(player, out result))
			{
				result = new List<AccoladeStaticData>();
			}
			return result;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00027E34 File Offset: 0x00026034
		public void Award(PlayerState player, AccoladeStaticData accolade)
		{
			List<AccoladeStaticData> list;
			if (!this._awards.TryGetValue(player, out list))
			{
				list = (this._awards[player] = new List<AccoladeStaticData>());
			}
			list.Add(accolade);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00027E6C File Offset: 0x0002606C
		public bool HasAwardOfRank(PlayerState player, int rank)
		{
			return this.GetAccoladesForPlayer(player).Any((AccoladeStaticData t) => t.Rank == rank);
		}

		// Token: 0x04000417 RID: 1047
		private Dictionary<PlayerState, List<AccoladeStaticData>> _awards = new Dictionary<PlayerState, List<AccoladeStaticData>>();
	}
}
