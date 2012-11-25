namespace GameCore.Battle
{
	public class ItemBattleInfo
	{
		public static ItemBattleInfo Empty = new ItemBattleInfo();

		public ItemBattleInfo(int _dv, int _pv, int _pvi, int _toHit, Dice _dmg)
		{
			DV = _dv;
			PV = _pv;
			PVI = _pvi;
			ToHit = _toHit;
			Dmg = _dmg;
		}

		private ItemBattleInfo()
		{
		}

		public int DV { get; set; }
		public int PV { get; set; }
		public int PVI { get; set; }

		public int ToHit { get; set; }
		public Dice Dmg { get; set; }

		public override string ToString()
		{
			return string.Format("[{0},{1}/{2}]{{{3},{4}}}", DV, PVI, PV, ToHit, Dmg);
		}
	}
}