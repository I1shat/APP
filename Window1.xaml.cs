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
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace APP
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string sqlQuery = @"INSERT INTO Request (Surname, Name, District, Address, Phone) Values ( @Sn, @N, @D, @Ad, @P)";
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.Sealing_of_metersConnectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@Sn", surnameTextBox.Text);
            command.Parameters.AddWithValue("@N", nameTextBox.Text);
            command.Parameters.AddWithValue("@D", districtComboBox.Text);
            command.Parameters.AddWithValue("@Ad", addressTextBox.Text);
            command.Parameters.AddWithValue("@P", phoneTextBox.Text);
            command.ExecuteNonQuery();
            conn.Close();


            DateTime day_of_recording = DateTime.Today.AddDays(2);


            int Id_Request = -1;

            sqlQuery = @"select Id from Request";                  //получаем номер нужной заявки (последней) для создания онлайн записи
            conn.Open();
            command.CommandText = sqlQuery;
            SqlDataReader reader = command.ExecuteReader(); 
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    Id_Request = reader.GetInt32(0);
                }
            }
            reader.Close();
            conn.Close();
            

            int[] mas_Performer = new int[4] {0, 0, 0, 0};                   //этот массив хранит в себе всех исполнителей Дзержинского района
            int count_P = -1, num_P1 = 0;
            
            int Id_Performer = -1;


            sqlQuery = @"select Id from Performer where District = '" + districtComboBox.Text + "'";
            conn.Open();
            command.CommandText = sqlQuery;
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                for (int i = 0; i < 4 && reader.Read(); i++)
                {
                    mas_Performer[i] = reader.GetInt32(0);
                    count_P++;
                }
            }
            reader.Close();
            conn.Close();



            if (districtComboBox.Text == "Дзержинский")
            {
                sqlQuery = @"select * from Dzerzhinsky";
                conn.Open();
                command.CommandText = sqlQuery;
                reader = command.ExecuteReader();

                /*Суть алгоритма заключается в том, что мы находим последнюю запись, смотрим номер исполнителя в очереди (в этой записи) и делаем новую запись исходя из этих данных*/
                if(reader.HasRows)
                {
                    while(reader.Read())                            //построчное чтение
                    {
                        for (int i = 0; i <= count_P; i++)           //перебираем всех исполнителей определенного района (Дзержинского) и находим номер исполнителя в записи
                        {
                            if (reader.GetInt32(2) == mas_Performer[i])         
                            {
                                num_P1 = i; 
                            }
                        }

                        if (num_P1 + 1 > count_P)                        //если в последней записи стоит исполнитель последний на очереди, то в новую запись записывается первый в очереди исполнитель на следующий день после последней записи
                        {
                            Id_Performer = mas_Performer[0];
                            if (reader.GetDateTime(3) > DateTime.Today.AddDays(1)) day_of_recording = reader.GetDateTime(3).AddDays(1);
                        }
                        else                                            //в противном случае в новую запись записывается следующий на очереди исполнитель в тот же день, который указан в последней записи
                        {
                            Id_Performer = mas_Performer[num_P1 + 1];
                            if (reader.GetDateTime(3) > DateTime.Today.AddDays(1)) day_of_recording = reader.GetDateTime(3);
                        }
                    }
                }
                else
                {
                    Id_Performer = mas_Performer[0];
                }
                reader.Close();
                conn.Close();

                sqlQuery = @" INSERT INTO Dzerzhinsky (Id_Request, Id_Performer, Date) Values ( @Id_R, @Id_P, @Date)";
                conn.Open();
                command.CommandText = sqlQuery;

                command.Parameters.AddWithValue("@Id_R", Id_Request);
                command.Parameters.AddWithValue("@Id_P", Id_Performer);
                command.Parameters.AddWithValue("@Date", day_of_recording);

                command.ExecuteNonQuery();
                conn.Close();

            }
            else if(districtComboBox.Text == "Промышленный")
            {
                sqlQuery = @"select * from Industrial";
                conn.Open();
                command.CommandText = sqlQuery;
                reader = command.ExecuteReader();

                /*Суть алгоритма заключается в том, что мы находим последнюю запись, смотрим номер исполнителя в очереди (в этой записи) и делаем новую запись исходя из этих данных*/
                if (reader.HasRows)
                {
                    while (reader.Read())                            //построчное чтение
                    {
                        for (int i = 0; i <= count_P; i++)           //перебираем всех исполнителей определенного района (Дзержинского) и находим номер исполнителя в записи
                        {
                            if (reader.GetInt32(2) == mas_Performer[i])
                            {
                                num_P1 = i;
                            }
                        }

                        if (num_P1 + 1 > count_P)                        //если в последней записи стоит исполнитель последний на очереди, то в новую запись записывается первый в очереди исполнитель на следующий день после последней записи
                        {
                            Id_Performer = mas_Performer[0];
                            if (reader.GetDateTime(3) > DateTime.Today.AddDays(1)) day_of_recording = reader.GetDateTime(3).AddDays(1);
                        }
                        else                                            //в противном случае в новую запись записывается следующий на очереди исполнитель в тот же день, который указан в последней записи
                        {
                            Id_Performer = mas_Performer[num_P1 + 1];
                            if (reader.GetDateTime(3) > DateTime.Today.AddDays(1)) day_of_recording = reader.GetDateTime(3);
                        }
                    }
                }
                else
                {
                    Id_Performer = mas_Performer[0];
                }
                reader.Close();
                conn.Close();

                sqlQuery = @" INSERT INTO Industrial (Id_Request, Id_Performer, Date) Values ( @Id_R, @Id_P, @Date)";
                conn.Open();
                command.CommandText = sqlQuery;

                command.Parameters.AddWithValue("@Id_R", Id_Request);
                command.Parameters.AddWithValue("@Id_P", Id_Performer);
                command.Parameters.AddWithValue("@Date", day_of_recording);

                command.ExecuteNonQuery();
                conn.Close();
            }
            else if(districtComboBox.Text == "Ленинский")
            {
                sqlQuery = @"select * from Leninsky";
                conn.Open();
                command.CommandText = sqlQuery;
                reader = command.ExecuteReader();

                /*Суть алгоритма заключается в том, что мы находим последнюю запись, смотрим номер исполнителя в очереди (в этой записи) и делаем новую запись исходя из этих данных*/
                if (reader.HasRows)
                {
                    while (reader.Read())                            //построчное чтение
                    {
                        for (int i = 0; i <= count_P; i++)           //перебираем всех исполнителей определенного района (Дзержинского) и находим номер исполнителя в записи
                        {
                            if (reader.GetInt32(2) == mas_Performer[i])
                            {
                                num_P1 = i;
                            }
                        }

                        if (num_P1 + 1 > count_P)                        //если в последней записи стоит исполнитель последний на очереди, то в новую запись записывается первый в очереди исполнитель на следующий день после последней записи
                        {
                            Id_Performer = mas_Performer[0];
                            if (reader.GetDateTime(3) > DateTime.Today.AddDays(1)) day_of_recording = reader.GetDateTime(3).AddDays(1);
                        }
                        else                                            //в противном случае в новую запись записывается следующий на очереди исполнитель в тот же день, который указан в последней записи
                        {
                            Id_Performer = mas_Performer[num_P1 + 1];
                            if (reader.GetDateTime(3) > DateTime.Today.AddDays(1)) day_of_recording = reader.GetDateTime(3);
                        }
                    }
                }
                else
                {
                    Id_Performer = mas_Performer[0];
                }
                reader.Close();
                conn.Close();

                sqlQuery = @" INSERT INTO Leninsky (Id_Request, Id_Performer, Date) Values ( @Id_R, @Id_P, @Date)";
                conn.Open();
                command.CommandText = sqlQuery;

                command.Parameters.AddWithValue("@Id_R", Id_Request);
                command.Parameters.AddWithValue("@Id_P", Id_Performer);
                command.Parameters.AddWithValue("@Date", day_of_recording);

                command.ExecuteNonQuery();
                conn.Close();
            }   
            else if(districtComboBox.Text == "Центральный")
            {
                sqlQuery = @"select * from Central";
                conn.Open();
                command.CommandText = sqlQuery;
                reader = command.ExecuteReader();

                /*Суть алгоритма заключается в том, что мы находим последнюю запись, смотрим номер исполнителя в очереди (в этой записи) и делаем новую запись исходя из этих данных*/
                if (reader.HasRows)
                {
                    while (reader.Read())                            //построчное чтение
                    {
                        for (int i = 0; i <= count_P; i++)           //перебираем всех исполнителей определенного района (Дзержинского) и находим номер исполнителя в записи
                        {
                            if (reader.GetInt32(2) == mas_Performer[i])
                            {
                                num_P1 = i;
                            }
                        }

                        if (num_P1 + 1 > count_P)                        //если в последней записи стоит исполнитель последний на очереди, то в новую запись записывается первый в очереди исполнитель на следующий день после последней записи
                        {
                            Id_Performer = mas_Performer[0];
                            if (reader.GetDateTime(3) > DateTime.Today.AddDays(1)) day_of_recording = reader.GetDateTime(3).AddDays(1);
                        }
                        else                                            //в противном случае в новую запись записывается следующий на очереди исполнитель в тот же день, который указан в последней записи
                        {
                            Id_Performer = mas_Performer[num_P1 + 1];
                            if (reader.GetDateTime(3) > DateTime.Today.AddDays(1)) day_of_recording = reader.GetDateTime(3);
                        }
                    }
                }
                else
                {
                    Id_Performer = mas_Performer[0];
                }
                reader.Close();
                conn.Close();

                sqlQuery = @" INSERT INTO Central (Id_Request, Id_Performer, Date) Values ( @Id_R, @Id_P, @Date)";
                conn.Open();
                command.CommandText = sqlQuery;

                command.Parameters.AddWithValue("@Id_R", Id_Request);
                command.Parameters.AddWithValue("@Id_P", Id_Performer);
                command.Parameters.AddWithValue("@Date", day_of_recording);

                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            APP.Sealing_of_metersDataSet sealing_of_metersDataSet = ((APP.Sealing_of_metersDataSet)(this.FindResource("sealing_of_metersDataSet")));
            // Загрузить данные в таблицу Request. Можно изменить этот код как требуется.
            APP.Sealing_of_metersDataSetTableAdapters.RequestTableAdapter sealing_of_metersDataSetRequestTableAdapter = new APP.Sealing_of_metersDataSetTableAdapters.RequestTableAdapter();
            sealing_of_metersDataSetRequestTableAdapter.Fill(sealing_of_metersDataSet.Request);
            System.Windows.Data.CollectionViewSource requestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("requestViewSource")));
            requestViewSource.View.MoveCurrentToFirst();
        }
    }
}
