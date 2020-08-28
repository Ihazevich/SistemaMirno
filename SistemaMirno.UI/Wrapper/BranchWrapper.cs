using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class BranchWrapper : ModelWrapper<Branch>
    {
        public BranchWrapper()
            : base(new Branch())
        {
        }

        public BranchWrapper(Branch model)
            : base(model)
        {
        }

        public int Id { get { return GetValue<int>(); } }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the User password.
        /// </summary>
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

        public long Cash
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    if (Name.Length < 4)
                    {
                        yield return "El nombre es muy corto.";
                    }

                    break;

                case nameof(Address):
                    if (Address.Length < 4)
                    {
                        yield return "Dirección invalida.";
                    }

                    break;

                case nameof(City):
                    if (City.Length < 4)
                    {
                        yield return "Ciudad invalida.";
                    }

                    break;

                case nameof(Department):
                    if (Department.Length < 4)
                    {
                        yield return "Departamento invalido.";
                    }

                    break;
            }
        }
    }
}
