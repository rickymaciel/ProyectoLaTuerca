using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LATUERCA.Models;
using LaTuerca.Models;

namespace LaTuerca.Controllers
{
    public class EmpresasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Empresas
        public ActionResult Index()
        {
            return View(db.Empresas.ToList());
        }

        // GET: Empresas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // GET: Empresas/Create
        public ActionResult Create()
        {
            return View();
        }


        public String getNombreEmpresa()
        {
            var mostrar = db.Empresas.Where(c => c.Estado == true).Select(c => c.Nombre).FirstOrDefault();
            return mostrar;
        }
        public String getWebEmpresa()
        {
            var mostrar = db.Empresas.Where(c => c.Estado == true).Select(c => c.Web).FirstOrDefault();
            return mostrar;
        }
        public String getEmailEmpresa()
        {
            var mostrar = db.Empresas.Where(c => c.Estado == true).Select(c => c.Email).FirstOrDefault();
            return mostrar;
        }
        public String getNombreCortoEmpresa()
        {
            var mostrar = db.Empresas.Where(c => c.Estado == true).Select(c => c.NombreCorto).FirstOrDefault();
            return mostrar;
        }

        public JsonResult getEmpresa()
        {
            var query = from c in db.Empresas.Where(c => c.Estado == true) select new { c.Id, c.Nombre, c.NombreCorto, c.Ruc, c.Email, c.Pais, c.Ciudad, c.Departamento, c.Web, c.Telefono, c.Direccion };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,NombreCorto,Ruc,Departamento,Ciudad,Pais,Email,Direccion,Telefono,Web,Estado")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Empresas.Add(empresa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,NombreCorto,Ruc,Departamento,Ciudad,Pais,Email,Direccion,Telefono,Web,Estado")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empresa empresa = db.Empresas.Find(id);
            db.Empresas.Remove(empresa);
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
