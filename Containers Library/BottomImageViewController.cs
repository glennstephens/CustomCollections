using System;
using MonoTouch.UIKit;

namespace Orchard.iOSContainers
{
	/// <summary>
	/// A basic container class that holds an image at the bottom of the screen so that promotional
	/// material can be placed there
	/// </summary>
	public class BottomImageViewController : MonoTouch.UIKit.UIViewController
	{
		public BottomImageViewController () : base()
		{
		}

		private UIImage _image = null;

		public UIImage Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (_image != value)
				{
					_image = value;
					if (_imageView != null)
					{
						_imageView.Image = _image;
						View.SetNeedsDisplay();
					}
				}
			}
		}

		UIViewController _childController = null;

		public UIViewController ChildController
		{
			get
			{
				return _childController;
			}
			set
			{
				if (_childController != value)
				{
					_childController = value;
					View.SetNeedsDisplay();
				}
			}
		}

		private float _imageHeight = 48;

		public float ImageHeight
		{
			get
			{
				return _imageHeight;
			}
			set
			{
				if (_imageHeight != value)
				{
					_imageHeight = value;
					View.SetNeedsDisplay();
				}
			}
		}

		TapableImageView _imageView;
		UIView _childView;

		public void FireTouchEvent()
		{
			if (OnImageTouch != null)
				OnImageTouch(this, EventArgs.Empty);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Add the Image
			_imageView = new TapableImageView ();
			_imageView.Image = _image;
			_imageView.OnTouch += (s, e) => {
				FireTouchEvent();
			};

			View.Add (_imageView);

			// Add the View Controller
			_childView = ChildController.View;
			View.Add(_childView);
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();

			// Layout the items on the frame
			_imageView.Frame = new System.Drawing.RectangleF(0, this.View.Bounds.Height - _imageHeight, 
			                                                 this.View.Bounds.Width, _imageHeight);
			_childView.Frame = new System.Drawing.RectangleF(0, 0, 
			                                                 this.View.Bounds.Width, 
			                                                 this.View.Bounds.Height - _imageHeight);
		}

		public event EventHandler OnImageTouch = null;
	}
}

