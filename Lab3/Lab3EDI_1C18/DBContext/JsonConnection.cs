using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TDA.Clases;
using Lab3EDI_1C18.Models;
using System.IO;

namespace Lab3EDI_1C18.DBContext
{
    public class JsonConnection
    {
        private static volatile JsonConnection Instance;
        private static object syncRoot = new Object();

        public List<Partido> listaPartido = new List<Partido>();

        public ArbolAVL<Partido, int> CargaPartidoNum = new ArbolAVL<Partido, int>();
        public List<Partido> listaNoPartidoCargados = new List<Partido>();

        public ArbolAVL<Partido, DateTime> CargaPartidoFecha = new ArbolAVL<Partido, DateTime>();
        public List<Partido> listaFechaPartidoCargados = new List<Partido>();

        

        public int IDActual { get; set; }

        private JsonConnection()
        {
            
            IDActual = 0;
        }

        public static JsonConnection getInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (syncRoot)
                    {
                        if (Instance == null)
                        {
                            Instance = new JsonConnection();
                        }
                    }
                }
                return Instance;
            }
        }
    }
}