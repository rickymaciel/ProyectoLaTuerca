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
    public class MovimientoCajasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MovimientoCajas
        public ActionResult Index()
        {
            var movimientoCajas = db.MovimientoCajas.Include(m => m.Caja);
            return View(movimientoCajas.ToList());
        }

        // GET: MovimientoCajas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoCaja movimientoCaja = db.MovimientoCajas.Find(id);
            if (movimientoCaja == null)
            {
                return HttpNotFound();
            }
            return View(movimientoCaja);
        }

        // GET: MovimientoCajas/Create
        public ActionResult Create()
        {
            ViewBag.CajaId = new SelectList(db.Cajas, "Id", "Usuario");
            return View();
        }

        // POST: MovimientoCajas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CajaId,Concepto,Movimiento,Monto")] MovimientoCaja movimientoCaja)
        {
            if (ModelState.IsValid)
            {
                db.MovimientoCajas.Add(movimientoCaja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CajaId = new SelectList(db.Cajas, "Id", "Usuario", movimientoCaja.CajaId);
            return View(movimientoCaja);
        }

        // GET: MovimientoCajas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoCaja movimientoCaja = db.MovimientoCajas.Find(id);
            if (movimientoCaja == null)
            {
                return HttpNotFound();
            }
            ViewBag.CajaId = new SelectList(db.Cajas, "Id", "Usuario", movimientoCaja.CajaId);
            return View(movimientoCaja);
        }

        // POST: MovimientoCajas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CajaId,Concepto,Movimiento,Monto")] MovimientoCaja movimientoCaja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movimientoCaja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CajaId = new SelectList(db.Cajas, "Id", "Usuario", movimientoCaja.CajaId);
            return View(movimientoCaja);
        }

        // GET: MovimientoCajas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoCaja movimientoCaja = db.MovimientoCajas.Find(id);
            if (movimientoCaja == null)
            {
                return HttpNotFound();
            }
            return View(movimientoCaja);
        }

        // POST: MovimientoCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovimientoCaja movimientoCaja = db.MovimientoCajas.Find(id);
            db.MovimientoCajas.Remove(movimientoCaja);
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
