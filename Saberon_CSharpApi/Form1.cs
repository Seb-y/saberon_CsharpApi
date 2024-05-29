using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CsharpApi
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                txtOutput.Clear();
                HttpResponseMessage response = await client.GetAsync("http://localhost/ACT%208/saberon_phpapi/api.php");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                var json = JObject.Parse(responseBody);
                var formattedOutput = FormatJsonOutput(json);

                txtOutput.Text = formattedOutput;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string FormatJsonOutput(JObject json)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Users:");
            foreach (var user in json["users"])
            {
                sb.AppendLine($"  ID: {user["userid"]}");
                sb.AppendLine($"  Username: {user["username"]}");
                sb.AppendLine($"  Email: {user["email"]}");
                sb.AppendLine();
            }

            sb.AppendLine("Departments:");
            foreach (var dept in json["departments"])
            {
                sb.AppendLine($"  ID: {dept["dnumber"]}");
                sb.AppendLine($"  Name: {dept["dname"]}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private async void btnPost_Click(object sender, EventArgs e)
        {
            var userData = new { username = txtUsername.Text, pass = txtPassword.Text, email = txtEmail.Text };
            string json = JsonConvert.SerializeObject(userData);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost/ACT%208/saberon_phpapi/api.php", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                txtOutput.Text = responseBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private async void btnPostDept_Click(object sender, EventArgs e)
        {
            var deptData = new { dname = txtDeptName.Text };
            string json = JsonConvert.SerializeObject(deptData);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost/ACT%208/saberon_phpapi/api.php", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                txtOutput.Text = responseBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}