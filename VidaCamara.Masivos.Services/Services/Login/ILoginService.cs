using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Login;
//using VidaCamara.Masivos.Services.Dtos.Login;
using VidaCamara.Domain.Services.LoginModule;

namespace VidaCamara.Masivos.Services.Services.LoginModule
{
    public interface ILoginService
    {
        Task<Login> GetLogin(LoginParam param);
        Task<IEnumerable<Modulo>> GetMenu(int param);
        Task<IEnumerable<Combo>> GetCombo(ComboParam param);
        //Task<BrandDto> DelBrand(BrandDto brandDto);
    }
}
