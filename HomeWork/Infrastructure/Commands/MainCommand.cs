using System;

namespace HomeWork.Infrastructure.Commands
{
    internal class MainCommand : BaseCommand//класс основной команды
    {

        private readonly Action<object> _Execute;

        private readonly Func<object, bool> _CanExecute;

        public MainCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentException(nameof(Execute));//выполняем или выбрасываем исключение
            _CanExecute = CanExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _CanExecute?.Invoke(parameter) ?? true;
        }

        public override void Execute(object parameter)
        {
            _Execute(parameter);
        }
    }
}