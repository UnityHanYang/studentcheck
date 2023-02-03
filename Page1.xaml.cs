using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data;

namespace 프로젝트
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page1 : Page
    {
        string server = "localhost";
        string database = "game";
        string port = "3300";
        string uid = "root";
        string pass1 = "bu1234";

        public class checklist
        {
            public string Grade { get; set; }
            public string Name { get; set; }

            public bool IsChecked { get; set; }
        }

        public Page1()
        {
            InitializeComponent();
        }

        private void sel()
        {
            string str;
            DataSet ds = new DataSet();
            string con = string.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, uid, pass1);
            using (MySqlConnection mysql = new MySqlConnection(con))
            {
                string loginschool = schoolt.Text;
                string logingrade = gradet.Text;
                string dbselect = "select * from checklist where school='" + loginschool + "' and numbers ='" + logingrade + "' order by grade ASC;";

                grd.Header = "학년, 반";
                if (logingrade[1] == '0')
                {
                    str = logingrade[0] + "학년 " + logingrade[2] + "반";
                }
                else
                {
                    str = logingrade[0] + "학년 " + logingrade[1] + logingrade[2] + "반";
                }
                MySqlDataAdapter adpt = new MySqlDataAdapter(dbselect, mysql);
                adpt.Fill(ds, "checklist");

                
            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                if (r["conditions"].ToString() == "X")
                {
                    List<checklist> _list = new List<checklist>
                    {
                    new checklist { Grade = str, Name = r["name"].ToString(), IsChecked = false}
                    };
                    listv.Items.Add(_list);
                }
                else
                {
                    List<checklist> _list = new List<checklist>
                    {
                    new checklist { Grade = str, Name = r["name"].ToString(), IsChecked = true}
                    };
                    listv.Items.Add(_list);
                }
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string con = string.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, uid, pass1);
            using (MySqlConnection mysql = new MySqlConnection(con))
            {
                string loginschool = schoolt.Text;
                string logingrade = gradet.Text;
                string dbselect = "select * from checklist where school='" + loginschool + "' and grade='" + logingrade + "' or school='" + loginschool + "' and numbers ='" + logingrade + "';";
                
                mysql.Open();
                MySqlCommand selectcom = new MySqlCommand(dbselect, mysql);
                using (MySqlDataReader usera = selectcom.ExecuteReader())
                {
                    listv.Items.Clear();
                    while (usera.Read())
                    {
                        if (logingrade == usera["numbers"].ToString())
                        {
                            sel();
                            break;
                        }
                        else if(logingrade == usera["grade"].ToString())
                        {
                            grd.Header = "학년반번호";
                            if (usera["conditions"].ToString() == "X")
                            {
                                List<checklist> _list = new List<checklist>
                                {
                                new checklist { Grade = usera["grade"].ToString(), Name = usera["name"].ToString(), IsChecked = false}
                                };
                                listv.Items.Add(_list);
                            }
                            else
                            {
                                List<checklist> _list = new List<checklist>
                                {
                                new checklist { Grade = usera["grade"].ToString(), Name = usera["name"].ToString(), IsChecked = true}
                                };
                                listv.Items.Add(_list);
                            }
                        }
                    }
                }
                mysql.Close();
            }
        }
    }
}
