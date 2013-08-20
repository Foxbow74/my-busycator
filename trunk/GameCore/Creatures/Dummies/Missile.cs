using System.Collections.Generic;
using System.Linq;
using GameCore.Acts.Combat;
using GameCore.Acts.Movement;
using GameCore.Battle;
using GameCore.Essences;
using GameCore.Essences.Weapons;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Creatures.Dummies
{
	public class Missile : AbstractDummyCreature
	{
		private readonly LightSource m_light;
		private readonly List<Point> m_path;
		private int m_step = 1;

		public Missile(WorldLayer _layer, LiveMapCell _from, int _speed, Item _ammo, LiveMapCell _to)
			: base(_ammo.ENoun, _layer, _speed)
		{
			Ammo = _ammo;
			var d = (_to.PathMapCoords - _from.PathMapCoords)*10;
			if(d.Lenght>32)
			{
				d = d*31/(int)d.Lenght;
			}
			World.TheWorld.CreatureManager.AddCreature(this, _from.WorldCoords, _from.LiveCoords, _layer);
			m_path = _from.LiveCoords.GetLineToPoints(_from.LiveCoords + d).ToList();
			m_light = new LightSource(10, new FColor(2f, 1f, 0.8f, 0.4f));
		}

		public override Material Material { get { return Ammo.Material; } }

		public override ILightSource Light { get { return m_light; } }

		public Item Ammo { get; private set; }

		public override ETileset Tileset { get { return Ammo.Tileset; } }

		public override EThinkingResult Thinking()
		{
			var nextPoint = m_path[m_step] - m_path[m_step - 1];
			m_step++;

			var nextCell = this[nextPoint];
			if (nextCell.Creature != null)
			{
				AddActToPool(new AtackAct(), nextPoint);
				return EThinkingResult.NORMAL;
			}
			var passable = nextCell.GetIsCanShootThrough(this);
			var canMove = m_step < m_path.Count;
			if (passable < 1)
			{
				nextCell = World.TheWorld.LiveMap.GetCell(GeoInfo.LiveCoords);
				canMove = false;
			}
			if (!canMove)
			{
				nextCell.AddItem(Ammo);
				MessageManager.SendMessage(this, WorldMessage.Turn);
				return EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE;
			}
			AddActToPool(new MoveAct(), nextPoint);
			return EThinkingResult.NORMAL;
		}

		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			yield return Ammo as IWeapon;
		}

		public override CreatureBattleInfo CreateBattleInfo()
		{
			var info = Ammo.CreateItemInfo(this);
			return new CreatureBattleInfo(this, 0, 0, info.Dmg, 0, info.ToHit);
		}
	}
}