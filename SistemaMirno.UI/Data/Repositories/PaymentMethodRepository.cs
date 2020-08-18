// <copyright file="PaymentMethodRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod, MirnoDbContext>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(MirnoDbContext context)
         : base(context)
        {
        }
    }
}
