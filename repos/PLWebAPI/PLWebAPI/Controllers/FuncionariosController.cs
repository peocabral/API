using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PLWebAPI.Models;
using PLWebAPI.Business;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace PLWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        //Cria um dicionário que retorna o peso por área de atuação
        public Dictionary<string, int> pesoAreaAtuacao(){
            Dictionary<string, int> pesoFuncionario = new Dictionary<string, int>();
            pesoFuncionario.Add("diretoria", 1);
            pesoFuncionario.Add("contabilidade", 2);
            pesoFuncionario.Add("financeiro", 2);
            pesoFuncionario.Add("tecnologia", 2);
            pesoFuncionario.Add("serviços gerais", 3);
            pesoFuncionario.Add("relacionamento com o cliente", 5);
            return pesoFuncionario;
        }
        //Compara o salario do funcionário com o salario mínimo estabelecido e retorna o peso por faixa de salário
        public int pesoFaixaSalarial(string cargo, float salario){
            var is_estagiario = cargo.ToLower() == "estagiario";
            var salario_minimo = 998.00;
            if (is_estagiario || salario <= salario_minimo * 3)
            {
                return 1;
            }
            else if (salario_minimo * 3 < salario && salario <= salario_minimo * 5){
                return 2;
            }
            else if (salario_minimo * 5 < salario && salario <= salario_minimo * 8)
            {
                return 2;
            }
            else{
                return 5;
            }
        }
        //Compara a data de admissão com a data atual e devolve o peso por tempo de admissão
        public int pesoDataAdmissao(DateTime data_de_admissao)
        {
            int difdate = Convert.ToInt32((DateTime.Now - data_de_admissao).Days);
            int ano = 365;

            if (difdate <= ano)
            {
                return  1;
            }
            else if (difdate >= ano && difdate <= ano *3)
            {
                return 2;
            }
            else if(difdate >= ano*3 && difdate <= ano*8)
            {
                return 3;
            }
            else
            {
                return 5;
            }
        }
        /*Função responsável por executar a regra estabelecida para chegar ao bonus do funcionário
          Retorna o valor da participação do funcionário*/
        public decimal regraBonus(Funcionarios funcionario)
        {
            var peso_area_dict = pesoAreaAtuacao();
            var peso_area_atuacao = peso_area_dict[funcionario.area.ToLower()];
            var peso_faixa_salarial = pesoFaixaSalarial(funcionario.cargo, (float)funcionario.salario_bruto);
            var peso_tempo_admissao = pesoDataAdmissao(funcionario.data_de_admissao);

            return ((
                    (funcionario.salario_bruto * peso_tempo_admissao) +
                    (funcionario.salario_bruto * peso_area_atuacao)) /
                    (funcionario.salario_bruto * peso_faixa_salarial)) * 12;
        }

        /*Retorna uma lista de funcionários com os valores de participação, total disponibilizado, total distribuido
          e o saldo total disponibilizado*/
        [HttpGet]
        public ActionResult List()
        {
            CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
            var funcionarios = new FuncionariosBusiness().List();       
            List<Dictionary<string, object>> participacoes = new List<Dictionary<string, object>>();
            decimal total_distribuido = 0;
            decimal total_disponibilizado = 4000;
              
            foreach (var funcionario in funcionarios)
            {
                decimal valor_participacao = regraBonus(funcionario);
                total_distribuido += valor_participacao;

                Dictionary<string, object> participacao = new Dictionary<string, object>();
                participacao.Add("matricula", funcionario.matricula);
                participacao.Add("nome", funcionario.nome);
                participacao.Add("valor_da_participação",String.Format("{0:C}", valor_participacao));
                participacoes.Add(participacao);
            }

            Dictionary<string, object> resultado = new Dictionary<string, object>();
            resultado.Add("participacoes", participacoes);
            resultado.Add("total_de_funcionarios", participacoes.Count);
            resultado.Add("total_distribuido", String.Format("{0:C}", total_distribuido));
            resultado.Add("total_disponibilizado", String.Format("{0:C}",total_disponibilizado));
            resultado.Add("saldo_total_disponibilizado", String.Format("{0:C}", (total_disponibilizado - total_distribuido)));

            var result = JsonConvert.SerializeObject(resultado, Formatting.Indented);
          
            return Content(result);
        }

        // GET api/values/5
        // Retorna os valores do funcionário por ID 
        [HttpGet("{id}")]
        public ActionResult<Funcionarios> Get(int id)
        {
            return new FuncionariosBusiness().Get(id);
        }

        // POST api/values
        // não utilizado
        [HttpPost]
        public ActionResult<Funcionarios> Edit(FormCollection form)
        {
            var result = new FuncionariosBusiness().Save(
                               form["id"],
                               form["matricula"],
                               form["nome"],
                               form["area"],
                               form["cargo"],
                               form["salario_bruto"],
                               form["data_de_admissao"]);
            if (result)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Erro");
        }

        // PUT api/values/5
        // não utilizado
        [HttpPut("{id}")]
        public ActionResult<Funcionarios> Create(FormCollection form)
        {
            
            var result = new FuncionariosBusiness().Save(
                               "0",
                               form["matricula"],
                               form["nome"],
                               form["area"],
                               form["cargo"],
                               form["salario_bruto"],
                               form["data_de_admissao"]);
            if (result)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Erro");
        }

        // DELETE api/values/5
        // não utilizado
        [HttpDelete("{id}")]
        public ActionResult<Funcionarios> Delete(int id)
        {
            bool result = new FuncionariosBusiness().Delete(id);
            if (result)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Erro");
        }
    }
}
