using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _connectionString = "Data Source=data.db";

        public async Task GetChatData(string idr)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT user.fname, chat.message, chat.date, chat.data, chat.cID, chat.id1 FROM chat JOIN user ON chat.id1 = user.uID WHERE chat.idr = '{idr}'";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var fname = reader.GetString(0);
                        var message = reader.GetString(1);
                        var date = reader.GetString(2);
                        var user_data = reader.GetString(3);
                        var cID = reader.GetString(4);
                        var id1 = reader.GetString(5);
                        await Clients.Caller.SendAsync("ReceiveMessage", fname, message, user_data, date, cID, id1);
                    }
                }
                
                connection.Close();
            }
        }

        public async Task SendMessage(string id1, string id2, string message, string user_data, string date, string idr, string fname)
        {
            int? cID = null;
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO chat (id1, id2, message, data, date, idr) VALUES ('{id1}', '{id2}', '{message}', '{user_data}', '{date}', '{idr}')";
                command.ExecuteNonQuery();

                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT MAX(cID) FROM chat";
                var result = selectCommand.ExecuteScalar();

                cID = Convert.ToInt32(result);

                connection.Close();
            }
            string notice = $"MESSAGE FROM {fname}!!!";

            // Menggunakan connectionId sebagai id2.
            await Clients.Group(idr).SendAsync("ReceiveMessage", fname, message, user_data, date, cID, id1);
            await Clients.Group(id2).SendAsync("noticeMessage", notice);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}




