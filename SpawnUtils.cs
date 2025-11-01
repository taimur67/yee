using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000290 RID: 656
	public static class SpawnUtils
	{
		// Token: 0x06000CC1 RID: 3265 RVA: 0x00033BC8 File Offset: 0x00031DC8
		public static T SpawnGameItem<T>(GameItemStaticData data) where T : GameItem, new()
		{
			T t = Activator.CreateInstance<T>();
			t.ConfigureFrom(data);
			return t;
		}
	}
}
