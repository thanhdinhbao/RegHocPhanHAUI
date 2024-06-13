﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace RegHocPhanHAUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        async void GetAllModules()
        {
            var options = new RestClientOptions()
            {

                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://sv.haui.edu.vn/ajax/register/action.htm?cmd=moduleopen&v=F9440123FCF48CBB13A5184614B8362E", Method.Post);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("accept-language", "en-US,en;q=0.9,vi;q=0.8,ar;q=0.7,de;q=0.6");
            request.AddHeader("content-length", "0");
            request.AddHeader("cookie", "_ga_G1VDE91S1Z=GS1.3.1687399002.1.0.1687399002.0.0.0; UqZBpD3n=v1Uh+GSQ__YGU; kVisit=5d6d9522-e4fc-4e1a-a007-23b9d503e36d; _ga_ZG0RX9N53W=GS1.3.1691194734.2.1.1691194937.0.0.0; _ga_61721GVZDB=GS1.1.1691810371.1.0.1691810376.55.0.0; __Host-UqZBpD3n=v1Uh+GSQ__YGU; _ga=GA1.1.1106234729.1687399002; _ga_M6W50XGDVZ=GS1.1.1718155398.13.1.1718156204.0.0.0; _ga_N1ND2KBTYH=GS1.1.1718155400.13.1.1718156204.0.0.0; ASP.NET_SessionId=ciifsj3hogew2ro3x1isgjbe; onehauisv=F878D39A967227A3512895F4699619751E871EAFF666620F2192B52440078DD84207661024E9D5D8E50B87264DC8354B268586686E8699A50CDF639FC84551A05324630B240A83698154E88E0F187F42D51238D71B3315C5CED9EDD79AFDBF57685468C82A123D44AD79D8F62733C95B02BFA1765F45D942E08BDA47C57FA64F4E5FFDA91ED343BF2776C81A82C261FB5BB8FE9BCA518109BD3A9F3C74146FE0B9D8DED89F9C4C3BE8E55861A6C8AE007811CB51551256590ADCE3D4DC43B3399E9E5046E2F47173C304D57F219801E5E041234BD93B2C3D26B6370317FE3ACB7323D3E87D2BF132E946F02D108399AABDCB0C4AB96139CCF863AFE3BB4EB0A041548E649108BFADD31BA43437FF46736672A22003C5B3BD74F0B10C11F22CD81F4DEB7B4FB95B47CFF745832E01E26F7E99899C4DDA1DE511093D92319FDDB8EBAFD4DD1FEDC4DCDFD2A1D481E393D918C58820F0F88A65614348EDA93C4EEF11C22BC2A927ED7F47E01E1F181A8D07; _ga_S8WJEW3D2H=GS1.1.1718276018.20.1.1718276623.0.0.0; __Host-UqZBpD3n=v1Uh+GSQ__YGU");
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

            // Bind the DataTable to the DataGridView
            dtModules.DataSource = parsed_json;

        }

        async void GetClassByModules()
        {
            var options = new RestClientOptions()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://sv.haui.edu.vn/ajax/register/action.htm?cmd=classbymodulesid&v=F9440123FCF48CBB13A5184614B8362E", Method.Post);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("accept-language", "en-US,en;q=0.9,vi;q=0.8,ar;q=0.7,de;q=0.6");
            request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("cookie", "_ga_G1VDE91S1Z=GS1.3.1687399002.1.0.1687399002.0.0.0; UqZBpD3n=v1Uh+GSQ__YGU; kVisit=5d6d9522-e4fc-4e1a-a007-23b9d503e36d; _ga_ZG0RX9N53W=GS1.3.1691194734.2.1.1691194937.0.0.0; _ga_61721GVZDB=GS1.1.1691810371.1.0.1691810376.55.0.0; __Host-UqZBpD3n=v1Uh+GSQ__YGU; _ga=GA1.1.1106234729.1687399002; _ga_M6W50XGDVZ=GS1.1.1718155398.13.1.1718156204.0.0.0; _ga_N1ND2KBTYH=GS1.1.1718155400.13.1.1718156204.0.0.0; ASP.NET_SessionId=ciifsj3hogew2ro3x1isgjbe; onehauisv=F878D39A967227A3512895F4699619751E871EAFF666620F2192B52440078DD84207661024E9D5D8E50B87264DC8354B268586686E8699A50CDF639FC84551A05324630B240A83698154E88E0F187F42D51238D71B3315C5CED9EDD79AFDBF57685468C82A123D44AD79D8F62733C95B02BFA1765F45D942E08BDA47C57FA64F4E5FFDA91ED343BF2776C81A82C261FB5BB8FE9BCA518109BD3A9F3C74146FE0B9D8DED89F9C4C3BE8E55861A6C8AE007811CB51551256590ADCE3D4DC43B3399E9E5046E2F47173C304D57F219801E5E041234BD93B2C3D26B6370317FE3ACB7323D3E87D2BF132E946F02D108399AABDCB0C4AB96139CCF863AFE3BB4EB0A041548E649108BFADD31BA43437FF46736672A22003C5B3BD74F0B10C11F22CD81F4DEB7B4FB95B47CFF745832E01E26F7E99899C4DDA1DE511093D92319FDDB8EBAFD4DD1FEDC4DCDFD2A1D481E393D918C58820F0F88A65614348EDA93C4EEF11C22BC2A927ED7F47E01E1F181A8D07; _ga_S8WJEW3D2H=GS1.1.1718279326.21.0.1718279337.0.0.0; __Host-UqZBpD3n=v1Uh+GSQ__YGU");
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
            var body = @"fid=4834";
            request.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", body, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetAllModules();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = textBox1.Text;
                string colName = dtModules.Columns[1].Name;//Column Number of Search

                ((DataTable)dtModules.DataSource).DefaultView.RowFilter = string.Format(colName + " like '%{0}%'", searchValue.Trim().Replace("'", "''"));
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2_Click(null, null);
        }
    }
}

