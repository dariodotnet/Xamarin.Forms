using System;
using System.ComponentModel;
using Android.Content;
using Android.Support.V7.Widget;
using AView = Android.Views.View;
using Android.Views;
using Xamarin.Forms.Internals;
using AColor = Android.Graphics.Color;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Android.Graphics.Drawables;
using Android.Graphics;
using Xamarin.Forms.Platform.Android.FastRenderers;
using Android.OS;
using Android.Widget;
using Android.Content.Res;
using Android.Support.V4.Widget;
using AAttribute = Android.Resource.Attribute;

namespace Xamarin.Forms.Platform.Android
{
	public class CheckBoxRendererBase :
		AppCompatCheckBox,
		IVisualElementRenderer,
		AView.IOnFocusChangeListener,
		CompoundButton.IOnCheckedChangeListener
	{
		bool _disposed;
		bool _skipInvalidate;
		int? _defaultLabelFor;
		VisualElementTracker _tracker;
		VisualElementRenderer _visualElementRenderer;
		IPlatformElementConfiguration<PlatformConfiguration.Android, CheckBox> _platformElementConfiguration;
		CheckBox _checkBox;

		static int[][] _checkedStates = new int[][]
					{
						new int[] { AAttribute.StateEnabled, AAttribute.StateChecked },
						new int[] { AAttribute.StateEnabled, -AAttribute.StateChecked },
						new int[] { -AAttribute.StateEnabled, AAttribute.StateChecked },
						new int[] { -AAttribute.StateEnabled, -AAttribute.StatePressed },
					};

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
		public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

		public CheckBoxRendererBase(Context context) : base(context) => Init();

		public CheckBoxRendererBase(Context context, int defStyleAttr) : base(context, null, defStyleAttr) => Init();

		void Init()
		{
			// These set the defaults so visually it matches up with other platforms
			SetPadding(0, 0, 0, 0);
			SoundEffectsEnabled = false;
			SetOnCheckedChangeListener(this);

			Tag = this;
		}

		protected override void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = true;

			if (disposing)
			{
				_tracker?.Dispose();
				_tracker = null;


				if (Element != null)
				{
					Element.PropertyChanged -= OnElementPropertyChanged;

					if (Android.Platform.GetRenderer(Element) == this)
					{
						Element.ClearValue(Android.Platform.RendererProperty);
					}

					Element = null;
				}
			}

			base.Dispose(disposing);
		}

		public override void Invalidate()
		{
			if (_skipInvalidate)
			{
				_skipInvalidate = false;
				return;
			}

			base.Invalidate();
		}

		Size MinimumSize()
		{
			return new Size();
		}

		SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			if (_disposed)
			{
				return new SizeRequest();
			}
			Measure(widthConstraint, heightConstraint);
			return new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), MinimumSize());
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{

			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			if (!(element is CheckBox checkBox))
			{
				throw new ArgumentException("Element is not of type " + typeof(CheckBox), nameof(element));
			}

			CheckBox oldElement = Element;
			Element = checkBox;

			Performance.Start(out string reference);

			if (oldElement != null)
			{
				oldElement.PropertyChanged -= OnElementPropertyChanged;
			}

			element.PropertyChanged += OnElementPropertyChanged;

			if (_tracker == null)
			{
				_tracker = new VisualElementTracker(this);

			}

			if (_visualElementRenderer == null)
			{
				_visualElementRenderer = new VisualElementRenderer(this);
			}

			Performance.Stop(reference);
			this.EnsureId();

			UpdateOnColor();
			UpdateIsChecked();

			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(oldElement, Element));
			Element?.SendViewInitialized(Control);
		}

		// CheckBox related
		void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == CheckBox.TintColorProperty.PropertyName)
			{
				UpdateOnColor();
			}
			else if (e.PropertyName == CheckBox.IsCheckedProperty.PropertyName)
			{
				UpdateIsChecked();
			}

			ElementPropertyChanged?.Invoke(this, e);
		}

		void IOnCheckedChangeListener.OnCheckedChanged(CompoundButton buttonView, bool isChecked)
		{
			((IElementController)Element).SetValueFromRenderer(CheckBox.IsCheckedProperty, isChecked);
		}

		void UpdateIsChecked()
		{
			if (Element == null || Control == null)
				return;

			Checked = Element.IsChecked;
		}


		protected virtual ColorStateList GetColorStateList()
		{
			var tintColor = Element.TintColor == Color.Default ? Color.Accent.ToAndroid() : Element.TintColor.ToAndroid();

			var list = new ColorStateList(
					_checkedStates,
					new int[]
					{
						tintColor,
						tintColor,
						tintColor,
						tintColor
					});

			return list;
		}

		void UpdateOnColor()
		{

			if (Element == null || Control == null)
				return;

			var mode = PorterDuff.Mode.SrcIn;

			CompoundButtonCompat.SetButtonTintList(Control, GetColorStateList());
			CompoundButtonCompat.SetButtonTintMode(Control, mode);

		}


		// general state related
		void IOnFocusChangeListener.OnFocusChange(AView v, bool hasFocus)
		{
			((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, hasFocus);
		}
		// general state related



		IPlatformElementConfiguration<PlatformConfiguration.Android, CheckBox> OnThisPlatform()
		{
			if (_platformElementConfiguration == null)
				_platformElementConfiguration = Element.OnThisPlatform();

			return _platformElementConfiguration;
		}

		public void SetLabelFor(int? id)
		{
			if (_defaultLabelFor == null)
				_defaultLabelFor = LabelFor;

			LabelFor = (int)(id ?? _defaultLabelFor);
		}



		void IVisualElementRenderer.UpdateLayout() => _tracker?.UpdateLayout();
		VisualElement IVisualElementRenderer.Element => Element;
		AView IVisualElementRenderer.View => this;
		ViewGroup IVisualElementRenderer.ViewGroup => null;
		VisualElementTracker IVisualElementRenderer.Tracker => _tracker;

		protected CheckBox Element
		{
			get => _checkBox;
			private set
			{
				_checkBox = value;
				_platformElementConfiguration = null;
			}
		}

		protected AppCompatCheckBox Control => this;
	}
}