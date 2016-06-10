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
    public class DetallesFacturaClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DetallesFacturaClientes
        public ActionResult Index()
        {
            var detallesFacturaClientes = db.DetallesFacturaClientes.Include(d => d.FacturaCliente).Include(d => d.Repuesto);
            return View(detallesFacturaClientes.ToList());
        }

        // GET: DetallesFacturaClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallesFacturaCliente detallesFacturaCliente = db.DetallesFacturaClientes.Find(id);
            if (detallesFacturaCliente == null)
            {
                return HttpNotFound();
            }
            return View(detallesFacturaCliente);
        }

        // GET: DetallesFacturaClientes/Create
        public ActionResult Create()
        {
            ViewBag.FacturaClienteId = new SelectList(db.FacturaClientes, "Id", "Metodo");
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre");
            return View();
        }

        // POST: DetallesFacturaClientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FacturaClienteId,Cantidad,RepuestoId,Precio")] DetallesFacturaCliente detallesFacturaCliente)
        {
            if (ModelState.IsValid)
            {
                db.DetallesFacturaClientes.Add(detallesFacturaCliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacturaClienteId = new SelectList(db.FacturaClientes, "Id", "Metodo", detallesFacturaCliente.FacturaClienteId);
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre", detallesFacturaCliente.RepuestoId);
            return View(detallesFacturaCliente);
        }

        // GET: DetallesFacturaClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallesFacturaCliente detallesFacturaCliente = db.DetallesFacturaClientes.Find(id);
            if (detallesFacturaCliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacturaClienteId = new SelectList(db.FacturaClientes, "Id", "Metodo", detallesFacturaCliente.FacturaClienteId);
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre", detallesFacturaCliente.RepuestoId);
            return View(detallesFacturaCliente);
        }

        // POST: DetallesFacturaClientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FacturaClienteId,Cantidad,RepuestoId,Precio")] DetallesFacturaCliente detallesFacturaCliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallesFacturaCliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacturaClienteId = new SelectList(db.FacturaClientes, "Id", "Metodo", detallesFacturaCliente.FacturaClienteId);
            ViewBag.RepuestoId = new SelectList(db.Repuestoes, "Id", "Nombre", detallesFacturaCliente.RepuestoId);
            return View(detallesFacturaCliente);
        }

        // GET: DetallesFacturaClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallesFacturaCliente detallesFacturaCliente = db.DetallesFacturaClientes.Find(id);
            if (detallesFacturaCliente == null)
            {
                return HttpNotFound();
            }
            return View(detallesFacturaCliente);
        }

        // POST: DetallesFacturaClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetallesFacturaCliente detallesFacturaCliente = db.DetallesFacturaClientes.Find(id);
            db.DetallesFacturaClientes.Remove(detallesFacturaCliente);
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
