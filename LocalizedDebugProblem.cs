using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003E3 RID: 995
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LocalizedDebugProblem : Problem
	{
		// Token: 0x060013AC RID: 5036 RVA: 0x0004AFC9 File Offset: 0x000491C9
		[JsonConstructor]
		public LocalizedDebugProblem()
		{
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0004AFD1 File Offset: 0x000491D1
		public LocalizedDebugProblem(string locKey)
		{
			this._locKey = locKey;
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x0004AFE0 File Offset: 0x000491E0
		public override string LocKey
		{
			get
			{
				return this._locKey;
			}
		}

		// Token: 0x040008E5 RID: 2277
		[JsonProperty]
		public string _locKey;
	}
}
