using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Helper;

namespace VidaCamara.Masivos.Services.Services.Apeseg
{
    public class ApesegService : IApesegService
    {
        private readonly IApesegRepository _apesegRepository;

        public ApesegService(IApesegRepository apesegRepository)
        {
            _apesegRepository = apesegRepository;
        }

        public async Task<RegistrarResponse> Apeseg_Registrar(RegistroSOATRequest request)
        {
            RegistrarParam param = new RegistrarParam
            {
                CodigoAseguradora = request.CodigoAseguradora,
                PolizaCertificado = request.PolizaCertificado,
                FechaInicioVigencia = request.FechaInicioVigencia,
                FechaFinVigencia = request.FechaFinVigencia,
                CodigoTipoPersona = request.CodigoTipoPersona,
                NombreContratante = request.NombreContratante,
                CodigoTipoDocumento = request.CodigoTipoDocumento,
                NumeroDocumento = request.NumeroDocumento,
                PlacaVehiculo = request.PlacaVehiculo,
                CodigoUsoVehiculo = request.CodigoUsoVehiculo,
                CodigoClaseVehiculo = request.CodigoClaseVehiculo,
                PaisPlaca = request.PaisPlaca,
                FechaIngreso = DateTime.Now.ToString("dd/MM/yyyy"), // request.FechaRegistro,
                CodigoUbigeo = request.CodigoUbigeo,
                NumeroSerieMotor = request.NumeroSerieMotor,
                FechaControlPolicial = request.FechaControlPolicial,
                TipoCertificado = request.TipoCertificado,
                TelefonoContacto = request.TelefonoContacto,
                CorreoContacto = request.CorreoContacto,
                UsuarioCreacion = request.UsuarioRegistro,
                Marca = request.Marca,
                NumeroAsientos = request.NumeroAsientos,
                ModeloVehiculo = request.ModeloVehiculo
            };

            return await _apesegRepository.Apeseg_Registrar(param);
        }

        public async Task<ModificarResponse> Apeseg_Actualizar(ModificarSOATRequest request)
        {
            ModificarParam param = new ModificarParam
            {
                CodigoAseguradora = request.CodigoAseguradora,
                PolizaCertificado = request.PolizaCertificado,
                DigitoVerificador = request.DigitoVerificador,
                FechaInicioVigencia = request.FechaInicioVigencia,
                FechaFinVigencia = request.FechaFinVigencia,
                CodigoTipoPersona = request.CodigoTipoPersona,
                NombreContratante = request.NombreContratante,
                CodigoTipoDocumento = request.CodigoTipoDocumento,
                NumeroDocumento = request.NumeroDocumento,
                PlacaVehiculo = request.PlacaVehiculo,
                CodigoUsoVehiculo = request.CodigoUsoVehiculo,
                CodigoClaseVehiculo = request.CodigoClaseVehiculo,
                PaisPlaca = request.PaisPlaca,
                FechaActualizacion = DateTime.Now.ToString("dd/MM/yyyy"),
                CodigoUbigeo = request.CodigoUbigeo,
                NumeroSerieMotor = request.NumeroSerieMotor,
                NumeroSerieChasis = request.NumeroSerieChasis,
                FechaControlPolicial = request.FechaControlPolicial,
                TipoCertificado = request.TipoCertificado,
                UsuarioModificacion = request.UsuarioRegistro,
                Marca = request.Marca,
                NumeroAsientos = request.NumeroAsientos,
                ModeloVehiculo = request.ModeloVehiculo
            };

            return await _apesegRepository.Apeseg_Actualizar(param);
        }

        public async Task<AnularResponse> Apeseg_Anular(AnulacionSOATRequest request)
        {
            AnularParam param = new AnularParam
            {
                codigoAseguradora = request.codigoAseguradora,
                polizaCertificado = request.polizaCertificado,
                digitoVerificador = request.digitoVerificador,
                codigoTipoAnulacion = request.codigoTipoAnulacion,
                fechaAnulacion = DateTime.Now.ToString("dd/MM/yyyy"),
                usuarioModificacion = request.UsuarioRegistro
            };

            return await _apesegRepository.Apeseg_Anular(param);
        }

        public async Task<ConsultarResponse> Consultar(ConsultaSOATRequest request)
        {
            ConsultarParam param = new ConsultarParam
            {
                placa = request.placa,
                subscriptionKey = request.subscriptionKey
            };

            return await _apesegRepository.Consultar(param);
        }

    }
}