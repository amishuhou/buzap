using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace UsedParts.UI.Interactions.Behaviors
{
    /// <summary>
    /// <see cref="Behavior">Behavior</see> that updates <see cref="PasswordBox">PasswordBox</see> Password <see cref="Binding">binding</see> source.
    /// </summary>
    public class PasswordBoxUpdateBindingBehavior : SafeBehavior<PasswordBox>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += OnTextChanged;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PasswordChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject == null)
            {
                return;
            }

            var binging = AssociatedObject.GetBindingExpression(PasswordBox.PasswordProperty);
            
            if (binging != null)
            {
                binging.UpdateSource();
            }
        }
    }
}
