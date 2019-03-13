using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Cursor Files|*.txt",
                Title = "Select a Cursor File"
            };

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label1.Text = openFileDialog1.FileName;
                StreamReader st = new StreamReader(openFileDialog1.FileName);
                string sLine = "";
                
                List<string> sets = new List<string>();
                List<string> tokens = new List<string>();
                List<string> actions = new List<string>();
                string error = "";

                bool isSets = false;
                bool isToken = false;
                bool isAction = false;

                int linea = 0;
                while (sLine != null)
                {
                    linea++;
                    sLine = st.ReadLine();
                    
                    if (sLine != null)
                    {
                        if (sLine.Trim() == "SETS")
                        {
                            isSets = true;
                        }
                        else if (sLine.Trim() == "TOKENS")
                        {
                            isSets = false;
                            isToken = true;
                        }
                        else if (sLine.Trim() == "ACTIONS")
                        {
                            isToken = false;
                            isAction = true;
                        }
                        else
                        {
                            if (isSets)
                            {
                                string nombreSet = sLine.Substring(0, sLine.IndexOf('=')).Trim();
                                string valorSet = sLine.Substring(sLine.IndexOf('=') + 1).Trim();
                                int x = 0;
                                while (x<valorSet.Length)
                                {
                                    string chara = valorSet.Substring(x, 1);
                                    if (chara == "'")
                                    {
                                        int y = 2;
                                        string prueba = valorSet.Substring(x, y);
                                        while (prueba.EndsWith("'") == false)
                                        {
                                            y++;
                                            //Se debe validar que todavia haya espacio, en caso contrario error en linea
                                            try
                                            {
                                                prueba = valorSet.Substring(x, y);
                                            }catch(Exception)
                                            {
                                                label1.Text = "Error en linea: " + linea;
                                                break;
                                            }
                                        }
                                        x = x + y;
                                    }
                                    else if(chara == ".")
                                    {
                                        if (valorSet.Substring(x,2) == "..")
                                        {
                                            x++;
                                        }
                                        else
                                        {
                                            //Error en la linea;
                                            break;
                                        }
                                        x++;
                                    }
                                    else if(chara == " ")
                                    {
                                        x++;
                                    }
                                    else if (chara == "+")
                                    {
                                        //Cuando se tiene que unir dos extremos de un array -implementacion pendiente- xdxd
                                        x++;
                                    }
                                    else if (valorSet.Contains("CHR("))
                                    {
                                        int y = 2;
                                        string prueba = valorSet.Substring(x, y);
                                        while (prueba.EndsWith(")") == false)
                                        {
                                            y++;
                                            //Se debe validar que todavia haya espacio, en caso contrario error en linea
                                            try
                                            {
                                                prueba = valorSet.Substring(x, y);
                                            }
                                            catch (Exception)
                                            {
                                                label1.Text = "Error en linea: " + linea;
                                                break;
                                            }
                                        }
                                        x = x + y;
                                    }
                                }
                                sets.Add(sLine);
                            }
                            else if (isToken) //Cuando se trata de un TOKEN
                            {
                                string nombreToken = sLine.Substring(0, sLine.IndexOf('=')).Trim();
                                string valorToken = sLine.Substring(sLine.IndexOf('=') + 1).Trim();
                                tokens.Add(sLine);
                                int x = 0;
                                while (x < valorToken.Length)
                                {
                                    string chara = valorToken.Substring(x, 1);
                                    if (chara == "'")
                                    {
                                        int y = 2;
                                        string prueba = valorToken.Substring(x, y);
                                        while (prueba.EndsWith("'") == false)
                                        {
                                            y++;
                                            //Se debe validar que todavia haya espacio, en caso contrario error en linea
                                            try
                                            {
                                                prueba = valorToken.Substring(x, y);
                                            }
                                            catch (Exception)
                                            {
                                                label1.Text = "Error en linea: " + linea;
                                                break;
                                            }
                                        }
                                        x = x + y;
                                    }
                                    else if (chara == " ")
                                    {
                                        x++;
                                    }
                                    else if (chara == "(")
                                    {
                                        //QUE ESTE SEA MI ULTIMO CASO POR QUE ES EL QUE MERECE COPY PASTE JEJE
                                    }
                                    else if (chara == "*")
                                    {
                                        //SOLO DEBEMOS VALIDAR QUE NO SE ENCUENTRE NINGUN OTRO ASTERISCO CERCA
                                        x++;
                                    }
                                    else if (chara == "|")
                                    {
                                        //Solo se debe validar que no venga ningun otro OR cerca
                                        x++;
                                    }
                                    else
                                    {
                                        //Se trata de un set, validar que exista
                                        int y = x;
                                        char prueba = valorToken[y];
                                        
                                        while (char.IsLetterOrDigit(prueba))
                                        {
                                            y++;
                                            if (y < valorToken.Length)
                                            {
                                                prueba = valorToken[y];
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        bool existe = false;
                                        chara = valorToken.Substring(x, y-x);
                                        foreach (string item in sets)
                                        {
                                            string nombreSet = item.Substring(0, sLine.IndexOf('=')).Trim();
                                            if (nombreSet == chara)
                                            {
                                                existe = true;
                                            }
                                        }
                                        if (!existe)
                                        {
                                            label1.Text = "Error en linea:" + linea;
                                        }
                                        x = y;
                                    }
                                }
                            }
                            else if (isAction && sLine.Contains("ERROR"))
                            {
                                if (sLine.Equals("ERROR = 54"))
                                {
                                    error = sLine;
                                }
                                else
                                {
                                    label1.Text = "Error en la linea: " + linea;
                                }
                            }
                            else if (isAction)
                            {
                                actions.Add(sLine);
                            }
                        }
                    }       
                }
                st.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}
