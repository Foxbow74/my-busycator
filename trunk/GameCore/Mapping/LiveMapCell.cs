using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping.Layers;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;
using GameCore.Essences;
using GameCore.Essences.Things;

namespace GameCore.Mapping
{
	public class LiveMapCell
	{
		private readonly List<Item> m_items = new List<Item>();
		private readonly Point m_liveCoords;
		private MapBlock m_mapBlock;
		private uint m_seenMask;

		public LiveMapCell(LiveMapBlock _liveMapBlock, Point _liveCoords)
		{
			LiveMapBlock = _liveMapBlock;
			m_liveCoords = _liveCoords;
		}

		public FColor Visibility { get; set; }

		public FColor Lighted { get; set; }

		public bool IsSeenBefore { get; private set; }

		public float Rnd { get; private set; }

		public Point OnLiveMapCoords { get; private set; }

		public int BlockRandomSeed { get { return LiveMapBlock.MapBlock.RandomSeed; } }

		public IEnumerable<Item> Items { get { return m_items; } }

		public Thing Thing
		{
		    get { return m_thing; }
		    set
		    {
		        m_thing = value;
				ResetTempStates();
		    }
		}

	    public Creature Creature
		{
			get
			{
                var tuple = LiveMapBlock.MapBlock.Creatures.FirstOrDefault(_tuple => _tuple.Item2 == LiveCoords);
                return tuple==null?null:tuple.Item1;
			}
		}

		public ETerrains Terrain { get; private set; }

		public TerrainAttribute TerrainAttribute { get; private set; }

		public bool IsCanShootThrough { get { return TerrainAttribute.IsCanShootThrough; } }

		public IEnumerable<ITileInfoProvider> TileInfoProviders
		{
			get
			{
				foreach (var splatter in m_splatters)
				{
					yield return splatter;
				}
				if (Thing != null)
				{
					yield return Thing;
				}
				foreach (var item in Items)
				{
					yield return item;
				}
				var cr = Creature;
				if (cr != null)
				{
					yield return cr;
				}
			}
		}


		public IEnumerable<ITileInfoProvider> FoggedTileInfoProviders
		{
			get
			{
				if (EssenceHelper.Is<Stair>(Thing))
				{
					yield return Thing;
				}
			}
		}

		public Point LiveCoords { get { return m_liveCoords; } }

        private FColor? m_transparentColor;
	    private Thing m_thing;

	    public FColor TransparentColor
		{
			get
			{
                if (m_transparentColor == null)
                {
                    var attr = TerrainAttribute;
                    var opacity = attr.Opacity;
                    if (opacity < 1 && Thing != null)
                    {
                        opacity += Thing.Opacity;
                    }
                    if (opacity < 1 && Creature != null)
                    {
                        opacity += Creature.Opacity;
                    }
                    if (opacity < 1 && LiveCoords == World.TheWorld.Avatar.LiveCoords)
                    {
                        opacity += World.TheWorld.Avatar.Opacity;
                    }
                    if (opacity < 1)
                    {
                        opacity += Items.Sum(_item => _item.Opacity);
                    }
                    var trancparence = 1f - Math.Min(1f, opacity);

                    m_transparentColor = new FColor(trancparence, 1f, 1f, 1f);
                }
			    return m_transparentColor.Value;
			}
		}

		public LiveMapBlock LiveMapBlock { get; private set; }

		public Point WorldCoords { get; private set; }

		public Point MapBlockId { get { return m_mapBlock.BlockId; } }

		public Point InBlockCoords { get; private set; }

		public Room InRoom
		{
			get
			{
				var room = LiveMapBlock.MapBlock.Rooms.FirstOrDefault(_room => _room.RoomRectangle.Contains(InBlockCoords));
				return room;
			}
		}

		public Building InBuilding
		{
			get
			{
				var surface = World.TheWorld.Avatar.Layer as Surface;
				if (surface == null) return null;
				var blockId = MapBlockId;
				var building = surface.City.Buildings.FirstOrDefault(_building => _building.BlockId == blockId && _building.Room.RoomRectangle.Contains(InBlockCoords));
				return building;
			}
		}

		public Point PathMapCoords { get; private set; }

		public float FogColorMultiplier
		{
			get
			{
				if (EssenceHelper.Is<Stair>(Thing)) return 1f;
				return TerrainAttribute.IsPassable == 0 ? 1f : 0.8f;
			}
		}

		public float DungeonFogColorMultiplier
		{
			get
			{
				if (EssenceHelper.Is<Stair>(Thing)) return 1f;
				return TerrainAttribute.IsPassable == 0 ? 0.8f : 1f;
			}
		}

		public void SetMapCell(MapBlock _mapBlock, Point _inBlockCoords, Point _worldCoords, float _rnd, Point _onLiveMapCoords, LiveMap _liveMap)
		{
			ResetTempStates();

			Rnd = _rnd;
			m_items.Clear();
			Thing = null;

			WorldCoords = _worldCoords;
			InBlockCoords = _inBlockCoords;
			m_mapBlock = _mapBlock;
			m_seenMask = ((UInt32) 1) << InBlockCoords.X;

			Terrain = _mapBlock.Map[_inBlockCoords.X, _inBlockCoords.Y];
			TerrainAttribute = TerrainAttribute.GetAttribute(Terrain);

			IsSeenBefore = (_mapBlock.SeenCells[_inBlockCoords.Y] & m_seenMask) != 0;
			OnLiveMapCoords = _onLiveMapCoords;

			ClearTemp();
			UpdatePathFinderMapCoord();
		}

		public void UpdatePathFinderMapCoord() { PathMapCoords = (m_mapBlock.BlockId - World.TheWorld.AvatarBlockId + LiveMap.ActiveQpoint)*Constants.MAP_BLOCK_SIZE + InBlockCoords; }

		public void SetIsSeenBefore()
		{
			if (!IsSeenBefore)
			{
				IsSeenBefore = true;
				m_mapBlock.SeenCells[InBlockCoords.Y] |= m_seenMask;
			}
		}

		public void ClearTemp()
		{
			Visibility = FColor.Empty;
			Lighted = FColor.Black;
		}

		public override string ToString()
		{
			return m_liveCoords + " WC:" + (WorldCoords == null ? "<null>" : WorldCoords.ToString());
			;
		}

		internal void AddItemIntenal(Item _item)
		{
		    m_items.Add(_item);
			ResetTempStates();
		}

		public void AddItem(Item _item)
		{
			if (m_mapBlock.AddEssence(_item, InBlockCoords))
			{
				m_items.Add(_item);
			}
		}

		public void AddCreature(Creature _creature)
		{
			if (Creature == null)
			{
				_creature.LiveCoords = LiveCoords;
				//m_mapBlock.AddCreature(_creature, InBlockCoords);
			}
		}

		public Item ResolveFakeItem(Creature _creature, FakedItem _fakeItem)
		{
			RemoveItem(_fakeItem);
			var item = (Item) _fakeItem.ResolveFake(_creature);
			AddItem(item);
			return item;
		}

		public Essence ResolveFakeThing(Creature _creature)
		{
			var fakedThing = (FakedThing) Thing;
			m_mapBlock.RemoveEssence(fakedThing, InBlockCoords);
			Thing = (Thing) fakedThing.ResolveFake(_creature);
			m_mapBlock.AddEssence(Thing, InBlockCoords);
			return Thing;
		}

		public IEnumerable<EssenceDescriptor> GetAllAvailableItemDescriptors<TEssence>(Creature _creature) where TEssence : Essence
		{
			foreach (var item in Items.OfType<TEssence>())
			{
				yield return new EssenceDescriptor(item, LiveCoords, null);
			}
			if (typeof (TEssence).IsAssignableFrom(typeof (Item)))
			{
				var thing = Thing as Container;
				if (thing != null && !thing.IsLockedFor(this, _creature))
				{
					foreach (var item in thing.GetItems(_creature).Items.OfType<TEssence>())
					{
						yield return new EssenceDescriptor(item, LiveCoords, thing);
					}
				}
			}
		}

		private float? m_isPassable = null;
		private List<Splatter> m_splatters = new List<Splatter>();

		public float GetIsPassableBy(Creature _creature, bool _pathFinding = false)
		{
			if(m_isPassable.HasValue) return m_isPassable.Value;

			if (Creature != null)
			{
				m_isPassable = 0;
				return 0f;
			}
			
			if (Thing != null)
			{
				if (Thing.Is<ClosedDoor>() && Thing.IsLockedFor(this, _creature))
				{
					return _pathFinding?0.99f:0f;
				}
			}
			m_isPassable = TerrainAttribute.IsPassable;
			return m_isPassable.Value;
		}

		public float GetIsCanShootThrough(Missile _creature)
		{
			if (Creature != null) return 0f;
			if (Thing != null)
			{
				if (Thing.Is<ClosedDoor>() && Thing.IsLockedFor(this, _creature))
				{
					return 0f;
				}
			}
			return TerrainAttribute.IsCanShootThrough ? 1f : 0f;
		}

		public void RemoveItem(Item _item)
		{
			if (!m_items.Remove(_item))
			{
				throw new ApplicationException();
			}
			ResetTempStates();
			m_mapBlock.RemoveEssence(_item, InBlockCoords);
		}

		public void ResetTempStates()
		{
			m_transparentColor = null;
			m_isPassable = null;
		}

		public void AddSplatter(Splatter _splatter)
		{
			m_splatters.Add(_splatter);
		}
	}
}