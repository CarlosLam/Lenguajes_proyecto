using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Nodo
    {
        string Valor { get; set; }
        Nodo HijoIzq { get; set; }
        Nodo HijoDer { get; set; }

        List<int> First { get; set; }
        List<int> Last { get; set; }
        bool Nullable { get; set; }
        int Posicion { get; set; }

        public Nodo(string valor, int pos)
        {

            this.Valor = valor;
            this.Posicion = pos;
            this.HijoDer = null;
            this.HijoIzq = null;
            First.Add(pos);
            Last.Add(pos);
            Nullable = false;
        }

        public Nodo(string valor, Nodo hijoIzq, Nodo hijoDer)
        {
            this.Valor = valor;
            this.HijoIzq = hijoIzq;
            this.HijoDer = hijoDer;
            this.Posicion = 0;

            switch(Valor)
            {
                case "|":
                    //Se añaden los first y last del hijo derecho
                    foreach (int item in hijoDer.First)
                    {
                        if (!First.Contains(item))
                        {
                            First.Add(item);
                        }
                    }
                    foreach (int item in hijoDer.Last)
                    {
                        if (!Last.Contains(item))
                        {
                            Last.Add(item);
                        }
                    }
                    //Se añaden los first y last del hijo izquierdo
                    foreach (int item in hijoIzq.First)
                    {
                        if (!First.Contains(item))
                        {
                            First.Add(item);
                        }
                    }
                    foreach (int item in hijoIzq.Last)
                    {
                        if (!Last.Contains(item))
                        {
                            Last.Add(item);
                        }
                    }
                    //Validar su nullabilidad
                    if (hijoIzq.Nullable == true || hijoDer.Nullable == true)
                        this.Nullable = true;
                    else
                        this.Nullable = false;
                    break;

                case "ZQ$": //Concatenacion
                    //Calcular el first
                    if (hijoIzq.Nullable == true)
                    {
                        foreach (int item in hijoDer.First)
                        {
                            if (!First.Contains(item))
                            {
                                First.Add(item);
                            }
                        }
                        foreach (int item in hijoIzq.First)
                        {
                            if (!First.Contains(item))
                            {
                                First.Add(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (int item in hijoIzq.First)
                        {
                            if (!First.Contains(item))
                            {
                                First.Add(item);
                            }
                        }
                    }

                    //CALCULAR EL LAST
                    if (hijoDer.Nullable == true)
                    {
                        foreach (int item in hijoIzq.Last)
                        {
                            if (!Last.Contains(item))
                            {
                                Last.Add(item);
                            }
                        }
                        foreach (int item in hijoDer.Last)
                        {
                            if (!Last.Contains(item))
                            {
                                Last.Add(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (int item in hijoDer.Last)
                        {
                            if (!Last.Contains(item))
                            {
                                Last.Add(item);
                            }
                        }
                    }
                    //Validar su nullabilidad
                    if (hijoIzq.Nullable == true && hijoDer.Nullable == true)
                        this.Nullable = true;
                    else
                        this.Nullable = false;
                    break;

                default: //Cuando se trata de *,+,?
                    //Calcular el first
                    foreach (int item in hijoIzq.First)
                    {
                        if (!First.Contains(item))
                        {
                            First.Add(item);
                        }
                    }
                    //Calcular el last
                    foreach (int item in hijoIzq.Last)
                    {
                        if (!Last.Contains(item))
                        {
                            Last.Add(item);
                        }
                    }

                    //Calcular la nullabilidad
                    if (valor == "*" || valor == "?")
                        this.Nullable = true;
                    else
                        this.Nullable = false;
                    break;
            }
        }
    }
}
