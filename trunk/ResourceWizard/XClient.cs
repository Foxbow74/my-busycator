using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Threading;
using GameCore.Storage;
using XTransport.Client;

namespace ResourceWizard
{
	class XClient : AbstractXClient<EStoreKind>
	{
		public override Guid UserUid
		{
			get { return Guid.Empty; }
		}

		protected override IEnumerable<KeyValuePair<EStoreKind, EStoreKind>> GetAbstractRootKindMap()
		{
			yield break;
		}

		protected override int KindToInt(EStoreKind _kind)
		{
			return (int)_kind;
		}

		protected override EStoreKind IntToKind(int _kind)
		{
			return (EStoreKind)_kind;
		}

		protected override void ObjectReleased(Guid _uid, EStoreKind _kind)
		{
		}

		protected override Dispatcher GetUiDispatcher()
		{
			return UiDispatcher;
		}

		public static Dispatcher UiDispatcher { get; set; }


	}

	public class TestConverter : IValueConverter
	{

		public object Convert(object _value, Type _targetType, object _parameter, CultureInfo _culture)
		{
			return String.Format(_culture, "{0:N2}", _value);
		}

		public object ConvertBack(object _value, Type _targetType, object _parameter, CultureInfo _culture)
		{
			throw new NotImplementedException();
		}

	}
}