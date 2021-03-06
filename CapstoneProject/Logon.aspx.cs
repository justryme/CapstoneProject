﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class Logon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void Login_Click(object sender, EventArgs e)
        {
            LdapAuthentication adAuth = new LdapAuthentication();
            try
            {
                if (true == adAuth.IsAuthenticated(txtUsername.Text, txtPassword.Text))
                {
                    //string groups = adAuth.GetGroups(txtDomain.Text, txtUsername.Text, txtPassword.Text);
                    string userData = string.Empty;

                    //Create the ticket, and add the groups.
                    bool isCookiePersistent = chkPersist.Checked;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, txtUsername.Text, DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, userData);

                    //Encrypt the ticket.
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    //Create a cookie, and then add the encrypted ticket to the cookie as data.
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    if (true == isCookiePersistent)
                        authCookie.Expires = authTicket.Expiration;

                    //Add the cookie to the outgoing cookies collection.
                    Response.Cookies.Add(authCookie);

                    //You can redirect now.
                    Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
                }
                else
                {
                    errorLabel.Text = "Authentication did not succeed. Check user name and password.";
                }
            }
            catch (Exception ex)
            {
                errorLabel.Text = "Error authenticating. " + ex.Message;
            }
        }

    }
}