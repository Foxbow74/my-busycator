using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Acts
{
	public class TakeAct : Act
	{
		public TakeAct() : base(20)
		{
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			var intelligent = (Intelligent) _creature;
			var notTaken = _creature.GetNotTakenAvailableItems(GetParameter<Point>());

			if (!notTaken.Any())
			{
				// Если ничего не доступно
				if (!_silence)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "тут ничего нет."));
				}
				return EActResults.NOTHING_HAPPENS;
			}

			var toTake = GetParameter<ThingDescriptor>().ToList();

			if (toTake.Count==0)
			{
				// Если в параметрах нет уточнений
				if (notTaken.Count()==1)
				{
					toTake.AddRange(notTaken);
				}
				else
				{
					//получаем все возможные координаты ячеек, где лежат не взятые предметы, ограничив параметром направления, если таковой представлен
					var coords = notTaken.Select(_descriptor => _descriptor.WorldCoords).Distinct();
					if(coords.Count()>1)
					{
						MessageManager.SendMessage(this, new AskDirectionMessage(this, _creature.Coords));
					}
					else
					{
						MessageManager.SendMessage(this, new AskSelectThingsMessage(notTaken, this));
					}
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
			}
			
			if (toTake.Count>1)
			{
				foreach (var descr in toTake)
				{
					var act = new TakeAct();
					act.AddParameter(descr);
					act.AddParameter(GetParameter<Point>());
					_creature.AddActToPool(act);
				}
				return EActResults.NOTHING_HAPPENS;
			}

			var descriptor = toTake.First();
			var count = 1;
			if(descriptor.Container==null)
			{
				Map.GetMapCell(descriptor.WorldCoords).RemoveObjectFromBlock();
			}
			else
			{
				var get = descriptor.Container.GetItems(_creature).Items.Where(_item => _item.GetHashCode() == descriptor.Thing.GetHashCode());
				if (get.Count() > 1)
				{
					var cnt = GetParameter<int>();
					if (cnt.Any())
					{
						count = cnt.Single();
					}
					else
					{
						MessageManager.SendMessage(this, new AskHowMuchMessage(this, descriptor, get.Count()));
						return EActResults.NEED_ADDITIONAL_PARAMETERS;
					}

				}
				for (var i = 0; i < count; ++i)
				{
					descriptor.Container.GetItems(_creature).Remove((Item)descriptor.Thing);
				}
			}
			for (var i = 0; i < count; ++i)
			{
				intelligent.ObjectTaken((Item)descriptor.Thing);
			}
			if (!_silence && count>0)
			{
				var suffix = count > 1 ? (", " + count + " штук.") : ".";
				if (intelligent is Avatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, descriptor.Thing + " взят" + suffix));
				}
				else
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature + " взял " + descriptor.Thing + suffix));
				}
			}
			return EActResults.DONE;
		}
	}
}