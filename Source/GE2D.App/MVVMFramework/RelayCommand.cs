using System;
using System.Windows.Input;

namespace GE2D.App.MVVMFramework;

public class RelayCommand : ICommand
{
    private readonly Action<object> execute;
    private readonly Func<object, bool>? canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        if (parameter is null)
            throw new ArgumentNullException(nameof(parameter));

        return canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        if (execute != null && parameter != null)
            execute(parameter);
    }
}
