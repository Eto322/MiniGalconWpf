using System.Windows.Input;
using System;

namespace GalConeServerMapGeneratorUi.inf;

public class RelayCommand : ICommand
{
    private Action<object> _execute;
    private Predicate<object> _canExecute;



    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {

        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null ? true : _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}