﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Uno.Extensions;
using Uno.Foundation.Extensibility;
using Uno.Logging;

namespace Microsoft.UI.Xaml.Controls
{
	public partial class ProgressRing : Control
	{
		private AnimatedVisualPlayer.ILottieVisualSourceProvider _lottieProvider;

		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, OnIsActivePropertyChanged));

		private Windows.UI.Xaml.Controls.AnimatedVisualPlayer _player;

		public bool IsActive
		{
			get => (bool)GetValue(IsActiveProperty);
			set => SetValue(IsActiveProperty, value);
		}

		public ProgressRing()
		{
			DefaultStyleKey = this;

			ApiExtensibility.CreateInstance(this, out _lottieProvider);

			if (_lottieProvider == null)
			{
				this.Log().Error($"{nameof(ProgressRing)} control needs the Uno.UI.Lottie package to run properly.");
			}

			RegisterPropertyChangedCallback(ForegroundProperty, OnForegroundPropertyChanged);
			RegisterPropertyChangedCallback(BackgroundProperty, OnbackgroundPropertyChanged);
		}

		protected override AutomationPeer OnCreateAutomationPeer() => new ProgressRingAutomationPeer(progressRing: this);

		protected override void OnApplyTemplate()
		{
			_player = GetTemplateChild("IndeterminateAnimatedVisualPlayer") as Windows.UI.Xaml.Controls.AnimatedVisualPlayer;

			SetAnimatedVisualPlayerSource();

			ChangeVisualState();
		}

		private void OnForegroundPropertyChanged(DependencyObject sender, DependencyProperty dp)
		{
			if (Background is SolidColorBrush background)
			{
				// TODO
			}
		}

		private void OnbackgroundPropertyChanged(DependencyObject sender, DependencyProperty dp)
		{
			if (Foreground is SolidColorBrush foreground)
			{
				// TODO
			}
		}

		private static void OnIsActivePropertyChanged(DependencyObject dependencyobject, DependencyPropertyChangedEventArgs args)
		{
			(dependencyobject as ProgressRing)?.ChangeVisualState();
		}

		private void SetAnimatedVisualPlayerSource()
		{
			// Nothing to do here yet
			if (_lottieProvider != null && _player != null)
			{
				var sourceFile = new Uri("ms-appx:///Microsoft/UI/Xaml/Controls/ProgressRing/loading.json");
				var animatedVisualSource = _lottieProvider.CreateFromLottieAsset(sourceFile);
				_player.Source = animatedVisualSource;
			}
		}

		private void ChangeVisualState()
		{
			if (IsActive)
			{
				// Support for older templates
				VisualStateManager.GoToState(this, "Active", true);

				var _ = _player?.PlayAsync(0, 1, true);
			}
			else
			{
				VisualStateManager.GoToState(this, "Inactive", true);
				_player?.Stop();
			}
		}
	}
}
