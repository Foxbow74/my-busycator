using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences;
using GameCore.Essences.Faked;
using GameCore.Essences.Things;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;

namespace GameCore.Mapping
{
	public class LiveMapCell
	{
		private readonly List<Item> m_items = new List<Item>();
		private readonly Point m_liveCoords;
		private readonly List<Splatter> m_splatters = new List<Splatter>();
		private float? m_isPassable;
		private MapBlock m_mapBlock;
		private uint m_seenMask;
		private Thing m_thing;
		private FColor? m_transparentColor;

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
				ResetCached();
		    }
		}

		public Thing GetResolvedThing(Creature _creature)
		{
			if (m_thing == null) return null;
			var ft = m_thing as FakedThing;
			if (ft == null)
			{
				return m_thing;
			}
			return (Thing)ResolveFakeThing(_creature);
		}

	    public Creature Creature
		{
			get
			{
				CreatureGeoInfo geoInfo;
				if(World.TheWorld.CreatureManager.CreatureByPoint.TryGetValue(WorldCoords, out geoInfo))
				{
					return geoInfo.Creature;
				}
				return null;
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

		public FColor TransparentColor
		{
			get
			{
                if (m_transparentColor == null)
                {
                    var trancparence = 1f - Math.Min(1f, CalcOpacity());
                    if (Terrain == ETerrains.RED_BRICK_WINDOW)
	                {
						m_transparentColor = new FColor(trancparence, 1f, 0.1f, 0.1f);
	                }
	                else
	                {
						m_transparentColor = new FColor(trancparence, 0f, 0f, 0f);

	                }
                }
			    return m_transparentColor.Value;
			}
		}

	    public float CalcOpacity()
	    {
	        var opacity = TerrainAttribute.Opacity;
	        if (opacity < 1 && Thing != null)
	        {
	            opacity += Thing.Opacity;
	        }
	        if (opacity < 1 && Creature != null)
	        {
	            opacity += Creature.Opacity;
	        }
	        //if (opacity < 1 && LiveCoords == World.TheWorld.Avatar.LiveCoords)
	        //{
	        //    opacity += World.TheWorld.Avatar.Opacity;
	        //}
	        if (opacity < 1)
	        {
	            opacity += Items.Sum(_item => _item.Opacity);
	        }
	        return opacity;
	    }

	    public LiveMapBlock LiveMapBlock { get; private set; }

		public Point WorldCoords { get { return LiveMapBlock.WorldCoords + InBlockCoords; } }

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
			    return null;
                //var surface = World.TheWorld.Avatar.GeoInfo.Layer as Surface;
                //if (surface == null || surface.City==null) return null;
                //var blockId = MapBlockId;
                //var building = surface.City.Buildings.FirstOrDefault(_building => _building.BlockId == blockId && _building.Room.RoomRectangle.Contains(InBlockCoords));
                //return building;
			}
		}

		public Point PathMapCoords { get; set; }

		public float FogColorMultiplier
		{
			get
			{
				if (EssenceHelper.Is<Stair>(Thing)) return 1f;
				return TerrainAttribute.IsNotPassable ? 1f : 0.8f;
			}
		}

		public float DungeonFogColorMultiplier
		{
			get
			{
				if (EssenceHelper.Is<Stair>(Thing)) return 1f;
				return TerrainAttribute.IsNotPassable ? 0.8f : 1f;
			}
		}

		public bool IsVisibleNow { get; private set; }

		public FColor FinalLighted { get; private set; }

		public void SetMapCell(MapBlock _mapBlock, Point _inBlockCoords, float _rnd, Point _onLiveMapCoords, LiveMap _liveMap)
		{
			ResetCached();

			Rnd = _rnd;
			m_items.Clear();
			Thing = null;

			InBlockCoords = _inBlockCoords;
			m_mapBlock = _mapBlock;
			m_seenMask = ((UInt32) 1) << InBlockCoords.X;

			Terrain = _mapBlock.Map[_inBlockCoords.X, _inBlockCoords.Y];
			TerrainAttribute = TerrainAttribute.GetAttribute(Terrain);

			IsSeenBefore = (_mapBlock.SeenCells[_inBlockCoords.Y] & m_seenMask) != 0;
			OnLiveMapCoords = _onLiveMapCoords;

			ClearTemp();
		}

		public void ClearTemp()
		{
			Visibility = FColor.Empty;
			Lighted = FColor.Black;
			FinalLighted = FColor.Black;
			IsVisibleNow = false;
		}

		public override string ToString()
		{
			return m_liveCoords + " WC:" + (WorldCoords == null ? "<null>" : WorldCoords.ToString());
			;
		}

		internal void AddItemIntenal(Item _item)
		{
		    m_items.Add(_item);
			ResetCached();
		}

		public void AddItem(Item _item)
		{
			if (m_mapBlock.AddEssence(_item, InBlockCoords))
			{
				m_items.Add(_item);
			}
		}

		public Item ResolveFakeItem(Creature _creature, FakedItem _fakeItem)
		{
			Debug.WriteLine("RESOLVE " + _fakeItem);
			RemoveItem(_fakeItem);
			var item = (Item)_fakeItem.Essence.Clone(_creature);
			AddItem(item);
			return item;
		}

		public Thing ResolveFakeThing(Creature _creature)
		{
			var fakedThing = (FakedThing) Thing;
			m_mapBlock.RemoveEssence(fakedThing, InBlockCoords);
			Thing = (Thing) fakedThing.Essence.Clone(_creature);
			m_mapBlock.AddEssence(Thing, InBlockCoords);
			return Thing;
		}

		public IEnumerable<EssenceDescriptor> GetAllAvailableItemDescriptors<TEssence>(Creature _creature) where TEssence : Essence
		{
			var items = Items.OfType<TEssence>().ToArray();
			foreach (var item in items)
			{
				if (item is FakedItem)
				{
					yield return new EssenceDescriptor(ResolveFakeItem(_creature, item as FakedItem), this, null, _creature);
				}
				else
				{
					yield return new EssenceDescriptor(item, this, null, _creature);
				}
			}
			if (typeof (TEssence).IsAssignableFrom(typeof (Item)))
			{
				var thing = Thing as Container;
				if (thing != null && !thing.IsLockedFor(this, _creature))
				{
					foreach (var item in thing.GetItems(_creature).Items.OfType<TEssence>())
					{
						yield return new EssenceDescriptor(item, this, thing, _creature);
					}
				}
			}
		}

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
			m_isPassable = TerrainAttribute.Passability;
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
			ResetCached();
			m_mapBlock.RemoveEssence(_item, InBlockCoords);
		}

		public void ResetCached()
		{
			m_transparentColor = null;
			m_isPassable = null;
		}

		public int AddSplatter(int _max, FColor _color)
		{
			if(Terrain!=ETerrains.NONE)
			{
				var result = World.Rnd.Next(_max+1)%Splatter.COUNT;
				m_splatters.Add(new Splatter(_color, result));
				return result;
			}
			return 0;
		}

		public void UpdateVisibility(float _fogLightness, FColor _ambient)
		{
			FinalLighted = Lighted.Screen(_ambient).Multiply(Visibility);
			if (FinalLighted.Lightness() > _fogLightness)
			{
				IsVisibleNow = true;
				if (!IsSeenBefore)
				{
					IsSeenBefore = true;
					m_mapBlock.SeenCells[InBlockCoords.Y] |= m_seenMask;
				}
			}
		}

		public void UpdateAvatarCellVisibility()
		{
			//FinalLighted = FinalLighted.Screen(FColor.White.Multiply(0.5f));
			if (!IsVisibleNow)
			{
				
				IsVisibleNow = true;
				if (!IsSeenBefore)
				{
					IsSeenBefore = true;
					m_mapBlock.SeenCells[InBlockCoords.Y] |= m_seenMask;
				}
			}
		}
	}
}