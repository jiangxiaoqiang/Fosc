using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fosc.Dolphin.Common.Db.Sqlite;
using Fosc.Dolphin.Common.Security;

namespace Fosc.Dolphin.UI.Dev
{
    public partial class FrmTest : Form
    {
        public FrmTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string text = "Hello, world!";
                string entropy = null;
                string description;

                MessageBox.Show(string.Format("Plaintext: {0}\r\n", text));

                // Call DPAPI to encrypt data with user-specific key.
                string encrypted = Dpapi.Encrypt(Dpapi.KeyType.UserKey,
                                                  text,
                                                  entropy,
                                                  "My Data");
                MessageBox.Show(string.Format("Encrypted: {0}\r\n", encrypted));

                // Call DPAPI to decrypt data.
                string decrypted = Dpapi.Decrypt(encrypted,
                                                    entropy,
                                                out description);
                MessageBox.Show(string.Format("Decrypted: {0} <<<{1}>>>\r\n",decrypted, description));
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Console.WriteLine(ex.Message);
                    ex = ex.InnerException;
                }
            }
        }
    }
}
