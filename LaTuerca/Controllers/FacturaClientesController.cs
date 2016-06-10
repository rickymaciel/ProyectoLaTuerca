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
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", facturaCliente.ClienteId);
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.FacturaClientes.Add(facturaCliente);

                        db.SaveChanges();

                        //commit transaction
                        dbTran.Commit();
                        TempData["notice"] = "Factura guadada con exito!";
                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs
                        dbTran.Rollback();
                        //Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                        TempData["notice"] = "Rollback transaction if exception occurs!" + ex.Message;
                        return View(facturaCliente);
                    }

                }

                //TempData["notice"] = "Error desconocido!";
                return View(facturaCliente);
            }

            TempData["notice"] = "Error validaciones!";
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
    }
}
