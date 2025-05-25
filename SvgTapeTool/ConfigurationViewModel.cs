using SvgTapeTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASCIIToPaperTapeSVG
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public float TapeHeight { get; set; } = Properties.Settings.Default.TapeHeight;
        public float HorizontalMargin { get; set; } = Properties.Settings.Default.HorizontalMargin;
        public float ColumnSpacing { get; set; } = Properties.Settings.Default.ColumnSpacing;
        public float DataHoleDiam { get; set; } = Properties.Settings.Default.DataHoleDiam;
        public float SprocketHoleDiam { get; set; } = Properties.Settings.Default.SprocketHoleDiam;
        public float MaxTapeLengthIn { get; set; } = Properties.Settings.Default.MaxTapeLengthIn;

        public ICommand ApplyConfigCommand => new RelayCommand(ApplyConfig);

        private void ApplyConfig()
        {
            TapeConstants.TAPE_HEIGHT = TapeHeight;
            TapeConstants.HORIZONTAL_MARGIN = HorizontalMargin;
            TapeConstants.COLUMN_SPACING = ColumnSpacing;
            TapeConstants.DATA_HOLE_DIAM = DataHoleDiam;
            TapeConstants.SPROCKET_HOLE_DIAM = SprocketHoleDiam;
            TapeConstants.MAX_TAPE_LENGTH_IN = MaxTapeLengthIn;

            // Save to user settings
            Properties.Settings.Default.TapeHeight = TapeHeight;
            Properties.Settings.Default.HorizontalMargin = HorizontalMargin;
            Properties.Settings.Default.ColumnSpacing = ColumnSpacing;
            Properties.Settings.Default.DataHoleDiam = DataHoleDiam;
            Properties.Settings.Default.SprocketHoleDiam = SprocketHoleDiam;
            Properties.Settings.Default.MaxTapeLengthIn = MaxTapeLengthIn;
            Properties.Settings.Default.Save();
        }

        // Implement INotifyPropertyChanged as needed
    }

    // Add the RelayCommand implementation if it is missing
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
