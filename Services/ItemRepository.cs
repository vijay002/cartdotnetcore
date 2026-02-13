using demoapp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace demoapp.Services
{
    public class ItemRepository
    {
        private readonly string _connectionstring;
        public ItemRepository(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<ItemModelView>> GetItemList(string path)
        {
            var result = new List<ItemModelView>();
            try
            {
                await using var connection = new SqlConnection(_connectionstring);
                await using var command = new SqlCommand("uspGetItems",connection);
                {
                    command.CommandType = CommandType.StoredProcedure;
                }
                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(new ItemModelView
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader["ItemName"] as string,
                        Category = reader["Category"] as string,
                        Description = reader["Description"] as string,
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        //Image = $"{path}/{reader["imagepath"]}"
                        ImagePath = $"{path}/{reader["imagepath"]}"
                        //Image = path + @"//" + Convert.ToString(reader["imagepath"])
                    });
                }

                return result;
                /*

                using (var conn = new SqlConnection(_connectionstring))
                {
                    SqlCommand cmd = new SqlCommand("uspGetItems", conn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    {
                        //reader.AsQueryable
                        result.Add(new ItemModelView()
                        {
                            Id = Convert.ToInt32(reader["Id"].ToString()),
                            Name = reader["ItemName"].ToString(),
                            Category = reader["Category"].ToString(),
                            Description = reader["Description"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                            Price = Convert.ToDecimal(reader["Price"].ToString()),
                            Image = path +@"//" + Convert.ToString(reader["imagepath"])
                        });
                    }
                    conn.Close();
                }
                */
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }

        public bool InsertItem(ItemCreateModelView create)
        {
            bool result = false;
            try
            {
                
                using (var conn = new SqlConnection(_connectionstring))
                {
                    conn.Open();
                    var command = new SqlCommand("uspInsertItem", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.VarChar).Value = create.Id;
                    command.Parameters.Add("@ItemName", SqlDbType.VarChar).Value = create.Name;
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = create.Description;
                    command.Parameters.Add("@Category", SqlDbType.VarChar).Value = create.Category;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = create.Quantity;
                    command.Parameters.Add("@Price", SqlDbType.Int).Value = create.Price;
                    command.Parameters.Add("@Imagepath", SqlDbType.VarChar).Value = create.FileName;
                    command.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = create.createdby;

                    var resultobj= command.ExecuteScalar();
                    //return true;
                }
               
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }

        public async Task<bool> DeletebyId(int id)
        {
            await using var conn = new SqlConnection(_connectionstring);
            await conn.OpenAsync();
            await using var command = new SqlCommand("Delete from tblItem where id = @id", conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
            var result = command.ExecuteNonQuery();
            if (result > 0)
                return true;

            return false;
        }

        public async Task<ItemCreateModelView> GetItemById(int id)
        {
            ItemCreateModelView result = new ItemCreateModelView();
            string path = Path.Combine("images");
            await using var connect = new SqlConnection(_connectionstring);
            await using var command = new SqlCommand("select * from tblItem where id = @id ", connect);
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
            }

            await connect.OpenAsync();
            //await using var reader =  await command.ExecuteScalarAsync();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result = new ItemCreateModelView()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader["ItemName"] as  string,
                    Category = reader["Category"] as string,
                    Description = reader["Description"] as string,
                     //= path + @"/" +reader["ImagePath"] as string,
                    FileName = reader["ImagePath"] as string,
                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                };

            }

            return result;
        }

        public async Task<bool> CheckoutProcess(List<ItemModelView> items)
        {
            await using SqlConnection conn = new SqlConnection(_connectionstring);
            await conn.OpenAsync();
            
            foreach (var item in items)
            {
                await using SqlCommand cmd = new SqlCommand("uspCheckOutProcess", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@itemid",item.Id ));
                cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                cmd.Parameters.Add(new SqlParameter("@Price", item.Price));
                cmd.ExecuteNonQuery();
            }
            return true;
        }

    }
}
