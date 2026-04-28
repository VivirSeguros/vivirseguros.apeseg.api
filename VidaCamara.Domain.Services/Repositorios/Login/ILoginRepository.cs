using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Login;

namespace VidaCamara.Domain.Services.LoginModule
{
    public interface ILoginRepository
    {
        Task<Login> GetLogin(LoginParam param);
        Task<IEnumerable<Modulo>> GetMenu(int param);
        Task<IEnumerable<Combo>> GetCombo(ComboParam param);
        Task<bool> CheckIfAD(string username);
        //Task<Brand> DelBrand(Brand brand);
    }
}
