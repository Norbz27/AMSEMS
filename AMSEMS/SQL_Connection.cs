using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSEMS
{
    internal class SQL_Connection
    {
        //cloud
        //public static string connection = @"Server=tcp:norbz.database.windows.net,1433;Initial Catalog=db_Amsems;Persist Security Info=False;User ID=CloudSA4a47677a@norbz;Password=nozurbnorberto27@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;";

        //local
        //public static string connection = @"Data Source=LAPTOP-78S661F5;Initial Catalog=db_Amsems;Integrated Security=True";

        //ngrok
        static string server = "0.tcp.ap.ngrok.io,15802"; // Ngrok tunnel URL
        static string database = "db_Amsems"; // Database name
        static string username = "nor"; // Database username
        static string password = "12345"; // Database password

        public static string connection = $"Server={server};Database={database};User Id={username};Password={password};";

    }
}
