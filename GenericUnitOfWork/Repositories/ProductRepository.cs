using BussinessCore.Model;
using GenericUnitOfWork.Base;

namespace GenericUnitOfWork
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(MyAppContext context)
            : base(context)
        {

        }
    }
}
