using MySql.Data.MySqlClient;

public class Conexao
{
    private static string connStr = "server=localhost;database=onbiscato;uid=root;pwd=;";
    public static MySqlConnection Conectar()
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        conn.Open();
        return conn;
    }
}

