﻿using System;
using CoreGraphics;
using UIKit;

namespace Xamarin.Forms.Platform.iOS
{
	public class FormsCheckBox : UIControl
	{
		public virtual float DefaultSize => 30.0f;

		Color _checkColor, _tintColor;
		bool _isChecked;
		bool _isEnabled;
		public EventHandler CheckedChanged;

		public FormsCheckBox()
		{
			BackgroundColor = UIColor.Clear;
		}

		public bool IsChecked
		{
			get => _isChecked;
			set
			{
				if (value == _isChecked)
					return;

				_isChecked = value;
				SetNeedsDisplay();
			}
		}

		public bool IsEnabled
		{
			get => _isEnabled;
			set
			{
				if (value == _isEnabled)
					return;

				_isEnabled = value;

				UserInteractionEnabled = IsEnabled;

				SetNeedsDisplay();
			}
		}

		public Color CheckColor
		{
			get => _checkColor;
			set
			{
				if (_checkColor == value)
					return;

				_checkColor = value;
				SetNeedsDisplay();
			}
		}

		public Color CheckBoxTintColor
		{
			get => _tintColor;
			set
			{
				if (_tintColor == value)
					return;

				_tintColor = value;
				SetNeedsDisplay();
			}
		}

		protected virtual UIBezierPath GetCheckBoxPath(CGRect backgroundRect) => UIBezierPath.FromOval(backgroundRect);

		public override void Draw(CGRect rect)
		{
			var checkedColor = (CheckBoxTintColor.IsDefault ? base.TintColor : CheckBoxTintColor.ToUIColor());
			checkedColor.SetFill();
			checkedColor.SetStroke();

			var width = Bounds.Size.Width;
			var height = Bounds.Size.Height;

			var outerDiameter = Math.Min(width, height);
			var lineWidth = 2.0 / DefaultSize * outerDiameter;
			var diameter = outerDiameter - 3 * lineWidth;

			var xOffset = diameter + lineWidth * 2 <= width ? lineWidth * 2 : (width - diameter) / 2;
			var hPadding = xOffset;
			var vPadding = (nfloat)((height - diameter) / 2);

			var backgroundRect = new CGRect(xOffset, vPadding, diameter, diameter);
			var boxPath = GetCheckBoxPath(backgroundRect);
			boxPath.LineWidth = (nfloat)lineWidth;
			boxPath.Stroke();
			if (IsChecked)
			{
				boxPath.Fill();
				var checkPath = new UIBezierPath
				{
					LineWidth = (nfloat)0.077,
					LineCapStyle = CGLineCap.Round,
					LineJoinStyle = CGLineJoin.Round
				};
				var context = UIGraphics.GetCurrentContext();
				context.SaveState();
				context.TranslateCTM((nfloat)hPadding + (nfloat)(0.05 * diameter), vPadding + (nfloat)(0.1 * diameter));
				context.ScaleCTM((nfloat)diameter, (nfloat)diameter);
				checkPath.MoveTo(new CGPoint(0.72f, 0.22f));
				checkPath.AddLineTo(new CGPoint(0.33f, 0.6f));
				checkPath.AddLineTo(new CGPoint(0.15f, 0.42f));
				(CheckColor.IsDefault ? UIColor.White : CheckColor.ToUIColor()).SetStroke();
				checkPath.Stroke();
				context.RestoreState();
			}

		}

		public override bool BeginTracking(UITouch uitouch, UIEvent uievent)
		{
			IsChecked = !IsChecked;
			CheckedChanged?.Invoke(this, null);
			return base.BeginTracking(uitouch, uievent);
		}
	}
}