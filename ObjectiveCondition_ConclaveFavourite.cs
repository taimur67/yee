using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000563 RID: 1379
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_ConclaveFavourite : ObjectiveCondition
	{
		// Token: 0x06001A84 RID: 6788 RVA: 0x0005C794 File Offset: 0x0005A994
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			bool flag = false;
			using (List<PlayerState>.Enumerator enumerator = base.GetPotentialCandidates(context, owner, this.ConclaveFavouriteRole, this.ConclaveFavouriteArchfiend, false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == context.CurrentTurn.ConclaveFavouriteId)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x04000C05 RID: 3077
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Player)]
		public ObjectivePlayerRole ConclaveFavouriteRole;

		// Token: 0x04000C06 RID: 3078
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> ConclaveFavouriteArchfiend;
	}
}
