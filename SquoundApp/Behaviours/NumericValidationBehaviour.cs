using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp.Behaviours
{
    public partial class NumericValidationBehaviour : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            
            bindable.TextChanged -= OnTextChanged;
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                // Extract only digits from the new text value.
                var digitsOnly = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

                // Remove any unnecessary leading zeros.
                var trimmed = digitsOnly.TrimStart('0');

                // Null or empty strings assigned zero character.
                if (string.IsNullOrEmpty(trimmed))
                {
                    //trimmed = "0";
                }

                // If the text differs from the current text, update it.
                if (entry.Text != trimmed)
                {
                    entry.Text = trimmed;
                }
            }
        }
    }
}
