using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaTuerca.Models;
using System.Globalization;
using Microsoft.AspNet.Identity;

namespace LaTuerca.Controllers
{
    public class FacturaProveedorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FacturaProveedors
        public ActionResult Index()
        {
            var facturaProveedors = db.FacturaProveedors.Include(f => f.Proveedor);
            return View(facturaProveedors.ToList().Where(w => w.Pagado == false && w.TotalPagado < 1));
        }


        // GET: FacturaProveedors/Edit/5
        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaProveedor facturaProveedor = db.FacturaProveedors.Find(id);
            if (facturaProveedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            return View(facturaProveedor);
        }

        // GET: FacturaProveedors
        public ActionResult Presupuestos()
        {
            var facturaProveedors = db.FacturaProveedors.Include(f => f.Proveedor);
            return View(facturaProveedors.ToList().Where(w => w.Pagado == false && w.TotalPagado < 1));
        }


        // GET: FacturaProveedors
        public ActionResult Facturados()
        {
            var facturaProveedors = db.FacturaProveedors.Include(f => f.Proveedor);
            return View(facturaProveedors.ToList().Where(w => w.Pagado == true && w.TotalPagado > 0));
        }

        // GET: FacturaProveedors
        public ActionResult Pagados()
        {
            var facturaProveedors = db.FacturaProveedors.Include(f => f.Proveedor);
            return View(facturaProveedors.ToList().Where(w => w.Pagado == true));
        }

        // GET: Compras/Transaction
        public ActionResult Factura()
        {
            var facturaProveedor = new FacturaProveedor();
            DateTime Fecha = DateTime.Now;
            facturaProveedor.Fecha = Fecha;
            facturaProveedor.FechaPago = DateTime.Now.AddDays(30); //se le suman 30 dias
            var proximo = (from inv in db.FacturaProveedors orderby inv.NumeroFactura descending select inv).FirstOrDefault();
            if (proximo != null)
            {
                facturaProveedor.NumeroFactura = proximo.NumeroFactura + 1;
            }
            else
            {
                facturaProveedor.NumeroFactura = 10000000;
            }
            return View(facturaProveedor);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Factura(FacturaProveedor facturaProveedor)
        {
            //ViewBag.ClienteId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        Caja c = db.Cajas.Find(ObtenerUltimoCajaAbierto());
                        int cierre = (int)c.Cierre;
                        if (cierre >= facturaProveedor.TotalPagado)
                        {
                            GuardarFactura(facturaProveedor);
                            GuardarFacturaDetalles(facturaProveedor);
                            ActualizarStock(facturaProveedor);
                            ActualizarCaja(facturaProveedor);
                            GuardarMovimiento(facturaProveedor);

                            dbTran.Commit();
                            TempData["notice"] = "La Factura Número: " + facturaProveedor.NumeroFactura + " fue guardada correctamente!";
                        }
                        else
                        {
                            TempData["notice"] = "No hay fondo suficiente para realizar la compra";
                            return RedirectToAction("Factura");
                        }
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

            TempData["notice"] = "Todos los campos son requeridos!";
            return View(facturaProveedor);

        }

        public void GuardarFactura(FacturaProveedor facturaProveedor)
        {
            using (var context = new ApplicationDbContext())
            {
                var factura = new FacturaProveedor
                {
                    Fecha = facturaProveedor.Fecha,
                    FechaPago = facturaProveedor.FechaPago,
                    Metodo = facturaProveedor.Metodo,
                    NumeroFactura = facturaProveedor.NumeroFactura,
                    Pagado = facturaProveedor.Pagado,
                    ProveedorId = facturaProveedor.ProveedorId,
                    Total = facturaProveedor.Total,
                    TotalPagado = facturaProveedor.TotalPagado
                };
                context.FacturaProveedors.Add(factura);
                context.SaveChanges();
            }
        }

        public void GuardarFacturaDetalles(FacturaProveedor facturaProveedor)
        {
            using (var contextDetalles = new ApplicationDbContext())
            {
                foreach (var item in facturaProveedor.detallesFacturaProveedor)
                {
                    int idmax =  ObtenerIdMax();
                    var detalles = new DetallesFacturaProveedor
                    {
                        FacturaProveedorId = idmax,
                        RepuestoId = item.RepuestoId,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                    };
                    contextDetalles.DetallesFacturaProveedors.Add(detalles);
                }
                contextDetalles.SaveChanges();
            }
        }

        public void ActualizarStock(FacturaProveedor facturaProveedor)
        {
            using (var context = new ApplicationDbContext())
            {
                foreach (var item in facturaProveedor.detallesFacturaProveedor)
                {
                    Repuesto r = db.Repuestoes.Find(item.RepuestoId);
                    int cant = r.Stock + item.Cantidad;
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


        public void ActualizarCaja(FacturaProveedor facturaProveedor)
        {
            using (var context = new ApplicationDbContext())
            {
                Caja c = db.Cajas.Find(ObtenerUltimoCajaAbierto());
                int cantOperaciones = (int)c.Operaciones + 1;
                int sumaSalida = (int)c.Salida + facturaProveedor.TotalPagado;
                int cierre = (int) c.Cierre - facturaProveedor.TotalPagado;
                var caja = new Caja
                {
                    Id = c.Id,
                    Fecha_Apertura= c.Fecha_Apertura,
                    Inicial = c.Inicial,
                    Operaciones = cantOperaciones,
                    Fecha_Cierre = c.Fecha_Cierre,
                    Salida = sumaSalida,
                    Entrada = c.Entrada,
                    Cierre = cierre,
                    Estado = c.Estado,
                    Usuario = c.Usuario
                };
                context.Cajas.Add(caja);
                context.Entry(caja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void GuardarMovimiento(FacturaProveedor facturaProveedor)
        {
            using (var context = new ApplicationDbContext())
            {
                var idmax = ObtenerUltimoCajaAbierto();
                var movimiento = new MovimientoCaja
                {
                    Fecha = DateTime.Now,
                    CajaId = idmax,
                    Concepto = "Factura Compra Nº: "+facturaProveedor.NumeroFactura,
                    Movimiento = "Salida",
                    Ingreso = 0,
                    Egreso = facturaProveedor.TotalPagado,
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
            var idmax = db.FacturaProveedors.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id);
            return idmax;
        }


        public int sumaIngreso()
        {
            var sumaIngreso = db.MovimientoCajas.Where(c => c.CajaId == ObtenerUltimoCajaAbierto()).Select(x => x.Ingreso).DefaultIfEmpty(0).Sum();
            return sumaIngreso;
        }


        // GET: FacturaProveedors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaProveedor facturaProveedor = db.FacturaProveedors.Find(id);
            if (facturaProveedor == null)
            {
                return HttpNotFound();
            }
            return View(facturaProveedor);
        }


        // GET: FacturaProveedors/Edit/5
        public ActionResult Pagar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaProveedor facturaProveedor = db.FacturaProveedors.Find(id);
            if (facturaProveedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre", facturaProveedor.detallesFacturaProveedor);
            return View(facturaProveedor);
        }

        // POST: FacturaProveedors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pagar([Bind(Include = "Id,Fecha,FechaPago,NumeroFactura,ProveedorId,Total,TotalPagado,Metodo,Pagado")] FacturaProveedor facturaProveedor)
        {
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            if (ModelState.IsValid)
            {
                db.Entry(facturaProveedor).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "Factura generada!";
                return RedirectToAction("Pagar");
            }

            TempData["notice"] = "No se creo la factura!";
            return View(facturaProveedor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Proveedors.Add(proveedor);
                db.SaveChanges();
            }
            TempData["notice"] = "Se creo un nuevo proveedor con éxito!";
            return RedirectToAction("Factura", "FacturaProveedors");
        }

        // GET: FacturaProveedors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaProveedor facturaProveedor = db.FacturaProveedors.Find(id);
            if (facturaProveedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            return View(facturaProveedor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,FechaPago,NumeroFactura,ProveedorId,Total,TotalPagado,Metodo,Pagado")] FacturaProveedor facturaProveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facturaProveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            TempData["notice"] = "Se modifico la factura con éxito!";
            return View(facturaProveedor);
        }

        // GET: FacturaProveedors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacturaProveedor facturaProveedor = db.FacturaProveedors.Find(id);
            if (facturaProveedor == null)
            {
                return HttpNotFound();
            }
            return View(facturaProveedor);
        }

        // POST: FacturaProveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FacturaProveedor facturaProveedor = db.FacturaProveedors.Find(id);
            db.FacturaProveedors.Remove(facturaProveedor);
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
                var data = new { repuesto.Id, repuesto.Nombre, repuesto.PrecioCosto, repuesto.Stock };
                repuestoslist.Add(data);
                return Json(repuestoslist, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Search(string term)
        {
            var routeList = db.Repuestoes.Where(r => r.Nombre.Contains(term)).Take(10)
                    .Select(r => new { r.Id, r.Nombre, r.PrecioCosto, r.Stock });
            return Json(routeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getProveedores()
        {
            var query = from c in db.Proveedors select new { c.Id, c.RazonSocial, c.Ruc, c.Telefono, c.Direccion };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BusquedaProveedor(string term)
        {
            //Proveedor proveedor = db.Proveedors.Where(e => e.RazonSocial.Contains(term)).FirstOrDefault();
            Proveedor proveedor = db.Proveedors.Where(e => e.RazonSocial.Contains(term)).FirstOrDefault();
            if (proveedor == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var data = result.Where(s => s.Value.ToLower().Contains(term.ToLower())).Select(w => w).ToList();
                var data = new { proveedor.Id, proveedor.RazonSocial, proveedor.Ruc, proveedor.Telefono, proveedor.Direccion };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AutoCompleteProveedores(string term)
        {
            var proveedor = (from r in db.Proveedors where r.RazonSocial.ToLower().Contains(term.ToLower()) select new { r.Id, r.RazonSocial, r.Ruc, r.Telefono, r.Direccion });
            return Json(proveedor, JsonRequestBehavior.AllowGet);
        }

        public enum Metodo
        {
            Contado
        }
    }
}
