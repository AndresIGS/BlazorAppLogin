using BlazorAppLogin.Shared;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorAppLogin.Client.Models
{
    public class Autenticacion : AuthenticationStateProvider
    {

        private readonly ISessionStorageService _sessionStorageService;
        private ClaimsPrincipal _sininformacion = new ClaimsPrincipal(new ClaimsIdentity());

        public Autenticacion(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        public async Task ActualizarEstado(SesionDTO? sesionusuario)
        {
            ClaimsPrincipal claimsPrincipal;

            if (sesionusuario != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name,sesionusuario.Nombre),
                    new Claim(ClaimTypes.Email,sesionusuario.Correo),
                    new Claim(ClaimTypes.Role,sesionusuario.Rol)

                },"JwtAuth"));

                await _sessionStorageService.GuardarStorage("sesionusuario", sesionusuario);

            }
            else
            {
                claimsPrincipal = _sininformacion;
                await _sessionStorageService.RemoveItemAsync("sesionusuario");
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));

        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var sesionusuario = await _sessionStorageService.ObtenerStorage<SesionDTO>("sesionusuario");
            if (sesionusuario == null)
                return await Task.FromResult(new AuthenticationState(_sininformacion));

            var claimprincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name,sesionusuario.Nombre),
                    new Claim(ClaimTypes.Email,sesionusuario.Correo),
                    new Claim(ClaimTypes.Role,sesionusuario.Rol)

                }, "JwtAuth"));

                return await Task.FromResult(new AuthenticationState(claimprincipal));


        }
    }
}
