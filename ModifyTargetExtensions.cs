using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000603 RID: 1539
	public static class ModifyTargetExtensions
	{
		// Token: 0x06001CB2 RID: 7346 RVA: 0x00062F78 File Offset: 0x00061178
		public static int[] GetPlayerIds(this EventTargets targetOption, TurnState turn, PlayerState player)
		{
			int[] result;
			switch (targetOption)
			{
			case EventTargets.Self:
				result = new int[]
				{
					player.Id
				};
				break;
			case EventTargets.Others:
				result = IEnumerableExtensions.ToArray<int>(from x in turn.EnumeratePlayerStates(false, false)
				select x.Id into id
				where id != player.Id
				select id);
				break;
			case EventTargets.All:
				result = IEnumerableExtensions.ToArray<int>(from x in turn.EnumeratePlayerStates(false, false)
				select x.Id);
				break;
			case EventTargets.AllIncludingForceMajeure:
				result = IEnumerableExtensions.ToArray<int>(from x in turn.EnumeratePlayerStates(true, false)
				select x.Id);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}
	}
}
