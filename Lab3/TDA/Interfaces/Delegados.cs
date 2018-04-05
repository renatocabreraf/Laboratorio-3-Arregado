using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDA.Clases;


namespace TDA.Interfaces
{
    public class Delegados
    {
        public delegate int CompararNodoDlg<T>(T actual, T nuevo);
        public delegate void RecorridoDlg<T>(Nodo<T> actual);
        public delegate K ObtenerKey<T, K>(T dato);
    }
}
