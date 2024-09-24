using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class Base
        {
            public static BaseStatus GetStatus(int baseId)
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
                {
                    con.Open();

                    StringBuilder query = new StringBuilder();
                    query.Append($@"SELECT {nameof(BaseStatus)} FROM {DatabaseTables.Base} WHERE {nameof(Models.Base.Id)} = @{nameof(Models.Base.Id)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Id)}", baseId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (BaseStatus)reader.GetInt32(0);
                            }
                        }
                    }
                }

                return BaseStatus.Active;
            }
        }
    }
}