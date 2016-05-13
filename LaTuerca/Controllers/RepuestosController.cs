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
            var repuesto = new Repuesto();
            var marcas = db.Marcas.ToList();
            var modelos = db.Modeloes.ToList();
            var categorias = db.Categorias.ToList();
            var proveedores = db.Proveedors.ToList();
            var repuestoModelView = new RepuestoViewModel(repuesto, marcas, modelos, categorias, proveedores);

            return View(repuestoModelView);
        }

        // POST: Repuestos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Repuesto repuesto, int Marca, int Modelo,int Categoria,int Proveedor)
        {
            if (ModelState.IsValid)
            {
                var marca = db.Marcas.Single(m => m.Id == Marca);
                var proveedor = db.Proveedors.Single(p => p.Id == Proveedor);
                var modelo = db.Modeloes.Single(p => p.Id == Modelo);
                var categoria = db.Categorias.Single(p => p.Id == Categoria);
                repuesto.MarcaId = marca.Id;
                repuesto.ModeloId = modelo.Id;
                repuesto.ProveedorId = proveedor.Id;
                repuesto.CategoriaId = categoria.Id;
                db.Repuestoes.Add(repuesto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(repuesto);
        }

        // GET: Repuestos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repuesto repuesto = db.Repuestoes.Find(id);
            var marcas = db.Marcas.ToList();
            var modelos = db.Modeloes.ToList();
            var proveedores = db.Proveedors.ToList();
            var categorias = db.Categorias.ToList();
            var repuestoModelView = new RepuestoViewModel(repuesto, marcas, modelos,categorias, proveedores);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuestoModelView);
        }

        // POST: Repuestos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Repuesto repuesto, int Marca, int Proveedor, int Modelo,int Categoria)
        {
            if (ModelState.IsValid)
            {
                var marca = db.Marcas.Single(m => m.Id == Marca);
                var modelo = db.Modeloes.Single(m => m.Id == Modelo);
                var proveedor = db.Proveedors.Single(p => p.Id == Proveedor);
                var categoria = db.Categorias.Single(p => p.Id == Categoria);
                repuesto.MarcaId = marca.Id;
                repuesto.ModeloId = modelo.Id;
                repuesto.ProveedorId = proveedor.Id;
                repuesto.CategoriaId = categoria.Id;
                db.Entry(repuesto).State = EntityState.Modified;
                db.SaveChanges();
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
