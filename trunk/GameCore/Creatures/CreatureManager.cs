using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures.Dummies;
using GameCore.Essences.Faked;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Creatures
{
	public class CreatureManager
	{
		public CreatureManager()
		{
			PointByCreature = new Dictionary<CreatureGeoInfo, Point>();
			CreatureByPoint = new Dictionary<Point, CreatureGeoInfo>();
			DummyCreatureByPoint = new Dictionary<Point, List<CreatureGeoInfo>>();
			InfoByCreature = new Dictionary<Creature, CreatureGeoInfo>();
			OutOfScope = new Dictionary<Creature, CreatureGeoInfo>();
		}

		public Dictionary<CreatureGeoInfo, Point> PointByCreature { get; private set; }
		public Dictionary<Point, CreatureGeoInfo> CreatureByPoint { get; private set; }
		public Dictionary<Point, List<CreatureGeoInfo>> DummyCreatureByPoint { get; private set; }
		public Dictionary<Creature, CreatureGeoInfo> InfoByCreature { get; private set; }
		public Dictionary<Creature, CreatureGeoInfo> OutOfScope { get; private set; }

		public CreatureGeoInfo AddCreature(Creature _creature, Point _worldCoords, Point _liveCoords, WorldLayer _layer = null)
		{
			if(_creature is FakedCreature)
			{
				_creature = (Creature)((FakedCreature)_creature).Essence.Clone(World.TheWorld.Avatar);
			}

			if(InfoByCreature.ContainsKey(_creature))
			{
				throw new ApplicationException();
			}

			if(_layer==null)
			{
				_layer = World.TheWorld.Avatar.GeoInfo.Layer;
			}

			CreatureGeoInfo geoInfo;
			if(!OutOfScope.TryGetValue(_creature, out geoInfo))
			{
				geoInfo = new CreatureGeoInfo(_creature, _worldCoords) {Layer = _layer};
			}
			else
			{
				OutOfScope.Remove(_creature);
			}

			InfoByCreature.Add(_creature, geoInfo);
			
			PointByCreature.Add(geoInfo, _worldCoords);
			if (_creature is AbstractDummyCreature)
			{
				List<CreatureGeoInfo> list;
				if(!DummyCreatureByPoint.TryGetValue(_worldCoords, out list))
				{
					list = new List<CreatureGeoInfo>();
					DummyCreatureByPoint.Add(_worldCoords, list);
				}
				list.Add(geoInfo);
			}
			else
			{
				CreatureByPoint.Add(_worldCoords, geoInfo);
			}

			geoInfo.LiveCoords = _liveCoords;

			_creature.GeoInfo = geoInfo;
			geoInfo[0,0].ResetCached();

			if (_creature.GeoInfo.WorldCoords != _worldCoords)
			{
				throw new ApplicationException();
			}

			return geoInfo;
		}

		public void CreatureIsDead(Creature _creature)
		{
			var info = InfoByCreature[_creature];

			_creature[0,0].ResetCached();
			_creature.GeoInfo = null;
			_creature.ClearActPool();
			InfoByCreature.Remove(_creature);
			OutOfScope.Remove(_creature);
			if(!PointByCreature.Remove(info))
			{
				throw new ApplicationException();
			}

			if (_creature is AbstractDummyCreature)
			{
				if (!DummyCreatureByPoint[info.WorldCoords].Remove(info))
				{
					throw new ApplicationException();
				}
			}
			else
			{
				if(!CreatureByPoint.Remove(info.WorldCoords))
				{
					throw new ApplicationException();
				}
			}
		}

		public void ExcludeCreature(Creature _creature)
		{
			_creature.ClearActPool();
			_creature[0,0].ResetCached();
			var info = InfoByCreature[_creature];
			OutOfScope.Add(_creature, info);
			InfoByCreature.Remove(_creature);

			PointByCreature.Remove(info);
			if (_creature is AbstractDummyCreature)
			{
				if(!DummyCreatureByPoint[info.WorldCoords].Remove(info))
				{
					throw new ApplicationException();
				}
			}
			else
			{
				if(!CreatureByPoint.Remove(info.WorldCoords))
				{
					throw new ApplicationException();
				}

				var blockId = BaseMapBlock.GetBlockId(info.WorldCoords);
				info.Layer[blockId].CreaturesAdd(_creature, BaseMapBlock.GetInBlockCoords(info.WorldCoords));
			}
			
			info[0, 0].ResetCached();
			info.LiveCoords = null;
		}

		public void MoveCreature(Creature _creature, Point _worldCoord, WorldLayer _layer = null)
		{
			if (_layer == null)
			{
				_layer = World.TheWorld.Avatar.GeoInfo.Layer;
			}

			_creature[0, 0].ResetCached();

			var info = InfoByCreature[_creature];

			if (_creature.GeoInfo.WorldCoords != info.WorldCoords)
			{
				throw new ApplicationException();
			}

			if (_layer != info.Layer)
			{
				if (_creature.IsAvatar)
				{
					
					var allCreatures = InfoByCreature.Keys.ToArray();
					foreach (var creature in allCreatures)
					{
						if (creature.IsAvatar)
						{
							continue;
						}
						if (creature is AbstractDummyCreature)
						{
							CreatureIsDead(creature);
						}
						else
						{
							ExcludeCreature(creature);
						}
					}
					info.Layer = _layer;
					World.TheWorld.LiveMap.Reset();
				}
				else
				{
					throw new NotImplementedException();
				}
			}
			else
			{
				if (info.WorldCoords == _worldCoord)
				{
					throw new ApplicationException();
				}

				PointByCreature.Remove(info);

				var oldBlockId = BaseMapBlock.GetBlockId(info.WorldCoords);
				var newBlockId = BaseMapBlock.GetBlockId(_worldCoord);

				var delta = _worldCoord - info.WorldCoords;
				if (oldBlockId != newBlockId)
				{
					var newCell = info[delta];
					if (newCell.LiveMapBlock.IsBorder)
					{
						ExcludeCreature(_creature);
					}
					if (_creature.IsAvatar)
					{
						World.TheWorld.LiveMap.AvatarChangesBlock(oldBlockId, newBlockId, newCell.LiveMapBlock.LiveMapBlockId);
					}
				}
				if (info.LiveCoords != null)
				{
					if (_creature is AbstractDummyCreature)
					{
						if (!DummyCreatureByPoint[info.WorldCoords].Remove(info))
						{
							throw new ApplicationException();
						}
						List<CreatureGeoInfo> list;
						if (!DummyCreatureByPoint.TryGetValue(_worldCoord, out list))
						{
							list = new List<CreatureGeoInfo>();
							DummyCreatureByPoint.Add(_worldCoord, list);
						}
						list.Add(info);
					}
					else
					{
						if (!CreatureByPoint.Remove(info.WorldCoords))
						{
							throw new ApplicationException();
						}
						CreatureByPoint.Add(_worldCoord, info);
					}
					info.LiveCoords += delta;
					info.WorldCoords = _worldCoord;

					PointByCreature.Add(info, _worldCoord);
				}
			}
			if (info.LiveCoords != null)
			{
				_creature[0, 0].ResetCached();
				if (_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, WorldMessage.AvatarMove);
				}
			}
		}

		public Creature FirstActiveCreature
		{
			get
			{
				Creature first = World.TheWorld.Avatar;
				foreach (var creature in InfoByCreature.Keys)
				{
					if (first.BusyTill > creature.BusyTill)
					{
						first = creature;
					}
				}
				return first;
			}
		}

		public IEnumerable<Tuple<ILightSource, CreatureGeoInfo>> LightSources()
		{
			foreach (var pair in InfoByCreature)
			{
				if (pair.Key.Light != null) yield return new Tuple<ILightSource, CreatureGeoInfo>(pair.Key.Light, pair.Value);
			}

		}

		public void MoveCreatureOnDelta(Creature _creature, Point _delta)
		{
			MoveCreature(_creature, InfoByCreature[_creature].WorldCoords + _delta);
		}
	}
}
