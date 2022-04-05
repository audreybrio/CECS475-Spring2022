using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mm.DataAccessLayer
{
    public class StandardRepository : Repository<Standard>, IStandardRepository
    {
        public StandardRepository() : base(new SchoolDBEntities())
        {

        }
    }
}
