using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TDA.Interfaces.Delegados;

namespace TDA.Clases
{
    public class ArbolAVL<T, K>
    {
        public Nodo<T> raiz { get; set; }
        CompararNodoDlg<K> _fnCompararLave;
        ObtenerKey<T, K> _fnObtenerLlave;
        private int size;
        private int cont;
        private int altura;
        public bool hizoEquilibrio = false;

        public ArbolAVL()
        {
            raiz = null;
            size = 0;
            cont = 0;
        }

        public int CalcularAltura(Nodo<T> Nodo)
        {
            altura = 0;
            CalcularAltura(Nodo, altura);
            return altura;
        }

        public void CalcularAltura(Nodo<T> aux, int nivel)
        {
            if (aux != null)
            {
                CalcularAltura(aux.izquierdo, nivel + 1);
                if (nivel > altura)
                {
                    altura = nivel;
                }
                CalcularAltura(aux.derecho, nivel + 1);
            }
        }

        public void ActualizarFactorDeBalance(Nodo<T> aux)
        {
            if (aux != null)
            {
                ActualizarFactorDeBalance(aux.izquierdo);
                if (aux.derecho != null && aux.izquierdo != null)
                {
                    aux.factor = (CalcularAltura(aux.derecho) + 2) - (CalcularAltura(aux.izquierdo) + 2);
                }
                else if (aux.derecho == null && aux.izquierdo != null)
                {
                    aux.factor = (1 - (CalcularAltura(aux.izquierdo) + 2));
                }
                else if (aux.derecho != null && aux.izquierdo == null)
                {
                    aux.factor = ((CalcularAltura(aux.derecho) + 2) - 1);
                }
                else
                {
                    aux.factor = 0;
                }
                ActualizarFactorDeBalance(aux.derecho);
            }
        }


        public void EnOrden(RecorridoDlg<T> recorrido)
        {
            RecorridoEnOrdenInterno(recorrido, raiz);
        }



        public void Insertar(T dato)
        {
            if ((this.FuncionCompararLlave == null) || (this.FuncionObtenerLlave == null))
                throw new Exception("No se han inicializado las funciones para operar la estructura");

            if (dato == null)
                throw new ArgumentNullException("El dato ingresado está vacio");

            if (raiz == null)
                raiz = new Nodo<T>(dato);
            else
            {
                Nodo<T> siguiente = raiz;
                K llaveInsertar = this.FuncionObtenerLlave(dato);
                bool yaInsertado = false;
                hizoEquilibrio = false;
                while (!yaInsertado)
                {
                    K llaveSiguiente = this.FuncionObtenerLlave(siguiente.valor);

                    // > 0 si el primero es mayor < 0 si el primero es menor y 0 si son iguales
                    int comparacion = this.FuncionCompararLlave(llaveInsertar, llaveSiguiente);

                    if (comparacion == 0)
                    {
                        //  throw new Exception("El dato ingresado posee una llave que ya existe en la estructura");
                        yaInsertado = true;
                    }
                    else
                    {
                        if (comparacion > 0)
                        {
                            if (siguiente.derecho == null)
                            {
                                siguiente.derecho = new Nodo<T>(dato);
                                siguiente.derecho.Padre = siguiente;
                                Equilibrar(siguiente); //es Izquierdo = false, es nuevo = true;
                                yaInsertado = true;
                            }
                            else
                            {
                                siguiente = siguiente.derecho as Nodo<T>;
                            }

                        }
                        else
                        {
                            if (siguiente.izquierdo == null)
                            {
                                siguiente.izquierdo = new Nodo<T>(dato);
                                siguiente.izquierdo.Padre = siguiente;
                                Equilibrar(siguiente); //Es izquierdo = true, es nuevo true;
                                yaInsertado = true;
                            }
                            else
                            {
                                siguiente = siguiente.izquierdo as Nodo<T>;
                            }
                        }
                    }//Fin del if comparaci{on

                } //Fin del ciclo
            }
        }

        public Nodo<T> Obtenerraiz()
        {
            return raiz;
        }

        public void PostOrden(RecorridoDlg<T> recorrido)
        {
            RecorridoPostOrdenInterno(recorrido, raiz);
        }

        public void PreOrden(RecorridoDlg<T> recorrido)
        {
            RecorridoPreOrdenInterno(recorrido, raiz);
        }

        public void Equilibrar(Nodo<T> aux)
        {
            while (aux != null)
            {
                ActualizarFactorDeBalance(raiz);
                if (aux.factor < -1)
                {
                    if (aux.izquierdo.factor == 1)
                    {
                        RotacionIzquierda(aux.izquierdo);
                    }
                    RotacionDerecha(aux);
                    hizoEquilibrio = true;
                }
                else if (aux.factor > 1)
                {
                    if (aux.derecho.factor == -1)
                    {
                        RotacionDerecha(aux.derecho);
                    }
                    RotacionIzquierda(aux);
                    hizoEquilibrio = true;
                }

                aux = aux.Padre;
            }
            
        }

        public void RotacionDerecha(Nodo<T> nodo)
        {
            if (nodo != raiz)
            {
                bool derecho;
                if (nodo.Padre.derecho == nodo)
                {
                    derecho = true;
                }
                else
                {
                    derecho = false;
                }

                if (derecho)
                {
                    nodo.Padre.derecho = nodo.izquierdo;
                }
                else
                {
                    nodo.Padre.izquierdo = nodo.izquierdo;
                }

                nodo.izquierdo.Padre = nodo.Padre;
            }

            nodo.Padre = nodo.izquierdo;
            Nodo<T> auxDer = nodo.izquierdo.derecho;
            nodo.izquierdo.derecho = nodo;

            if (auxDer != null)
            {
                auxDer.Padre = nodo;
            }

            nodo.izquierdo = auxDer;

            if (nodo == raiz)
            {
                raiz = nodo.Padre;
                raiz.Padre = null;
            }
        }

        public void RotacionIzquierda(Nodo<T> nodo)
        {
            if (nodo != raiz)
            {
                bool derecho;
                if (nodo.Padre.derecho == nodo)
                {
                    derecho = true;
                }
                else
                {
                    derecho = false;
                }


                if (derecho)
                {
                    nodo.Padre.derecho = nodo.derecho;
                }
                else
                {
                    nodo.Padre.izquierdo = nodo.derecho;
                }

                nodo.derecho.Padre = nodo.Padre;
            }

            nodo.Padre = nodo.derecho;
            Nodo<T> auxIz = nodo.derecho.izquierdo;
            nodo.derecho.izquierdo = nodo;

            if (auxIz != null)
            {
                auxIz.Padre = nodo;
            }

            nodo.derecho = auxIz;

            if (nodo == raiz)
            {
                raiz = nodo.Padre;
                raiz.Padre = null;
            }
        }

        private void InsercionInterna(Nodo<T> actual, Nodo<T> nuevo)
        {
            if (actual.CompareTo(nuevo.valor) < 0)
            {
                if (actual.derecho == null)
                {
                    actual.derecho = nuevo;
                    nuevo.Padre = actual;

                }
                else
                {
                    InsercionInterna(actual.derecho, nuevo);
                }
            }
            else if (actual.CompareTo(nuevo.valor) > 0)
            {
                if (actual.izquierdo == null)
                {
                    actual.izquierdo = nuevo;
                    nuevo.Padre = actual;
                }
                else
                {
                    InsercionInterna(actual.izquierdo, nuevo);
                }
            }
        } //Fin de inserción interna.

        private void RecorridoEnOrdenInterno(RecorridoDlg<T> recorrido, Nodo<T> actual)
        {
            if (actual != null)
            {
                RecorridoEnOrdenInterno(recorrido, actual.izquierdo);

                recorrido(actual);

                RecorridoEnOrdenInterno(recorrido, actual.derecho);
            }
        }

        private void RecorridoPostOrdenInterno(RecorridoDlg<T> recorrido, Nodo<T> actual)
        {
            if (actual != null)
            {
                RecorridoEnOrdenInterno(recorrido, actual.izquierdo);

                RecorridoEnOrdenInterno(recorrido, actual.derecho);

                recorrido(actual);
            }
        }

        private void RecorridoPreOrdenInterno(RecorridoDlg<T> recorrido, Nodo<T> actual)
        {
            if (actual != null)
            {
                recorrido(actual);

                RecorridoEnOrdenInterno(recorrido, actual.izquierdo);

                RecorridoEnOrdenInterno(recorrido, actual.derecho);
            }
        }

        public void Eliminar(K _key)
        {

        }

        public CompararNodoDlg<K> FuncionCompararLlave
        {
            get
            {
                return _fnCompararLave;
            }
            set
            {
                _fnCompararLave = value;
            }
        }

        public ObtenerKey<T, K> FuncionObtenerLlave
        {
            get
            {
                return _fnObtenerLlave;
            }
            set
            {
                _fnObtenerLlave = value;
            }
        }

        public void Eliminar2(K llave)
        {
            if ((this.FuncionCompararLlave == null) || (this.FuncionObtenerLlave == null))

                throw new Exception("No se han inicializado las funciones para operar la estructura");

            if (Equals(llave, default(K)))
                throw new ArgumentNullException("La llave enviada no es valida");

            if (raiz == null)
                throw new Exception("El arbol se encuentra vacio");
            else //Si el árbol no está vacio
            {
                Nodo<T> siguiente = raiz;
                Nodo<T> padre = null;
                bool Esizquierdo = false;
                bool encontrado = false;
                hizoEquilibrio = false;
                while (!encontrado)
                {
                    K llaveSiguiente = this.FuncionObtenerLlave(siguiente.valor);
                    // > 0 si el primero es mayor < 0 si el primero es menor y 0 si son iguales
                    int comparacion = this.FuncionCompararLlave(llave, llaveSiguiente);

                    if (comparacion == 0)
                    {

                        if ((siguiente.derecho == null) && (siguiente.izquierdo == null)) //Si es una hoja
                        {
                            T miDato = siguiente.valor;
                            if ((padre != null))
                            {
                                if (Esizquierdo)
                                    padre.izquierdo = null;
                                else
                                    padre.derecho = null;
                            }
                            else //Si padre es null entonces es la raiz
                            {
                                raiz = null;
                            }
                            Equilibrar(padre);
                            return;
                            //  return miDato;
                        }
                        else
                        {
                            if (siguiente.derecho == null) //Si solo tiene rama izquierdo
                            {
                                T miDato = siguiente.valor;
                                if ((padre != null))
                                {
                                    if (Esizquierdo)
                                        padre.izquierdo = siguiente.izquierdo;
                                    else
                                        padre.derecho = siguiente.derecho;

                                    Equilibrar(padre);
                                }
                                else
                                {
                                    raiz = siguiente.izquierdo as Nodo<T>;
                                }
                                return;
                                //   return miDato;
                            }
                            else if (siguiente.izquierdo == null)  //Si solo tiene rama derecho
                            {
                                T miDato = siguiente.valor;
                                if ((padre != null))
                                {
                                    if (Esizquierdo)
                                        padre.izquierdo = siguiente.derecho;
                                    else
                                        padre.derecho = siguiente.derecho;

                                    Equilibrar(padre);
                                }
                                else
                                {
                                    raiz = siguiente.derecho as Nodo<T>;
                                }
                                return;
                                //      return miDato;
                            }
                            else  //Tiene ambas ramas el que lo sustituirá será el mas izquierdo de los derechos
                            {
                                Nodo<T> aEliminar = siguiente;
                                siguiente = siguiente.derecho as Nodo<T>;
                                int cont = 0;
                                while (siguiente.izquierdo != null)
                                {
                                    padre = siguiente;
                                    siguiente = siguiente.izquierdo as Nodo<T>;
                                    cont++;
                                }

                                if (cont > 0)
                                {
                                    if (padre != null)
                                    {
                                        T miDato = aEliminar.valor;
                                        aEliminar.valor = siguiente.valor;
                                        padre.izquierdo = null;
                                        Equilibrar(padre);
                                        return;
                                        //        return miDato;
                                    }

                                }
                                else
                                {
                                    siguiente.izquierdo = aEliminar.izquierdo;

                                    if (padre != null)
                                    {
                                        if (Esizquierdo)
                                        {
                                            padre.izquierdo = aEliminar.derecho;
                                            Equilibrar(siguiente);
                                        }
                                        else
                                        {
                                            padre.derecho = aEliminar.derecho;
                                            Equilibrar(siguiente);
                                        }
                                    }
                                    else //Es la raiz
                                    {
                                        if (Esizquierdo)
                                            raiz = aEliminar.derecho as Nodo<T>;
                                        else
                                            raiz = aEliminar.derecho as Nodo<T>;
                                        Equilibrar(raiz);
                                    }
                                    return;

                                    //       return aEliminar.valor;
                                }

                            }
                        }
                    }
                    else
                    {
                        if (comparacion > 0)
                        {
                            if (siguiente.derecho == null)
                            {
                                return;
                                //   return default(T);
                            }
                            else
                            {
                                padre = siguiente;
                                Esizquierdo = false;
                                siguiente = siguiente.derecho as Nodo<T>;
                            }

                        }
                        else //menor que 0
                        {
                            if (siguiente.izquierdo == null)
                            {
                                return;
                                // return default(T);
                            }
                            else
                            {
                                padre = siguiente;
                                Esizquierdo = true;
                                siguiente = siguiente.izquierdo as Nodo<T>;
                            }
                        }
                    }//Fin del if comparaci{on

                } //Fin del ciclo

            }//Fin del if que verifica que no exista ningún dato.
            return;
            //   return default(T);
        }


        public bool ValidacionArbolDegenerado(Nodo<T> actual)
        {
            cont = 0;
            Validarderecho(actual);
            if (actual.derecho == null)
            {
                Validarizquierdo(actual);
            }
            else
            {
                Validarizquierdo(actual.izquierdo);
            }

            if (cont == size)
            {
                return true;
            }
            return false;
        }

        public void Validarderecho(Nodo<T> actual)
        {
            if (actual != null)
            {
                while (actual.derecho != null)
                {
                    if (actual.factor == 1)
                    {
                        cont++;
                    }
                    actual = actual.derecho;
                    if (actual.izquierdo != null)
                    {
                        Validarizquierdo(actual);
                    }
                }
            }
        }

        public void Validarizquierdo(Nodo<T> actual)
        {
            if (actual != null)
            {
                while (actual.izquierdo != null)
                {
                    if (actual.factor == 1)
                    {
                        cont++;
                    }
                    actual = actual.izquierdo;
                    if (actual.derecho != null)
                    {
                        Validarderecho(actual);
                    }
                }
            }
        }
    }
}
