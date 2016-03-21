using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using MegCiptvAccounts.Models;
using Microsoft.VisualBasic.FileIO;

namespace MegCiptvAccounts.Helper
{
    public class listManager
    {
        public listaResult ProcessaStream(Stream streamList)
        {
            listaResult viewmodel = new listaResult();

            IList<listaDados> lista = new List<listaDados>();
            int lines = 0;

            using (var parser = new TextFieldParser(streamList))
            {
                parser.CommentTokens = new[] { "#" };

                while (!parser.EndOfData)
                {
                    var currentRow = parser.ReadLine();
                    lines++;

                    if (currentRow.StartsWith("http://") && currentRow.Contains("/live/") &&
                        currentRow.EndsWith(".ts"))
                    {
                        string[] variasPartes = currentRow.Split('/');

                        string servidor = variasPartes[2];
                        string username = variasPartes[4];
                        string password = variasPartes[5];

                        bool jaExiste = false;

                        foreach (var item in lista)
                        {


                            if (
                                (item.servidor == servidor) &&
                                (item.username == username) &&
                                (item.password == password)
                                )
                            {
                                jaExiste = true;
                                break;
                            }


                        } // foreach

                        string url = string.Concat("http://", servidor, "/get.php?username=", username, "&password=",
                            password, "&type=m3u_plus&output=mpegts");

                        if (!jaExiste)

                        {
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                            req.AllowAutoRedirect = true;
                            req.Method = "HEAD";
                            req.Timeout = 5000;

                            try
                            {

                                using (WebResponse resp = req.GetResponse())
                                {
                                    if (!string.IsNullOrEmpty(resp.Headers.Get("Content-Disposition")))
                                    {
                                        lista.Add(new listaDados
                                        {
                                            servidor = servidor,
                                            username = username,
                                            password = password,
                                            working = true
                                        });
                                    }
                                    else
                                    {
                                        lista.Add(new listaDados
                                        {
                                            servidor = servidor,
                                            username = username,
                                            password = password,
                                            working = false
                                        });

                                    }
                                } // using
                            }
                            catch (Exception)
                            {
                                lista.Add(new listaDados
                                {
                                    servidor = servidor,
                                    username = username,
                                    password = password,
                                    working = false
                                });
                            }


                        } // if não existe


                    } // if http:// and /live/ and .ts




                } // while
            } // using

            viewmodel.linhasProcessadas = lines;
            viewmodel.ListaDados = lista;
            return viewmodel;
        }



    }
}

