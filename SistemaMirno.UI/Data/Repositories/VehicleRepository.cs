using System;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.Data.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle, MirnoDbContext>, IVehicleRepository
    {
        public VehicleRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }
    }
}
