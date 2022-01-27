using System;
using System.Windows.Input;

namespace HomeWork.Infrastructure
{
    internal abstract class BaseCommand : ICommand//базовый класс команды
    {
        public event EventHandler CanExecuteChanged//событие вызова команды
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object parameter);//метод проверки может ли команда быть использована

        public abstract void Execute(object parameter);//метод с кодом команды
    }
}