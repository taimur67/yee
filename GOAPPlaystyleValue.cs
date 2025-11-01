using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000414 RID: 1044
	[Serializable]
	public class GOAPPlaystyleValue
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0004F114 File Offset: 0x0004D314
		[JsonIgnore]
		public float ChanceScalar
		{
			get
			{
				ActionChance chance = this.Chance;
				if (chance <= ActionChance.Likely)
				{
					if (chance == ActionChance.Forbidden)
					{
						return 1f;
					}
					if (chance == ActionChance.VeryLikely)
					{
						return -0.9f;
					}
					if (chance == ActionChance.Likely)
					{
						return -0.5f;
					}
				}
				else
				{
					if (chance == ActionChance.Normal)
					{
						return 0f;
					}
					if (chance == ActionChance.Unlikely)
					{
						return 0.5f;
					}
					if (chance == ActionChance.VeryUnlikely)
					{
						return 0.9f;
					}
				}
				return 0f;
			}
		}

		// Token: 0x040009BC RID: 2492
		[JsonProperty]
		public ActionID ActionID;

		// Token: 0x040009BD RID: 2493
		[JsonProperty]
		public ActionChance Chance;
	}
}
