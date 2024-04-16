using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Interface
{
    public interface IAuditable
    {
        public DateTime CreationDate { get; set; }
    }
}
