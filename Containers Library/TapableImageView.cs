using System;
using MonoTouch.UIKit;

namespace Orchard.iOSContainers
{
	/// <summary>
	/// A basic container class that holds an image at the bottom of the screen so that promotional
	/// material can be placed there
	/// </summary>

	public class TapableImageView : UIImageView
	{
		public TapableImageView() : base()
		{
			UserInteractionEnabled = true;
		}
		
		public override bool CanBecomeFirstResponder {
			get {
				return true;
			}
		}

		public event EventHandler OnTouch = null;

		public override void TouchesEnded (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			if (OnTouch != null)
				OnTouch(this, EventArgs.Empty);
		}
	}
}
