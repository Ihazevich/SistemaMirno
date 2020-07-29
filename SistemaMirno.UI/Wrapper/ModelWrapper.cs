using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Runtime.CompilerServices;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// Generic model wrapper class.
    /// </summary>
    /// <typeparam name="T">The type of model for the wrapper.</typeparam>
    public class ModelWrapper<T> : WrapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelWrapper{T}"/> class.
        /// </summary>
        /// <param name="model">The model to use.</param>
        public ModelWrapper(T model)
        {
            Model = model;
        }

        /// <summary>
        /// Gets or sets the wrapper model.
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Gets the value of a property from the wrapper model.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The property value of type <typeparamref name="TValue"/>.</returns>
        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        /// <summary>
        /// Sets the value of a property from the wrapper model.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">The property name.</param>
        protected virtual void SetValue<TValue>(
            TValue value,
            [CallerMemberName] string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName, value);
        }

        /// <summary>
        /// Validates a property with the specified name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A collection representing the erorrs found after validation.</returns>
        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }

        private void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);

            // First validate data annotations
            ValidateDataAnnotations(propertyName, currentValue);

            // Secondly, validate custom errors
            ValidateCustomError(propertyName);
        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        private void ValidateCustomError(string propertyName)
        {
            var errors = ValidateProperty(propertyName);

            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }
    }
}
