using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Numerics;
using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class Cartao
{
    public Cartao(string adquirente, List<Taxa> taxas)
    {
        Adquirente = adquirente;
        Taxas = taxas;
    }
    public string Adquirente { get; private set; }
    public List<Taxa> Taxas { get; private set; }
}
public class Taxa
{
    public Taxa(string bandeira, decimal credito, decimal debito)
    {
        Bandeira = bandeira;
        Credito = credito;
        Debito = debito;
    }

    public string Bandeira { get; private set; }

    
    public decimal Credito { get; private set; }
    public decimal Debito { get; private set; }
}

public class Retorno
{
    public Retorno(decimal valorLiquido)
    {
        ValorLiquido = valorLiquido;
    }

    public decimal ValorLiquido { get; private set; }
}

namespace DesafioDesenvolvimento.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MdrController : ControllerBase
    {
        public List<Taxa> teste = new List<Taxa>();
        public List<Taxa> teste2 = new List<Taxa>();
        public List<Taxa> teste3 = new List<Taxa>();
        public List<Cartao> adquirentes = new List<Cartao>();
        public double valorLiquido ;

        private readonly ILogger<MdrController> _logger;

        public MdrController(ILogger<MdrController> logger)
        {
            _logger = logger;

            teste.Add(new Taxa("Visa", 2.25M, 2.00M));
            teste.Add(new Taxa("Master", 2.35M, 1.98M));
            teste2.Add(new Taxa("Visa", 2.50M, 2.08M));
            teste2.Add(new Taxa("Master", 2.65M, 1.75M));
            teste3.Add(new Taxa("Visa", 2.75M, 2.16M));
            teste3.Add(new Taxa("Master", 3.10M, 1.58M));
            

            adquirentes.Add(new Cartao(
                "Adquirente A",
                teste
            ));
                adquirentes.Add(new Cartao(
                "Adquirente B",
                teste2
            ));
            adquirentes.Add(new Cartao(
                "Adquirente C",
                teste3
            ));
        }
        


        [HttpGet()]

        public IActionResult Get()
        {
            string json = JsonConvert.SerializeObject(adquirentes, Formatting.Indented,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return Ok(json);
        }
        [HttpPost("/transaction")]

        public IActionResult Post(String Adquirente,string Bandeira, String Tipo, Decimal Valor )
        {
            var bandeiraMaiusculo = Bandeira.ToUpper();
            //var tipoMaiusculo = Tipo.ToUpper();

            var adq =
            adquirentes.Where(

                x => x.Adquirente.Equals("Adquirente " +Adquirente)
                ).FirstOrDefault();

            var band = adq.Taxas.Where(
                y => y.Bandeira.ToUpper().Equals(bandeiraMaiusculo)
                ).FirstOrDefault();

            var taxa = Tipo.ToUpper().Equals("DEBITO") ? band.Debito : band.Credito;

            Retorno solucao = new Retorno(Valor - (Valor * taxa / 100));

            string json = JsonConvert.SerializeObject(solucao, Formatting.Indented,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }); ;

            /*var teste = adquirentes;
            if (adquirentes == teste ) {
                var percent = 2.25;
                valorLiquido = Valor - (Valor * percent / 100);
            }
            else { }*/

            
            return Ok( json);
        }

        

        private bool IsNumeric(string strNumber)
        {
            double number;
            bool isNumber = double.TryParse(strNumber, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo,
                out number);
            return isNumber;
        }
        
       
    }
}