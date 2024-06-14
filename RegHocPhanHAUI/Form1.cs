using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
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

        private string RunCMD(string cmd)
        {
            Process cmdProcess;
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.Arguments = "/c " + cmd;
            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.StartInfo.Verb = "runas";
            cmdProcess.Start();
            string output = cmdProcess.StandardOutput.ReadToEnd();
            cmdProcess.WaitForExit();
            if (String.IsNullOrEmpty(output))
                return "";
            return output;
        }

        void CheckLicense()
        {
            string output = RunCMD("wmic diskdrive get serialNumber"); // check số serial ổ cứng
            using (StreamWriter HDD = new StreamWriter("HDD.txt", true))
            {
                HDD.WriteLine(output);
                HDD.Close();
            }
            string[] lines = File.ReadAllLines("HDD.txt");
            File.Delete("HDD.txt");
            string str = Regex.Replace(lines[2], @"\s", ""); // lấy serial đầu tiên

            string outputs = RunCMD("wmic bios get serialnumber"); // check số serial bios
            using (StreamWriter BIOS = new StreamWriter("bios.txt", true))
            {
                BIOS.WriteLine(outputs);
                BIOS.Close();
            }
            string[] liness = File.ReadAllLines("bios.txt");
            File.Delete("bios.txt");
            string strs = Regex.Replace(liness[2], @"\s", ""); // lấy serial đầu tiên

            string keys = string.Concat(strs, str);

            HttpClient httpClient = new HttpClient();
            string text2 = keys;
            string requestUri2 = "https://docs.google.com/spreadsheets/d/1JwQmNaha0kaZzEOCnxjqb3j04zEkZwR6MxAoUyg14Ig/edit?usp=sharing";
            string text3 = httpClient.GetAsync(requestUri2).Result.Content.ReadAsStringAsync().Result.ToString();
            Match match2 = Regex.Match(text3.ToString(), text2 + ".*?(?=ok)");
            bool flag2 = match2 != Match.Empty;
            if (flag2)
            {
                string[] array = match2.ToString().Split(new char[]
                {
                            '|'
                });
                //string siteurlold = "https://mmosorfware.com/time.php";
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //string htmlold = new System.Net.WebClient().DownloadString(siteurlold);
                //string[] arrayn = htmlold.ToString().Split(new char[]
                //{
                //             '/'
                //});
                //int dayn = Int32.Parse(arrayn[0]);
                //int monthn = Int32.Parse(arrayn[1]);
                //int yearn = Int32.Parse(arrayn[2]);

                DateTime time = DateTime.Now;
                int dayn = time.Day;
                int monthn = time.Month;
                int yearn = time.Year;

                string[] arrays = array[1].ToString().Split(new char[]
               {
                            '/'
               });

                int dayt = Int32.Parse(arrays[0]);
                int montht = Int32.Parse(arrays[1]);
                int yeart = Int32.Parse(arrays[2]);

                System.DateTime now = new System.DateTime(yearn, monthn, dayn);
                System.DateTime then = new System.DateTime(yeart, montht, dayt);
                System.TimeSpan diff1 = then.Subtract(now);


                int days = (int)Math.Ceiling(diff1.TotalDays);

                bool flag3 = days <= 0;
                if (flag3)
                {
                    MessageBox.Show("Vui lòng liên hệ admin để gia hạn.", "Phần mềm hết hạn" + days, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //this.tslExp.Text = array[1].ToString();
                    //this.tslRemainTime.Text = "Còn lại: " + days.ToString() + " ngày";
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("Đăng Nhập Thành Công !", "Còn lại: " + days + " ngày!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //this.tslExp.Text = array[1].ToString();
                    //this.tslRemainTime.Text = "Còn lại: " + days.ToString() + " ngày";
                }
            }

            else
            {
                MessageBox.Show(string.Format("Bạn chưa mua bản quyền tool, vui lòng bấm Ctrl + C và gửi mã \"{0}\" cho chúng tôi để kích hoạt tool, bấm OK để sao chép key!", keys), "Thông báo active bản quyền!", MessageBoxButtons.OK);
                Clipboard.SetText(keys);
                //Environment.Exit(Environment.ExitCode);
                Application.Exit();
            }
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

        #region Request

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
            var options = new RestClientOptions()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://sv.haui.edu.vn/ajax/register/action.htm?cmd=addclass&v=" + kv, Method.Post);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("accept-language", "en-US,en;q=0.9,vi;q=0.8,ar;q=0.7,de;q=0.6");
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
            request.AlwaysMultipartFormData = true;
            request.AddParameter("class", ClassID);
            request.AddParameter("ctdk", "886");
            RestResponse response = await client.ExecuteAsync(request);
            Notification parsed_json = JsonConvert.DeserializeObject<Notification>(response.Content);

            MessageBox.Show(parsed_json.Message);
        }

        #endregion

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
                GetCookie();
                GetAllModules(kverify, cookie);
            }
        }

        private void dtModules_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            string fid = dgvModules.Rows[e.RowIndex].Cells[0].Value.ToString();
            GetClassByModules(fid, kverify, cookie);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Check key
            CheckLicense();

            //dgvClass.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvModules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            //Thêm cột đăng kí
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.HeaderText = "Đăng kí";
            btnColumn.Name = "btnColumn";
            btnColumn.Text = "Đăng kí";
            btnColumn.UseColumnTextForButtonValue = true; // Hiển thị text trên nút
            // Thêm cột vào DataGridView
            dgvClass.Columns.Add(btnColumn);

            //Đọc file config(kv và cookie)
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

        void GetCookie()
        {
            string filePath = Environment.CurrentDirectory + "/config/account.txt";
            string lines = File.ReadAllText(filePath);
            string[] dt = lines.Split('|');

            string user = dt[0];
            string pass = dt[1];


            ChromeDriverService cService = ChromeDriverService.CreateDefaultService();
            cService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(cService);
            try
            {
                // Navigate to Url
                driver.Navigate().GoToUrl("https://one.haui.edu.vn/loginapi/sv");

                driver.FindElement(By.Id("ctl00_inpUserName")).SendKeys(user);
                driver.FindElement(By.Id("ctl00_inpPassword")).SendKeys(pass);
                driver.FindElement(By.Id("ctl00_butLogin")).Click();

                Thread.Sleep(1000);

                lblAccountName.Text = driver.FindElement(By.ClassName("user-name")).Text;

                // Get All available cookies
                var cookies = driver.Manage().Cookies.AllCookies;
                //Bỏ qua khảo sát
                try
                {
                    driver.SwitchTo().Alert().Accept();
                }
                catch
                {

                }
                string ck = string.Empty;
                ck += cookies[0].Name.ToString() + "=" + cookies[0].Value.ToString() + ";";
                ck += cookies[1].Name.ToString() + "=" + cookies[1].Value.ToString() + ";";
                ck += cookies[2].Name.ToString() + "=" + cookies[2].Value.ToString() + ";";
                ck += cookies[3].Name.ToString() + "=" + cookies[3].Value.ToString() + ";";
                ck += cookies[4].Name.ToString() + "=" + cookies[4].Value.ToString() + ";";
                ck += cookies[5].Name.ToString() + "=" + cookies[5].Value.ToString();

                //MessageBox.Show(ck);

                string htmlContent = driver.PageSource;

                string pattern = @"var kverify\s*=\s*'([^']+)';";
                Match match = Regex.Match(htmlContent, pattern);
                string kv = string.Empty;

                if (match.Success)
                {
                    kv = match.Groups[1].Value;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy giá trị của biến kverify");
                }

                string path_config = Environment.CurrentDirectory + "/config/data.txt";

                File.WriteAllText(path_config, kv + "|" + ck);

                Thread.Sleep(2000);

            }
            finally
            {
                driver.Quit();
                MessageBox.Show("Lấy cookie thành công!");
            }
        }

        private void btnGetCookie_Click(object sender, EventArgs e)
        {
            GetCookie();
        }
    }
}


