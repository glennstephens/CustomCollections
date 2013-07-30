using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Drawing;

namespace Orchard.iOSContainers
{
	// A version of the UITabBarController but has animations for the buttons and is highlighted by the background
	public class AnimatedTabBarViewController : UIViewController
	{
		public AnimatedTabBarViewController () : base()
		{
		}

		public class TabEntry
		{
			public UIImage ButtonImage;
			public UIViewController ViewController;

			public UILabel Label = null;
			public TapableImageView Button = null;

			public bool HasCreated = false;

			public override string ToString ()
			{
				return ViewController.Title;
			}
		}

		public List<TabEntry> Tabs = new List<TabEntry>();

		public void AddTab(UIImage buttonImage, UIViewController viewController)
		{
			TabEntry entry = new TabEntry();
			entry.ButtonImage = buttonImage;
			entry.ViewController = viewController;
			Tabs.Add(entry);
		}

		private UIColor _backgroundColor = UIColor.White;

		public UIColor BackgroundColor
		{
			get
			{
				return _backgroundColor;
			}
			set
			{
				if (_backgroundColor != value)
				{
					_backgroundColor = value;
					View.SetNeedsDisplay();
				}
			}
		}

		private UIColor _selectedItemBackgroundColor = UIColor.Blue;
		
		public UIColor SelectedItemBackgroundColor
		{
			get
			{
				return _selectedItemBackgroundColor;
			}
			set
			{
				if (_selectedItemBackgroundColor != value)
				{
					_selectedItemBackgroundColor = value;
					View.SetNeedsDisplay();
				}
			}
		}

		private UIImage _backgroundImage = null;

		public UIImage BackgroundImage
		{
			get
			{
				return _backgroundImage;
			}
			set
			{
				if (_backgroundImage != value)
				{
					_backgroundImage = value;
					View.SetNeedsDisplay();
				}
			}
		}

		private float _tabHeight = 48f;

		public float TabHeight
		{
			get
			{
				return _tabHeight;
			}
			set
			{
				if (_tabHeight != value)
				{
					_tabHeight = value;
					View.SetNeedsLayout();
					View.SetNeedsDisplay();
				}
			}
		}

		private int _activeTabIndex = -1;

		public int ActiveTabIndex
		{
			get
			{
				return _activeTabIndex;
			}
			set
			{
				_activeTabIndex = value;

				// Hide the View Controllers where needed

				// Show the new View Controller

				View.SetNeedsLayout();
				View.SetNeedsDisplay();
			}
		}

		UIView _bottomContainer;
		UIImageView _bottomImage;
		UIView _highlightShape;

		UIView _tabContainer;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Create the controls that are required to use
			_bottomContainer = new UIView();
			_bottomContainer.BackgroundColor = this.BackgroundColor;
			Add (_bottomContainer);

			// Add the background Image
			_bottomImage = new UIImageView();
			//_bottomImage.Image = _bottomImage;
			Add(_bottomImage);

			// Add the Selection Shape
			_highlightShape = new UIView();
			_highlightShape.BackgroundColor = this.SelectedItemBackgroundColor;
			_bottomContainer.Add(_highlightShape);

			AddButtons();

			// Create the Tab Controls
			_tabContainer = new UIView();
			Add (_tabContainer);

			ShowCurrentTab();
		}

		float PaddingSizeHorizontal;
		float PaddingSizeVertical;
		float RectangleWidth;
		float RectangleHeight;

		private void AddButtons ()
		{
			PaddingSizeHorizontal = 8f;
			PaddingSizeVertical = 2f;
			RectangleWidth = (View.Frame.Width - (Tabs.Count + 1) * PaddingSizeHorizontal) / Tabs.Count;
			RectangleHeight = TabHeight - 2 * PaddingSizeVertical;

			// Create the Buttons and Images
			float currentX = PaddingSizeHorizontal;

			for (int i=0; i < Tabs.Count; i++)
			{
				TapableImageView buttonImage = new TapableImageView();
				buttonImage.Image = Tabs[i].ButtonImage;
				_bottomContainer.Add(buttonImage);
				buttonImage.Frame = new RectangleF(currentX, PaddingSizeVertical, RectangleWidth, RectangleHeight);
				buttonImage.ContentMode = UIViewContentMode.ScaleAspectFit;
				buttonImage.Tag = i;
				buttonImage.OnTouch += ProcessTabButtonTouch;
				Tabs[i].Button = buttonImage;

				// Add the Label
				UILabel label = new UILabel();
				label.Frame = buttonImage.Frame;
				label.Text = Tabs[i].ViewController.Title;
				label.TextAlignment = UITextAlignment.Center;
				label.BackgroundColor = UIColor.Clear;
				_bottomContainer.Add(label);
				Tabs[i].Label = label;

				currentX += RectangleWidth + PaddingSizeHorizontal;
			}

			_highlightShape.Frame = new RectangleF(PaddingSizeHorizontal, PaddingSizeVertical, RectangleWidth, RectangleHeight);
			_highlightShape.BackgroundColor = UIColor.FromRGBA(0f, 0f, 0f, 0.5f);
		}

		void ProcessTabButtonTouch (object sender, EventArgs e)
		{
			// Change the Tab
			ActiveTabIndex = (sender as TapableImageView).Tag;
			ShowCurrentTab();

			// Animate the Button Popping in and out
			UIView.BeginAnimations("MoveShapyThing");
			UIView.SetAnimationDuration(0.4f);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);

			_highlightShape.Frame = new RectangleF(PaddingSizeHorizontal + ActiveTabIndex * (RectangleWidth + PaddingSizeHorizontal), 
			                                       PaddingSizeVertical, RectangleWidth, RectangleHeight);

			UIView.CommitAnimations();
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();

			// Layout the items on the frame
			_bottomContainer.Frame = new System.Drawing.RectangleF(0, this.View.Frame.Height - TabHeight, 
			                                                       this.View.Frame.Width, TabHeight);
			_tabContainer.Frame = new System.Drawing.RectangleF(0, 0, 
			                                                    this.View.Frame.Width, 
			                                                    this.View.Frame.Height - _bottomContainer.Frame.Height);
		}

		private void ShowCurrentTab ()
		{
			// Do we have an active tab index
			if (Tabs.Count == 0)
				return;

			if (ActiveTabIndex < 0 || ActiveTabIndex >= Tabs.Count )
				ActiveTabIndex = 0;

			var entry = Tabs[ActiveTabIndex];

			// See if we have created it or not
			foreach (var t in Tabs)
			{
				// Only add the Tab if it has been created
				if (t.HasCreated)
				{
					t.ViewController.View.Hidden = t != entry;
				} else {
					if (t == entry)
					{
						// Add the control to the list
						_tabContainer.Add(t.ViewController.View);
						t.HasCreated = true;
						t.ViewController.View.Frame = _tabContainer.Frame;
					}
				}
			}
		}
	}
}

