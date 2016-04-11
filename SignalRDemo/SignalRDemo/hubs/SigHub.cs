using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

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
            public void Synchronize(string id, string color)
            {
            //get updates

            //convert to list

            //for loop
            //call redraw with cellID and Color
            Redraw(id, color);
                
            }

            /// <summary>
            /// Update the Table with each click
            /// </summary>
            public void Redraw(string cellID, string color)
            {
                Clients.All.redraw(cellID, color);
            }
        }
}