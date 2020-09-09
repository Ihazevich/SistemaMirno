using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class VehicleWrapper : ModelWrapper<Vehicle>
    {
        public VehicleWrapper()
            : base(new Vehicle())
        {
        }

        public VehicleWrapper(Vehicle model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public string VehicleModel
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Year
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Patent
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime PatentExpiration
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime DinatranExpiration
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime FireExtinguisherExpiration
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }


        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
            }

            foreach (var error in base.ValidateProperty(propertyName))
            {
                if (error != null)
                {
                    yield return error;
                }
            }
        }
    }
}
