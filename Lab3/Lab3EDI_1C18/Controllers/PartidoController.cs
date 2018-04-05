using Lab3EDI_1C18.DBContext;
using Lab3EDI_1C18.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TDA.Clases;

namespace Lab3EDI_1C18.Controllers
{
    public class PartidoController : Controller
    {
        DefaultConnection db = DefaultConnection.getInstance;
        // GET: Partido
        public ActionResult Index()
        {
            return View(db.listaPartido.ToList());
        }

        public ActionResult Buscar(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Partido cg = db.listaNoPartido.Find(x => x.noPartido == id);

            if (cg == null)
            {
                return HttpNotFound();
            }

            return View(cg);
        }

        public ActionResult Buscar1(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Partido cg = db.listaFechaPartido.Find(x => x.FechaPartido.ToString("MMddyyyy") == id);

            if (cg == null)
            {
                return HttpNotFound();
            }

            return View(cg);
        }


        public ActionResult IndexPorFecha()
        {
            return View(db.listaFechaPartido.ToList());
        }

        public ActionResult IndexPorNumero()
        {
            return View(db.listaNoPartido.ToList());
        }

        // GET: Partido/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Partido/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Partido/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "noPartido,FechaPartido,Grupo,Pais1,Pais2,Estadio")] Partido partido)
        {
            try
            {
                // TODO: Add insert logic here
                db.arbolFechaPartido.FuncionObtenerLlave = ObtenerFecha;
                db.arbolFechaPartido.FuncionCompararLlave = CompararFecha;
                db.arbolNoPartido.FuncionObtenerLlave = ObtenerNumero;
                db.arbolNoPartido.FuncionCompararLlave = CompararNumero;

                db.listaFechaPartido.Clear();
                db.listaNoPartido.Clear();

                db.arbolFechaPartido.Insertar(partido);
                db.arbolNoPartido.Insertar(partido);
                db.listaPartido.Add(partido);
                Escribir_archivoNum(partido);
                Escribir_archivoFecha(partido);
                if (db.arbolFechaPartido.hizoEquilibrio == true)
                {
                    EscribirEquilibrioFecha(partido);
                }

                if (db.arbolNoPartido.hizoEquilibrio == true)
                {
                    EscribirEquilibrioNum(partido);
                }
                db.arbolFechaPartido.EnOrden(RecorrerPartidoInFecha);
                db.arbolNoPartido.EnOrden(RecorrerPartidoInNumero);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public void EscribirEquilibrioNum(Partido info, bool sobreescribir = true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolManualNum.txt", sobreescribir);
            sw.WriteLine("Se hizo equilibrio con el partido " + info.noPartido);
            sw.Close();
        }       
        public void Escribir_archivoNum(Partido info, bool sobreescribir = true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolManualNum.txt", sobreescribir);
            sw.WriteLine("Se inserto el partido NO. " + info.noPartido);
            sw.Close();
        }
        public void Escribir_EliminararchivoNum(Partido info, bool sobreescribir = true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolManualNum.txt", sobreescribir);
            sw.WriteLine("Se elimino el partido NO. " + info.noPartido);
            sw.Close();
        }
        public void EscribirEquilibrioFecha(Partido info, bool sobreescribir = true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolManualFecha.txt", sobreescribir);
            sw.WriteLine("Se hizo equilibrio con el partido " + info.noPartido);
            sw.Close();
        }
        public void Escribir_archivoFecha(Partido info, bool sobreescribir = true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolManualFecha.txt", sobreescribir);
            sw.WriteLine("Se inserto el partido NO. " + info.FechaPartido);
            sw.Close();
        }

        public void Escribir_EliminararchivoFecha(Partido info, bool sobreescribir = true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolManualFecha.txt", sobreescribir);
            sw.WriteLine("Se elimino el partido NO. " + info.FechaPartido);
            sw.Close();
        }



        // GET: Partido/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Partido/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Partido/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Partido cg = db.listaNoPartido.Find(x => x.noPartido == id);

            if (cg == null)
            {
                return HttpNotFound();
            }

            return View(cg);
        }

        // POST: Partido/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                Partido cg = db.listaFechaPartido.Find(x => x.noPartido == id);
                db.arbolNoPartido.Eliminar2(cg.noPartido);
                Escribir_EliminararchivoNum(cg);
                if (db.arbolNoPartido.hizoEquilibrio == true)
                {
                    EscribirEquilibrioNum(cg);
                }

                db.listaFechaPartido.Clear();
                
                db.arbolNoPartido.EnOrden(RecorrerPartidoInNumero);

                return RedirectToAction("IndexPorNumero");
            }
            catch
            {
                return View();
            }
        }

        [ValidateInput(false)]
        public ActionResult Delete1(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Partido cg = db.listaFechaPartido.Find(x => x.FechaPartido.ToString("MMddyyyy") == id);

            if (cg == null)
            {
                return HttpNotFound();
            }

            return View(cg);
        }

        // POST: CargaPartido/Delete/5
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete1(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                Partido cg = db.listaFechaPartido.Find(x => x.FechaPartido.ToString("MMddyyyy") == id);
                db.arbolFechaPartido.Eliminar2(cg.FechaPartido);
                Escribir_EliminararchivoFecha(cg);
                if (db.arbolFechaPartido.hizoEquilibrio == true)
                {
                    EscribirEquilibrioFecha(cg);
                }

                db.listaFechaPartido.Clear();
                

                db.arbolFechaPartido.EnOrden(RecorrerPartidoInFecha);

                return RedirectToAction("IndexInFecha");
            }
            catch
            {
                return View();
            }
        }

        public void RecorrerPartidoInFecha(Nodo<Partido> actual)
        {
            db.listaFechaPartido.Add(actual.valor);
        }

        public void RecorrerPartidoInNumero(Nodo<Partido> actual)
        {
            db.listaNoPartido.Add(actual.valor);
        }

        public static int ObtenerNumero(Partido dato)
        {
            return dato.noPartido;
        }

        public static DateTime ObtenerFecha(Partido dato)
        {
            return dato.FechaPartido;
        }

        public static int CompararNumero(int actual, int nuevo)
        {
            return actual.CompareTo(nuevo);
        }

        public static int CompararFecha(DateTime actual, DateTime nuevo)
        {
            return actual.CompareTo(nuevo);
        }
    }
}
