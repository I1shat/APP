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
using Word = Microsoft.Office.Interop.Word;

namespace APP
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            APP.Sealing_of_metersDataSet sealing_of_metersDataSet = ((APP.Sealing_of_metersDataSet)(this.FindResource("sealing_of_metersDataSet")));
            // Загрузить данные в таблицу Industrial. Можно изменить этот код как требуется.
            APP.Sealing_of_metersDataSetTableAdapters.IndustrialTableAdapter sealing_of_metersDataSetIndustrialTableAdapter = new APP.Sealing_of_metersDataSetTableAdapters.IndustrialTableAdapter();
            sealing_of_metersDataSetIndustrialTableAdapter.Fill(sealing_of_metersDataSet.Industrial);
            System.Windows.Data.CollectionViewSource industrialViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("industrialViewSource")));
            industrialViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Dzerzhinsky. Можно изменить этот код как требуется.
            APP.Sealing_of_metersDataSetTableAdapters.DzerzhinskyTableAdapter sealing_of_metersDataSetDzerzhinskyTableAdapter = new APP.Sealing_of_metersDataSetTableAdapters.DzerzhinskyTableAdapter();
            sealing_of_metersDataSetDzerzhinskyTableAdapter.Fill(sealing_of_metersDataSet.Dzerzhinsky);
            System.Windows.Data.CollectionViewSource dzerzhinskyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("dzerzhinskyViewSource")));
            dzerzhinskyViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Leninsky. Можно изменить этот код как требуется.
            APP.Sealing_of_metersDataSetTableAdapters.LeninskyTableAdapter sealing_of_metersDataSetLeninskyTableAdapter = new APP.Sealing_of_metersDataSetTableAdapters.LeninskyTableAdapter();
            sealing_of_metersDataSetLeninskyTableAdapter.Fill(sealing_of_metersDataSet.Leninsky);
            System.Windows.Data.CollectionViewSource leninskyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("leninskyViewSource")));
            leninskyViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Central. Можно изменить этот код как требуется.
            APP.Sealing_of_metersDataSetTableAdapters.CentralTableAdapter sealing_of_metersDataSetCentralTableAdapter = new APP.Sealing_of_metersDataSetTableAdapters.CentralTableAdapter();
            sealing_of_metersDataSetCentralTableAdapter.Fill(sealing_of_metersDataSet.Central);
            System.Windows.Data.CollectionViewSource centralViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("centralViewSource")));
            centralViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Performer. Можно изменить этот код как требуется.
            APP.Sealing_of_metersDataSetTableAdapters.PerformerTableAdapter sealing_of_metersDataSetPerformerTableAdapter = new APP.Sealing_of_metersDataSetTableAdapters.PerformerTableAdapter();
            sealing_of_metersDataSetPerformerTableAdapter.Fill(sealing_of_metersDataSet.Performer);
            System.Windows.Data.CollectionViewSource performerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("performerViewSource")));
            performerViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Request. Можно изменить этот код как требуется.
            APP.Sealing_of_metersDataSetTableAdapters.RequestTableAdapter sealing_of_metersDataSetRequestTableAdapter = new APP.Sealing_of_metersDataSetTableAdapters.RequestTableAdapter();
            sealing_of_metersDataSetRequestTableAdapter.Fill(sealing_of_metersDataSet.Request);
            System.Windows.Data.CollectionViewSource requestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("requestViewSource")));
            requestViewSource.View.MoveCurrentToFirst();
        }

        private void dzerzhinskyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)             //Кнопка формирования отчета
        {
            string id_P = idTextBox.Text;
            int index;

            Word.Application app = new Word.Application(); 

            Word.Document doc = app.Documents.Add();

            Word.Paragraph p = doc.Content.Paragraphs.Add();

            string Today = DateTime.Today.ToString();
            string NotToday = DateTime.Today.AddDays(-30).ToString();

            for (int i = 0; i < 8; i++)
            {
                Today = Today.Remove(Today.Length - 1);
                NotToday = NotToday.Remove(NotToday.Length - 1);
            }

            p.Range.Text = "Отчет о выполненной работе от " + NotToday + " по " + Today; 

            p.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            p.Format.SpaceAfter = 20; p.Range.InsertParagraphAfter();

            p = doc.Content.Paragraphs.Add();

            p.Range.Text = "Исполнитель: " + surnameTextBox.Text + ' ' + nameTextBox.Text;

            p.Format.Alignment =

            Word.WdParagraphAlignment.wdAlignParagraphLeft;

            p.Format.SpaceAfter = 20;

            p.Range.InsertParagraphAfter();

            p = doc.Content.Paragraphs.Add();

            int rowCount = 1;

            if (districtTextBox.Text == "Дзержинский")
            {
                foreach (System.Data.DataRowView row_D in dzerzhinskyDataGrid.Items)
                {
                    if (row_D.Row.ItemArray[2].ToString() == id_P && (Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) < DateTime.Today && Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) > DateTime.Today.AddDays(-30)))
                    {
                        rowCount++;
                    }
                }
            }
            else if (districtTextBox.Text == "Промышленный")
            {
                foreach (System.Data.DataRowView row_D in industrialDataGrid.Items)
                {
                    if (row_D.Row.ItemArray[2].ToString() == id_P && (Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) < DateTime.Today && Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) > DateTime.Today.AddDays(-30)))
                    {
                        rowCount++;
                    }
                }
            }
            else if (districtTextBox.Text == "Ленинский")
            {
                foreach (System.Data.DataRowView row_D in leninskyDataGrid.Items)
                {
                    if (row_D.Row.ItemArray[2].ToString() == id_P && (Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) < DateTime.Today && Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) > DateTime.Today.AddDays(-30)))
                    {
                        rowCount++;
                    }
                }
            }
            else if (districtTextBox.Text == "Центральный")
            {
                foreach (System.Data.DataRowView row_D in centralDataGrid.Items)
                {
                    if (row_D.Row.ItemArray[2].ToString() == id_P && (Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) < DateTime.Today && Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) > DateTime.Today.AddDays(-30)))
                    {
                        rowCount++;
                    }
                }
            }



            Word.Table tab = doc.Tables.Add(p.Range, rowCount, 3);                //поменять !!!!!!!!

            tab.Borders.Enable = 1;

            tab.Cell(1, 1).Range.Text = "Номер заявки";

            tab.Cell(1, 2).Range.Text = "Дата выполнения";

            tab.Cell(1, 3).Range.Text = "Адрес";


            index = -1;

            if (districtTextBox.Text == "Дзержинский")
            {
                foreach (System.Data.DataRowView row_D in dzerzhinskyDataGrid.Items)
                {
                    if (row_D.Row.ItemArray[2].ToString() == id_P && (Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) < DateTime.Today  && Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) > DateTime.Today.AddDays(-30)))
                    {
                        index++;

                        tab.Cell(index + 2, 1).Range.Text = row_D.Row.ItemArray[1].ToString();

                        NotToday = row_D.Row.ItemArray[3].ToString();

                        for (int i = 0; i < 8; i++)
                        {
                            NotToday = NotToday.Remove(NotToday.Length - 1);
                        }

                        tab.Cell(index + 2, 2).Range.Text = NotToday;

                        tab.Cell(index + 2, 3).Range.Text = row_D.Row.ItemArray[8].ToString();
                    }
                }
            }
            else if (districtTextBox.Text == "Промышленный")
            {
                foreach (System.Data.DataRowView row_D in industrialDataGrid.Items)
                {
                    if (row_D.Row.ItemArray[2].ToString() == id_P && (Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) < DateTime.Today && Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) > DateTime.Today.AddDays(-30)))
                    {
                        index++;

                        tab.Cell(index + 2, 1).Range.Text = row_D.Row.ItemArray[1].ToString();

                        NotToday = row_D.Row.ItemArray[3].ToString();

                        for (int i = 0; i < 8; i++)
                        {
                            NotToday = NotToday.Remove(NotToday.Length - 1);
                        }

                        tab.Cell(index + 2, 2).Range.Text = NotToday;

                        tab.Cell(index + 2, 3).Range.Text = row_D.Row.ItemArray[8].ToString();
                    }
                }
            }
            else if (districtTextBox.Text == "Ленинский")
            {
                foreach (System.Data.DataRowView row_D in leninskyDataGrid.Items)
                {
                    if (row_D.Row.ItemArray[2].ToString() == id_P && (Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) < DateTime.Today && Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) > DateTime.Today.AddDays(-30)))
                    {
                        index++;

                        tab.Cell(index + 2, 1).Range.Text = row_D.Row.ItemArray[1].ToString();

                        NotToday = row_D.Row.ItemArray[3].ToString();

                        for (int i = 0; i < 8; i++)
                        {
                            NotToday = NotToday.Remove(NotToday.Length - 1);
                        }

                        tab.Cell(index + 2, 2).Range.Text = NotToday;

                        tab.Cell(index + 2, 3).Range.Text = row_D.Row.ItemArray[8].ToString();
                    }
                }
            }
            else if (districtTextBox.Text == "Центральный")
            {
                foreach (System.Data.DataRowView row_D in centralDataGrid.Items)
                {
                    if (row_D.Row.ItemArray[2].ToString() == id_P && (Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) < DateTime.Today && Convert.ToDateTime(row_D.Row.ItemArray[3].ToString()) > DateTime.Today.AddDays(-30)))
                    {
                        index++;

                        tab.Cell(index + 2, 1).Range.Text = row_D.Row.ItemArray[1].ToString();

                        NotToday = row_D.Row.ItemArray[3].ToString();

                        for (int i = 0; i < 8; i++)
                        {
                            NotToday = NotToday.Remove(NotToday.Length - 1);
                        }

                        tab.Cell(index + 2, 2).Range.Text = NotToday;

                        tab.Cell(index + 2, 3).Range.Text = row_D.Row.ItemArray[8].ToString();
                    }
                }
            }

            doc.Save();
        }
    }
}
