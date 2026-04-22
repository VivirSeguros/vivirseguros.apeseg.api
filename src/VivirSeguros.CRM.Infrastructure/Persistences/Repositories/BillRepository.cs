using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Entities.Billing;
using VivirSeguros.CRM.Infrastructure.Persistences.Contexts;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public BillRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<BillFactBase> GetAsync(string skey)
        {
            BillFactBase response = new BillFactBase();
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "dbo.sp_TrabajoDatos_SEL";
                var parameters = new DynamicParameters();
                parameters.Add("P_SKEY", skey);

                using (var dr = connection.ExecuteReader(query, param: parameters, commandType: CommandType.StoredProcedure))//<Bill_Fact>(query, param: parameters, commandType: CommandType.StoredProcedure);
                {
                    while (dr.Read())
                    {
                        if (dr["idTipoComprobante"].ToString() == "2")
                        {
                            response.tipoComprobante = dr["idTipoComprobante"] == DBNull.Value ? "" : dr["idTipoComprobante"].ToString();
                            response.idProducto = dr["idProducto"] == DBNull.Value ? 0 : int.Parse(dr["idProducto"].ToString());
                            response.boleta = new BoletaBase();
                            response.boleta.IDE = new IdentificadorDocumento();
                            response.boleta.IDE.numeracion = dr["Numeracion"].ToString();
                            response.boleta.IDE.fechaEmision = dr["FechaEmision"].ToString();
                            response.boleta.IDE.codTipoDocumento = dr["CodTipoDocumento"].ToString();
                            response.boleta.IDE.tipoMoneda = dr["TipoMoneda"].ToString();
                            response.boleta.IDE.fechaVencimiento = dr["FechaVencimiento"] == DBNull.Value ? "" : dr["FechaVencimiento"].ToString();

                            response.boleta.EMI = new EmisorElectronico();
                            response.boleta.EMI.tipoDocId = dr["TipoDocIdEmi"] == DBNull.Value ? "" : dr["TipoDocIdEmi"].ToString();
                            response.boleta.EMI.numeroDocId = dr["NroDocEmi"] == DBNull.Value ? "" : dr["NroDocEmi"].ToString();
                            response.boleta.EMI.nombreComercial = dr["NombreComercialEmi"] == DBNull.Value ? "" : dr["NombreComercialEmi"].ToString();
                            response.boleta.EMI.razonSocial = dr["RazonSocialEmi"] == DBNull.Value ? "" : dr["RazonSocialEmi"].ToString();
                            response.boleta.EMI.ubigeo = dr["UbigeoEmi"] == DBNull.Value ? "" : dr["UbigeoEmi"].ToString();
                            response.boleta.EMI.direccion = dr["DireccionEmi"] == DBNull.Value ? "" : dr["DireccionEmi"].ToString();
                            response.boleta.EMI.codigoPais = dr["CodigoPaisEmi"] == DBNull.Value ? "" : dr["CodigoPaisEmi"].ToString();
                            response.boleta.EMI.telefono = dr["TelefonoEmi"] == DBNull.Value ? "" : dr["TelefonoEmi"].ToString();
                            response.boleta.EMI.correoElectronico = dr["CorreoEmi"] == DBNull.Value ? "" : dr["CorreoEmi"].ToString();
                            response.boleta.EMI.codigoAsigSUNAT = dr["CodigoSunatEmi"] == DBNull.Value ? "" : dr["CodigoSunatEmi"].ToString();

                            response.boleta.REC = new ReceptorElectronico();
                            response.boleta.REC.tipoDocId = dr["TipoDocIdRec"] == DBNull.Value ? "" : dr["TipoDocIdRec"].ToString();
                            response.boleta.REC.numeroDocId = dr["NumeroDocIdRec"] == DBNull.Value ? "" : dr["NumeroDocIdRec"].ToString();
                            response.boleta.REC.razonSocial = dr["NombreCliente"] == DBNull.Value ? "" : dr["NombreCliente"].ToString();
                            response.boleta.REC.direccion = dr["DireccionRec"] == DBNull.Value ? "" : dr["DireccionRec"].ToString();
                            response.boleta.REC.correoElectronico = dr["EmailRec"] == DBNull.Value ? "" : dr["EmailRec"].ToString();

                            response.boleta.CAB = new CabeceraBase();
                            response.boleta.CAB.gravadas = new Gravadas();
                            response.boleta.CAB.gravadas.codigo = dr["CodOperacion"] == DBNull.Value ? "" : dr["CodOperacion"].ToString();
                            response.boleta.CAB.gravadas.totalVentas = dr["PrimaNeta"] == DBNull.Value ? "" : dr["PrimaNeta"].ToString();
                            response.boleta.CAB.totalImpuestos = new List<TotalImpuesto>();
                            TotalImpuesto impuestoCab = new TotalImpuesto();
                            impuestoCab.idImpuesto = dr["CodImpuesto"] == DBNull.Value ? "" : dr["CodImpuesto"].ToString();
                            impuestoCab.montoImpuesto = dr["Igv"] == DBNull.Value ? "" : dr["Igv"].ToString();
                            response.boleta.CAB.totalImpuestos.Add(impuestoCab);
                            response.boleta.CAB.importeTotal = dr["Prima"] == DBNull.Value ? "" : dr["Prima"].ToString();
                            response.boleta.CAB.tipoOperacion = dr["TipoOperacion"] == DBNull.Value ? "" : dr["TipoOperacion"].ToString();
                            response.boleta.CAB.leyenda = new Leyenda();
                            response.boleta.CAB.leyenda.codigo = dr["CodLeyenda"] == DBNull.Value ? "" : dr["CodLeyenda"].ToString();
                            response.boleta.CAB.leyenda.descripcion = dr["DescripcionLeyenda"] == DBNull.Value ? "" : dr["DescripcionLeyenda"].ToString();
                            response.boleta.CAB.montoTotalImpuestos = dr["Igv"] == DBNull.Value ? "" : dr["Igv"].ToString();

                            Detalle detalle = new Detalle();
                            TotalImpuestoDetalle impuesto = new TotalImpuestoDetalle();
                            detalle.numeroItem = dr["NumeroItem"] == DBNull.Value ? "" : dr["NumeroItem"].ToString();
                            detalle.codigoProducto = dr["NombreProducto"] == DBNull.Value ? "" : dr["NombreProducto"].ToString();
                            detalle.descripcionProducto = dr["NombreProducto"] == DBNull.Value ? "" : dr["NombreProducto"].ToString();
                            detalle.cantidadItems = dr["CantidadItems"] == DBNull.Value ? "" : dr["CantidadItems"].ToString();
                            detalle.unidad = dr["Unidad"] == DBNull.Value ? "" : dr["Unidad"].ToString();
                            detalle.valorUnitario = dr["PrimaNeta"] == DBNull.Value ? "" : dr["PrimaNeta"].ToString();
                            detalle.precioVentaUnitario = dr["Prima"] == DBNull.Value ? "" : dr["Prima"].ToString();

                            impuesto.idImpuesto = dr["CodImpuesto"] == DBNull.Value ? "" : dr["CodImpuesto"].ToString();
                            impuesto.montoImpuesto = dr["Igv"] == DBNull.Value ? "" : dr["Igv"].ToString();
                            impuesto.tipoAfectacion = dr["TipoAfectacion"] == DBNull.Value ? "" : dr["TipoAfectacion"].ToString();
                            impuesto.montoBase = dr["PrimaNeta"] == DBNull.Value ? "" : dr["PrimaNeta"].ToString();
                            impuesto.porcentaje = dr["PorcentajeIGV"] == DBNull.Value ? "" : dr["PorcentajeIGV"].ToString();
                            detalle.valorVenta = dr["PrimaNeta"] == DBNull.Value ? "" : dr["PrimaNeta"].ToString();
                            detalle.codProductoSunat = dr["CodigoProductoSunat"] == DBNull.Value ? "" : dr["CodigoProductoSunat"].ToString();
                            detalle.montoTotalImpuestos = dr["Igv"] == DBNull.Value ? "" : dr["Igv"].ToString();
                            detalle.totalImpuestos = new List<TotalImpuestoDetalle>();
                            detalle.totalImpuestos.Add(impuesto);

                            response.boleta.DET = new List<Detalle>();
                            response.boleta.DET.Add(detalle);

                            response.boleta.ADI = new List<CampoAdicional>();
                            CampoAdicional campo = new CampoAdicional();
                            campo.tituloAdicional = dr["TituloAdicional"] == DBNull.Value ? "" : dr["TituloAdicional"].ToString();
                            campo.valorAdicional = dr["ValorAdicional"] == DBNull.Value ? "" : dr["ValorAdicional"].ToString();

                            response.idEstado = dr["idEstado"] == DBNull.Value ? 0 : int.Parse(dr["idEstado"].ToString());
                            response.boleta.ADI.Add(campo);
                        }
                        else
                        {
                            response.tipoComprobante = dr["idTipoComprobante"] == DBNull.Value ? "" : dr["idTipoComprobante"].ToString();
                            response.idProducto = dr["idProducto"] == DBNull.Value ? 0 : int.Parse(dr["idProducto"].ToString());
                            response.factura = new FacturaBase();
                            response.factura.IDE = new IdentificadorDocumento();
                            response.factura.IDE.numeracion = dr["Numeracion"].ToString();
                            response.factura.IDE.fechaEmision = dr["FechaEmision"].ToString();
                            response.factura.IDE.codTipoDocumento = dr["CodTipoDocumento"].ToString();
                            response.factura.IDE.tipoMoneda = dr["TipoMoneda"].ToString();
                            response.factura.IDE.fechaVencimiento = dr["FechaVencimiento"] == DBNull.Value ? "" : dr["FechaVencimiento"].ToString();

                            response.factura.EMI = new EmisorElectronico();
                            response.factura.EMI.tipoDocId = dr["TipoDocIdEmi"] == DBNull.Value ? "" : dr["TipoDocIdEmi"].ToString();
                            response.factura.EMI.numeroDocId = dr["NroDocEmi"] == DBNull.Value ? "" : dr["NroDocEmi"].ToString();
                            response.factura.EMI.nombreComercial = dr["NombreComercialEmi"] == DBNull.Value ? "" : dr["NombreComercialEmi"].ToString();
                            response.factura.EMI.razonSocial = dr["RazonSocialEmi"] == DBNull.Value ? "" : dr["RazonSocialEmi"].ToString();
                            response.factura.EMI.ubigeo = dr["UbigeoEmi"] == DBNull.Value ? "" : dr["UbigeoEmi"].ToString();
                            response.factura.EMI.direccion = dr["DireccionEmi"] == DBNull.Value ? "" : dr["DireccionEmi"].ToString();
                            response.factura.EMI.codigoPais = dr["CodigoPaisEmi"] == DBNull.Value ? "" : dr["CodigoPaisEmi"].ToString();
                            response.factura.EMI.telefono = dr["TelefonoEmi"] == DBNull.Value ? "" : dr["TelefonoEmi"].ToString();
                            response.factura.EMI.correoElectronico = dr["CorreoEmi"] == DBNull.Value ? "" : dr["CorreoEmi"].ToString();
                            response.factura.EMI.codigoAsigSUNAT = dr["CodigoSunatEmi"] == DBNull.Value ? "" : dr["CodigoSunatEmi"].ToString();

                            response.factura.REC = new ReceptorElectronico();
                            response.factura.REC.tipoDocId = dr["TipoDocIdRec"] == DBNull.Value ? "" : dr["TipoDocIdRec"].ToString();
                            response.factura.REC.numeroDocId = dr["NumeroDocIdRec"] == DBNull.Value ? "" : dr["NumeroDocIdRec"].ToString();
                            response.factura.REC.razonSocial = dr["NombreCliente"] == DBNull.Value ? "" : dr["NombreCliente"].ToString();
                            response.factura.REC.direccion = dr["DireccionRec"] == DBNull.Value ? "" : dr["DireccionRec"].ToString();
                            response.factura.REC.correoElectronico = dr["EmailRec"] == DBNull.Value ? "" : dr["EmailRec"].ToString();

                            response.factura.CAB = new CabeceraBase();
                            response.factura.CAB.gravadas = new Gravadas();
                            response.factura.CAB.gravadas.codigo = dr["CodOperacion"] == DBNull.Value ? "" : dr["CodOperacion"].ToString();
                            response.factura.CAB.gravadas.totalVentas = dr["PrimaNeta"] == DBNull.Value ? "" : dr["PrimaNeta"].ToString();
                            response.factura.CAB.totalImpuestos = new List<TotalImpuesto>();
                            TotalImpuesto impuestoCab = new TotalImpuesto();
                            impuestoCab.idImpuesto = dr["CodImpuesto"] == DBNull.Value ? "" : dr["CodImpuesto"].ToString();
                            impuestoCab.montoImpuesto = dr["Igv"] == DBNull.Value ? "" : dr["Igv"].ToString();
                            response.factura.CAB.totalImpuestos.Add(impuestoCab);
                            response.factura.CAB.importeTotal = dr["Prima"] == DBNull.Value ? "" : dr["Prima"].ToString();
                            response.factura.CAB.tipoOperacion = dr["TipoOperacion"] == DBNull.Value ? "" : dr["TipoOperacion"].ToString();
                            response.factura.CAB.leyenda = new Leyenda();
                            response.factura.CAB.leyenda.codigo = dr["CodLeyenda"] == DBNull.Value ? "" : dr["CodLeyenda"].ToString();
                            response.factura.CAB.leyenda.descripcion = dr["DescripcionLeyenda"] == DBNull.Value ? "" : dr["DescripcionLeyenda"].ToString();
                            response.factura.CAB.montoTotalImpuestos = dr["Igv"] == DBNull.Value ? "" : dr["Igv"].ToString();

                            Detalle detalle = new Detalle();
                            TotalImpuestoDetalle impuesto = new TotalImpuestoDetalle();
                            detalle.numeroItem = dr["NumeroItem"] == DBNull.Value ? "" : dr["NumeroItem"].ToString();
                            detalle.codigoProducto = dr["NombreProducto"] == DBNull.Value ? "" : dr["NombreProducto"].ToString();
                            detalle.descripcionProducto = dr["NombreProducto"] == DBNull.Value ? "" : dr["NombreProducto"].ToString();
                            detalle.cantidadItems = dr["CantidadItems"] == DBNull.Value ? "" : dr["CantidadItems"].ToString();
                            detalle.unidad = dr["Unidad"] == DBNull.Value ? "" : dr["Unidad"].ToString();
                            detalle.valorUnitario = dr["PrimaNeta"] == DBNull.Value ? "" : dr["PrimaNeta"].ToString();
                            detalle.precioVentaUnitario = dr["Prima"] == DBNull.Value ? "" : dr["Prima"].ToString();

                            impuesto.idImpuesto = dr["CodImpuesto"] == DBNull.Value ? "" : dr["CodImpuesto"].ToString();
                            impuesto.montoImpuesto = dr["Igv"] == DBNull.Value ? "" : dr["Igv"].ToString();
                            impuesto.tipoAfectacion = dr["TipoAfectacion"] == DBNull.Value ? "" : dr["TipoAfectacion"].ToString();
                            impuesto.montoBase = dr["PrimaNeta"] == DBNull.Value ? "" : dr["PrimaNeta"].ToString();
                            impuesto.porcentaje = dr["PorcentajeIGV"] == DBNull.Value ? "" : dr["PorcentajeIGV"].ToString();
                            detalle.valorVenta = dr["PrimaNeta"] == DBNull.Value ? "" : dr["PrimaNeta"].ToString();
                            detalle.codProductoSunat = dr["CodigoProductoSunat"] == DBNull.Value ? "" : dr["CodigoProductoSunat"].ToString();
                            detalle.montoTotalImpuestos = dr["Igv"] == DBNull.Value ? "" : dr["Igv"].ToString();
                            detalle.totalImpuestos = new List<TotalImpuestoDetalle>();
                            detalle.totalImpuestos.Add(impuesto);

                            response.factura.DET = new List<Detalle>();
                            response.factura.DET.Add(detalle);

                            response.factura.ADI = new List<CampoAdicional>();
                            CampoAdicional campo = new CampoAdicional();
                            campo.tituloAdicional = dr["TituloAdicional"] == DBNull.Value ? "" : dr["TituloAdicional"].ToString();
                            campo.valorAdicional = dr["ValorAdicional"] == DBNull.Value ? "" : dr["ValorAdicional"].ToString();
                            response.factura.ADI.Add(campo);
                            campo.tituloAdicional = dr["TituloAdicionalFac"] == DBNull.Value ? "" : dr["TituloAdicionalFac"].ToString();
                            campo.valorAdicional = dr["TituloAdicionalFac"] == DBNull.Value ? "" : dr["TituloAdicionalFac"].ToString();

                            response.idEstado = dr["idEstado"] == DBNull.Value ? 0 : int.Parse(dr["idEstado"].ToString());
                            response.factura.ADI.Add(campo);
                        }
                    }
                }   
            }

            return Task.FromResult(response);
        }
    }
}
