using Lab3EDI_1C18.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TDA.Clases;

namespace Lab3EDI_1C18.DBContext
{
    public class DefaultConnection
    {
        private static volatile DefaultConnection Instance;
        private static object syncRoot = new Object();

        public List<Partido> listaPartido = new List<Partido>();

        public ArbolAVL<Partido, int> arbolNoPartido = new ArbolAVL<Partido, int>();
        public List<Partido> listaNoPartido = new List<Partido>();

        public ArbolAVL<Partido, DateTime> arbolFechaPartido = new ArbolAVL<Partido, DateTime>();
        public List<Partido> listaFechaPartido = new List<Partido>();


        public int IDActual { get; set; }

        private DefaultConnection()
        {
            IDActual = 0;
        }

        public static DefaultConnection getInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (syncRoot)
                    {
                        if (Instance == null)
                        {
                            Instance = new DefaultConnection();
                        }
                    }
                }
                return Instance;
            }
        }
    }
}