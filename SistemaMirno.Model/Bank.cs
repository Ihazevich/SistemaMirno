using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class Bank : ModelBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey(nameof(BankAccount.BankId))]
        public virtual ICollection<BankAccount> BankAccounts { get; set; } = new HashSet<BankAccount>();
    }
}
