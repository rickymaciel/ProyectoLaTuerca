using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaTuerca.Models;

namespace LaTuerca.Controllers
{
    public class DetallesFacturaProveedorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DetallesFacturaProveedors
        public ActionResult Index()
        {
            var detallesFacturaProveedors = db.DetallesFacturaProveedors.Include(d => d.FacturaProveedor).Include(d => d.Repuesto);
            return View(detallesFacturaProveedors.ToList());
        }

        // GET: DetallesFacturaProveedors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallesFacturaProveedor detallesFacturaProveedor = db.DetallesFacturaProveedors.Find(id);
            if (detallesFacturaProveedor == null)
            {
                return HttpNotFound();
            }
            return View(detallesFacturaProveedor);
        }

        // GET: DetallesFacturaProveedors/Create
        public ActionResult Create()
        {
            ViewBag.FacturaProveedorId = new SelectList(db.FacturaProveedors, "Id", "Metodo");
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre");
            return View();
        }

        // POST: DetallesFacturaProveedors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FacturaProveedorId,Cantidad,RepuestoId,Precio")] DetallesFacturaProveedor detallesFacturaProveedor)
        {
            if (ModelState.IsValid)
            {
                db.DetallesFacturaProveedors.Add(detallesFacturaProveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacturaProveedorId = new SelectList(db.FacturaProveedors, "Id", "Metodo", detallesFacturaProveedor.FacturaProveedorId);
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre", detallesFacturaProveedor.RepuestoId);
            return View(detallesFacturaProveedor);
        }

        // GET: DetallesFacturaProveedors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallesFacturaProveedor detallesFacturaProveedor = db.DetallesFacturaProveedors.Find(id);
            if (detallesFacturaProveedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacturaProveedorId = new SelectList(db.FacturaProveedors, "Id", "Metodo", detallesFacturaProveedor.FacturaProveedorId);
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre", detallesFacturaProveedor.RepuestoId);
            return View(detallesFacturaProveedor);
        }

        // POST: DetallesFacturaProveedors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FacturaProveedorId,Cantidad,RepuestoId,Precio")] DetallesFacturaProveedor detallesFacturaProveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallesFacturaProveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacturaProveedorId = new SelectList(db.FacturaProveedors, "Id", "Metodo", detallesFacturaProveedor.FacturaProveedorId);
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre", detallesFacturaProveedor.RepuestoId);
            return View(detallesFacturaProveedor);
        }

        // GET: DetallesFacturaProveedors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallesFacturaProveedor detallesFacturaProveedor = db.DetallesFacturaProveedors.Find(id);
            if (detallesFacturaProveedor == null)
            {
                return HttpNotFound();
            }
            return View(detallesFacturaProveedor);
        }

        // POST: DetallesFacturaProveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetallesFacturaProveedor detallesFacturaProveedor = db.DetallesFacturaProveedors.Find(id);
            db.DetallesFacturaProveedors.Remove(detallesFacturaProveedor);
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
