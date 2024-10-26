using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiReportes.Context;
using ApiReportes.Models;
using ApiReportes.Services;
using Microsoft.Reporting.NETCore;

namespace ApiReportes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
       
        //inyeccion del servicio cliente

        private readonly IClienteService _clienteService;
       // private readonly ClienteService clienteService ;


        public ClientesController(IClienteService clienteService )
        {
            _clienteService = clienteService;
                       

        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            //llamado al servicio para listar los clientes
            var clientes = await _clienteService.GetClientes();
            return Ok(clientes);
            
        }

        //endpoint para generar el reporte

        [HttpGet("reporte")]
        public async Task<IActionResult> GenerarReporteClientes(string format = "PDF", string extension = "pdf")
        {
            //obtengo los clientes de la base de datos
            var clientes = await _clienteService.GetClientes();

            //creo el reporte
           using var report = new LocalReport();

            //se carga el reporte
            ClienteService.Load(report, clientes);

            //renderizo el reporte como pdf
            var result = report.Render("PDF", null, out _, out _, out _, out _, out _);

            //retorno el reporte
            return File(result, "application/pdf", "CargosReport.pdf");

        }

        //endpoint para generar el reporte de las ordenes de los clientes
        [HttpGet("reporteOrdenes")]
        public async Task<IActionResult> GetReporteOrdenesClientes()
        {
            var pdf = await _clienteService.GetReporteOrdenesClientes();
            return File(pdf, "application/pdf", "ReporteOrdenesClientes.pdf");
        }


    }
}
