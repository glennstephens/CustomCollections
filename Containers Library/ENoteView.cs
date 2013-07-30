using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace Orchard.iOSContainers
{
	public class ENoteView : UIView
	{
		UIViewController _controller;
		ENoteViewController _ttv;
		
		public ENoteView(ENoteViewController ttv, UIViewController controller)
		{
			_ttv = ttv;
			_controller = controller;
			BackgroundColor = UIColor.Clear;
		}
		
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			
			// Change the Tab Page
			if (!_ttv.IsShowingSingle)
				_ttv.SelectViewController(_controller);
			else
				_ttv.ShowAll();
		}

		public override void Draw (RectangleF frame)
		{
			return;

			//// General Declarations
			var colorSpace = CGColorSpace.CreateDeviceRGB();
			var context = UIGraphics.GetCurrentContext();
			
			//// Color Declarations
			UIColor gradientColor = UIColor.FromRGBA(0.462f, 0.838f, 1.000f, 1.000f);
			UIColor blueBorder = UIColor.FromRGBA(0.000f, 0.590f, 1.000f, 1.000f);
			
			//// Gradient Declarations
			var gradientColors = new CGColor [] {UIColor.White.CGColor, UIColor.FromRGBA(0.748f, 0.920f, 1.000f, 1.000f).CGColor, gradientColor.CGColor};
			var gradientLocations = new float [] {0, 0.71f, 1};
			var gradient = new CGGradient(colorSpace, gradientColors, gradientLocations);
			
			//// Frames
			
			//// Abstracted Attributes
			var textContent = _controller.Title;
			
			//// Bezier Drawing
			UIBezierPath bezierPath = new UIBezierPath();
			bezierPath.MoveTo(new PointF(frame.GetMinX() + 0.99530f * frame.Width, frame.GetMinY() + 0.27632f * frame.Height));
			bezierPath.AddLineTo(new PointF(frame.GetMinX() + 0.99530f * frame.Width, frame.GetMinY() + 0.32895f * frame.Height));
			bezierPath.AddLineTo(new PointF(frame.GetMinX() + 0.99530f * frame.Width, frame.GetMinY() + 0.96053f * frame.Height));
			bezierPath.AddLineTo(new PointF(frame.GetMinX() + 0.00352f * frame.Width, frame.GetMinY() + 0.96053f * frame.Height));
			bezierPath.AddLineTo(new PointF(frame.GetMinX() + 0.00352f * frame.Width, frame.GetMinY() + 0.38158f * frame.Height));
			bezierPath.AddLineTo(new PointF(frame.GetMinX() + 0.00352f * frame.Width, frame.GetMinY() + 0.27632f * frame.Height));
			bezierPath.AddCurveToPoint(new PointF(frame.GetMinX() + 0.07487f * frame.Width, frame.GetMinY() + 0.01316f * frame.Height), new PointF(frame.GetMinX() + 0.00352f * frame.Width, frame.GetMinY() + 0.13098f * frame.Height), new PointF(frame.GetMinX() + 0.03547f * frame.Width, frame.GetMinY() + 0.01316f * frame.Height));
			bezierPath.AddLineTo(new PointF(frame.GetMinX() + 0.92395f * frame.Width, frame.GetMinY() + 0.01316f * frame.Height));
			bezierPath.AddCurveToPoint(new PointF(frame.GetMinX() + 0.99530f * frame.Width, frame.GetMinY() + 0.27632f * frame.Height), new PointF(frame.GetMinX() + 0.96335f * frame.Width, frame.GetMinY() + 0.01316f * frame.Height), new PointF(frame.GetMinX() + 0.99530f * frame.Width, frame.GetMinY() + 0.13098f * frame.Height));
			bezierPath.ClosePath();
			context.SaveState();
			bezierPath.AddClip();
			var bezierBounds = bezierPath.Bounds;
			context.DrawLinearGradient(gradient,
			                           new PointF(bezierBounds.GetMidX(), bezierBounds.GetMinY()),
			                           new PointF(bezierBounds.GetMidX(), bezierBounds.GetMaxY()),
			                           0);
			context.RestoreState();
			blueBorder.SetStroke();
			bezierPath.LineWidth = 1;
			bezierPath.Stroke();
			
			
			//// Text Drawing
			var textRect = new RectangleF(frame.GetMinX() + (float)Math.Floor(frame.Width * 0.02410f + 0.5f), frame.GetMinY() + (float)Math.Floor(frame.Height * 0.25000f + 0.5f), (float)Math.Floor(frame.Width * 0.94578f + 0.5f) - (float)Math.Floor(frame.Width * 0.02410f + 0.5f), (float)Math.Floor(frame.Height * 0.91667f + 0.5f) - (float)Math.Floor(frame.Height * 0.25000f + 0.5f));
			UIColor.Black.SetFill();
			new NSString(textContent).DrawString(textRect, UIFont.FromName("Helvetica", 12), UILineBreakMode.WordWrap, UITextAlignment.Center);
			

		}
	}
	
}
