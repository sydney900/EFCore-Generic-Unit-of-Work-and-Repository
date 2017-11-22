using BussinessCore.DTO;
using BussinessCore.Model;
using GenericUnitOfWork.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericUnitOfWork
{
    public class ClientRepository : Repository<Client>
    {
        public ClientRepository(MyAppContext context)
            : base(context)
        {
            //context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        System.Linq.Expressions.Expression<System.Func<Client, ClientDto>> selectorClient = c => new ClientDto
        {
            Id = c.Id,
            ClientName = c.ClientName,
            Email = c.Email,
            LastModified = c.LastModified
        };

        public List<ClientDto> GetAllClientsSortByName()
        {
            return _entities.OrderBy(c => c.ClientName).Select(selectorClient).ToList();
        }

        public async Task<List<ClientDto>> GetAllClientsSortByNameAsync()
        {
            return await _entities.OrderBy(c => c.ClientName).Select(selectorClient).ToListAsync();
        }

    }
}
