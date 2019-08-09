﻿using System;
using System.ComponentModel;
using CoreAnimation;
using UndderControl.Effects;
using UndderControl.iOS.Effects;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(IOSRoundCornersEffect), nameof(RoundCornersEffect))]
namespace UndderControl.iOS.Effects
{
    class IOSRoundCornersEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                PrepareContainer();
                SetCornerRadius();
            }
            catch { }
        }

        protected override void OnDetached()
        {
            try
            {
                Container.Layer.CornerRadius = new nfloat(0);
            }
            catch { }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == RoundCornersEffect.CornerRadiusProperty.PropertyName)
                SetCornerRadius();
        }

        private void PrepareContainer()
        {
            Container.ClipsToBounds = true;
            Container.Layer.AllowsEdgeAntialiasing = true;
            Container.Layer.EdgeAntialiasingMask = CAEdgeAntialiasingMask.All;
        }

        private void SetCornerRadius()
        {
            var cornerRadius = RoundCornersEffect.GetCornerRadius(Element);
            Container.Layer.CornerRadius = new nfloat(cornerRadius);
        }
    }
}