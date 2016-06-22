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

namespace LaTuerca.Controllers
{
    public class FacturaClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public static class Cultures
        {
            public static readonly CultureInfo Paraguay = CultureInfo.GetCultureInfo("es-PY");
        }

        // GET: FacturaClientes
        public ActionResult Index()
        {
            var facturaClientes = db.FacturaClientes.Include(f => f.Cliente);
            return View(facturaClientes.ToList());
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
            ViewBag.ProveedorId = new SelectList(db.Clientes, "Id", "RazonSocial", facturaCliente.ClienteId);
            if (ModelState.IsValid)
            {
                db.Entry(facturaCliente).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "Factura generada!";
                return RedirectToAction("Pagar");
            }

            TempData["notice"] = "No se creo la factura!";
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


        // GET: Compras/Transaction
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
                        GuardarFactura(facturaCliente);
                        GuardarFacturaDetalles(facturaCliente);
                        //commit transaction
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
                /*
                if (facturaCliente.Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FacturaCliente editfacturaCliente = db.FacturaClientes.Find(facturaCliente.Id);
                if (editfacturaCliente == null)
                {
                    return HttpNotFound();
                }
                return View(editfacturaCliente);

                //TempData["notice"] = "Error desconocido!";
                //return View(facturaProveedor);
                 */
                return RedirectToAction("Pagar/"+facturaCliente.Id);
            }

            TempData["notice"] = "Todos los campos son requeridos!";
            return View(facturaCliente);

        }

        public void GuardarFactura(FacturaCliente facturaCliente)
        {
            db.FacturaClientes.Add(facturaCliente);
            db.SaveChanges();

        }

        public void GuardarFacturaDetalles(FacturaCliente facturaCliente)
        {
            foreach (var item in facturaCliente.detallesFacturaCliente)
            {
                var detalles = new DetallesFacturaCliente
                {
                    //FacturaClienteId = db.FacturaClientes.ToList().Select(e => e.Id).Max(),
                    FacturaClienteId = ObtenerIdMax(),
                    RepuestoId = item.RepuestoId,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio,
                };
                if (detalles == null)
                {
                    TempData["notice"] = "El Detalle de Factura esta vacío!";
                }
                else
                {
                    TempData["notice"] = "El Detalle de Factura fue guardado!";
                    db.DetallesFacturaClientes.Add(detalles);

                }
            }
        }

        public int ObtenerIdMax()
        {
            //int idmax = db.FacturaProveedors.Max(item => item.Id);
            //int? idmax = db.FacturaProveedors.Max(item => (int?)item.Id);
            var idmax = db.FacturaClientes.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id);
            if (idmax == 0)
            {
                return idmax + 1;
            }
            else
            {
                return idmax + 1;
            }
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
                /*
                if (facturaCliente.Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FacturaCliente editfacturaCliente = db.FacturaClientes.Find(facturaCliente.Id);
                if (editfacturaCliente == null)
                {
                    return HttpNotFound();
                }
                return View(editfacturaCliente);

                //TempData["notice"] = "Error desconocido!";
                //return View(facturaProveedor);
                 */
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
            Financiado
        }
    }
}
