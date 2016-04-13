using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data;
using System.Data.SqlClient;
using Hangfire;

namespace SignalRDemo.hubs
{
    public class SigHub:Hub
    {

            /// <summary>
            /// Sends message through the chat interface
            /// </summary>
            /// <param name="username">taken from user input</param>
            /// <param name="message">taken from user input</param>
            public void Send(string username, string message)
            {
                Clients.All.sendmessage(username, message);
            }

            /// <summary>
            /// Uses the send Message function to display when a user logs in
            /// Also add's their name to a list of users
            /// </summary>
            /// <param name="username"></param>
            public void Login(string username)
            {
            Send(username, "logged in");
            }

            /// <summary>
            /// use SendMessage to alert the chat board
            /// Remove user from list
            /// </summary>
            /// <param name="username"></param>
            public void Logout(string username)
            {
            Send(username, "logged out");
            }

        /// <summary>
        /// Synchronize new users with table and chat data
        /// </summary>
        public void Synchronize()
        {
           
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection("Data Source = winserv; Initial Catalog = gilmourd_db; Integrated Security = True");
            //string connString = @"Data Source = winserv; Initial Catalog = gilmourd_db; Integrated Security = True";
            string query = "SELECT * FROM [Signal_R_Draw]";
            // SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            if (dataTable.Rows.Count != 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Clients.All.redraw(row.ItemArray[1], row.ItemArray[2]);
                }
            }
        }

        public bool Truncate()
        {
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection("Data Source = winserv; Initial Catalog = gilmourd_db; Integrated Security = True");
            //string connString = @"Data Source = winserv; Initial Catalog = gilmourd_db; Integrated Security = True";
            string query = "Truncate Table [Signal_R_Draw]";
            // SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();     
            cmd.ExecuteNonQuery();        
            conn.Close();
            return Truncate();   
        }

        /// <summary>
        /// Update the Table with each click
        /// </summary>
        public void Redraw(string cellID, string color)
        {

            SqlConnection conn = new SqlConnection("Data Source = winserv; Initial Catalog = gilmourd_db; Integrated Security = True");
            conn.Open();
            SqlCommand command = new SqlCommand("INSERT INTO [Signal_R_Draw](ID, CellID, Color) values(1,'" + cellID + "','" + color + "')", conn);
            command.ExecuteNonQuery();
            conn.Close();
            Clients.All.redraw(cellID, color);
        }
    }
}