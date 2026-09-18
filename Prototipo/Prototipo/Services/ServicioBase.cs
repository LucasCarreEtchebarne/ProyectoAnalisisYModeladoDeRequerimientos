using Prototipo.Data;

namespace Prototipo.Services
{
    public abstract class ServicioBase
    {
        protected readonly HotelColibriContext Db;

        protected ServicioBase(HotelColibriContext db)
        {
            Db = db;
        }
    }
}
