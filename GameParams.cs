using System;
using Core.StaticData;

namespace LoG
{
	// Token: 0x020002E7 RID: 743
	public class GameParams
	{
		// Token: 0x04000667 RID: 1639
		public int PlayerCount;

		// Token: 0x04000668 RID: 1640
		public ConfigRef<GameParam_Data> MapPreset;

		// Token: 0x04000669 RID: 1641
		public ConfigRef<BoardSize_GenerationData> BoardSizePreset;

		// Token: 0x0400066A RID: 1642
		public ConfigRef<PlacesOfPower_GenerationData> PlacesOfPowerPreset;

		// Token: 0x0400066B RID: 1643
		public int StartingPrestige;

		// Token: 0x0400066C RID: 1644
		public ConfigRef<GameDuration_Data> GameDurationPreset;

		// Token: 0x0400066D RID: 1645
		public ConfigRef<TurnTimer_Data> TurnTimerPreset;

		// Token: 0x0400066E RID: 1646
		public ConfigRef<RiverData> RiversPreset;
	}
}
