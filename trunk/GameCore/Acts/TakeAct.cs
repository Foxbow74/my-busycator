using System.Collections.Generic;
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
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "тут ничего нет?"));
				}
				return EActResults.NOTHING_HAPPENS;
			}

			var toTakeParameter = GetParameter<IEnumerable<ThingDescriptor>>().ToList();

			if (!toTakeParameter.Any())
			{
				// Если в параметрах нет уточнений
				if (notTaken.Count()==1)
				{
					toTakeParameter.Add(notTaken);
				}
				else
				{
					//получаем все возможные координаты ячеек, где лежат не взятые предметы, ограничив параметром направления, если таковой представлен
					var coords = notTaken.Select(_descriptor => _descriptor.WorldCoords).Distinct();
					if(coords.Count()>1)
					{
						MessageManager.SendMessage(this, new AskDirectionMessage(this));
					}
					else
					{
						MessageManager.SendMessage(this, new AskSelectThingsMessage(notTaken, this));
					}
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
			}

			foreach (var descriptors in toTakeParameter)
			{
				foreach (var itemDescriptor in descriptors)
				{
					var thing = itemDescriptor.Thing;
					if (itemDescriptor.Container == null)
					{
						//шмотка на земле
						Map.GetMapCell(itemDescriptor.WorldCoords).RemoveObjectFromBlock();
					}
					else
					{
						itemDescriptor.Container.GetItems(_creature).Remove((Item)thing);
					}
					intelligent.ObjectTaken((Item)thing);
					if(_silence)
					{
						if (intelligent is Avatar)
						{
							MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, thing + " взят."));
						}
						else
						{
							MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature + " взял " + thing));
						}
					}
				}
			}
			return EActResults.DONE;
		}
	}
}