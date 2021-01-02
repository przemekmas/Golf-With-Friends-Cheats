using System;
using System.Windows;
using System.Windows.Controls;

namespace Golf_With_Friends_Cheats.Controls
{
    [TemplatePart(Name = "PART_SetButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ActiveCheckBox", Type = typeof(CheckBox))]
    [TemplatePart(Name = "PART_ValueTextBox", Type = typeof(TextBox))]
    public abstract class SetMemoryValueControl : Control
    {
        private ActiveTimer _activeTimer;
        private TextBox _valueTextBox;

        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register(nameof(Text), typeof(string), typeof(SetMemoryValueControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty IntervalValueProperty =
          DependencyProperty.Register(nameof(IntervalValue), typeof(int), typeof(SetMemoryValueControl), new UIPropertyMetadata(null));

        public abstract int Address { get; }
        public abstract int[] Offsets { get; }

        public string Text
        {
            get { return GetValue(TextProperty).ToString(); }
            set { SetValue(TextProperty, value); }
        }

        public int IntervalValue
        {
            get { return (int)GetValue(IntervalValueProperty); }
            set { SetValue(IntervalValueProperty, value); }
        }

        public string DefaultValue
        { 
            get
            {
                return _valueTextBox?.Text;
            }
            set
            {
                _valueTextBox.Text = value;
            }
        }

        static SetMemoryValueControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SetMemoryValueControl), new FrameworkPropertyMetadata(typeof(SetMemoryValueControl)));
        }

        public override void OnApplyTemplate()
        {
            Loaded += ControlLoaded;
            var setButton = (Button)Template.FindName("PART_SetButton", this);
            var activeCheckBox = (CheckBox)Template.FindName("PART_ActiveCheckBox", this);
            _valueTextBox = (TextBox)Template.FindName("PART_ValueTextBox", this);
            if (setButton != null)
            {
                setButton.Click += OnSetButtonClick;
            }
            if (activeCheckBox != null)
            {
                activeCheckBox.Checked += OnActiveChecked;
                activeCheckBox.Unchecked += OnActiveUnchecked;
            }
        }

        protected abstract void ControlLoaded(object sender, RoutedEventArgs e);

        private void OnActiveUnchecked(object sender, RoutedEventArgs e)
        {
            _activeTimer?.Stop();
        }

        private void OnActiveChecked(object sender, RoutedEventArgs e)
        {
            _activeTimer = new ActiveTimer(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    Execute(DefaultValue);
                });
            }, IntervalValue);
            _activeTimer.Start();
        }

        private void OnSetButtonClick(object sender, RoutedEventArgs e)
        {
            Execute(DefaultValue);
        }

        private void Execute(string value)
        {
            try
            {
                ExecuteAction(value);
            }
            catch (Exception exception)
            {
                _activeTimer?.Stop();
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public abstract void ExecuteAction(string value);
    }
}