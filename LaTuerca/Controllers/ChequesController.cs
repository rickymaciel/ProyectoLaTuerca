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
    public class ChequesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cheques
        public ActionResult Index()
        {
            var cheques = db.Cheques.Include(c => c.Banco).Include(c => c.Proveedor);
            return View(cheques.ToList());
        }

        // GET: Cheques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cheque cheque = db.Cheques.Find(id);
            if (cheque == null)
            {
                return HttpNotFound();
            }
            return View(cheque);
        }

        // GET: Cheques/Create
        public ActionResult Create()
        {
            ViewBag.BancoId = new SelectList(db.Bancoes, "Id", "Nombre");
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial");
            return View();
        }

        // POST: Cheques/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BancoId,ProveedorId,NroCheque,Monto,Lugar,Fecha,nroCuenta,Serie,Estado")] Cheque cheque)
        {
            if (ModelState.IsValid)
            {
                db.Cheques.Add(cheque);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BancoId = new SelectList(db.Bancoes, "Id", "Nombre", cheque.BancoId);
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", cheque.ProveedorId);
            return View(cheque);
        }

        // GET: Cheques/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cheque cheque = db.Cheques.Find(id);
            if (cheque == null)
            {
                return HttpNotFound();
            }
            ViewBag.BancoId = new SelectList(db.Bancoes, "Id", "Nombre", cheque.BancoId);
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", cheque.ProveedorId);
            return View(cheque);
        }

        // POST: Cheques/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BancoId,ProveedorId,NroCheque,Monto,Lugar,Fecha,nroCuenta,Serie,Estado")] Cheque cheque)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cheque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BancoId = new SelectList(db.Bancoes, "Id", "Nombre", cheque.BancoId);
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", cheque.ProveedorId);
            return View(cheque);
        }

        // GET: Cheques/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cheque cheque = db.Cheques.Find(id);
            if (cheque == null)
            {
                return HttpNotFound();
            }
            return View(cheque);
        }

        // POST: Cheques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cheque cheque = db.Cheques.Find(id);
            db.Cheques.Remove(cheque);
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
