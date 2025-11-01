using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000560 RID: 1376
	[Serializable]
	public class ObjectiveCondition_CollectedManuscriptsWorthOfFragmentsThisTurn : BooleanStateObjectiveCondition
	{
		// Token: 0x06001A80 RID: 6784 RVA: 0x0005C5B8 File Offset: 0x0005A7B8
		protected override bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (isInitialProgress)
			{
				return false;
			}
			IEnumerable<Identifier> ids = (from t in context.CurrentTurn.GetGameEvents<ItemsAcquiredEvent>()
			where t.Owner == owner.Id
			select t).SelectMany((ItemsAcquiredEvent t) => t.AcquiredItems);
			List<Manuscript> list = IEnumerableExtensions.ToList<Manuscript>(context.CurrentTurn.EnumerateGameItems<Manuscript>(ids));
			Dictionary<ManuscriptStaticData, List<Manuscript>> dictionary = new Dictionary<ManuscriptStaticData, List<Manuscript>>();
			foreach (Manuscript manuscript in list)
			{
				ManuscriptStaticData manuscriptStaticData;
				if (context.Database.TryGet<ManuscriptStaticData>(manuscript.StaticDataId, out manuscriptStaticData) && manuscriptStaticData.FragmentCount >= this.MinNumberFragmentsNeeded)
				{
					List<Manuscript> list2;
					if (!dictionary.TryGetValue(manuscriptStaticData, out list2))
					{
						list2 = (dictionary[manuscriptStaticData] = new List<Manuscript>());
					}
					list2.Add(manuscript);
				}
			}
			foreach (KeyValuePair<ManuscriptStaticData, List<Manuscript>> keyValuePair in dictionary)
			{
				ManuscriptStaticData manuscriptStaticData2;
				List<Manuscript> list3;
				keyValuePair.Deconstruct(out manuscriptStaticData2, out list3);
				ManuscriptStaticData manuscriptStaticData3 = manuscriptStaticData2;
				List<Manuscript> list4 = list3;
				int manuscriptCurrentFragmentCount = context.GetManuscriptCurrentFragmentCount(owner.Id, manuscriptStaticData3.Id);
				if (manuscriptCurrentFragmentCount >= manuscriptStaticData3.FragmentCount)
				{
					int count = list4.Count;
					if (manuscriptCurrentFragmentCount - count < manuscriptStaticData3.FragmentCount)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000BFF RID: 3071
		[JsonProperty]
		public int MinNumberFragmentsNeeded;
	}
}
