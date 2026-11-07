using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.BusinessLogic.Mesas;

public class AgregarMesaBL : IAgregarMesaBL
{
    private readonly IAgregarMesaDA _agregarDA;
    private readonly IExisteNumeroMesaDA _existeNumeroDA;

    public AgregarMesaBL(IAgregarMesaDA agregarDA, IExisteNumeroMesaDA existeNumeroDA)
    {
        _agregarDA = agregarDA;
        _existeNumeroDA = existeNumeroDA;
    }

    public async Task<ResultadoOperacion> AgregarAsync(MesaDto mesa)
    {
        ReglasMesa.Normalizar(mesa);

        var error = ReglasMesa.Validar(mesa);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeNumeroDA.ExisteNumeroAsync(mesa.NumeroMesa, 0))
        {
            return ResultadoOperacion.Error(Mensajes.MesaDuplicada);
        }

        var id = await _agregarDA.AgregarAsync(mesa);
        return ResultadoOperacion.Ok(Mensajes.MesaRegistrada, id);
    }
}
