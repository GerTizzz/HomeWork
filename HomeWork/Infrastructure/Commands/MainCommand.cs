using System;

namespace HomeWork.Infrastructure.Commands
{
    internal class MainCommand : BaseCommand//класс основной команды
    {

        private readonly Action<object> _execute;

        private readonly Func<object, bool> _canExecute;

        public MainCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _execute = Execute ?? throw new ArgumentException(nameof(Execute));//выполняем или выбрасываем исключение
            _canExecute = CanExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}