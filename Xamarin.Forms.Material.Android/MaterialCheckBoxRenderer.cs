﻿using System;
using Android.Content;
using Android.Content.Res;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using AColorUtils = Android.Support.V4.Graphics.ColorUtils;
using AAttribute = Android.Resource.Attribute;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace Xamarin.Forms.Material.Android
{
	public class MaterialCheckBoxRenderer : CheckBoxRendererBase
	{
		static int[][] _checkedStates = new int[][]
					{
						new int[] { AAttribute.StateEnabled, AAttribute.StateChecked },
						new int[] { AAttribute.StateEnabled, -AAttribute.StateChecked },
						new int[] { -AAttribute.StateEnabled, AAttribute.StateChecked },
						new int[] { -AAttribute.StateEnabled, -AAttribute.StatePressed }
					};

		public MaterialCheckBoxRenderer(Context context) : base(MaterialContextThemeWrapper.Create(context), Resource.Attribute.materialCheckBoxStyle)
		{

		}



		protected override ColorStateList GetColorStateList()
		{
			if (Element.TintColor != Color.Default)
				return base.GetColorStateList();

			int[] checkBoxColorsList = new int[4];

			int colorControlActivated = MaterialColors.Light.PrimaryColor;
			int colorSurface = MaterialColors.Light.SurfaceColor;
			int colorOnSurface = MaterialColors.Light.OnSurfaceColor;

			checkBoxColorsList[0] = Layer(colorSurface, colorControlActivated, 1f);
			checkBoxColorsList[1] = Layer(colorSurface, colorOnSurface, 0.54f);
			checkBoxColorsList[2] = Layer(colorSurface, colorOnSurface, 0.38F);
			checkBoxColorsList[3] = Layer(colorSurface, colorOnSurface, 0.38F);


			return new ColorStateList(_checkedStates, checkBoxColorsList);
		}

		public static int Layer(
		   int backgroundColor,
		   int overlayColor,
		   float overlayAlpha)
		{
			var alpha = ((uint)overlayColor) >> 24;
			int computedAlpha = (int)Math.Round(alpha * overlayAlpha);
			int computedOverlayColor = AColorUtils.SetAlphaComponent(overlayColor, computedAlpha);
			return Layer(backgroundColor, computedOverlayColor);
		}

		public static int Layer(int backgroundColor, int overlayColor)
		{
			return AColorUtils.CompositeColors(overlayColor, backgroundColor);
		}
	}
}