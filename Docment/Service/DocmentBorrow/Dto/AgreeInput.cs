using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    public class AgreeInput
    {
       public List<Guid> DocmentIds { get; set; }
        public Guid BorrowId { get; set; }
    }
}
