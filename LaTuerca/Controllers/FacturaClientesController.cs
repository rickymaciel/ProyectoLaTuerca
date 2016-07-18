using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaTuerca.Models;
using Microsoft.AspNet.Identity;
using RazorPDF;

namespace LaTuerca.Controllers
{
    public class FacturaClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FacturaClientes
        public ActionResult Index()
        {
            var facturaClientes = db.FacturaClientes.Include(f => f.Cliente);
            return View(facturaClientes.ToList());
        }

        public ActionResult InformeVentas()
        {
            return View(db.FacturaClientes.ToList().Where(w => w.Pagado == true && w.TotalPagado > 0));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfimarPago(int? id)
        {
            //ViewBag.ClienteId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            if (ModelState.IsValid)
            {
                FacturaCliente f = db.FacturaClientes.Find(id);
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        ActualizarFactura(id);
                        ActualizarStock(f);
                        ActualizarCaja(f);
                        GenerarMovimiento(id);
                        dbTran.Commit();
                        TempData["notice"] = "La Factura Número: " + f.NumeroFactura + " fue guardada correctamente!";
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        TempData["notice"] = "No se pudo realizar la transacción!" + ex.Message;
                        return RedirectToAction("Factura");
                    }

                }
                return RedirectToAction("Details/" + ObtenerIdMax());
            }
            TempData["notice"] = "Error";
            return RedirectToAction("Factura");
        }
        public void ActualizarFactura(int? id)
        {
            using (var context = new ApplicationDbContext())
            {
                string fecha = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime FechadePago = DateTime.ParseExact(fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                FacturaCliente factura = db.FacturaClientes.Find(id);
                var newfactura = new FacturaCliente
                {
                    Id = factura.Id,
                    ClienteId = factura.ClienteId,
                    Fecha = factura.Fecha,
                    FechaPago = FechadePago,
                    Metodo = "Contado",
                    NumeroFactura = factura.NumeroFactura,
                    Pagado = true,
                    Total = factura.Total,
                    TotalPagado = factura.Total
                };
                context.FacturaClientes.Add(newfactura);
                context.Entry(newfactura).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void GenerarMovimiento(int? id)
        {
            FacturaCliente factura = db.FacturaClientes.Find(id);
            using (var context = new ApplicationDbContext())
            {
                var movimiento = new MovimientoCaja
                {
                    Fecha = DateTime.Now,
                    CajaId = ObtenerUltimoCajaAbierto(),
                    Concepto = "Factura Venta Nº: " + factura.NumeroFactura,
                    Movimiento = "Entrada",
                    Ingreso = factura.Total,
                    Egreso = 0,
                    Saldo = (int)ObtenerSaldoUltimoCajaAbierto()
                };
                context.MovimientoCajas.Add(movimiento);
                context.SaveChanges();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResumenVenta(DateTime from, DateTime to)
        {
            var fromDate = from;
            var toDate = to;

            var resumen = db.FacturaClientes.Where(x => x.Pagado == true).Where(x => x.Fecha >= fromDate).Where(x => x.Fecha <= toDate).ToList().OrderBy(c => c.Fecha);

            return new PdfResult(resumen, "ResumenVentas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public object ResumenVentas(DateTime from, DateTime to)
        {
            var fromDate = from;
            var toDate = to;
            var resumen = db.FacturaClientes.Where(x => x.Pagado == true).Where(x => x.Fecha >= fromDate).Where(x => x.Fecha <= toDate).ToList().OrderBy(c => c.Fecha);
            var pdf = new RazorPDF.PdfResult(resumen, "ResumenVentas");
            pdf.ViewBag.Title = "Titulo del Reporte";
            pdf.ViewBag.Desde = from;
            pdf.ViewBag.Hasta = to;
            return pdf;
        }

        public ActionResult PrintFactura(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaCliente venta = db.FacturaClientes.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }

            var detalleVenta = from c in db.DetallesFacturaClientes where c.FacturaClienteId == venta.Id select c;

            return new RazorPDF.PdfResult(detalleVenta, "PrintFactura");

        }


        // GET: FacturaProveedors/Edit/5
        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaCliente facturaClientes = db.FacturaClientes.Find(id);
            if (facturaClientes == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaClientes.ClienteId);
            return View(facturaClientes);
        }


        // GET: FacturaProveedors/Edit/5
        public ActionResult Pagar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaCliente facturaCliente = db.FacturaClientes.Find(id);
            if (facturaCliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", facturaCliente.ClienteId);
            return View(facturaCliente);
        }
        // GET: FacturaProveedors
        public ActionResult Presupuestos()
        {
            var facturaClientes = db.FacturaClientes.Include(f => f.Cliente);
            return View(facturaClientes.ToList().Where(w => w.Pagado == false && w.TotalPagado < 1));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pagar([Bind(Include = "Id,Fecha,FechaPago,NumeroFactura,ClienteId,Total,TotalPagado,Metodo,Pagado")] FacturaCliente facturaCliente)
        {
            ViewBag.ClienteId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaCliente.ClienteId);
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        GuardarFacturaDetalles(facturaCliente);
                        ActualizarStock(facturaCliente);
                        ActualizarCaja(facturaCliente);
                        GuardarMovimiento(facturaCliente);
                        db.Entry(facturaCliente).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["notice"] = "La factura Número: " + facturaCliente.NumeroFactura + " fue generada correctamente! ";
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        TempData["notice"] = "No se pudo realizar la transacción! " + ex.Message;
                        return RedirectToAction("Factura");
                    }

                }
                return RedirectToAction("Details/" + facturaCliente.Id);
            }
            TempData["notice"] = "Todos los campos son requeridos!";
            return View(facturaCliente);

        }

        // GET: FacturaProveedors
        public ActionResult Facturados()
        {
            var facturaClientes = db.FacturaClientes.Include(f => f.Cliente);
            return View(facturaClientes.ToList().Where(w => w.Pagado == true && w.TotalPagado > 0));
        }

        // GET: FacturaProveedors
        public ActionResult Pagados()
        {
            var facturaClientes = db.FacturaClientes.Include(f => f.Cliente);
            return View(facturaClientes.ToList().Where(w => w.Pagado == true));
        }


        // GET: Factura/Transaction
        public ActionResult Factura()
        {
            var facturaCliente = new FacturaCliente();

            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime Fecha = DateTime.ParseExact(fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            facturaCliente.Fecha = Fecha;
            facturaCliente.FechaPago = DateTime.Now.AddDays(30); //30 dias

            if (!facturaCliente.Pagado == true)
            {
                //se genera el proximo numero de factura
                var proximo = (from inv in db.FacturaClientes orderby inv.NumeroFactura descending select inv).FirstOrDefault();

                if (proximo != null)
                {
                    facturaCliente.NumeroFactura = proximo.NumeroFactura + 1;
                }
                else
                {
                    facturaCliente.NumeroFactura = 1000000;
                }
            }

            return View(facturaCliente);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Factura(FacturaCliente facturaCliente)
        {
            //ViewBag.ClienteId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        if(facturaCliente.Pagado == true){
                            GuardarFactura(facturaCliente);
                            GuardarFacturaDetalles(facturaCliente);
                            ActualizarStock(facturaCliente);
                            ActualizarCaja(facturaCliente);
                            GuardarMovimiento(facturaCliente);
                            TempData["notice"] = "La Factura Número: " + facturaCliente.NumeroFactura + " fue generada correctamente! ";
                        }
                        else
                        {
                            GuardarFactura(facturaCliente);
                            GuardarFacturaDetalles(facturaCliente);
                            TempData["notice"] = "El Presupuesto Número: " + facturaCliente.NumeroFactura + " fue guardado correctamente! ";

                        }
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        TempData["notice"] = "No se pudo realizar la transacción!" + ex.Message;
                        return RedirectToAction("Factura");
                    }

                }
                return RedirectToAction("Details/"+ObtenerIdMax());
            }

            TempData["notice"] = "Todos los campos son requeridos!";
            return View(facturaCliente);

        }

        public void GuardarFactura(FacturaCliente facturaCliente)
        {
            using (var context = new ApplicationDbContext())
            {
                var factura = new FacturaCliente
                {
                    Fecha = facturaCliente.Fecha,
                    FechaPago = facturaCliente.FechaPago,
                    Metodo = facturaCliente.Metodo,
                    NumeroFactura = facturaCliente.NumeroFactura,
                    Pagado = facturaCliente.Pagado,
                    ClienteId = facturaCliente.ClienteId,
                    Total = facturaCliente.Total,
                    TotalPagado = facturaCliente.TotalPagado
                };
                context.FacturaClientes.Add(factura);
                context.SaveChanges();
            }
        }

        public void GuardarFacturaDetalles(FacturaCliente facturaCliente)
        {
            using (var contextDetalles = new ApplicationDbContext())
            {
                foreach (var item in facturaCliente.detallesFacturaCliente)
                {
                    int idmax = ObtenerIdMax();
                    var detalles = new DetallesFacturaCliente
                    {
                        FacturaClienteId = idmax,
                        RepuestoId = item.RepuestoId,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                    };
                    contextDetalles.DetallesFacturaClientes.Add(detalles);
                }
                contextDetalles.SaveChanges();
            }
        }

        public void ActualizarStock(FacturaCliente facturaCliente)
        {
            using (var context = new ApplicationDbContext())
            {
                foreach (var item in facturaCliente.detallesFacturaCliente)
                {
                    Repuesto r = db.Repuestoes.Find(item.RepuestoId);
                    int cant = r.Stock - item.Cantidad;
                    var repuesto = new Repuesto
                    {
                        Id = item.RepuestoId,
                        Stock = cant,
                        Nombre = r.Nombre,
                        ProveedorId = r.ProveedorId,
                        CategoriaId = r.CategoriaId,
                        ModeloId = r.ModeloId,
                        PrecioCosto = r.PrecioCosto,
                        PrecioVenta1 = r.PrecioVenta1,
                        PrecioVenta2 = r.PrecioVenta2,
                        PrecioVenta3 = r.PrecioVenta3,
                        StockMinimo = r.StockMinimo,
                        StockMaximo = r.StockMaximo
                    };
                    context.Repuestoes.Add(repuesto);
                    context.Entry(repuesto).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }



        public void ActualizarCaja(FacturaCliente facturaCliente)
        {
            using (var context = new ApplicationDbContext())
            {
                Caja c = db.Cajas.Find(ObtenerUltimoCajaAbierto());
                int cantOperaciones = (int)c.Operaciones + 1;
                int sumaEntrada = (int)c.Entrada + facturaCliente.Total;
                int cierre = (int)c.Cierre + facturaCliente.Total;
                var caja = new Caja
                {
                    Id = c.Id,
                    Fecha_Apertura = c.Fecha_Apertura,
                    Inicial = c.Inicial,
                    Operaciones = cantOperaciones,
                    Fecha_Cierre = c.Fecha_Cierre,
                    Salida = c.Salida,
                    Entrada = sumaEntrada,
                    Cierre = cierre,
                    Estado = c.Estado,
                    Usuario = c.Usuario
                };
                context.Cajas.Add(caja);
                context.Entry(caja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void GuardarMovimiento(FacturaCliente facturaCliente)
        {
            using (var context = new ApplicationDbContext())
            {
                var movimiento = new MovimientoCaja
                {
                    Fecha = DateTime.Now,
                    CajaId = ObtenerUltimoCajaAbierto(),
                    Concepto = "Factura Venta Nº: " + facturaCliente.NumeroFactura,
                    Movimiento = "Entrada",
                    Ingreso = facturaCliente.Total,
                    Egreso = 0,
                    Saldo = (int)ObtenerSaldoUltimoCajaAbierto()
                };
                context.MovimientoCajas.Add(movimiento);
                context.SaveChanges();
            }
        }


        public int? ObtenerSaldoUltimoCajaAbierto()
        {
            var Nick = User.Identity.GetUserName();
            var IndexNick = Nick.IndexOf("@");
            var usuario = Nick.Substring(0, IndexNick);
            int? ultimosaldocajaabierto = db.Cajas.Where(c => c.Estado == false && c.Usuario == usuario).Select(c => c.Cierre).DefaultIfEmpty(0).Max();
            return ultimosaldocajaabierto;
        }

        public int ObtenerUltimoCajaAbierto()
        {
            var Nick = User.Identity.GetUserName();
            var IndexNick = Nick.IndexOf("@");
            var usuario = Nick.Substring(0, IndexNick);
            var ultimoabierto = db.Cajas.Where(c => c.Estado == false && c.Usuario == usuario).Select(c => c.Id).DefaultIfEmpty(0).Max();
            return ultimoabierto;
        }

        public int ObtenerIdMax()
        {
            var idmax = db.FacturaClientes.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id);
            return idmax;
        }


        // GET: FacturaClientes/Editar/1
        public ActionResult Editar(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaCliente facturaCliente = db.FacturaClientes.Find(id);

            DetallesFacturaCliente detallesFacturaCliente = db.DetallesFacturaClientes.Find(facturaCliente.Id);

            if (facturaCliente == null)
            {
                return HttpNotFound();
            }

            return View(facturaCliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(FacturaCliente facturaCliente)
        {
            //ViewBag.ClienteId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        GuardarFactura(facturaCliente);
                        GuardarFacturaDetalles(facturaCliente);
                        //commit transaction
                        db.Entry(facturaCliente).State = EntityState.Modified;
                        dbTran.Commit();
                        TempData["notice"] = "El Presupuesto Número: " + facturaCliente.NumeroFactura + " fue guardado correctamente! ";
                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs
                        dbTran.Rollback();
                        //Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                        TempData["notice"] = "No se pudo realizar la transacción!" + ex.Message;
                        //return View(facturaCliente);
                        return View(facturaCliente);
                    }

                }
                return RedirectToAction("Pagar/" + facturaCliente.Id);
            }

            TempData["notice"] = "Todos los campos son requeridos!";
            return View(facturaCliente);

        }



        // GET: FacturaClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaCliente facturaCliente = db.FacturaClientes.Find(id);
            if (facturaCliente == null)
            {
                return HttpNotFound();
            }
            return View(facturaCliente);
        }

        // GET: FacturaClientes/Create
        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial");
            return View();
        }

        // POST: FacturaClientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,FechaPago,NumeroFactura,ClienteId,Total,TotalPagado,Metodo,Pagado")] FacturaCliente facturaCliente)
        {
            if (ModelState.IsValid)
            {
                db.FacturaClientes.Add(facturaCliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", facturaCliente.ClienteId);
            return View(facturaCliente);
        }

        // GET: FacturaClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaCliente facturaCliente = db.FacturaClientes.Find(id);
            if (facturaCliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", facturaCliente.ClienteId);
            return View(facturaCliente);
        }

        // POST: FacturaClientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,FechaPago,NumeroFactura,ClienteId,Total,TotalPagado,Metodo,Pagado")] FacturaCliente facturaCliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facturaCliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", facturaCliente.ClienteId);
            return View(facturaCliente);
        }

        // GET: FacturaClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaCliente facturaCliente = db.FacturaClientes.Find(id);
            if (facturaCliente == null)
            {
                return HttpNotFound();
            }
            return View(facturaCliente);
        }

        // POST: FacturaClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FacturaCliente facturaCliente = db.FacturaClientes.Find(id);
            db.FacturaClientes.Remove(facturaCliente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CargarRepuestos(int term)
        {
            var repuestoslist = new List<object>();
            Repuesto repuesto = db.Repuestoes.Where(e => e.Id == term).FirstOrDefault();
            if (repuesto == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new { repuesto.Id, repuesto.Nombre, repuesto.PrecioVenta1, repuesto.Stock };
                repuestoslist.Add(data);
                return Json(repuestoslist, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
            }
            TempData["notice"] = "El cliente " + cliente.RazonSocial + " fue guardado correctamente! ";
            return RedirectToAction("Factura", "FacturaClientes");
        }

        public ActionResult Search(string term)
        {
            var routeList = db.Repuestoes.Where(r => r.Nombre.Contains(term)).Take(10)
                    .Select(r => new { r.Id, r.Nombre, r.PrecioVenta1, r.Stock });
            return Json(routeList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getIdMax1()
        {
            var i = db.FacturaClientes.ToList().Select(e => e.Id).Max();
            //var query = from c in db.FacturaClientes select new { c.Id, c.RazonSocial, c.Documento, c.Direccion };
            return Json(i, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getClientes()
        {
            var query = from c in db.Clientes select new { c.Id, c.RazonSocial, c.Documento, c.Direccion };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BusquedaClientes(string term)
        {
            Cliente cliente = db.Clientes.Where(e => e.RazonSocial.Contains(term)).FirstOrDefault();
            if (cliente == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new { cliente.Id, cliente.RazonSocial, cliente.Documento, cliente.Telefono };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public enum Metodo
        {
            Contado,
            Presupuesto
        }
    }
}
