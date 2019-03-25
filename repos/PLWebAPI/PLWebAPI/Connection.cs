using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PLWebAPI
{
    public class Connection
    {
        public SqlConnection db = null;
        //Cria uma conexão com o banco de dados
        public Connection()
        {
            db = new SqlConnection(@"Server=.\SQL3;Database=db_webapi;Trusted_Connection=True;");
        }
    }
}
