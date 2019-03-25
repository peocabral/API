using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PLWebAPI.DAO;
using PLWebAPI.Models;

namespace PLWebAPI.Business
{
    public class FuncionariosBusiness : FuncionariosDAO
    {
        // Retorna uma lista de funcionários 
        public new List<Funcionarios> List()
        {
            var list = base.List();
            list = list.OrderBy(o => o.nome).ToList();
            return list;
        }
        // Retorna um funcionário
        public new Funcionarios Get(int id)
        {
            return base.Get(id);
        }
        //Deleta registros
        // não utilizado
        public new bool Delete(int id)
        {
            return base.Delete(id);
        }
        //Insere registros
        // não utilizado
        public bool Save(string id, string matricula, string nome, string area, string cargo, string salario_bruto, string data_de_admissao)
        {
            Funcionarios funcionario = new Funcionarios();
            funcionario.id = Convert.ToInt32(id);
            funcionario.matricula = matricula.Trim();
            funcionario.nome = nome.Trim();
            funcionario.area = area.Trim();
            funcionario.cargo = cargo.Trim();
            funcionario.salario_bruto = Convert.ToDecimal(id);
            funcionario.data_de_admissao = Convert.ToDateTime(data_de_admissao);
            if (funcionario.id == 0)
                return base.Insert(funcionario);
            else
                return base.Update(funcionario);
        }
    }
}
