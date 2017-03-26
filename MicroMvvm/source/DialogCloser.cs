using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicroMvvm
{
    /// <summary>
    /// http://stackoverflow.com/questions/501886/how-should-the-viewmodel-close-the-form
    /// Во ViewModel добавить свойство bool? DialogResult - обязательно вызывающее RaisePropertyChanged!
    /// В View добавиьт строчки:
    /// xmlns:xc="clr-namespace:MicroMvvm;assembly=MicroMvvm"
    /// xc:DialogCloser.DialogResult="{Binding DialogResult}">
    /// </summary>
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty = 
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
            {
                window.DialogResult = e.NewValue as bool?;                
            }
        }
        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);            
        }
    }
}
