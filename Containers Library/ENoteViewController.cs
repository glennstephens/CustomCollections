using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace Orchard.iOSContainers
{
	public class ENoteViewController : UIViewController
	{
		public ENoteViewController () : base()
		{
		}

		public List<UIViewController> ViewControllers = new List<UIViewController>();

		Dictionary<UIViewController, ENoteView> tabs = new Dictionary<UIViewController, ENoteView>();

		public UIImage backgroundImage = null;

		UIImageView _imgView; 

		protected virtual void SetupControls()
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (backgroundImage != null)
			{
				_imgView = new UIImageView(this.View.Bounds);
				_imgView.Image = backgroundImage;
				Add (_imgView);
			}

			SetupControls();

			SetupTabs ();

			View.BackgroundColor = UIColor.LightGray;
			//if (ViewControllers.Count > 0)
			//	SelectViewController(ViewControllers[0]);
		}

		public float TabStartY = 100f;
		public float StartingWidth = 320f;
		public float TabWidthDelta = 8f; 

		const float TabHeight = 35f;

		Dictionary<UIViewController, RectangleF> viewControllerStart = new Dictionary<UIViewController, RectangleF>();
		Dictionary<ENoteView, RectangleF> tabViewStart = new Dictionary<ENoteView, RectangleF>();

		void SetupTabs()
		{
			float currentYPos = TabStartY;
			float currentWidth = StartingWidth - (ViewControllers.Count * TabWidthDelta);

			foreach (UIViewController c in ViewControllers)
			{
				// Add the view in
				ENoteView ttv = new ENoteView(this, c);

				ttv.Frame = new RectangleF((320f-currentWidth) / 2, currentYPos, 
				                           currentWidth, TabHeight);
				c.View.Frame = new RectangleF((320f-currentWidth) / 2, currentYPos,
				                              currentWidth, View.Frame.Height);

				currentYPos += 28;
				currentWidth += TabWidthDelta;

				Add (c.View);
				Add (ttv);

				viewControllerStart[c] = c.View.Frame;
				tabViewStart[ttv] = ttv.Frame;

				tabs[c] = ttv;
			}
		}

		public EventHandler OnShowTab = null;
		public EventHandler OnShowAll = null;

		public void ShowAll(bool animate = true)
		{
			if (animate) {
				UIView.BeginAnimations ("FunkyTabReset");
				UIView.SetAnimationDuration(0.5f);
			}

			foreach (UIViewController c in ViewControllers)
			{
				ENoteView ttv = tabs[c];				
				ttv.Frame = tabViewStart[ttv];
				ttv.Alpha = 1.0f;
				c.View.Frame = viewControllerStart[c];
				c.View.Alpha = 1.0f;
			}

			if (OnShowAll != null)
			{
				OnShowAll(this, EventArgs.Empty);
			}

			if (animate)
			{
				UIView.CommitAnimations();
			}
			IsShowingSingle = false;
		}
		
		public void SelectViewController (UIViewController c, bool animate = true)
		{
			if (animate) {
				UIView.BeginAnimations ("FunkyTab");
				UIView.SetAnimationDuration(0.5f);
			}

			int selectedIndex = ViewControllers.IndexOf(c);

			// This makes the little Tab View Active for the selected ViewController c only. Everything else is unactive
			foreach (var vc in ViewControllers) {
				bool shouldShow = vc == c;

				int currentIndex = ViewControllers.IndexOf(vc);

				bool animateUp = currentIndex < selectedIndex;
				bool animateDown = currentIndex > selectedIndex;

				//tabItems [vc].Active = shouldShow;
				
				if (shouldShow)
				{
					// Show the View Controller
					//Add(vc.View);
					vc.View.Alpha = 1.0f;

					// Set the position of the View Controller
					vc.View.Frame = new RectangleF(0, 0, View.Frame.Width, View.Frame.Height);

					// Set the view of the Tab
					tabs[vc].Frame = new RectangleF(0, -TabHeight, View.Frame.Width, TabHeight);
				} else if (animateUp) {
					vc.View.Alpha = 0f;

					// Set the position of the View Controller
					vc.View.Frame = new RectangleF(vc.View.Frame.Left, -View.Frame.Height + TabHeight, vc.View.Frame.Width, View.Frame.Height);
					
					// Set the view of the Tab
					tabs[vc].Frame = new RectangleF(vc.View.Frame.Left, -View.Frame.Height, vc.View.Frame.Width, TabHeight);
				} else if (animateDown) {
					vc.View.Alpha = 0f;
					
					// Set the position of the View Controller
					vc.View.Frame = new RectangleF(vc.View.Frame.Left, View.Frame.Height + TabHeight, vc.View.Frame.Width, View.Frame.Height);
					
					// Set the view of the Tab
					tabs[vc].Frame = new RectangleF(vc.View.Frame.Left, View.Frame.Height, vc.View.Frame.Width, TabHeight);
				}
			}

			if (OnShowTab != null)
			{
				OnShowTab(this, EventArgs.Empty);
			}

			if (animate)
			{
				UIView.CommitAnimations();
			}

			IsShowingSingle = true;
		}

		public bool IsShowingSingle = false;
	}
}

