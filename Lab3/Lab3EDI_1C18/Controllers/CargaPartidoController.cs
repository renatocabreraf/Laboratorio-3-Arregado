using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab3EDI_1C18.DBContext;
using Lab3EDI_1C18.Models;
using TDA.Clases;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;

namespace Lab3EDI_1C18.Controllers
{
    public class CargaPartidoController : Controller
    {

       JsonConnection  db = JsonConnection.getInstance;
        // GET: CargaPartido
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buscar(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Partido cg = db.listaNoPartidoCargados.Find(x => x.noPartido == id);

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

            Partido cg = db.listaFechaPartidoCargados.Find(x => x.FechaPartido.ToString("MMddyyyy") == id);

            if (cg == null)
            {
                return HttpNotFound();
            }

            return View(cg);
        }



        public ActionResult IndexInNoPartido()
        {
            return View(db.listaNoPartidoCargados.ToList());
        }

        public ActionResult IndexInFecha()
        {
            return View(db.listaFechaPartidoCargados.ToList());
        }
      
        // GET: CargaPartido/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CargaPartido/Create
        public ActionResult Create()
        {
            return View();
        }

        public static int CompararNumero(int actual, int nuevo)
        {
            return actual.CompareTo(nuevo);
        }

        public static int CompararFecha(DateTime actual, DateTime nuevo)
        {
            return actual.CompareTo(nuevo);
        }

        public static int ObtenerNumero(Partido dato)
        {
            return dato.noPartido;
        }

        public static DateTime ObtenerFecha(Partido dato)
        {
            return dato.FechaPartido;
        }

        public static int CompararEnteros(Partido actual, Partido nuevo)
        {
            return actual.noPartido.CompareTo(nuevo.noPartido);
        }


        public static int Compararfecha(Partido actual, Partido nuevo)
        {
            return actual.FechaPartido.CompareTo(nuevo.FechaPartido);
        }

        // POST: CargaPartido/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase postedFile)
        {
           
            db.CargaPartidoFecha.FuncionObtenerLlave = ObtenerFecha;
            db.CargaPartidoFecha.FuncionCompararLlave = CompararFecha;
            db.CargaPartidoNum.FuncionObtenerLlave = ObtenerNumero;
            db.CargaPartidoNum.FuncionCompararLlave = CompararNumero;


            

            if (postedFile != null)
            {
                

                string filepath = string.Empty;
                
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                filepath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filepath);

                string csvData = System.IO.File.ReadAllText(filepath);

                db.listaFechaPartidoCargados.Clear();
                db.listaNoPartidoCargados.Clear();

                try
                {
                    JObject json = JObject.Parse(csvData);

                    foreach (JProperty property in json.Properties())
                    {

                        string x = property.Value.ToString();
                        Partido y = JsonConvert.DeserializeObject<Partido>(x);                       
                        db.CargaPartidoNum.Insertar(y);
                        Escribir_archivoNum(y);
                        if (db.CargaPartidoNum.hizoEquilibrio == true)
                        {
                            EscribirEquilibrioNum(y);
                        }         
                            
                        db.CargaPartidoFecha.Insertar(y);
                        Escribir_archivoFecha(y);
                        if (db.CargaPartidoFecha.hizoEquilibrio == true)
                        {
                            EscribirEquilibrioFecha(y);
                        }
                    }
                    db.CargaPartidoFecha.EnOrden(RecorrerPartidoInFecha);
                    db.CargaPartidoNum.EnOrden(RecorrerPartidoInNumero);

                    ViewBag.Message = "Cargado Exitosamente";
             

                }
                catch (Exception )
                {
                    ViewBag.Message = "Dato erroneo.";
                }

            }

            return View();
        }

        public void RecorrerPartidoInFecha(Nodo<Partido> actual)
        {
            db.listaFechaPartidoCargados.Add(actual.valor);
        }

        public void RecorrerPartidoInNumero(Nodo<Partido> actual)
        {
            db.listaNoPartidoCargados.Add(actual.valor);
        }

        public void EscribirEquilibrioNum (Partido info, bool sobreescribir = true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolJsonNum.txt", sobreescribir);
            sw.WriteLine("Se hizo equilibrio con el partido "+info.noPartido);
            sw.Close();
        }

        public void Escribir_archivoNum (Partido info, bool sobreescribir=true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
               Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolJsonNum.txt",sobreescribir);
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
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolJsonNum.txt", sobreescribir);
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
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolJsonFecha.txt", sobreescribir);
            sw.WriteLine("Se hizo equilibrio con el partido de fecha " + info.FechaPartido);
            sw.Close();
        }

        public void Escribir_archivoFecha(Partido info, bool sobreescribir = true)
        {
            string ruta = Server.MapPath("~/ArchivoLog/");
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolJsonFecha.txt", sobreescribir);
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
            StreamWriter sw = new StreamWriter(ruta + "\\LogDeArbolJsonFecha.txt", sobreescribir);
            sw.WriteLine("Se elimino el partido NO. " + info.FechaPartido);
            sw.Close();
        }

        // GET: CargaPartido/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CargaPartido/Edit/5
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

        // GET: CargaPartido/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Partido cg = db.listaNoPartidoCargados.Find(x => x.noPartido == id);

            if (cg == null)
            {
                return HttpNotFound();
            }

            return View(cg);
        }

        // POST: CargaPartido/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                Partido cg = db.listaNoPartidoCargados.Find(x => x.noPartido == id);
                db.CargaPartidoNum.Eliminar2(cg.noPartido);
                Escribir_EliminararchivoNum(cg);
                if (db.CargaPartidoNum.hizoEquilibrio == true)
                {
                    EscribirEquilibrioNum(cg);
                }

                db.listaNoPartidoCargados.Clear();
                
                db.CargaPartidoNum.EnOrden(RecorrerPartidoInNumero);
             
                return RedirectToAction("IndexInNoPartido");
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

            Partido cg = db.listaFechaPartidoCargados.Find(x => x.FechaPartido.ToString("MMddyyyy") == id);

            if (cg == null)
            {
                return HttpNotFound();
            }

            return View(cg);
        }

        // POST: CargaPartido/Delete/5
        [HttpPost,ValidateInput(false)]
        public ActionResult Delete1(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                Partido cg = db.listaFechaPartidoCargados.Find(x => x.FechaPartido.ToString("MMddyyyy") == id);
                db.CargaPartidoFecha.Eliminar2(cg.FechaPartido);
                Escribir_EliminararchivoFecha(cg);
                if (db.CargaPartidoFecha.hizoEquilibrio == true)
                {
                    EscribirEquilibrioFecha(cg);
                }

                db.listaFechaPartidoCargados.Clear();

                db.CargaPartidoFecha.EnOrden(RecorrerPartidoInFecha);

                return RedirectToAction("IndexInFecha");
            }
            catch
            {
                return View();
            }
        }



    }
}
