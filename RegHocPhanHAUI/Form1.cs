using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;

namespace RegHocPhanHAUI
{
    public partial class Form1 : Form
    {
        private DataTable dataTable;
        private BindingSource bindingSource;
        private DataTable dtClass;
        private BindingSource bSClass;

        public string kverify;
        public string cookie;

        public Form1()
        {
            InitializeComponent();
        }

        private DataTable ConvertToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            // Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                // Setting column names as Property names
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    // Inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        async void GetAllModules(string kv, string cookie)
        {
            var options = new RestClientOptions()
            {

                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://sv.haui.edu.vn/ajax/register/action.htm?cmd=moduleopen&v=" + kv, Method.Post);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("accept-language", "en-US,en;q=0.9,vi;q=0.8,ar;q=0.7,de;q=0.6");
            request.AddHeader("content-length", "0");
            request.AddHeader("cookie", cookie);
            request.AddHeader("origin", "https://sv.haui.edu.vn");
            request.AddHeader("priority", "u=1, i");
            request.AddHeader("referer", "https://sv.haui.edu.vn/register/");
            request.AddHeader("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            RestResponse response = await client.ExecuteAsync(request);
            List<Modules> parsed_json = JsonConvert.DeserializeObject<List<Modules>>(response.Content);

            // Convert the list to a DataTable
            dataTable = ConvertToDataTable(parsed_json);

            // Initialize BindingSource and set the DataSource to the DataTable
            bindingSource = new BindingSource();
            bindingSource.DataSource = dataTable;

            // Set the DataGridView DataSource to the BindingSource
            dgvModules.DataSource = bindingSource;

            textBox1.TextChanged += textBox1_TextChanged;

        }

        async void GetClassByModules(string fid, string kv, string cookie)
        {
            var options = new RestClientOptions()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://sv.haui.edu.vn/ajax/register/action.htm?cmd=classbymodulesid&v=" + kv, Method.Post);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("accept-language", "en-US,en;q=0.9,vi;q=0.8,ar;q=0.7,de;q=0.6");
            request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("cookie", cookie);
            request.AddHeader("origin", "https://sv.haui.edu.vn");
            request.AddHeader("priority", "u=1, i");
            request.AddHeader("referer", "https://sv.haui.edu.vn/register/");
            request.AddHeader("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            var body = @"fid=" + fid;
            request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);
            ClassByModules.Root parsed_json = JsonConvert.DeserializeObject<ClassByModules.Root>(response.Content);

            bSClass = new BindingSource();
            bSClass.DataSource = parsed_json.data;

            // Set the DataGridView DataSource to the BindingSource
            dgvClass.DataSource = bSClass;

            textBox2.TextChanged += textBox2_TextChanged;


        }

        async void AddClass(string ClassID, string kv, string cookie)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://sv.haui.edu.vn/ajax/register/action.htm?cmd=addclass&v=" + kv);
            request.Headers.Add("accept", "application/json, text/javascript, */*; q=0.01");
            request.Headers.Add("accept-language", "en-US,en;q=0.9,vi;q=0.8,ar;q=0.7,de;q=0.6");
            //request.Headers.Add("content-length", "128");
            request.Headers.Add("cookie", cookie);
            request.Headers.Add("origin", "https://sv.haui.edu.vn");
            request.Headers.Add("priority", "u=1, i");
            request.Headers.Add("referer", "https://sv.haui.edu.vn/register/");
            request.Headers.Add("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            request.Headers.Add("sec-fetch-dest", "empty");
            request.Headers.Add("sec-fetch-mode", "cors");
            request.Headers.Add("sec-fetch-site", "same-origin");
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");
            request.Headers.Add("x-requested-with", "XMLHttpRequest");
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(ClassID), "class");
            content.Add(new StringContent("886"), "ctdk");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            MessageBox.Show(await response.Content.ReadAsStringAsync());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Apply filter to the BindingSource based on the TextBox input
            string filter = textBox1.Text;
            if (string.IsNullOrWhiteSpace(filter))
            {
                bindingSource.RemoveFilter();
            }
            else
            {
                // Filter the data by the "Name" column
                bindingSource.Filter = $"ModulesName LIKE '%{filter}%'";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Apply filter to the BindingSource based on the TextBox input
            string filter = textBox2.Text;
            if (string.IsNullOrWhiteSpace(filter))
            {
                bSClass.RemoveFilter();
            }
            else
            {
                // Filter the data by the "Name" column
                bSClass.Filter = $"ClassCode LIKE '%{filter}%'";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllModules(kverify, cookie);
            }
            catch
            {
                MessageBox.Show("Kiểm tra lại file config!");
            }
        }

        private void dtModules_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.HeaderText = "Đăng kí";
            btnColumn.Name = "btnColumn";
            btnColumn.Text = "Đăng kí";
            btnColumn.UseColumnTextForButtonValue = true; // Hiển thị text trên nút
            // Thêm cột vào DataGridView
            dgvClass.Columns.Add(btnColumn);
            string fid = dgvModules.Rows[e.RowIndex].Cells[0].Value.ToString();
            GetClassByModules(fid, kverify, cookie);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //dgvClass.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvModules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            

            string filePath = Environment.CurrentDirectory + "/config/data.txt";
            string lines = File.ReadAllText(filePath);
            string[] dt = lines.Split('|');

            kverify = dt[0];
            cookie = dt[1];

        }
        private void ButtonClickHandler(int rowIndex)
        {
            int idx = dgvClass.CurrentCell.RowIndex;
            string ClassID = dgvClass.Rows[idx].Cells[1].Value.ToString();
            AddClass(ClassID, kverify, cookie);
        }

        private void dgvClass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvClass.Columns["btnColumn"].Index && e.RowIndex >= 0)
            {
                // Gọi hàm xử lý khi nút được nhấn
                ButtonClickHandler(e.RowIndex);
            }
        }
    }
}


