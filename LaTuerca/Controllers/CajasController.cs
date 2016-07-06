using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaTuerca.Models;
using Microsoft.AspNet.Identity;

namespace LaTuerca.Controllers
{
    public class CajasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cajas
        public ActionResult Index()
        {
            var caja = new Caja();
            //string fecha = DateTime.Now;
            DateTime Fecha = DateTime.Now;

            var Nick = User.Identity.GetUserName();
            var IndexNick = Nick.IndexOf("@");
            var usuario = Nick.Substring(0, IndexNick);
            if (ObtenerUltimoCajaAbierto() == 0)
            {
                TempData["notice"] = "Todas las cajas estan cerradas";
                TempData["Fecha_Apertura"] = Fecha;
            }
            else
            {
                TempData["notice"] = "Caja abierta";
                TempData["Fecha_Apertura"] = null;
            }
            return View(db.Cajas.ToList().Where(c => c.Usuario == usuario).OrderByDescending(c => c.Id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(Caja caja)
        {
            if (ModelState.IsValid)
            {
                db.Cajas.Add(caja);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Cajas");
        }
        // GET: Cajas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caja caja = db.Cajas.Find(id);
            if (caja == null)
            {
                return HttpNotFound();
            }
            return View(caja);
        }

        // GET: Cajas/Create
        public ActionResult Create()
        {
            var caja = new Caja();
            string fecha = DateTime.Now.ToString("{0:yyyy-MM-dd hh:mm:ss}");
            DateTime Fecha = DateTime.ParseExact(fecha, "{0:yyyy-MM-dd hh:mm:ss}", System.Globalization.CultureInfo.GetCultureInfo("es-PY"));
            caja.Fecha_Apertura = Fecha;
            caja.Operaciones = 0;
            return View(caja);
        }

        public int ObtenerUltimoCajaAbierto()
        {
            var Nick = User.Identity.GetUserName();
            var IndexNick = Nick.IndexOf("@");
            var usuario = Nick.Substring(0, IndexNick);
            var ultimoabierto = db.Cajas.Where(c => c.Estado == false && c.Usuario == usuario).Select(c => c.Id).DefaultIfEmpty(0).Max();
            return ultimoabierto;
        }
        // POST: Cajas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha_Apertura,Inicial,Fecha_Cierre,Cierre,Operaciones,Usuario,Estado")] Caja caja)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ObtenerUltimoCajaAbierto() == 0)
                    {
                        db.Cajas.Add(caja);
                        db.SaveChanges();
                        TempData["notice"] = "Caja iniciada con " + caja.Inicial + "";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Existe caja sin cerrar";
                    }
                }
                catch (Exception ex)
                {
                    TempData["notice"] = "La caja no pudo ser iniciado! " + ex.Message;
                    return View(caja);
                }

            }

            return View(caja);
        }

        // GET: Cajas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caja caja = db.Cajas.Find(id);
            if (caja == null)
            {
                return HttpNotFound();
            }

            string fecha = DateTime.Now.ToString("{0:yyyy-MM-dd hh:mm:ss}");
            DateTime Fecha = DateTime.ParseExact(fecha, "{0:yyyy-MM-dd hh:mm:ss}", System.Globalization.CultureInfo.GetCultureInfo("es-PY"));
            caja.Fecha_Cierre = Fecha;
            return View(caja);
        }

        // POST: Cajas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha_Apertura,Inicial,Fecha_Cierre,Cierre,Operaciones,Usuario,Estado")] Caja caja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caja);
        }

        // GET: Cajas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caja caja = db.Cajas.Find(id);
            if (caja == null)
            {
                return HttpNotFound();
            }
            return View(caja);
        }

        // POST: Cajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Caja caja = db.Cajas.Find(id);
            db.Cajas.Remove(caja);
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
