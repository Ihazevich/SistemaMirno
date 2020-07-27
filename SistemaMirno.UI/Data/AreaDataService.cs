using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public class AreaDataService : IDataService
    {
        public IEnumerable<BaseModel> GetAll()
        {
            yield return new ProductionArea { Name = "Lamina" };
            yield return new ProductionArea { Name = "Tericados" };
            yield return new ProductionArea { Name = "Prensa" };
            yield return new ProductionArea { Name = "Maquina" };
            yield return new ProductionArea { Name = "Perforacion" };
            yield return new ProductionArea { Name = "Lija" };
            yield return new ProductionArea { Name = "Filos" };
            yield return new ProductionArea { Name = "Banco" };
            yield return new ProductionArea { Name = "Lustre" };
            yield return new ProductionArea { Name = "Terminacion" };
        }
    }
}
