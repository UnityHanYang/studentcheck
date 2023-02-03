using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using MySql.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace 프로젝트
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string server = "localhost";
        string database = "game";
        string port = "3300";
        string uid = "root";
        string pass1 = "bu1234";
        bool check = false;

        private void resets()
        {
            string con = string.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, uid, pass1);
            using (MySqlConnection mysql = new MySqlConnection(con))
            {
                string dbupdate = "update checklist set conditions='X';";

                mysql.Open();
                MySqlCommand selectcom = new MySqlCommand(dbupdate, mysql);
                MySqlDataReader usera = selectcom.ExecuteReader();

            }
        }
        public MainWindow()
        {
            InitializeComponent();

            string da = DateTime.Now.ToString("tt");
            string te = DateTime.Now.ToString("hh");
            if (da == "오전" && int.Parse(te) > 11)
            {
                resets();
            }
            else if(da == "오전" && int.Parse(te) == 1)
            {
                resets();
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string con = string.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, uid, pass1);
            using (MySqlConnection mysql = new MySqlConnection(con))
            {
                mysql.Open();
                string logingrade = grade.Text;
                string loginname = name.Text;
                string loginpass = pass.Text;
                string loginschool = school.Text;
                string condi = "X";
                string num = logingrade[0].ToString() + logingrade[1].ToString() + logingrade[2].ToString();
                string dbinsert = "insert into checklist values('" + loginpass + "', '" + loginschool + "', '" + loginname + "', '" + logingrade + "', '" + condi + "', '"+num+"');";

                MySqlCommand cmd = new MySqlCommand(dbinsert, mysql);
                if (check == true) {
                    if (logingrade == string.Empty || loginname == string.Empty || loginpass == string.Empty || loginschool == string.Empty)
                    {
                        MessageBox.Show("회원정보를 모두 입력해주세요");
                    }
                    else
                    {
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("등록되었습니다. 학년반번호&비밀번호 or 이름&비밀번호를 입력하세요.");

                            grade.Text = string.Empty;
                            name.Text = string.Empty;
                            pass.Text = string.Empty;
                            school.Text = string.Empty;
                            check = false;
                        }
                        mysql.Close();
                    }
                }
                else
                {
                    MessageBox.Show("중복확인을 확인하세요");
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string con = string.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, uid, pass1);
            using (MySqlConnection mysql = new MySqlConnection(con))
            {
                string logingrade = grade.Text;
                string loginname = name.Text;
                string loginpass = pass.Text;
                string str = logingrade[0].ToString() + logingrade[1].ToString() + logingrade[2].ToString();
                int st = 0;
                string dbselect = "select * from checklist;";

                if (logingrade == string.Empty || loginpass == string.Empty)
                {
                    MessageBox.Show("학년반번호 혹은 비밀번호를 입력하세요");
                }
                else if (loginpass.Length < 4)
                {
                    MessageBox.Show("비밀번호의 자릿수가 4 이상이어야 합니다.");
                }
                else
                {
                    mysql.Open();
                    MySqlCommand selectcom = new MySqlCommand(dbselect, mysql);
                    MySqlDataReader usera = selectcom.ExecuteReader();
                    while (usera.Read())
                    {
                        if (loginpass == usera["password"].ToString() && logingrade == usera["grade"].ToString())
                        {
                            st = 1;
                        }
                    }
                    mysql.Close();

                    if (st == 1)
                    {
                        MessageBox.Show("타 학생과 같은 비밀번호, 학년, 반, 번호를 사용할 수 없습니다.");
                    }
                    else
                    {
                        MessageBox.Show("사용가능합니다.");
                        check = true;
                    }
                }
            }
        }


        private void update(string loginpass)
        {
            string con = string.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, uid, pass1);
            using (MySqlConnection mysql = new MySqlConnection(con))
            {
                string dbupdate = "update checklist set conditions='"+ DateTime.Now.ToString("tt hh시 mm분 출석")+ "' where password='"+loginpass+"';";
                
                mysql.Open();
                MySqlCommand selectcom = new MySqlCommand(dbupdate, mysql);
                MySqlDataReader usera = selectcom.ExecuteReader();

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string con = string.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, uid, pass1);
            using (MySqlConnection mysql = new MySqlConnection(con))
            {
                string logingrade = grade.Text;
                string loginname = name.Text;
                string loginpass = pass.Text;
                string dbselect = "select * from checklist where grade = '"+logingrade+"' and password = '"+loginpass+"' or name='"+loginname+"' and password='"+loginpass+"';";

                mysql.Open();
                MySqlCommand selectcom = new MySqlCommand(dbselect, mysql);
                MySqlDataReader usera = selectcom.ExecuteReader();
                while (usera.Read())
                {
                    if (logingrade == usera["grade"].ToString() && loginpass == usera["password"].ToString() || loginname == usera["name"].ToString() && loginpass == usera["password"].ToString())
                    {
                        MessageBox.Show(DateTime.Now.ToString("tt h시 m분 출석완료"));
                        update(loginpass);
                        attend.Visibility = Visibility.Hidden;
                        clist.Visibility = Visibility.Visible;
                    }
                    else if(loginpass != usera["password"])
                    {
                        MessageBox.Show("비밀번호가 올바르지 않습니다.");
                    }
                }
                mysql.Close();
            }
        }

        private void clist_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("본인 조회(학교&학년반번호)\n반 조회(학교&학년반)");
            Page1 pag = new Page1();
            this.Content = pag;
        }
    }
}
