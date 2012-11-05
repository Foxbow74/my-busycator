using System.Windows;
using System.Windows.Input;

namespace ClientCommonWpf
{
	public class PointHandler
	{
		#region LastPoint

		/// <summary>
		/// LastPoint Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty LastPointProperty =
			DependencyProperty.RegisterAttached("LastPoint", typeof(Point), typeof(PointHandler),
			   new FrameworkPropertyMetadata(new Point(0,0)));

		/// <summary>
		/// Gets the LastPoint property.  This dependency property 
		/// indicates ....
		/// </summary>
		public static Point GetLastPoint(DependencyObject d)
		{
			return (Point)d.GetValue(LastPointProperty);
		}

		/// <summary>
		/// Sets the LastPoint property.  This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetLastPoint(DependencyObject d, Point value)
		{
			d.SetValue(LastPointProperty, value);
		}

		#endregion

		#region HandleMouse

		/// <summary>
		/// HandleMouse Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty HandleMouseProperty =
			DependencyProperty.RegisterAttached("HandleMouse", typeof(bool), typeof(PointHandler),
				new FrameworkPropertyMetadata(false, OnHandleMouseChanged));

		/// <summary>
		/// Gets the HandleMouse property.  This dependency property 
		/// indicates ....
		/// </summary>
		public static bool GetHandleMouse(DependencyObject d)
		{
			return (bool)d.GetValue(HandleMouseProperty);
		}

		/// <summary>
		/// Sets the HandleMouse property.  This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetHandleMouse(DependencyObject d, bool value)
		{
			d.SetValue(HandleMouseProperty, value);
		}

		/// <summary>
		/// Handles changes to the HandleMouse property.
		/// </summary>
		private static void OnHandleMouseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as UIElement;
			if (control != null)
			{
				if ((bool)e.NewValue)
					control.MouseMove += ControlMouseDoubleClick;
				else
					control.MouseMove -= ControlMouseDoubleClick;
			}
		}

		static void ControlMouseDoubleClick(object _sender, MouseEventArgs _e)
		{
			SetLastPoint((DependencyObject)_sender, _e.GetPosition((IInputElement)_sender));
		}

		#endregion
	}
}
