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
    public class FacturaProveedorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public static class Cultures
        {
            public static readonly CultureInfo Paraguay = CultureInfo.GetCultureInfo("es-PY");
        }

        // GET: FacturaProveedors
        public ActionResult Index()
        {
            var facturaProveedors = db.FacturaProveedors.Include(f => f.Proveedor);
            return View(facturaProveedors.ToList());
        }



        // GET: Compras/Transaction
        public ActionResult Factura()
        {
            var facturaProveedor = new FacturaProveedor();

            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime Fecha = DateTime.ParseExact(fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            facturaProveedor.Fecha = Fecha;
            facturaProveedor.FechaPago = DateTime.Now.AddDays(30); //30 dias

            if (!facturaProveedor.Pagado == true)
            {
                //se genera el proximo numero de factura
                var proximo = (from inv in db.FacturaProveedors orderby inv.NumeroFactura descending select inv).FirstOrDefault();

                if (proximo != null)
                {
                    facturaProveedor.NumeroFactura = proximo.NumeroFactura + 1;
                }
                else
                {
                    facturaProveedor.NumeroFactura = 1000000;
                }
            }

            return View(facturaProveedor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Factura(FacturaProveedor facturaProveedor)
        {
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.FacturaProveedors.Add(facturaProveedor);

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
                        return View(facturaProveedor);
                    }

                }

                //TempData["notice"] = "Error desconocido!";
                return View(facturaProveedor);
            }

            TempData["notice"] = "Error validaciones!";
            return View(facturaProveedor);

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

        // GET: FacturaProveedors/Create
        public ActionResult Create()
        {
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial");
            return View();
        }

        // POST: FacturaProveedors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,FechaPago,NumeroFactura,ProveedorId,Total,TotalPagado,Metodo,Pagado")] FacturaProveedor facturaProveedor)
        {
            if (ModelState.IsValid)
            {
                db.FacturaProveedors.Add(facturaProveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", facturaProveedor.ProveedorId);
            return View(facturaProveedor);
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

        // POST: FacturaProveedors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                var data = new { repuesto.Id, repuesto.Nombre, repuesto.PrecioVenta1, repuesto.Stock };
                repuestoslist.Add(data);
                return Json(repuestoslist, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult getProveedores()
        {
            var query = from c in db.Proveedors select new { c.Id, c.RazonSocial, c.Ruc, c.Direccion };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BusquedaProveedor(string term)
        {
            Proveedor proveedor = db.Proveedors.Where(e => e.RazonSocial.Contains(term)).FirstOrDefault();
            if (proveedor == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new { proveedor.Id, proveedor.RazonSocial, proveedor.Ruc, proveedor.Telefono };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
