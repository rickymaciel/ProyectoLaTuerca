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
    public class RepuestosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Repuestos
        public ActionResult Index()
        {
            return View(db.Repuestoes.ToList());
        }

        // GET: Repuestos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repuesto repuesto = db.Repuestoes.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuesto);
        }

        // GET: Repuestos/Create
        public ActionResult Create()
        {
            Repuesto repuesto = new Repuesto();
            ViewBag.ModeloId = new SelectList(db.Modeloes, "Id", "NombreModelo");
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial");
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre");

            return View(repuesto);
        }

        // POST: Repuestos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ModeloId = new SelectList(db.Modeloes, "Id", "NombreModelo", repuesto.ModeloId);
                ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", repuesto.ProveedorId);
                ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", repuesto.CategoriaId);
                
                db.Repuestoes.Add(repuesto);
                db.SaveChanges();
                TempData["notice"] = "El producto ha sido guardado!";
                return RedirectToAction("Create", "Repuestos");
            }
            else
            {
                TempData["notice"] = "Ocurrio un error!";
                return RedirectToAction("Create", "Repuestos");
            }

        }

        // GET: Repuestos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repuesto repuesto = db.Repuestoes.Find(id);
            var repuestoModelView = new RepuestoViewModel(repuesto);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModeloId = new SelectList(db.Modeloes, "Id", "NombreModelo", repuesto.ModeloId);
            ViewBag.ProveedorId = new SelectList(db.Proveedors, "Id", "RazonSocial", repuesto.ProveedorId);
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", repuesto.CategoriaId);
            return View(repuesto);
        }

        // POST: Repuestos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                Repuesto r = db.Repuestoes.Find(repuesto.Id);
                r.ModeloId = repuesto.ModeloId;
                r.ProveedorId = repuesto.ProveedorId;
                r.CategoriaId = repuesto.CategoriaId;
                r.Nombre = repuesto.Nombre;
                r.PrecioCosto = repuesto.PrecioCosto;
                r.PrecioVenta1 = repuesto.PrecioVenta1;
                r.PrecioVenta2 = repuesto.PrecioVenta2;
                r.PrecioVenta3 = repuesto.PrecioVenta3;
                r.Stock = repuesto.Stock;
                r.StockMaximo = repuesto.StockMaximo;
                r.StockMinimo = repuesto.StockMinimo;
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "El producto ha sido modificado!";
                return RedirectToAction("Index");
            }
            return View(repuesto);
        }

        // GET: Repuestos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repuesto repuesto = db.Repuestoes.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuesto);
        }

        // POST: Repuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Repuesto repuesto = db.Repuestoes.Find(id);
            db.Repuestoes.Remove(repuesto);
            db.SaveChanges();
            TempData["notice"] = "El producto " + repuesto.Nombre + " ha sido eliminado!";
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
