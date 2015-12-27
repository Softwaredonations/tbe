using System;
using System.IO;
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
using System.Drawing;
using Microsoft.Win32;
using System.Threading;

namespace TattooBear
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string strDesktopPath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string strDesktopEventsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string strDesktopUsersPath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string strDesktopDatePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        //byte[] data;
        string strName = "";
        string imageName = "";



        public MainWindow()
        {
            InitializeComponent();
            Calendar calender = cal;
            strDesktopPath += "\\Tattoo Bear Events";
            strDesktopUsersPath = (strDesktopPath + "\\Customers");
            strDesktopEventsPath = (strDesktopPath + "\\Events");


            initType();
            initDN();
            initSexPref();
            LoadUsers();

            if (!System.IO.Directory.Exists(strDesktopPath))
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(strDesktopPath);
            }
            DateTime today = System.DateTime.Today;
            calender.SelectedDate = today;

        }

        public void LoadUsers()
        {
            foreach (string s in Directory.GetDirectories(strDesktopUsersPath))
            {
                UserList.Items.Add(s.Remove(0, strDesktopUsersPath.Length + 1));
            }
            SortLists();
        }

        public void SortLists()
        {

            if (UserList.Items.Count >= 2) {
                if (UserList.Items[0].ToString().Contains("ListBoxItem")) { UserList.Items.RemoveAt(0); }                    
                UserList.Items.SortDescriptions.Add(
                new System.ComponentModel.SortDescription("",
                System.ComponentModel.ListSortDirection.Ascending));
            }

            if (PaidListLB1.Items.Count >= 2)
            {
                PaidListLB1.Items.SortDescriptions.Add(
              new System.ComponentModel.SortDescription("",
              System.ComponentModel.ListSortDirection.Ascending));
            }
            if (UnpaidList.Items.Count >= 2)
            {
                UnpaidList.Items.SortDescriptions.Add(
                new System.ComponentModel.SortDescription("",
                System.ComponentModel.ListSortDirection.Ascending));
            }


        }

        public void initType()
        {
            TypeCB.Items.Add("Select");
            TypeCB.Items.Add("Solo Female");
            TypeCB.Items.Add("Solo Male");
            TypeCB.Items.Add("Couple");
            TypeCB.Items.Add("Group");
            TypeCB.Items.Add("Free");
            TypeCB.SelectedIndex = 0;
        }

        public void initDN()
        {
            DNCB.Items.Add("Select");
            DNCB.Items.Add("Day");
            DNCB.Items.Add("Night");
            DNCB.Items.Add("NoPref");
            DNCB.SelectedIndex = 0;
        }

        public void initSexPref()
        {
            SPCB.Items.Add("Select");
            SPCB.Items.Add("Straight");
            SPCB.Items.Add("Gay");
            SPCB.Items.Add("Bisexual");
            SPCB.Items.Add("TV/CD/TG/TS");
            SPCB.SelectedIndex = 0;
        }




        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get reference.
            var calendar = sender as Calendar;


            // ... See if a date is selected.
            if (calendar.SelectedDate.HasValue)
            {
                // ... Display SelectedDate in Title.
                DateTime date = calendar.SelectedDate.Value;
                this.Title = date.ToShortDateString();
                strDesktopDatePath = strDesktopEventsPath + "\\" + String.Format("{0:yyyy MM dd}", date);
                if (!System.IO.Directory.Exists(strDesktopDatePath))
                {
                    DirectoryInfo di = System.IO.Directory.CreateDirectory(strDesktopDatePath);
                }
                else
                {
                    UnpaidList.Items.Clear();
                    PaidListLB1.Items.Clear();
                    string lineOfText;
                    string path = strDesktopDatePath + "\\Paid.txt";
                    #region paid


                    if (!File.Exists(path))
                    {
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine("Price Type Sex Cancelled Name");
                            sw.Close();
                        }
                    }

                    var filestream = new System.IO.FileStream(strDesktopDatePath + "\\Paid.txt",
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read,
                                          System.IO.FileShare.ReadWrite);
                    var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);

                    while ((lineOfText = file.ReadLine()) != null)
                    {

                        string[] words = lineOfText.Split(' ');
                        int count = words[0].Length + words[1].Length + words[2].Length + words[3].Length + 4;
                        String Test = lineOfText.Substring(count);
                        if (Test != "Name")
                        {
                            PaidListLB1.Items.Add(Test.ToString());
                        }
                    }
                    #endregion

                    path = strDesktopDatePath + "\\Unpaid.txt";
                    #region unpaid
                    if (!File.Exists(path))
                    {
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine("Price Type Sex Cancelled Name");
                            sw.Close();
                        }
                    }
                    filestream = new System.IO.FileStream(strDesktopDatePath + "\\Unpaid.txt",
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read,
                                          System.IO.FileShare.ReadWrite);
                    file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);

                    while ((lineOfText = file.ReadLine()) != null)
                    {
                        string[] words = lineOfText.Split(' ');
                        int count = words[0].Length + words[1].Length + words[2].Length + words[3].Length + 3;
                        String Test = lineOfText.Substring(count);
                        if (!Test.Contains("Name"))
                        {
                            UnpaidList.Items.Add(Test);
                        }
                    }

                    filestream.Close();
                    #endregion
                }
            }


        }

        private void Addbutton_Click(object sender, RoutedEventArgs e)
        {
           
            if (NameTB.Text == "")
            {
            }
            else
            {
                
                string cn = NameTB.Text;
                decimal x;
                if (decimal.TryParse(PriceTB.Text, out x))
                {
                    if (PriceTB.Text.IndexOf('.') != -1 && PriceTB.Text.Split('.')[1].Length > 2)
                    {
                        MessageBox.Show("The maximum decimal points are 2!");
                        PriceTB.Focus();
                    }
                    else
                    {
                        //Validate price
                        PriceTB.Text = x.ToString("0.00");

                        //Check if customer visited before
                        String customer = strDesktopUsersPath + "\\" + NameTB.Text;
                        if (!System.IO.Directory.Exists(customer)) //first visit create user file
                        {
                            DirectoryInfo di = System.IO.Directory.CreateDirectory(customer);
                            if (TypeCB.SelectedIndex == 0 || DNCB.SelectedIndex == 0 || SPCB.SelectedIndex == 0 || ContactTB.Text.ToString() == "")
                            {
                                MessageBox.Show("Please fill in all details");
                                return;
                            }
                            String Details = "Type Day/Night SexualPreference Contact Visits Cancellations names";
                            String Details2 = TypeCB.SelectedItem.ToString() + " " + DNCB.SelectedItem.ToString() + " " + SPCB.SelectedItem.ToString() + " " + ContactTB.Text.ToString() + " 0" + " "+ "0 " + NameTB.Text.ToString();
                            using (StreamWriter writer = File.CreateText((customer + "\\UserInfo.txt")))
                            {
                                writer.WriteLine(Details);
                                writer.WriteLine(Details2);

                            }

                        }
                        //add to user list
                        if (!UserList.Items.Contains(cn))
                        {
                            UserList.Items.Add(cn);
                        }
                       // Price Type Sex Cancelled Name
                       String Add = PriceTB.Text + " " + TypeCB.SelectedItem.ToString() + " "+ SPCB.SelectedItem.ToString() + " " + 0 + " " + NameTB.Text;
                        if (PaidCheck.IsChecked == true)
                        {

                            TextWriter tw = new StreamWriter(strDesktopDatePath + "\\Paid.txt");
                            tw.WriteLine(Add);
                            tw.Close();
                            PaidListLB1.Items.Add(cn);
                            setPaid(cn);
                        }
                        else
                        {
                            TextWriter tw = new StreamWriter(strDesktopDatePath + "\\Unpaid.txt");
                            tw.WriteLine(Add);
                            tw.Close();
                            UnpaidList.Items.Add(cn);
                        }

                        if (imageName != "")
                        {
                            var tmpname = imageName + ".bak";
                            System.IO.File.Delete(tmpname);
                            System.IO.File.Move(imageName, strDesktopUsersPath + "\\" + NameTB.Text + "\\" + NameTB.Text + ".jpg");
                            imageName = "";
                            selimageLB.Content = "Selected Image";
                        }


                        NameTB.Text = "";
                        SPCB.SelectedIndex = 0;
                        TypeCB.SelectedIndex = 0;
                        PriceTB.Text = "0.00";
                        ContactTB.Text = "";
                        DNCB.SelectedIndex = 0;

                    }
                }
                else
                {
                    MessageBox.Show("Data invalid!");
                    PriceTB.Focus();
                }
            }
            SortLists();
        }


        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void photoB_Click(object sender, RoutedEventArgs e)
        {

            FileDialog fldlg = new OpenFileDialog();
            fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
            fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
            fldlg.ShowDialog();
            {
                strName = fldlg.SafeFileName;
                imageName = fldlg.FileName;
                //ImageSourceConverter isc = new ImageSourceConverter();
                //image.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                selimageLB.Content = imageName;
            }
            fldlg = null;

        }

        private void setpaidB_Click(object sender, RoutedEventArgs e)
        {
            if (UnpaidList.SelectedIndex != -1)
            {
                string cn = NameTB.Text;
                string line = null;
                string line_to_delete = UnpaidList.SelectedItem.ToString();
                string copyl = "";


                using (StreamReader reader = new StreamReader(strDesktopDatePath + "\\Unpaid.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(strDesktopDatePath + "\\Unpaid2.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Contains(line_to_delete)) { writer.WriteLine(line); }
                            else { copyl = line; }
                        }
                    }
                }
                using (StreamReader reader = new StreamReader(strDesktopDatePath + "\\Unpaid2.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(strDesktopDatePath + "\\Unpaid.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
                File.Delete(strDesktopDatePath + "\\Unpaid2.txt");

                if (copyl != "")
                {
                    TextWriter tw = new StreamWriter(strDesktopDatePath + "\\Paid.txt");
                    tw.WriteLine(copyl);
                    tw.Close();
                    
                }




                copyl = "";
                PaidListLB1.Items.Add(cn);
                UnpaidList.Items.RemoveAt(UnpaidList.SelectedIndex);
                setPaid(cn);

            }
            SortLists();

        }

        private void removeB_Click(object sender, RoutedEventArgs e)
        {

            if (PaidListLB1.SelectedIndex != -1)
            {

                string line = null;
                string line_to_delete = PaidListLB1.SelectedItem.ToString();
                string copyl = "";
                string cn = PaidListLB1.SelectedItem.ToString();
                string path = strDesktopUsersPath + "\\" + cn + "\\UserInfo.txt";


                using (StreamReader reader = new StreamReader(strDesktopDatePath + "\\Paid.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(strDesktopDatePath + "\\Paid2.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Contains(line_to_delete)) { writer.WriteLine(line); }
                            else { copyl = line; }
                        }
                    }
                }
                using (StreamReader reader = new StreamReader(strDesktopDatePath + "\\Paid2.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(strDesktopDatePath + "\\Paid.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }

                File.Delete(strDesktopDatePath + "\\Paid2.txt");
                if (PaidListLB1.SelectedIndex != -1)
                {
                    PaidListLB1.Items.RemoveAt(PaidListLB1.SelectedIndex);

                }
            }

                SortLists();

        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                String customer = strDesktopUsersPath + "\\" + UserList.SelectedItem.ToString();                
                if (!System.IO.Directory.Exists(customer)) //first visit create user file
                {
                    DirectoryInfo di = System.IO.Directory.CreateDirectory(customer);
                }
                else
                {
                    // load image image
                    Thread t = new Thread(setimage);
                    t.Start();


                    string cn = UserList.SelectedItem.ToString();
                    NameTB.Text = cn;
                    string customerVisits = strDesktopUsersPath + "\\" + cn;
                    //make visits list


                    //load userinfo
                    string path =  customerVisits + "\\UserInfo.txt";
                    if (File.Exists(path))
                    {

                        String Details = "Type Day/Night SexualPreference Contact Visits Cancellations names";
                        // String Details2 = TypeCB.SelectedItem.ToString() + " " + DNCB.SelectedItem.ToString() + " " + SPCB.SelectedItem.ToString() + " " + ContactTB.Text.ToString() + " " + NameTB.Text.ToString();

                        var filestream = new System.IO.FileStream(path,
                                         System.IO.FileMode.Open,
                                         System.IO.FileAccess.Read,
                                         System.IO.FileShare.ReadWrite);
                        var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
                        String lineOfText;
                        while ((lineOfText = file.ReadLine()) != null)
                        {
                            if (lineOfText != Details)
                            {

                                //new booking
                                string[] words = lineOfText.Split(' ');
                                TypeCB.SelectedItem = words[0].ToString();
                                DNCB.SelectedItem = words[1].ToString();
                                SPCB.SelectedItem = words[2].ToString();
                                ContactTB.Text = words[3].ToString();

                                //loaduser
                                UserName.Content = cn;
                                UserType.Content = "Type: " + words[0].ToString();
                                UserDN.Content = "D/N: " + words[1].ToString();
                                UserSP.Content = "Sex Pref: " + words[2].ToString();
                                UserCon.Content = "Contact: " + words[3].ToString();
                                UserV.Content = "Visits: " + words[4].ToString();
                                UserC.Content = "Cancellations: " + words[5].ToString();
                            }
                        }
                        filestream.Close();
                        file.Close();
                    }
                }
            }
            catch { imageBox.Source = null; }
        }

        public void setimage()
        {
    
            Dispatcher.BeginInvoke((Action)(() =>
            {
                string cn = UserList.SelectedItem.ToString();
                String customer = strDesktopUsersPath + "\\" + cn;
                Uri images = new Uri(customer + "\\" + cn + ".jpg");
                if (File.Exists(images.ToString())) {
                try
                {
                    ImageSource imageSource = new BitmapImage(images);
                    imageBox.Source = imageSource;
                }
                catch (Exception ce) { throw ce; }
                }
            }));

        }

        private void TypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TypeCB.SelectedItem.ToString())
            {
                case "Select":
                    break;
                case "Solo Female":
                    PriceTB.Text = "5.00";
                    break;
                case "Solo Male":
                    PriceTB.Text = "10.00";
                    break;
                case "Couple":
                    PriceTB.Text = "10.00";
                    break;
                case "Group":
                    PriceTB.Text = "";
                    break;
                case "Free":
                    PriceTB.Text = "0.00";
                    break;
            }

        }

        private void Cancelled_Click(object sender, RoutedEventArgs e)
        {
            if (PaidListLB1.SelectedIndex != -1)
            {
                

                string cn = PaidListLB1.SelectedItem.ToString();
                
                //load userinfo
                string path = strDesktopUsersPath + "\\" + cn + "\\UserInfo.txt";
                bool test = File.Exists(path.ToString());
                if (File.Exists(path))
                {
                    var filestream = new System.IO.FileStream(path,
                                        System.IO.FileMode.Open,
                                        System.IO.FileAccess.Read,
                                        System.IO.FileShare.ReadWrite);
                    var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
                    String lineOfText;
                    String Details = "Type Day/Night SexualPreference Contact Visits Cancellations names";

                    String newDetails = "";
                    while ((lineOfText = file.ReadLine()) != null)
                    {
                        if (lineOfText != Details)
                        {

                            //new booking
                            string[] words = lineOfText.Split(' ');
                            int cancellations = Convert.ToInt32(words[5]);
                            cancellations++;
                            words[5] = cancellations.ToString();
                            newDetails= string.Join(" ", words);
                        }
                    }

                    string[] lines = {
                        Details,
                        newDetails
                    };

                    System.IO.File.WriteAllLines(path, lines);
                    filestream.Close();
                    file.Close();
                }


                string line = null;
                string line_to_delete = PaidListLB1.SelectedItem.ToString();
                string copyl = "";

                path = strDesktopUsersPath + "\\" + cn + "\\UserInfo.txt";


                using (StreamReader reader = new StreamReader(strDesktopDatePath + "\\Paid.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(strDesktopDatePath + "\\Paid2.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Contains(line_to_delete)) { writer.WriteLine(line); }
                            else { copyl = line; }
                        }
                    }
                }
                using (StreamReader reader = new StreamReader(strDesktopDatePath + "\\Paid2.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(strDesktopDatePath + "\\Paid.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }

                File.Delete(strDesktopDatePath + "\\Paid2.txt");
                if (PaidListLB1.SelectedIndex != -1)
                {
                    PaidListLB1.Items.RemoveAt(PaidListLB1.SelectedIndex);

                }

                SortLists();
            }

        }

        public void setPaid(string User)
        {

            string path = strDesktopUsersPath + "\\" + User + "\\UserInfo.txt";
            bool test = File.Exists(path.ToString());
            if (File.Exists(path))
            {
                var filestream = new System.IO.FileStream(path,
                                    System.IO.FileMode.Open,
                                    System.IO.FileAccess.Read,
                                    System.IO.FileShare.ReadWrite);
                var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
                String lineOfText;
                String Details = "Type Day/Night SexualPreference Contact Visits Cancellations names";

                String newDetails = "";
                while ((lineOfText = file.ReadLine()) != null)
                {
                    if (lineOfText != Details)
                    {

                        //new booking
                        string[] words = lineOfText.Split(' ');
                        int cancellations = Convert.ToInt32(words[4]);
                        cancellations++;
                        words[4] = cancellations.ToString();
                        newDetails = string.Join(" ", words);
                    }
                }

                string[] lines = {
                        Details,
                        newDetails
                    };

                System.IO.File.WriteAllLines(path, lines);
                filestream.Close();
                file.Close();
            }
            SortLists();


        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            for (int i = UserList.Items.Count; i > 0; i--)
            {
                if (UserList.Items.GetItemAt(i).ToString().Contains(search.Text.ToString()))
                {
                    UserList.SelectedIndex = i;
                }
            }
        }
    }
}
