using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Portas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CalcularController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post(object calc)
        {
            try
            {
                Dictionary<string, string> s = new Dictionary<string, string>();
                s = JsonConvert.DeserializeObject<Dictionary<string, string>>(calc.ToString());

                //incrementar os tamanhos
                string v1, v2, metodo;
                v1 = s["v1"];
                v2 = s["v2"];
                metodo = s["metodo"].ToLower();
                while (true)
                {
                    if(v1.Length == v2.Length)
                    {
                        break;
                    }
                    if(v1.Length < v2.Length)
                    {
                        v1 = "0" + v1;
                    }
                    else if (v2.Length < v1.Length)
                    {
                        v2 = "0" + v2;
                    }
                }

                string saida = "";
                //and
                if(metodo == "and")
                {
                    for (int i = 0; i < v1.Length; i++)
                    {
                        int iv1 = int.Parse(v1[i].ToString()), iv2 = int.Parse(v2[i].ToString());
                        if(iv1 == 1 && iv2 == 1)
                        {
                            saida += "1";
                        }
                        else
                        {
                            saida += "0";
                        }
                    }
                }
                else if (metodo == "or")
                {
                    for (int i = 0; i < v1.Length; i++)
                    {
                        int iv1 = int.Parse(v1[i].ToString()), iv2 = int.Parse(v2[i].ToString());
                        if (iv1 == 1 || iv2 == 1)
                        {
                            saida += "1";
                        }
                        else
                        {
                            saida += "0";
                        }
                    }
                }
                else if (metodo == "xor")
                {
                    for (int i = 0; i < v1.Length; i++)
                    {
                        int iv1 = int.Parse(v1[i].ToString()), iv2 = int.Parse(v2[i].ToString());
                        if (iv1 == 1 ^ iv2 == 1)
                        {
                            saida += "1";
                        }
                        else
                        {
                            saida += "0";
                        }
                    }
                }
                else
                {
                    saida = "Método Inválido";
                }


                Dictionary<string, string> DicionarioSaida = new Dictionary<string, string>();
                DicionarioSaida.Add("v1", v1);
                DicionarioSaida.Add("v2", v2);
                DicionarioSaida.Add("Método", metodo);
                DicionarioSaida.Add("Resultado", saida);
                return Ok(DicionarioSaida);

            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
