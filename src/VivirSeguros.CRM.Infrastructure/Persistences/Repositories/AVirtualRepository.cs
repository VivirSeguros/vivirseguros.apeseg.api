using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vivirseguros.CorreoCentralizado.Utilities;
using VivirSeguros.CRM.Domain.Common;
using VivirSeguros.CRM.Domain.Entities.Producto;
using VivirSeguros.CRM.Domain.Entities.SAC;
using VivirSeguros.CRM.Domain.Entities.SAC.FONDOSMAX;
using VivirSeguros.CRM.Domain.Entities.SAC.RENTAMAX;
using VivirSeguros.CRM.Domain.Entities.SAC.RENTAVITALICIA;
using VivirSeguros.CRM.Domain.Entities.SAC.SOAT;
using VivirSeguros.CRM.Domain.Entities.SAC.VIVEMAX;
using VivirSeguros.CRM.Infrastructure.Persistences.Contexts;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Repositories
{
    public class AVirtualRepository : IAVirtualRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<AVirtualRepository> _logger;
        public AVirtualRepository(IConnectionFactory connectionFactory, ILogger<AVirtualRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<Producto>> GetProducto(string nroDoc)
        {
            List<Producto> lProducto = new List<Producto>();
            using (var connection = _connectionFactory.GetConnection)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@P_NRODOC", nroDoc);

                var response = await connection.QueryAsync<Producto>("dbo.sp_CRM_Producto_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<Producto>)response;

                //----- POLIZA FM-----
                Log.save(this, "Ingresa a polizas Fondos MAX");
                var lPoliza = await connection.QueryAsync<Poliza>("sp_FM_POLIZA_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto[0].PolizaFM = (List<Poliza>)lPoliza;

                for (int i = 0; i < lProducto[0].PolizaFM.Count; i++)
                {
                    var parameter2 = new DynamicParameters();
                    parameter2.Add("@P_NRODOC", nroDoc);
                    parameter2.Add("@P_FONDO", lProducto[0].PolizaFM[i].Portafolio);
                    parameter2.Add("@P_POLIZA", lProducto[0].PolizaFM[i].NroPoliza);

                    var lEstadoCuenta = await connection.QueryAsync<EstadoCuenta>("sp_FM_EstadoCuenta_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    lProducto[0].PolizaFM[i].EstadoCuenta = (List<EstadoCuenta>)lEstadoCuenta;

                    var lAsegurado = await connection.QuerySingleOrDefaultAsync<Asegurado>("sp_FM_Asegurado_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    lProducto[0].PolizaFM[i].Asegurado = (Asegurado)lAsegurado;

                    var lValorC = await connection.QueryAsync<ValorCuot>("sp_FM_ValorCuota_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    lProducto[0].PolizaFM[i].ValorCuota = (List<ValorCuot>)lValorC;

                    var lIndice = await connection.QueryAsync<IndiceAV>("sp_FM_IndiceAV_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    lProducto[0].PolizaFM[i].ValorIndice = (List<IndiceAV>)lIndice;

                }

                //----- SOAT -----
                Log.save(this, "Ingresa a polizas SOAT");
                var lAuto = await connection.QueryAsync<SOAT>("sp_Soat_Poliza_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto[1].PolizaSOAT = (List<SOAT>)lAuto;
                for (int i = 0; i < lProducto[1].PolizaSOAT.Count; i++)
                {
                    var parameters_aseg = new DynamicParameters();
                    parameters_aseg.Add("@p_idAuto", lProducto[1].PolizaSOAT[i].idAuto);
                    var lAsegurado = await connection.QuerySingleOrDefaultAsync<Asegurado>("sp_Soat_Asegurado_SEL", param: parameters_aseg, commandType: CommandType.StoredProcedure);

                    lProducto[1].PolizaSOAT[i].Asegurado = (Asegurado)lAsegurado;
                }
                //----- Vitalicia -----
                Log.save(this, "Ingresa a Renta Vitalicia");
                var lSeguroRV = await connection.QueryAsync<Vitalicia>("sp_RV_POLIZA_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto[2].PolizaRV = (List<Vitalicia>)lSeguroRV;

                for (int i = 0; i < lProducto[2].PolizaRV.Count; i++)
                {
                    var parameters_asegrv = new DynamicParameters();
                    parameters_asegrv.Add("@p_Poliza", lProducto[2].PolizaRV[i].NUM_POLIZA);
                    parameters_asegrv.Add("@P_NRODOC", nroDoc);
                    var lTitularR = await connection.QuerySingleOrDefaultAsync<TitularRV>("sp_RV_Titular_SEL", param: parameters_asegrv, commandType: CommandType.StoredProcedure);
                    lProducto[2].PolizaRV[i].TitularRV = (TitularRV)lTitularR;

                    var lAseguradoR = await connection.QuerySingleOrDefaultAsync<TitularRV>("sp_RV_Asegurado_SEL", param: parameters_asegrv, commandType: CommandType.StoredProcedure);
                    lProducto[2].PolizaRV[i].AseguradoRV = lAseguradoR;   //AE20230427

                    var parameter2 = new DynamicParameters();
                    parameter2.Add("@P_POLIZA", lProducto[2].PolizaRV[i].NUM_POLIZA);
                    parameter2.Add("@P_NRODOC", nroDoc);

                    var lBoletas = await connection.QueryAsync<RutaBoleta>("sp_RV_Boletas_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    lProducto[2].PolizaRV[i].TitularRV.Boletas = (List<RutaBoleta>)lBoletas;

                    var lBeneficiario = await connection.QueryAsync<BeneficiarioRV>("sp_RV_Beneficiario_SEL", param: parameters_asegrv, commandType: CommandType.StoredProcedure);
                    lProducto[2].PolizaRV[i].BeneficiarioRV = (List<BeneficiarioRV>)lBeneficiario;


                    for (int j = 0; j < lProducto[2].PolizaRV[i].BeneficiarioRV.Count; j++)
                    {
                        var parameter3 = new DynamicParameters();
                        parameter3.Add("@P_NRODOC", lProducto[2].PolizaRV[i].BeneficiarioRV[j].NoDoc);
                        parameter3.Add("@P_POLIZA", lProducto[2].PolizaRV[i].NUM_POLIZA);
                        lBoletas = await connection.QueryAsync<RutaBoleta>("sp_RV_Boletas_SEL", param: parameter3, commandType: CommandType.StoredProcedure);
                        lProducto[2].PolizaRV[i].BeneficiarioRV[j].Boletas = (List<RutaBoleta>)lBoletas;
                    }


                    var lFormaPago = await connection.QuerySingleOrDefaultAsync<FormaPagoRV>("sp_RV_FormaPago_SEL", param: parameters_asegrv, commandType: CommandType.StoredProcedure);
                    lProducto[2].PolizaRV[i].FormaPagoRV = (FormaPagoRV)lFormaPago;


                }

                //----- Privada -------
                Log.save(this, "Ingresa a Renta Privada");
                var lPrivada = await connection.QueryAsync<Privada>("sp_RM_POLIZA_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto[3].PolizaPRV = (List<Privada>)lPrivada;

                for (int i = 0; i < lProducto[3].PolizaPRV.Count; i++)
                {
                    var parameters_asegprv = new DynamicParameters();
                    parameters_asegprv.Add("@p_idModalidad", lProducto[3].PolizaPRV[i].idModalidad);
                    var lAsegurado = await connection.QuerySingleOrDefaultAsync<AseguradoPRV>("sp_RM_Asegurado_SEL", param: parameters_asegprv, commandType: CommandType.StoredProcedure);
                    lProducto[3].PolizaPRV[i].AseguradoPRV = (AseguradoPRV)lAsegurado;

                    var lBeneficiario = await connection.QueryAsync<BeneficiarioPRV>("sp_RM_Beneficiario_SEL", param: parameters_asegprv, commandType: CommandType.StoredProcedure);
                    lProducto[3].PolizaPRV[i].BeneficiarioPRV = (List<BeneficiarioPRV>)lBeneficiario;

                    var lFormaPago = await connection.QuerySingleOrDefaultAsync<FormaPagoPRV>("sp_RM_FormaPago_SEL", param: parameters_asegprv, commandType: CommandType.StoredProcedure);
                    lProducto[3].PolizaPRV[i].FormaPagoPRV = (FormaPagoPRV)lFormaPago;


                }

                //----- Sepelio-----
                Log.save(this, "Ingresa a polizas Sepelio");
                var lPolizaSepelio = await connection.QueryAsync<PolizaSepelio>("sp_SEP_POLIZA_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto[4].PolizaSepelio = (List<PolizaSepelio>)lPolizaSepelio;

                for (int i = 0; i < lProducto[4].PolizaSepelio.Count; i++)
                {
                    var parameter2 = new DynamicParameters();
                    parameter2.Add("@P_POLIZA", lProducto[4].PolizaSepelio[i].Poliza);

                    //var lEstadoCuenta = await connection.QueryAsync<EstadoCuenta>("sp_FM_EstadoCuenta_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    //lProducto[0].PolizaSepelio[i].EstadoCuenta = (List<EstadoCuenta>)lEstadoCuenta;

                    var lAsegurado = await connection.QuerySingleOrDefaultAsync<AseguradoSepelio>("sp_SEP_Asegurado_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    lProducto[4].PolizaSepelio[i].Asegurado = (AseguradoSepelio)lAsegurado;

                    var lBeneficiario = await connection.QueryAsync<BeneficiarioSepelio>("sp_SEP_Beneficiario_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    lProducto[4].PolizaSepelio[i].Beneficiarios = (List<BeneficiarioSepelio>)lBeneficiario;

                    var lValorC = await connection.QueryAsync<ValorCuotSepelio>("sp_SEP_ValorCuota_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    lProducto[4].PolizaSepelio[i].ValorCuota = (List<ValorCuotSepelio>)lValorC;

                    //var lIndice = await connection.QueryAsync<IndiceAV>("sp_FM_IndiceAV_SEL", param: parameter2, commandType: CommandType.StoredProcedure);
                    //lProducto[0].PolizaSepelio[i].ValorIndice = (List<IndiceAV>)lIndice;

                }

                return lProducto;

                Log.save(this, "Salir de Obtener Productos");
            }
        }

        public async Task<ResponseTransaction> EndosoOrdinarioAsync(EndosoOrdinario endosoOrdinario)
        {
            ResponseTransaction result = new ResponseTransaction();
            using (var connection = _connectionFactory.GetConnection1)
            {
                try
                {
                    var query = "soat.sp_AutoEndoso_INS";
                    var parameters = new DynamicParameters();
                    parameters.Add("P_NRODOCUMENTO", endosoOrdinario.NroDocumento);
                    parameters.Add("P_CORREO", endosoOrdinario.Email);
                    parameters.Add("P_CORREO1", endosoOrdinario.Email1);
                    parameters.Add("P_CORREO2", endosoOrdinario.Email2);
                    parameters.Add("P_TELEFONO", endosoOrdinario.celular);
                    parameters.Add("P_TELEFONO1", endosoOrdinario.celular1);
                    parameters.Add("P_TELEFONO2", endosoOrdinario.celular2);
                    parameters.Add("P_DIRECCION", endosoOrdinario.Direccion);
                    parameters.Add("P_PAIS", endosoOrdinario.Pais);
                    parameters.Add("P_DEPARTAMENTO", endosoOrdinario.Departamento);
                    parameters.Add("P_PROVINCIA", endosoOrdinario.Provincia);
                    parameters.Add("P_DISTRITO", endosoOrdinario.Distrito);
                    parameters.Add("P_COD_ERROR", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("P_MENSAJE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                    connection.Execute(query, param: parameters, commandType: CommandType.StoredProcedure);
                    result.ErrorCode = parameters.Get<int?>("P_COD_ERROR");
                    result.Message = parameters.Get<string>("P_MENSAJE");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    connection.Close();
                    throw ex;
                }
            }
            return result;
        }

        public async Task<ResponseTransaction> EndosoOrdinario2Async(EndosoOrdinario endosoOrdinario)
        {
            ResponseTransaction result = new ResponseTransaction();
            using (var connection = _connectionFactory.GetConnection2)
            {
                try
                {
                    var query = "dbo.PP_GenerarEndoso_INS";
                    var parameters = new DynamicParameters();
                    parameters.Add("P_NRODOCUMENTO", endosoOrdinario.NroDocumento);
                    parameters.Add("P_CORREO", endosoOrdinario.Email);
                    parameters.Add("P_CORREO1", endosoOrdinario.Email1);
                    parameters.Add("P_CORREO2", endosoOrdinario.Email2);
                    parameters.Add("P_TELEFONO", endosoOrdinario.celular);
                    parameters.Add("P_TELEFONO1", endosoOrdinario.celular1);
                    parameters.Add("P_TELEFONO2", endosoOrdinario.celular2);
                    parameters.Add("P_DIRECCION", endosoOrdinario.Direccion);
                    parameters.Add("P_PAIS", endosoOrdinario.Pais);
                    parameters.Add("P_DEPARTAMENTO", endosoOrdinario.Departamento);
                    parameters.Add("P_PROVINCIA", endosoOrdinario.Provincia);
                    parameters.Add("P_DISTRITO", endosoOrdinario.Distrito);
                    parameters.Add("P_COD_ERROR", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("P_MENSAJE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                    connection.Execute(query, param: parameters, commandType: CommandType.StoredProcedure);
                    result.ErrorCode2 = parameters.Get<int?>("P_COD_ERROR");
                    result.Message2 = parameters.Get<string>("P_MENSAJE");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    connection.Close();
                    throw ex;
                }
            }
            return result;
        }
        public async Task<ResponseTransaction> EndosoOrdinario3Async(EndosoOrdinario endosoOrdinario)
        {
            ResponseTransaction result = new ResponseTransaction();
            using (var connection = _connectionFactory.GetConnection3)
            {
                try
                {
                    var query = "dbo.sp_GenerarEndoso_INS";
                    var parameters = new DynamicParameters();
                    parameters.Add("P_NRODOCUMENTO", endosoOrdinario.NroDocumento);
                    parameters.Add("P_CORREO", endosoOrdinario.Email);
                    parameters.Add("P_CORREO1", endosoOrdinario.Email1);
                    parameters.Add("P_CORREO2", endosoOrdinario.Email2);
                    parameters.Add("P_TELEFONO", endosoOrdinario.celular);
                    parameters.Add("P_TELEFONO1", endosoOrdinario.celular1);
                    parameters.Add("P_TELEFONO2", endosoOrdinario.celular2);
                    parameters.Add("P_DIRECCION", endosoOrdinario.Direccion);
                    parameters.Add("P_PAIS", endosoOrdinario.Pais);
                    parameters.Add("P_DEPARTAMENTO", endosoOrdinario.Departamento);
                    parameters.Add("P_PROVINCIA", endosoOrdinario.Provincia);
                    parameters.Add("P_DISTRITO", endosoOrdinario.Distrito);
                    parameters.Add("P_COD_ERROR", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("P_MENSAJE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                    connection.Execute(query, param: parameters, commandType: CommandType.StoredProcedure);
                    result.ErrorCode3 = parameters.Get<int?>("P_COD_ERROR");
                    result.Message3 = parameters.Get<string>("P_MENSAJE");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    connection.Close();
                    throw ex;
                }

            }
            return result;
        }
        public async Task<ResponseTransaction> EndosoOrdinario4Async(EndosoOrdinario endosoOrdinario)
        {
            ResponseTransaction result = new ResponseTransaction();
            using (var connection = _connectionFactory.GetConnection4)
            {
                try
                {
                    var query = "dbo.S_InsertarEndosos";
                    var parameters = new DynamicParameters();
                    parameters.Add("NRODOCUMENTO", endosoOrdinario.NroDocumento);
                    parameters.Add("EMAIL", endosoOrdinario.Email);
                    parameters.Add("EMAIL1", endosoOrdinario.Email1);
                    parameters.Add("EMAIL2", endosoOrdinario.Email2);
                    parameters.Add("CELULAR", endosoOrdinario.celular);
                    parameters.Add("CELULAR1", endosoOrdinario.celular1);
                    parameters.Add("CELULAR2", endosoOrdinario.celular2);
                    parameters.Add("PAIS", endosoOrdinario.Pais);
                    parameters.Add("DEPARTAMENTO", endosoOrdinario.Departamento);
                    parameters.Add("PROVINCIA", endosoOrdinario.Provincia);
                    parameters.Add("DISTRITO", endosoOrdinario.Distrito);
                    parameters.Add("DIRECCION", endosoOrdinario.Direccion);
                    parameters.Add("P_COD_ERROR", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("P_MENSAJE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                    connection.Execute(query, param: parameters, commandType: CommandType.StoredProcedure);
                    result.ErrorCode4 = parameters.Get<int?>("P_COD_ERROR");
                    result.Message4 = parameters.Get<string>("P_MENSAJE");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    connection.Close();
                    throw ex;
                }

            }
            return result;
        }
        public async Task<IEnumerable<Cliente>> GetCliente(string NroPoliza, string NroDocumento, string Placa, string Nombre1, string ApellidoPaterno)
        {
            List<Cliente> lProducto = new List<Cliente>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);
                parameters.Add("@Placa", Placa);
                parameters.Add("@Nombre1", Nombre1);
                parameters.Add("@ApellidoPaterno", ApellidoPaterno);

                var response = await connection.QueryAsync<Cliente>("SP_AV_CLIENTE_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<Cliente>)response;


            }
            return lProducto;
        }
        #region Soat
        public async Task<IEnumerable<Polizasoat>> GetPolizaSoat(string NroPoliza, string NroDocumento)
        {
            List<Polizasoat> lProducto = new List<Polizasoat>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);


                var response = await connection.QueryAsync<Polizasoat>("SP_AV_SOAT_POLIZA_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<Polizasoat>)response;


            }
            return lProducto;
        }

        public async Task<IEnumerable<VehiculoSoat>> GetVehiculoSoat(string NroPoliza, string NroDocumento)
        {
            List<VehiculoSoat> lProducto = new List<VehiculoSoat>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<VehiculoSoat>("SP_AV_SOAT_VEHICULO_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<VehiculoSoat>)response;


            }
            return lProducto;
        }


        public async Task<IEnumerable<ClienteSoat>> GetClienteSoat(string NroPoliza, string NroDocumento)
        {
            List<ClienteSoat> lProducto = new List<ClienteSoat>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<ClienteSoat>("SP_AV_SOAT_CLIENTE_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<ClienteSoat>)response;


            }
            return lProducto;
        }

        #endregion

        #region Vivemax

        public async Task<IEnumerable<ClienteVivemax>> GetClienteVivemax(string NroPoliza, string NroDocumento)
        {
            List<ClienteVivemax> lProducto = new List<ClienteVivemax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<ClienteVivemax>("SP_AV_VIVEMAX_CLIENTE_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<ClienteVivemax>)response;


            }
            return lProducto;
        }


        public async Task<IEnumerable<ProductoVivemax>> GetProductoVivemax(string NroPoliza, string NroDocumento)
        {
            List<ProductoVivemax> lProducto = new List<ProductoVivemax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<ProductoVivemax>("SP_AV_VIVEMAX_PRODUCTO_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<ProductoVivemax>)response;


            }
            return lProducto;
        }



        public async Task<IEnumerable<PagoVivemax>> GetPagoVivemax(string NroPoliza, string NroDocumento)
        {
            List<PagoVivemax> lProducto = new List<PagoVivemax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<PagoVivemax>("SP_AV_VIVEMAX_PAGO_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<PagoVivemax>)response;


            }
            return lProducto;
        }

        #endregion


        #region Fondomax

        public async Task<IEnumerable<ClienteFondosmax>> GetClienteFondosmax(string NroPoliza, string NroDocumento)
        {
            List<ClienteFondosmax> lProducto = new List<ClienteFondosmax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<ClienteFondosmax>("SP_AV_FONDOSMAX_CLIENTE_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<ClienteFondosmax>)response;


            }
            return lProducto;
        }

        public async Task<IEnumerable<BeneficiarioFondosmax>> GetBeneficiarioFondosmax(string NroPoliza, string NroDocumento)
        {
            List<BeneficiarioFondosmax> lProducto = new List<BeneficiarioFondosmax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<BeneficiarioFondosmax>("SP_AV_FONDOSMAX_BENEFICIARIO_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<BeneficiarioFondosmax>)response;


            }
            return lProducto;
        }


        public async Task<IEnumerable<ProductoFondosmax>> GetProductoFondosmax(string NroPoliza, string NroDocumento)
        {
            List<ProductoFondosmax> lProducto = new List<ProductoFondosmax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<ProductoFondosmax>("SP_AV_FONDOSMAX_PRODUCTO_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<ProductoFondosmax>)response;


            }
            return lProducto;
        }


        public async Task<IEnumerable<PagoFondosmax>> GetPagoFondosmax(string NroPoliza, string NroDocumento)
        {
            List<PagoFondosmax> lProducto = new List<PagoFondosmax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<PagoFondosmax>("SP_AV_FONDOSMAX_PAGO_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<PagoFondosmax>)response;


            }
            return lProducto;
        }

        #endregion


        #region Rentamax


        public async Task<IEnumerable<TitularRentamax>> GetTitularRentamax(string NroPoliza, string NroDocumento)
        {
            List<TitularRentamax> lProducto = new List<TitularRentamax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<TitularRentamax>("SP_AV_RENTAMAX_TITULAR_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<TitularRentamax>)response;


            }
            return lProducto;
        }

        public async Task<IEnumerable<AseguradoRentamax>> GetAseguradoRentamax(string NroPoliza, string NroDocumento)
        {
            List<AseguradoRentamax> lProducto = new List<AseguradoRentamax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<AseguradoRentamax>("SP_AV_RENTAPRIVADA_RENTAMAX_ASEGURADO", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<AseguradoRentamax>)response;


            }
            return lProducto;
        }

        public async Task<IEnumerable<BeneficiarioRentamax>> GetBeneficiarioRentamax(string NroPoliza, string NroDocumento)
        {
            List<BeneficiarioRentamax> lProducto = new List<BeneficiarioRentamax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<BeneficiarioRentamax>("SP_AV_RENTAPRIVADA_RENTAMAX_BENEFICIARIO", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<BeneficiarioRentamax>)response;


            }
            return lProducto;
        }

        public async Task<IEnumerable<ProductoRentamax>> GetProductoRentamax(string NroPoliza, string NroDocumento)
        {
            List<ProductoRentamax> lProducto = new List<ProductoRentamax>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<ProductoRentamax>("SP_AV_RENTAPRIVADA_RENTAMAX_PRODUCTO", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<ProductoRentamax>)response;


            }
            return lProducto;
        }

        #endregion


        #region RentaVitalicia


        public async Task<IEnumerable<TitularRentaVitalicia>> GetTitularRentaVitalicia(string NroPoliza, string NroDocumento)
        {
            List<TitularRentaVitalicia> lProducto = new List<TitularRentaVitalicia>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<TitularRentaVitalicia>("SP_AV_SEGURORV_RENTAVITALICIA_TITULAR", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<TitularRentaVitalicia>)response;


            }
            return lProducto;
        }





        public async Task<IEnumerable<ProductoRentaVitalicia>> GetProductoRentaVitalicia(string NroPoliza, string NroDocumento)
        {
            List<ProductoRentaVitalicia> lProducto = new List<ProductoRentaVitalicia>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<ProductoRentaVitalicia>("SP_AV_SEGURORV_RENTAVITALICIA_PRODUCTO", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<ProductoRentaVitalicia>)response;


            }
            return lProducto;
        }



        public async Task<IEnumerable<PagoRentaVitalicia>> GetPagoRentaVitalicia(string NroPoliza, string NroDocumento)
        {
            List<PagoRentaVitalicia> lProducto = new List<PagoRentaVitalicia>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<PagoRentaVitalicia>("SP_AV_SEGURORV_RENTAVITALICIA_PAGO_SEL", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<PagoRentaVitalicia>)response;


            }
            return lProducto;
        }


        public async Task<IEnumerable<BeneficiarioRentaVitalicia>> GetBeneficiarioRentaVitalicia(string NroPoliza, string NroDocumento)
        {
            List<BeneficiarioRentaVitalicia> lProducto = new List<BeneficiarioRentaVitalicia>();
            using (var connection = _connectionFactory.GetConnection5)
            {
                Log.save(this, "Ingresa a Obtener Productos");
                var parameters = new DynamicParameters();
                parameters.Add("@NroPoliza", NroPoliza);
                parameters.Add("@NroDocumento", NroDocumento);



                var response = await connection.QueryAsync<BeneficiarioRentaVitalicia>("SP_AV_SEGURORV_RENTAVITALICIA_BENEFICIARIO", param: parameters, commandType: CommandType.StoredProcedure);
                lProducto = (List<BeneficiarioRentaVitalicia>)response;


            }
            return lProducto;
        }
        #endregion



    }
}
