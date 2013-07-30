using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Orchard.iOSContainers;

namespace TestContainers
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		UIViewController red, green, blue;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//SetupTabBarDisplay ();
			SetupEvernoteDisplay ();

			return true;
		}

//		AnimatedTabBarViewController _tabBar;
//
// 		Images were removed from the project as they were copyrighted. Feel free to use your own
//		void SetupTabBarDisplay ()
//		{
//			window = new UIWindow (UIScreen.MainScreen.Bounds);
//
//			red = new UIViewController ();
//			red.View.BackgroundColor = UIColor.Red;
//
//			green = new UIViewController ();
//			green.View.BackgroundColor = UIColor.Green;
//
//			blue = new UIViewController ();
//			blue.View.BackgroundColor = UIColor.Blue;
//
//			_tabBar = new AnimatedTabBarViewController ();
//			_tabBar.AddTab (UIImage.FromFile ("images/favourites_64.png"), red);
//			_tabBar.AddTab (UIImage.FromFile ("images/folders_64.png"), green);
//			_tabBar.AddTab (UIImage.FromFile ("images/mail_64.png"), blue);
//			_tabBar.ActiveTabIndex = 1;
//
//			window.RootViewController = _tabBar;
//			window.MakeKeyAndVisible ();
//		}

		ENoteViewController _enoteDisplay;

		void SetupEvernoteDisplay ()
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			_enoteDisplay = new ENoteViewController ();

			red = new ENoteTestController (_enoteDisplay);
			red.View.BackgroundColor = UIColor.Red;

			green = new ENoteTestController (_enoteDisplay);
			green.View.BackgroundColor = UIColor.Green;

			blue = new ENoteTestController (_enoteDisplay);
			blue.View.BackgroundColor = UIColor.Blue;

			_enoteDisplay.ViewControllers.AddRange (new UIViewController[] {
				red, green, blue
			});

			window.RootViewController = _enoteDisplay;
			window.MakeKeyAndVisible ();
		}
	}

	/// <summary>
	/// Used so we have a callback so we can animate back from the main controller
	/// You could achieve the same mechanism by having a reference to the ENoteViewController
	/// </summary>
	public class ENoteTestController : UIViewController
	{
		ENoteViewController _enote;

		public ENoteTestController(ENoteViewController enote) : base()
		{
			_enote = enote;
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			_enote.ShowAll (true);
		}
	}
}

