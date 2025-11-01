using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003E2 RID: 994
	[Serializable]
	public class DebugProblem : Problem
	{
		// Token: 0x060013AA RID: 5034 RVA: 0x0004AFB2 File Offset: 0x000491B2
		public DebugProblem(string str)
		{
			this._output = str;
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x0004AFC1 File Offset: 0x000491C1
		public override string DebugString
		{
			get
			{
				return this._output;
			}
		}

		// Token: 0x040008E4 RID: 2276
		[JsonProperty]
		private string _output;
	}
}
