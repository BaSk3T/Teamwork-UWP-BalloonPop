using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BalloonPop.CustomControls
{
    public sealed partial class Joystick : UserControl
    {
        public Joystick()
        {
            this.InitializeComponent();
        }

        public int Size
        {
            get { return (int)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(Joystick), new PropertyMetadata(0, new PropertyChangedCallback(ChangeSizeCustomProperty)));

        public int TouchSize
        {
            get { return (int)GetValue(TouchSizeProperty); }
            set { SetValue(TouchSizeProperty, value); }
        }

        public static readonly DependencyProperty TouchSizeProperty =
            DependencyProperty.Register("TouchSize", typeof(int), typeof(Joystick), new PropertyMetadata(0, new PropertyChangedCallback(ChangeTouchSizeCustomProperty)));

        public double TouchLeftPosition
        {
            get { return (double)GetValue(TouchLeftPositionProperty); }
            set { SetValue(TouchLeftPositionProperty, value); }
        }

        public static readonly DependencyProperty TouchLeftPositionProperty =
            DependencyProperty.Register("TouchLeftPosition", typeof(double), typeof(Joystick), new PropertyMetadata(0, new PropertyChangedCallback(ChangeTouchLeftPositionCustomProperty)));

        public double TouchTopPosition
        {
            get { return (double)GetValue(TouchTopPositionProperty); }
            set { SetValue(TouchTopPositionProperty, value); }
        }

        public static readonly DependencyProperty TouchTopPositionProperty =
            DependencyProperty.Register("TouchTopPosition", typeof(double), typeof(Joystick), new PropertyMetadata(0, new PropertyChangedCallback(ChangeTouchTopPositionCustomProperty)));

        private static void ChangeTouchSizeCustomProperty(DependencyObject property, DependencyPropertyChangedEventArgs e)
        {
            Joystick joystick = (Joystick)property;

            var sizeOfTouch = int.Parse(e.NewValue.ToString());

            joystick.InnerRing.Width = sizeOfTouch;
            joystick.InnerRing.Height = sizeOfTouch;
        }

        private static void ChangeSizeCustomProperty(DependencyObject property, DependencyPropertyChangedEventArgs e)
        {
            Joystick joystick = (Joystick)property;

            var sizeOfJoystick = int.Parse(e.NewValue.ToString());

            joystick.OuterRing.Width = sizeOfJoystick;
            joystick.OuterRing.Height = sizeOfJoystick;
        }

        private static void ChangeTouchLeftPositionCustomProperty(DependencyObject property, DependencyPropertyChangedEventArgs e)
        {
            Joystick joystick = (Joystick)property;

            var leftPosition = double.Parse(e.NewValue.ToString());

            joystick.InnerRing.SetValue(Canvas.LeftProperty, leftPosition);
        }

        private static void ChangeTouchTopPositionCustomProperty(DependencyObject property, DependencyPropertyChangedEventArgs e)
        {
            Joystick joystick = (Joystick)property;

            var topPosition = double.Parse(e.NewValue.ToString());

            joystick.InnerRing.SetValue(Canvas.TopProperty, topPosition);
        }
    }
}
