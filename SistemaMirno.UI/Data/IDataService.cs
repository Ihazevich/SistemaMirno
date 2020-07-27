using SistemaMirno.Model;
using System.Collections.Generic;

namespace SistemaMirno.UI.Data
{
    public interface IDataService
    {
        IEnumerable<BaseModel> GetAll();
    }
}