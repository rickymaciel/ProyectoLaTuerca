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
using RazorPDF;

namespace LaTuerca.Controllers
{
    public class CajasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ReporteCaja()
        {
            try
            {

                var cajas = db.Cajas.ToList();
                return new PdfResult(cajas, "ReporteCaja");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ReporteMovimientoCaja(int? id)
        {
            try
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
                //return View(caja);
                return new PdfResult(caja, "ReporteMovimientoCaja");
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MovimientoCaja(DateTime from, DateTime to)
        {
            var fromDate = from;
            var toDate = to;
            var movimientos = db.MovimientoCajas.Where(x => x.Fecha >= fromDate).Where(x => x.Fecha <= toDate).ToList().OrderByDescending(c => c.Fecha);

            var pdf = new RazorPDF.PdfResult(movimientos, "MovimientoCaja");
            pdf.ViewBag.Title = "Movimiento de Cajas";
            pdf.ViewBag.Desde = from;
            pdf.ViewBag.Hasta = to;
            return pdf;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MovimientoUsuario(DateTime from, DateTime to, String Usuarios)
        {
            var fromDate = from;
            var toDate = to;
            var usuario = Usuarios;

            string Nick = "";
            int IndexNick = 0;
            string NewNick = "";
            Nick = usuario;

            IndexNick = Nick.IndexOf("@");
            NewNick = Nick.Substring(0, IndexNick);

            var movimientos = db.MovimientoCajas.Where(x => x.Caja.Usuario == NewNick).Where(x => x.Fecha >= fromDate).Where(x => x.Fecha <= toDate).ToList().OrderByDescending(c => c.Fecha);

            var pdf = new RazorPDF.PdfResult(movimientos, "MovimientoUsuario");
            pdf.ViewBag.Title = "Movimiento de Cajas";
            pdf.ViewBag.Desde = from;
            pdf.ViewBag.Hasta = to;
            return pdf;
        }


        // GET: Cajas
        public ActionResult Index()
        {
            var caja = new Caja();
            String Fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var usuario = "";
            if (User.Identity.GetUserName() != "")
            {
                var Nick = User.Identity.GetUserName();
                var IndexNick = Nick.IndexOf("@");
                usuario = Nick.Substring(0, IndexNick);
            }
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


        // GET: Cajas
        public ActionResult InformeMovimientos()
        {
            ViewBag.Usuarios = new SelectList(db.Users, "Email", "Email");
            return View(db.Cajas.ToList());
        }


        [HttpPost]
        public ActionResult AjaxCreate(Caja caja)
        {
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        AbrirCaja(caja);
                        MovimientoInicial(caja);

                        dbTran.Commit();
                        TempData["notice"] = "Caja iniciada con: " + caja.Inicial + "!";
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        TempData["notice"] = "No se pudo realizar la transacción!" + ex.Message;
                        return View(caja);
                    }

                }
                //return RedirectToAction("Index", "Cajas");
                return RedirectToAction("Details/" + ObtenerUltimoCajaAbierto());
            }

            TempData["notice"] = "Todos los campos son requeridos!";
            return View(caja);

        }


        public void AbrirCaja(Caja caja)
        {
            using (var context = new ApplicationDbContext())
            {
                var abrir = new Caja
                {
                    Fecha_Apertura = caja.Fecha_Apertura,
                    Inicial = caja.Inicial,
                    Entrada = caja.Entrada,
                    Salida = 0,
                    Fecha_Cierre = caja.Fecha_Cierre,
                    Cierre = caja.Cierre,
                    Operaciones = caja.Operaciones,
                    Usuario = caja.Usuario
                };
                context.Cajas.Add(caja);
                context.SaveChanges();
            }
        }


        public void MovimientoInicial(Caja caja)
        {
            using (var context = new ApplicationDbContext())
            {
                var idmax = ObtenerUltimoCajaAbierto();
                var movimiento = new MovimientoCaja
                {
                    Fecha = DateTime.Now,
                    CajaId = idmax,
                    Concepto = "Apertura de caja",
                    Movimiento = "Entrada",
                    Ingreso = caja.Inicial,
                    Egreso = 0,
                    Saldo = caja.Inicial
                };
                context.MovimientoCajas.Add(movimiento);
                context.SaveChanges();
            }
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
            DateTime Fecha = DateTime.Now;
            caja.Fecha_Apertura = Fecha;
            caja.Operaciones = 0;
            return View(caja);
        }

        public int ObtenerUltimoCajaAbierto()
        {
            var U = User.Identity.GetUserName();
            var IndexU = U.IndexOf("@");
            var usuari = U.Substring(0, IndexU);
            var ultimoabierto = db.Cajas.Where(c => c.Estado == false && c.Usuario == usuari).Select(c => c.Id).DefaultIfEmpty(0).Max();
            return ultimoabierto;
        }


        public int ObtenerUltimoCajaAbiertoUsuario(String us)
        {
            var ultimoabierto = db.Cajas.Where(c => c.Estado == false && c.Usuario == us).Select(c => c.Id).DefaultIfEmpty(0).Max();
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
            DateTime Fecha = DateTime.Now;
            caja.Fecha_Cierre = Fecha;
            return View(caja);
        }

        // POST: Cajas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha_Apertura,Inicial,Entrada,Salida,Fecha_Cierre,Cierre,Operaciones,Usuario,Estado")] Caja caja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caja).State = EntityState.Modified;
                db.SaveChanges();
                TempData["notice"] = "Caja cerrada con: " + caja.Cierre + "!";
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
            TempData["notice"] = "Caja Nº: " + caja.Id + " fue eliminada!";
            return RedirectToAction("Index");
        }



        public ActionResult Search(string term)
        {
            var routeList = db.Users.Where(r => r.Email == term).Take(1)
                    .Select(r => new { r.Id, r.UserName});
            return Json(routeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getUsers()
        {
            var query = from c in db.Users select new { c.Id, c.Email};
            return Json(query, JsonRequestBehavior.AllowGet);
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
