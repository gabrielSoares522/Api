using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
            var filtro = Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento);
            
            var atualizacoes = Builders<Infectado>.Update.Set("sexo",dto.Sexo);
            atualizacoes = atualizacoes.Set("latitude",dto.Latitude);
            atualizacoes = atualizacoes.Set("longitude",dto.Longitude);

            _infectadosCollection.UpdateOne(filtro, atualizacoes);
            
            return Ok("atualizado com sucesso");
        }

        [HttpDelete("{dataNasc}")]
        public ActionResult RemoverInfectado(DateTime dataNasc)
        {
            var filtro = Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dataNasc);
            _infectadosCollection.DeleteOne(filtro);
            
            return Ok("removido com sucesso");
        }
    }
}