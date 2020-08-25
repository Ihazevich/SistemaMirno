using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.View.Services;

namespace SistemaMirno.UI.Data.Repositories
{
    public class BranchRepository : GenericRepository<Branch, MirnoDbContext> , IBranchRepository
    {
        public BranchRepository(MirnoDbContext context, IMessageDialogService dialogService, IEventAggregator eventAggregator) 
            : base(context, dialogService, eventAggregator)
        {
        }
    }
}
