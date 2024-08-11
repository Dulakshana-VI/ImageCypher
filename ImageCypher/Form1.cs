using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageCypher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = readXP();
        }

        #region props
        public string filepath;
        public string imagename;
        #endregion

        #region methods

        public bool writeXP()
        {
            try
            {
                string datestring = textBox1.Text;
                StringBuilder binaryString = new StringBuilder();
                foreach (char c in datestring)
                {
                    // Convert char to its ASCII integer value
                    int asciiValue = (int)c;

                    // Convert integer to a padded 8-bit binary string
                    string binaryChar = Convert.ToString(asciiValue, 2).PadLeft(8, '0');
                    binaryString.Append(binaryChar);
                }
                string bindatestring = binaryString.ToString();
                Console.WriteLine(bindatestring);
                //Console.WriteLine((bindatestring).Length);
                //return bindatestring;

                //write image
                Image image = Image.FromFile(filepath);
                Bitmap bitmap = (Bitmap)image;

                // Loop through each pixel in the selection area
                for (int x = 0; x < (bindatestring).Length; x++)
                {
                    // Get the current pixel color
                    Color oldColor = bitmap.GetPixel(x, 0);

                    // Define the new color (replace with your desired color)
                    Color newColor = Color.White;

                    if (bindatestring[x].ToString() == "1")
                    {
                        newColor = Color.FromArgb(0, 0, 0);
                    }
                    else
                    {
                        newColor = Color.FromArgb(255, 255, 255);
                    }

                    bitmap.SetPixel(x, 0, newColor);
                }



                // Save the modified image (optional)
                bitmap.Save("new_" + imagename, ImageFormat.Png);

                // Dispose of the objects (important)
                image.Dispose();
                bitmap.Dispose();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public string readXP()
        {

            Image image = Image.FromFile(filepath);
            Bitmap bitmap = (Bitmap)image;
            string exdate = "";

            for (int x = 0; x < 152; x++)
            {
                Color pixelColor = bitmap.GetPixel(x, 0);
                if (pixelColor.R == pixelColor.G && pixelColor.G == pixelColor.B && pixelColor.B == 0)
                {
                    exdate += "1";
                }
                else
                {
                    exdate += "0";
                }
            }
            //Console.WriteLine(exdate.Length);
            Console.WriteLine(exdate);
            
            //back to string
            string text = string.Empty;
            for (int i = 0; i < exdate.Length; i += 8)
            {
                string byteString = exdate.Substring(i, 8);
                int asciiValue = Convert.ToInt32(byteString, 2);
                char character = (char)asciiValue;
                text += (character);
            }
            Console.WriteLine(text);
            image.Dispose();
            return text;
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            bool isWriteSuccess = writeXP();

            if (isWriteSuccess)
            {
                MessageBox.Show("Successfully Written");
                //Console.WriteLine("Successfully Written");
                textBox1.Clear();
            }
            else
            {
                MessageBox.Show("Error. Try another Image");
                //Console.WriteLine("Successfully Written");
                textBox1.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image files (*.png)|*.png";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                filepath = fd.FileName;
                imagename = filepath.Split('\\')[filepath.Split('\\').Length-1];
                textBox2.Text = imagename;
            }
        }
    }
}
