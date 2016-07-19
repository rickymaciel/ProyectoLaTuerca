﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaTuerca.Models;
using System.IO;

namespace LaTuerca.Controllers
{
    public class MarcasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Marcas
        public ActionResult Index()
        {
            return View(db.Marcas.ToList());
        }

        // GET: Marcas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // GET: Marcas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,ImagenUrl")] Marca marca, HttpPostedFileBase Imagen)
        {
            if (ModelState.IsValid)
            {

                if (Imagen != null)
                {
                    string fecha = DateTime.Now.ToString("ddMMyyyyhhmmss");
                    var fileName = Path.GetFileName(marca.Nombre + fecha + Imagen.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/Uploads/Marcas/"), fileName);
                    Imagen.SaveAs(path);
                    marca.Imagen = fileName;
                }
                else
                {
                    var fileName = Path.GetFileName("Marcas/Default.jpg");
                    marca.Imagen = fileName;
                }

                db.Marcas.Add(marca);

                db.SaveChanges();
                TempData["notice"] = "La Marca " + marca.Nombre + " fue creada con éxito!";
                return RedirectToAction("Index");
            }
            TempData["notice"] = "La Marca " + marca.Nombre + " no pudo ser creada!";
            return View(marca);
        }

        [HttpPost]
        public ActionResult AjaxCreate(Marca marca, HttpPostedFileBase Imagen)
        {
            if (ModelState.IsValid)
            {

                if (Imagen != null)
                {
                    string fecha = DateTime.Now.ToString("ddMMyyyyhhmmss");
                    var fileName = Path.GetFileName(marca.Nombre + fecha + Imagen.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/Uploads/Marcas/"), fileName);
                    Imagen.SaveAs(path);
                    marca.Imagen = fileName;
                }
                else
                {
                    var fileName = Path.GetFileName("Marcas/Default.jpg");
                    marca.Imagen = fileName;
                }

                db.Marcas.Add(marca);
                db.SaveChanges();
                TempData["notice"] = "La Marca " + marca.Nombre + " fue creada con éxito!";
                return RedirectToAction("Create", "Repuestos");
            }
            TempData["notice"] = "La Marca " + marca.Nombre + " no pudo ser creada!";
            return RedirectToAction("Create", "Repuestos");
        }
        // GET: Marcas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            marca.Imagen = marca.Imagen;
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,ImagenUrl")] Marca marca, HttpPostedFileBase Imagen)
        {
            if (ModelState.IsValid)
            {
                if (Imagen != null)
                {
                    string fecha = DateTime.Now.ToString("ddMMyyyyhhmmss");
                    var fileName = Path.GetFileName(marca.Nombre + fecha + Imagen.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/Uploads/Marcas/"), fileName);
                    Imagen.SaveAs(path);
                    marca.Imagen = fileName;
                }
                else
                {
                    var fileName = Path.GetFileName("Marcas/Default.jpg");
                    marca.Imagen = fileName;
                }

                TempData["notice"] = "La Marca " + marca.Nombre + " fue modificada con éxito!";
                db.Entry(marca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["notice"] = "La Marca " + marca.Nombre + " no pudo ser modificada!";
            return View(marca);
        }

        // GET: Marcas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marca marca = db.Marcas.Find(id);
            db.Marcas.Remove(marca);
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
