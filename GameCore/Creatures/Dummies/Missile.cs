using System.Collections.Generic;
using System.Linq;
using GameCore.Acts.Combat;
using GameCore.Acts.Movement;
using GameCore.Essences;
using GameCore.Essences.Weapons;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Creatures.Dummies
{
	public class Missile : AbstractDummyCreature
	{
		private readonly LightSource m_light;
		private readonly List<Point> m_path;
		private Point m_from;
		private int m_step = 1;

		private Point m_target;

		public Missile(WorldLayer _layer, Point _liveCoords, int _speed, Item _ammo, Point _target)
			: base(_layer, _speed)
		{
			m_target = _target;
			m_from = _liveCoords;
			m_light = new LightSource(4, new FColor(4f, 1f, 0.8f, 0.4f));
			Ammo = _ammo;
			m_path = _liveCoords.GetLineToPoints(_target).ToList();
			LiveCoords = _liveCoords;
		}

		public override Material Material { get { return Ammo.Material; } }

		public override ILightSource Light { get { return m_light; } }

		public Item Ammo { get; private set; }

		public override ETileset Tileset { get { return Ammo.Tileset; } }

		public override string Name { get { return Ammo.GetName(World.TheWorld.Avatar); } }

		public override void Resolve(Creature _creature) { }

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
				nextCell = World.TheWorld.LiveMap.GetCell(LiveCoords);
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
			var weapon = Ammo as IWeapon;
			yield return weapon;
		}
	}

	public class Splatter:ITileInfoProvider
	{
		private readonly FColor m_color;
		private readonly EDirections m_direction;
		private readonly int m_tileIndex;

		public const int COUNT=13;

		public Splatter(FColor _color, int _tileIndex)
		{
			m_color = _color;
			m_tileIndex = _tileIndex;
			m_direction = World.Rnd.GetRandomDirection();
		}

		#region ITileInfoProvider Members

		public ETileset Tileset
		{
			get { return ETileset.SPLATTERS; }
		}

		public FColor LerpColor
		{
			get { return m_color; }
		}

		public EDirections Direction
		{
			get { return m_direction; }
		}

		public bool IsCorpse
		{
			get { return false; }
		}

		public int TileIndex
		{
			get { return m_tileIndex; }
		}

		#endregion
	}
}