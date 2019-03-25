using PLWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace PLWebAPI.DAO
{
    public class FuncionariosDAO : Connection
    {
        // Executa uma query que retorna uma lista de funcionários
        internal List<Funcionarios> List()
        {
            return db.Query<Funcionarios>("SELECT * FROM Funcionarios").ToList();
        }
        // Executa uma query que retorna um registro de funcionários
        internal Funcionarios Get(int id)
        {
            return db.Query<Funcionarios>("SELECT * FROM Funcionarios Where ID=@id", new { id = id }).SingleOrDefault();
        }
        // Executa uma query que apaga um registro de funcionários
        internal bool Delete(int id)
        {
            return db.Execute("Delete Funcionarios where id=@id", new { id = id }) == 1;
        }
        // Executa uma query que insere um registro em funcionários
        internal bool Insert(Funcionarios funcionario)
        {
            return db.Execute("Insert into Funcionarios(matricula,nome,area,cargo,salario_bruto,data_de_admissao) values (@matricula,@nome,@area,@cargo,@salario_bruto,@data_de_admissao)",
                new
                {
                    matricula = funcionario.matricula,
                    nome = funcionario.nome,
                    area = funcionario.area,
                    cargo = funcionario.cargo,
                    salario_bruto = funcionario.salario_bruto,
                    data_de_admissao = funcionario.data_de_admissao
                }) == 1;
        }
        // Executa uma query que edita um registro em funcionários
        internal bool Update(Funcionarios funcionario)
        {
            return db.Execute(@"Update Funcionarios SET matricula=@matricula,nome=@nome,area=@area,cargo=@cargo,salario_bruto=@salario_bruto,data_de_admissao=@data_de_admissao where id=@id",
                new
                {
                    id = funcionario.id,
                    matricula = funcionario.matricula,
                    nome = funcionario.nome,
                    area = funcionario.area,
                    cargo = funcionario.cargo,
                    salario_bruto = funcionario.salario_bruto,
                    data_de_admissao = funcionario.data_de_admissao
                }) == 1;
        }
    }
}

