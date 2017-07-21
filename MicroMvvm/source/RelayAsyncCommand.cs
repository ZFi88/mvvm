using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMvvm
{
	public class RelayAsyncCommand<T> : RelayCommand<T>
	{
		private bool isExecuting;
		public event EventHandler Started;
		public event EventHandler Ended;

		public bool IsExecuting => isExecuting;

		public RelayAsyncCommand(Action<T> execute, Predicate<T> canExecute = null)
			: base(execute, canExecute)
		{
		}		

		public override bool CanExecute(object parameter)
		{
			return base.CanExecute(parameter) && !isExecuting;
		}

		public override void Execute(object parameter)
		{
			try
			{
				isExecuting = true;
				Started?.Invoke(this, EventArgs.Empty);

				var task = Task.Factory.StartNew(() =>
				{
					base.Execute(parameter);
				});
				task.ContinueWith(t =>
				{
					OnRunWorkerCompleted(EventArgs.Empty);
				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
			catch (Exception ex)
			{
				OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
			}
		}

		private void OnRunWorkerCompleted(EventArgs e)
		{
			isExecuting = false;
			Ended?.Invoke(this, e);
		}
	}

	public class RelayAsyncCommand : RelayCommand
    {
        private bool isExecuting;
        public event EventHandler Started;
        public event EventHandler Ended;

        public bool IsExecuting => isExecuting;

        public RelayAsyncCommand(Action execute, Func<bool> canExecute)
            : base(execute, canExecute)
        {
        }

        public RelayAsyncCommand(Action execute)
            : base(execute)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && !isExecuting;
        }

        public override void Execute(object parameter)
        {
            try
            {
                isExecuting = true;
                Started?.Invoke(this, EventArgs.Empty);

                var task = Task.Factory.StartNew(() =>
                {
                    _execute();
                });
                task.ContinueWith(t =>
                {
                    OnRunWorkerCompleted(EventArgs.Empty);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
            }
        }

        private void OnRunWorkerCompleted(EventArgs e)
        {
            isExecuting = false;
            Ended?.Invoke(this, e);
        }
    }
}
