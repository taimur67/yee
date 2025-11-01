using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000415 RID: 1045
	[Serializable]
	public class GOAPPlaystyle : IdentifiableStaticData
	{
		// Token: 0x060014A9 RID: 5289 RVA: 0x0004F190 File Offset: 0x0004D390
		public bool TryGetActionPlaystyleValue(ActionID action, out GOAPPlaystyleValue playstyleValue)
		{
			foreach (GOAPPlaystyleValue goapplaystyleValue in this.Modifiers)
			{
				if (goapplaystyleValue.ActionID == action)
				{
					playstyleValue = goapplaystyleValue;
					return true;
				}
			}
			playstyleValue = null;
			return false;
		}

		// Token: 0x040009BE RID: 2494
		[JsonProperty]
		public List<GOAPPlaystyleValue> Modifiers = new List<GOAPPlaystyleValue>();
	}
}
