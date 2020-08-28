using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class ClientWrapper : ModelWrapper<Client>
    {
        public ClientWrapper()
            : base(new Client())
        {
        }

        public ClientWrapper(Client model)
            : base(model)
        {
        }

        public int Id
        {
            get { return GetValue<int>(); }
        }

        public string FullName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string RUC
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string PhoneNumber
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string City
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Department
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool IsRetail
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsWholesaler
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FullName):
                    if (FullName.Length < 4)
                    {
                        yield return "El nombre es muy corto.";
                    }

                    break;

                case nameof(IsRetail):
                    if (!(IsRetail || IsWholesaler))
                    {
                        yield return "Debe ser minorista o mayorista al menos.";
                    }

                    break;

                case nameof(IsWholesaler):
                    if (!(IsRetail || IsWholesaler))
                    {
                        yield return "Debe ser minorista o mayorista al menos.";
                    }

                    break;
            }
        }
    }
}
