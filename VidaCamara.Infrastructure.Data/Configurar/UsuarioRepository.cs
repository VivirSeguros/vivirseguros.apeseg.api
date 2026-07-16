using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Configurar;
using VidaCamara.Domain.Services.Repositorios.Configurar;
using VidaCamara.Infrastructure.Connection;

namespace VidaCamara.Infrastructure.Data.Configurar
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public UsuarioRepository(IOptions<AppSettings> appSettings,
                                    IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }


        public Task<IEnumerable<Usuario>> GetUsuario(int param)
        {
            IEnumerable<Usuario> result = null;
            List<Usuario> lUsuario = new List<Usuario>();
            List<SqlParameter> parameters = new List<SqlParameter> {
               new SqlParameter("@p_idUsuario", param)
            };
            Log.save(this, "EMPIEZA METODO GetUsuario PASE PARAMETROS A PROC dbo.sp_Usuario_SEL");
            try
            {
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_Usuario_SEL",
                                       parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    while (dr.Read())
                    {
                        var oUsuario = new Usuario
                        {
                            idUsuario = dr["idUsuario"] == DBNull.Value ? 0 : dr.GetInt32(dr.GetOrdinal("idUsuario")),
                            idPersona = dr["idPersona"] == DBNull.Value ? 0 : dr.GetInt32(dr.GetOrdinal("idPersona")),
                            idProducto = dr["idProducto"] == DBNull.Value ? 0 : dr.GetInt32(dr.GetOrdinal("idProducto")),
                            username = dr["Username"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Username")),
                            password = dr["Password"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Password")),
                            idTipoPerfil = dr["idTipoPerfil"] == DBNull.Value ? 0 : dr.GetInt32(dr.GetOrdinal("idTipoPerfil")),
                            idPuntoVenta = dr["idPuntoVenta"] == DBNull.Value ? 0 : dr.GetInt32(dr.GetOrdinal("idPuntoVenta")),
                            estado = dr["Estado"] == DBNull.Value ? 0 : dr.GetBoolean(dr.GetOrdinal("Estado")) ? 1 : 0,
                            idTipoPersona = dr["idTipoPersona"] == DBNull.Value ? 0 : dr.GetInt32(dr.GetOrdinal("idTipoPersona")),
                            idTipoDocumento = dr["idTipoDocumento"] == DBNull.Value ? 0 : dr.GetInt32(dr.GetOrdinal("idTipoDocumento")),
                            nroDoc = dr["NroDocumento"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("NroDocumento")),
                            nombre1 = dr["Nombre1"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Nombre1")),
                            nombre2 = dr["Nombre2"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Nombre2")),
                            apPaterno = dr["ApPaterno"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("ApPaterno")),
                            apMaterno = dr["ApMaterno"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("ApMaterno")),
                            razonSocial = dr["RazonSocial"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("RazonSocial")),
                            direccion = dr["Direccion"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Direccion")),
                            idFono = dr["idFono"] == DBNull.Value ? 0 : dr.GetInt32(dr.GetOrdinal("idFono")),
                            idDireccion = dr["idDireccion"] == DBNull.Value ? 0: dr.GetInt32(dr.GetOrdinal("idDireccion")),
                            idDepartamento = dr["idDepartamento"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("idDepartamento")),
                            idProvincia = dr["idProvincia"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("idProvincia")),
                            idDistrito = dr["idDistrito"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("idDistrito")),
                            fijo = dr["Fijo"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Fijo")),
                            celular = dr["Celular"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Celular")),
                            email = dr["Email"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Email")),
                            producto = dr["Descripcion"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Descripcion")),
                            usuarioCreacion = dr["UsuarioCreacion"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("UsuarioCreacion")),
                            fechaRegistro = dr["FechaRegistro"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("FechaRegistro")),
                            desNombre = dr["NombreCliente"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("NombreCliente")),
                            desPerfil = dr["Perfil"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Perfil")),
                            activeDirectory = dr["ActiveDirectory"] == DBNull.Value ? false : dr.GetBoolean(dr.GetOrdinal("ActiveDirectory")),
                        };
                        lUsuario.Add(oUsuario);
                    };
                    result = lUsuario as IEnumerable<Usuario>;
                }
                Log.save(this, "TERMINA METODO GetUsuario PASE PARAMETROS A PROC dbo.sp_Usuario_SEL");
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }
            
            return Task.FromResult<IEnumerable<Usuario>>(result);
        }

        public Task<Usuario> InsUsuario(Usuario param)
        {
            Object idPersona = -1;
            // Persona
            Log.save(this, "EMPIEZA METODO InsUsuario PASE PARAMETROS A PROC dbo.sp_Persona_INS");
            List<SqlParameter> parameters = new List<SqlParameter> {
               new SqlParameter("@p_idPersona", param.idPersona),
               new SqlParameter("@p_idTipoPersona", param.idTipoPersona),
               new SqlParameter("@p_idTipoDoc", param.idTipoDocumento),
               new SqlParameter("@p_idProducto", param.idProducto),
               new SqlParameter("@p_nroDoc", param.nroDoc),
               new SqlParameter("@p_nombre1", param.nombre1),
               new SqlParameter("@p_nombre2", param.nombre2),
               new SqlParameter("@p_apPat", param.apPaterno),
               new SqlParameter("@p_apMat", param.apMaterno),
               new SqlParameter("@p_RazSocial", param.razonSocial),
               new SqlParameter("@p_Usuario", param.user),
               //new SqlParameter("@p_idPersonaId", SqlDbType.Int) {Direction = ParameterDirection.ReturnValue}
            };
            SqlParameter P_idPersonaId = new SqlParameter("@p_idPersonaId", value: param.idPersona);
            P_idPersonaId.Direction = ParameterDirection.Output;
            parameters.Add(P_idPersonaId);
            try  {
                using (_connectionBase.ExecuteByStoredProcedure("sp_Persona_INS", parameters, ConnectionBase.enuTypeDataBase.sqlCon)) {};
                idPersona = int.Parse(P_idPersonaId.Value.ToString());
            }
            catch (Exception ex) {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
                param.result = 0;
            }
            Log.save(this, "TERMINA PASE PARAMETROS A PROC dbo.sp_Persona_INS");
            // Telefono
            Log.save(this, "EMPIEZA PASE PARAMETROS A PROC dbo.sp_Telefono_INS");
            List<SqlParameter> parameter2 = new List<SqlParameter> {
               new SqlParameter("@p_idFono", param.idFono),
               new SqlParameter("@p_idPersona", idPersona),
               new SqlParameter("@p_Fijo", param.fijo),
               new SqlParameter("@p_Celular", param.celular),
               new SqlParameter("@p_Email", param.email),
               new SqlParameter("@p_Email2", param.email2),
               new SqlParameter("@p_Usuario", param.user)
            };
            SqlParameter p_telefonoId = new SqlParameter("@p_telefonoId", value: param.idFono);
            p_telefonoId.Direction = ParameterDirection.Output;
            parameter2.Add(p_telefonoId);
            try  {
                using (_connectionBase.ExecuteByStoredProcedure("sp_Telefono_INS", parameter2, ConnectionBase.enuTypeDataBase.sqlCon)) {};
                param.idFono = int.Parse(p_telefonoId.Value.ToString());
            }
            catch (Exception ex)  {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
                param.result = 0;
            }
            Log.save(this, "TERMINA PASE PARAMETROS A PROC dbo.sp_Telefono_INS");
            // Direccion
            Log.save(this, "EMPIEZA PASE PARAMETROS A PROC dbo.sp_Direccion_INS");
            List<SqlParameter> parameter3 = new List<SqlParameter> {
               new SqlParameter("@p_iddireccion", param.idDireccion),
               new SqlParameter("@p_idPersona", idPersona),
               new SqlParameter("@p_Direccion", param.direccion),
               new SqlParameter("@p_idDepartamento", param.idDepartamento),
               new SqlParameter("@p_idProvincia", param.idProvincia),
               new SqlParameter("@p_idDistrito", param.idDistrito),
               new SqlParameter("@p_Usuario", param.user)
            };
            SqlParameter p_direccionId = new SqlParameter("@p_direccionId", value: param.idDireccion);
            p_direccionId.Direction = ParameterDirection.Output;
            parameter3.Add(p_direccionId);
            try  {
                using (_connectionBase.ExecuteByStoredProcedure("sp_Direccion_INS", parameter3, ConnectionBase.enuTypeDataBase.sqlCon)) {};
                param.idDireccion = int.Parse(p_direccionId.Value.ToString());
            }
            catch (Exception ex)   {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
                param.result = 0;
            }
            Log.save(this, "TERMINA PASE PARAMETROS A PROC dbo.sp_Direccion_INS");
            // Usuario
            Log.save(this, "EMPIEZA PASE PARAMETROS A PROC dbo.sp_Usuario_INS");
            List<SqlParameter> parameter4 = new List<SqlParameter> {
               new SqlParameter("@p_idUsuario", param.idUsuario),
               new SqlParameter("@p_idPersona", idPersona),
               new SqlParameter("@p_Username", param.username),
               new SqlParameter("@p_Password", param.password),
               new SqlParameter("@p_idTipoPerfil", param.idTipoPerfil),
               new SqlParameter("@p_idPuntoVenta", param.idPuntoVenta),
               new SqlParameter("@p_Estado", param.estado),
               new SqlParameter("@p_Usuario", param.user),
               new SqlParameter("@p_activeDirectory", param.activeDirectory),
            };
            try  {
                using (_connectionBase.ExecuteByStoredProcedure("sp_Usuario_INS", parameter4, ConnectionBase.enuTypeDataBase.sqlCon)) {} ;
                
                param.result = 1;
            }
            catch (Exception ex) {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
                param.result = 0;
            }
            Log.save(this, "TERMINA METODO InsUsuario PASE PARAMETROS A PROC dbo.sp_Usuario_INS");
            return Task.FromResult<Usuario>(param);
        }

        public Task<bool> ValidateExisteUsuario(string userName)
        {
            bool response = false;

            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                Log.save(this, "EMPIEZA METODO ValidateExisteUsuario FUNC soat.fnc_Usuario_ValidateUserName");
                response = (bool)_connectionBase.ExecuteScalarSqlFunction("soat.fnc_Usuario_ValidateUserName", new string[] { userName });
                Log.save(this, "TERMINA METODO ValidateExisteUsuario FUNC soat.fnc_Usuario_ValidateUserName");
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            return Task.FromResult<bool>(response);
        }
    }
}
