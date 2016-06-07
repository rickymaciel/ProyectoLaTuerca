using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaTuerca.Models;
using LaTuerca.ViewModels;


namespace LaTuerca.Controllers
{
    public class ComprasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Compras
        public ActionResult Index()
        {
            var compras = db.Compras.Include(c => c.Proveedor);
            return View(compras.ToList());
        }


        // GET: Compras/Transaction
        public ActionResult Factura()
        {
            var compra = new Compra();
            var detalles = new List<CompraDetalle>();
            //var repuestos = new List<Repuesto>();
            //var proveedores = new List<Proveedor>();
            var repuestos = db.Repuestoes.ToList();
            var proveedores = db.Proveedors.ToList();

            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime Fecha = DateTime.ParseExact(fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            compra.Fecha = Fecha;
            //compra.FechaPago = DateTime.Now.AddDays(30); //30 days after today

            if (!compra.Pagado == true)
            {
                //se genera el proximo numero de factura
                var proximo = (from inv in db.Compras
                               orderby inv.NumeroFactura descending
                               select inv).FirstOrDefault();
                if (proximo != null)
                    compra.NumeroFactura = proximo.NumeroFactura + 1;
            }

            var compraModelView = new CompraViewModel(compra, detalles, proveedores, repuestos);
            return View(compraModelView);
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

        // GET: Compras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        // GET: Invoices/Transaction
        public ActionResult Transaction()
        {
            var compra = new Compra();
            var detalles = new List<CompraDetalle>();
            //var repuestos = new List<Repuesto>();
            //var proveedores = new List<Proveedor>();
            var repuestos = db.Repuestoes.ToList();
            var proveedores = db.Proveedors.ToList();

            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime Fecha = DateTime.ParseExact(fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            compra.Fecha = Fecha;
            //compra.FechaPago = DateTime.Now.AddDays(30); //30 days after today

            if (!compra.Pagado == true)
            {
                //se genera el proximo numero de factura
                var proximo = (from inv in db.Compras
                               orderby inv.NumeroFactura descending
                               select inv).FirstOrDefault();
                if (proximo != null)
                    compra.NumeroFactura = proximo.NumeroFactura + 1;
            }

            var compraModelView = new CompraViewModel(compra, detalles, proveedores, repuestos);
            return View(compraModelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transaction(Compra compra)
        {
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Compras.Add(compra);
                        compra.CompraDetalles.Add(new CompraDetalle());
                        db.SaveChanges();

                        //commit transaction
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs
                        dbTran.Rollback();
                        Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                        return RedirectToAction("Create");
                    }

                }

                return RedirectToAction("Index");
            }

            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", compra.ProveedorId);
            return View(compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Factura(Compra compra)
        {
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Compras.Add(compra);
                        compra.CompraDetalles.Add(new CompraDetalle());
                        db.SaveChanges();

                        //commit transaction
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs
                        dbTran.Rollback();
                        Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                        return RedirectToAction("Create");
                    }

                }

                return RedirectToAction("Index");
            }

            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", compra.ProveedorId);
            return View(compra);
        }

        // GET: Compras/Create
        public ActionResult Create()
        {
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial");
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,NumeroFactura,ProveedorId,Total,Pagado")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                db.Compras.Add(compra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", compra.ProveedorId);
            return View(compra);
        }

        // GET: Compras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", compra.ProveedorId);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,NumeroFactura,ProveedorId,Total,Pagado")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(compra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", compra.ProveedorId);
            return View(compra);
        }

        // GET: Compras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Compra compra = db.Compras.Find(id);
            db.Compras.Remove(compra);
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
    }
}
