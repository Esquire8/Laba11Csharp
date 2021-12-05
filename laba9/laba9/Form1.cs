using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba9
{
    public partial class Form1 : Form
    {
        Song[] song;
        BankAccount[] accounts;
        Song mysong = new Song();

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            Songs();
        }

        private void btn1_1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int count = (int)numericUpDown1.Value;
            accounts = new BankAccount[count];
            for (int acc = 0; acc < count; acc++)
            {
                accounts[acc] = new BankAccount((Account)random.Next(0, 2), random.Next(100, 200000));
                comboNumbers.Items.Add(accounts[acc].GetNumber());
                comboFrom.Items.Add(accounts[acc].GetNumber());
                comboTo.Items.Add(accounts[acc].GetNumber());
                cBTransaction.Items.Add(accounts[acc].GetNumber());
            }
            DrawBankAccount();

        }
        private void DrawBankAccount()
        {
            richTextBox5.Clear();
            foreach (BankAccount account in accounts)
            {
                richTextBox5.Text += $"Аккаунт: {account.GetNumber()} - _{account.GetType()}_. Баланс: {account.GetBalance()} (руб.)\n\n";
            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if (PutNum.Value != 0)
            {
                foreach (BankAccount account in accounts)
                {
                    if (account.GetNumber().ToString() == comboNumbers.SelectedItem.ToString())
                    {
                        if (PutNum.Value != 0)
                        {
                            account.PutBalance((int)PutNum.Value);
                            DrawBankAccount();
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (PutNum.Value != 0)
            {
                foreach (BankAccount account in accounts)
                {
                    if (account.GetNumber().ToString() == comboNumbers.SelectedItem.ToString())
                    {
                        
                        if (PutNum.Value != 0)
                        {
                            bool successul = account.TakeBalance((int)PutNum.Value);
                            if (!successul)
                                MessageBox.Show("Недостаточно средств", "Невозможно");
                            DrawBankAccount();
                            return;
                        }
                    }
                }
            }
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                int floorCount = Convert.ToInt32(tB2_3.Text);
                int buildheight = Convert.ToInt32(tB2_2.Text);
                Building build = new Building();
                tB2_1.Text = Convert.ToString(build.GetBuildNumber());
                richTextBox2.Text = $"Высота здания: {buildheight}м\nКол-во этажей: {floorCount}\nВысота этажа: {build.GetBuildFloorHeight(floorCount, buildheight)}м";
            }
            catch
            {
                MessageBox.Show("Введите значения");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string str = tB10_2.Text;
            Laba10 letter = new Laba10();
            richTextBox3.Text = $"Revers: {letter.Reverse(str)}";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int temp = Convert.ToInt32(tB10_4.Text);
                Temperature t = new Temperature(temp);
                CheckArgImplementInterface(t);
                richTextBox4.Text = $"{t}";
            }
            catch
            {
                MessageBox.Show("Введите температуру");
            }
            
        }
        private void CheckArgImplementInterface(Temperature t)
        {
            IFormattable form;

            if (t is IFormattable)
            {
                form = (IFormattable)t;
            }
            else
            {
                form = null;
            }

            if (form is null)
            {
                MessageBox.Show("Не реализует IFormattable");
            }
            else
            {
                MessageBox.Show("Реализует IFormattable");
            }

        }
        private void Songs()
        {
            song = new Song[4];
            string[] names = new string[4] {"Без обид", "Без фокусов", "Space Cowboy", "Не твой"};
            string[] author = new string[4] {"Mnogoznal", "Markul", "ZillaKami", "Макс Корж"};
            song[0] = new Song();
            song[0].Name(author[0]);
            song[0].Author(names[0]);
            richTBSong_1.Text = $"{song[0].Title()}\n";
            for (int songId = 1; songId < song.Length; songId++)
            {
                song[songId] = new Song();
                song[songId].Name(author[songId]);
                song[songId].Author(names[songId]);
                song[songId].Prev(song[songId - 1]);
                richTBSong_1.Text += $"{song[songId].Title()}\n";
            }
            song[0].Prev(song[song.Length - 1]);

            richTBSong_2.Text = $"{song[0].Title()} == {song[1].Title()} ? : {song[0].Equals(song[1])}";
        }

        private void btnDispose_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            foreach (BankAccount account in accounts)
            {
                if (account.GetNumber().ToString() == cBTransaction.SelectedItem.ToString())
                {
                    account.Dispose(filename);
                    return;
                }
            }
        }

        private void cBTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (BankAccount account in accounts)
            {
                if (account.GetNumber().ToString() == cBTransaction.SelectedItem.ToString())
                {
                    richTextBox1.Text = "Транзакции: \n";
                    try
                    {
                        foreach (BankTransaction transaction in account.getTransactions())
                            richTextBox1.AppendText($"(Дата) - {transaction.TransferMoney} руб.\n");
                    }
                    catch (Exception)
                    {
                        richTextBox1.Text += "Нет транзакций";
                    }
                }
            }
        }
    }
    enum Account { Сберегательный, Накопительный };                         //Task1
    class BankAccount
    {
        private int accountNumber;
        private int accountBalance;
        private Account Type;

        private Queue<BankTransaction> Transactions = new Queue<BankTransaction>();

        public static int accNumber1 = 1;
        public BankAccount()
        {
            accountNumber = accNumber1;
            accNumberAdd();
        }
        public BankAccount(int newBalance)
        {
            accountBalance = newBalance;
            accountNumber = accNumber1;
            accNumberAdd();
        }
        public BankAccount(Account type, int newBalance)
        {
            accountBalance = newBalance;
            Type = type;
            accountNumber = accNumber1;
            accNumberAdd();
        }
        public void accNumberAdd()
        {
            accNumber1++;
        }
        public int GetBalance() { return accountBalance; }
        public Account GetType() { return Type; }
        
        public int GetNumber()                               //Task2
        {
            return accountNumber;
        }

        public bool TakeBalance(int acc)             //Task3
        {
            if (acc > accountBalance | acc < 0) { return false; }
            else
            {
                accountBalance -= acc;
                Transactions.Enqueue(new BankTransaction(acc));
                return true;
            }
        }
        public void PutBalance(int acc)
        {
            accountBalance += acc;
            Transactions.Enqueue(new BankTransaction(acc));
        }
        public static bool TryMoneyTransfer(ref BankAccount accountFrom, ref BankAccount accountTo, int money)
        {
            if (accountFrom.GetNumber() != accountTo.GetNumber())
            {
                if (accountFrom.TakeBalance(money))
                {
                    accountTo.PutBalance(money);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public int Remittance(ref int accNum1, int sum)                 //Laba10Task1
        {
            int result = accNum1;
            accountBalance = accNum1;
            if (accNum1 >= sum)
            {
                result =  accNum1 + sum;
                return result;
            }
            else
            {
                MessageBox.Show("Слишком большая сумма" ,"Ошиб очка");
            }
            return result;
        }
        public void Dispose(string filepath)
        {
            StreamWriter streamWriter = new StreamWriter(filepath);
            int index = 0;
            foreach (BankTransaction transaction in Transactions)
            {
                streamWriter.WriteLine($"({index}): {transaction.TransferMoney}");
                index++;
            }
            streamWriter.Close();
            GC.SuppressFinalize(this); //вызвать метод GC.SuppressFinalize
        }
        public Queue<BankTransaction> getTransactions()
        {
            return Transactions;
        }
    }

    class Building
    {
        public int buildNumber { get; private set; }
        public int buildHeight { get; private set; }
        public int buildFloor { get; private set; }
        public int buildApartments { get; private set; }
        public int buildEntrance { get; private set; }

        static int bNum = 1;

        public int GetBuildNumber()
        {
            buildNumber = bNum;
            bNum++;
            return buildNumber;
        }
        public int GetBuildFloorHeight(int floorCount, int buildheight)
        {
            int result = buildheight/floorCount;
            return result;
        }
    }

    class Laba10
    {
        public string Reverse(string str)                               //Laba10Task2
        {
            char[] letter = str.ToCharArray();
            Array.Reverse(letter);
            str = new string(letter);
            return str;
        }
    }
    public class Temperature : IFormattable
    {
        private decimal temp;
        public Temperature(decimal temperature)
        {
            if (temperature < -273.15m)
                throw new ArgumentOutOfRangeException(String.Format("{0} is less than absolute zero.", temperature));
            this.temp = temperature;
        }
        public string ToString(string format, IFormatProvider provider)
        {
            return temp.ToString("F2", provider) + " °C";
        }
        
    }
    class Song
    {
        string name;
        string author;
        Song prev;
        public Song() { }                   //Laba11Task5
        public Song(string Name, string Author)
        {
            name = Name;
            author = Author;
            prev = null;
        }
        public Song(string Name, string Author, Song Prev)
        {
            name = Name;
            author = Author;
            prev = Prev;
        }
        public void Name(string nAme)
        {
            name = nAme;
        }
        public void Author(string aUthor)
        {
            author = aUthor;
        }
        public string Title()
        {
            return $"{name} + {author}";
        }
        public void Prev(Song song)
        {
            prev = song;
        }
        public override bool Equals(object d)
        {
            if (d.GetType() == GetType())
                return true;
            else
                return false;
        }
    }
    class BankTransaction
    {
        public readonly double TransferMoney;
        public readonly DateTime DateTimeInfo;

        public BankTransaction(double transferSumm)
        {
            TransferMoney = transferSumm;
        }
    }
}
