using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Modbus.Common
{
    public partial class DataTab : UserControl
    {
        protected int _displayCtrlCount;

        public DataTab()
        {
            InitializeComponent();
        }

        #region properties

        public ushort StartAddress
        {
            get
            {
                ushort rVal = 0;
                try
                {
                    if (txtStartAdress.Text.IndexOf("0x", 0, txtStartAdress.Text.Length) == 0)
                    {
                        string str = txtStartAdress.Text.Replace("0x", "");
                        rVal = Convert.ToUInt16(str, 16);
                    }
                    // bugfix "not accepting hex register address"
                    else
                        rVal = Convert.ToUInt16(txtStartAdress.Text);
                }
                catch (Exception)
                {
                    txtStartAdress.Text = "0";
                }
                return rVal;
            }
            set
            {
                txtStartAdress.Text = Convert.ToString(value);
            }
        }

        public ushort DataLength
        {
            get
            {
                ushort rVal = 64;
                try
                {
                    if (txtSize.Text.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string str = txtSize.Text.Replace("0x", "");
                        rVal = Convert.ToUInt16(str, 16);
                    }
                    else
                        rVal = Convert.ToUInt16(txtSize.Text);
                }
                catch (Exception)
                {
                    txtSize.Text = "64";
                }
                return rVal;
            }
            set
            {
                if (txtSize.Text.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                {
                    // obey hex input
                    txtSize.Text = "0x"+Convert.ToString(value, 16);
                }
                else
                    txtSize.Text = Convert.ToString(value);
            }
        }

        private void txtSize_TextChanged(object sender, EventArgs e)
        {
            if (DataLength > 127)
            {
                DataLength = 127;
            }
        }

        public bool ShowDataLength
        {
            get
            {
                return txtSize.Visible;
            }
            set
            {
                txtSize.Visible = value;
                labelTxtSize.Visible = value;
            }
        }

        public event EventHandler OnApply;

        public ushort[] RegisterData { get; set; }
        public DisplayFormat DisplayFormat { get; set; }

        #endregion

        #region Data Table

        public void RefreshData()
        {
            // Create as many textboxes as fit into window
            groupBoxData.Visible = false;
            groupBoxData.Controls.Clear();
            var idxControl = 0;
            var screenX = 10;
            var screenY = 20;
            while (screenX < groupBoxData.Size.Width - 100)
            {
                var labData = new Label();
                groupBoxData.Controls.Add(labData);
                labData.Size = new Size(40, 20);
                labData.Location = new Point(screenX, screenY);
                labData.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
                switch (DisplayFormat)
                {
                    case DisplayFormat.LED:
                        var bulb = new LedBulb();
                        groupBoxData.Controls.Add(bulb);
                        bulb.Size = new Size(25, 25);
                        bulb.Location = new Point(screenX + 40, screenY - 5);
                        bulb.Padding = new Padding(3);
                        bulb.Color = Color.Red;
                        bulb.On = false;
                        bulb.Tag = idxControl;
                        bulb.Click += BulbClick;
                        screenY = screenY + bulb.Size.Height + 10;
                        labData.Text = Convert.ToString(idxControl);
                        break;
                    case DisplayFormat.Binary:
                        var txtDataB = new TextBox();
                        groupBoxData.Controls.Add(txtDataB);
                        txtDataB.Size = new Size(110, 20);
                        txtDataB.Location = new Point(screenX + 40, screenY - 2);
                        txtDataB.TextAlign = HorizontalAlignment.Right;
                        txtDataB.Tag = idxControl;
                        txtDataB.Leave += TxtDataBinaryLeave;
                        txtDataB.Enter += txtData_Enter;
                        txtDataB.KeyPress += txtDataBinaryKeyPress;
                        txtDataB.MaxLength = 16;
                        screenY = screenY + txtDataB.Size.Height + 5;
                        labData.Text = Convert.ToString(StartAddress + idxControl);
                        break;
                    case DisplayFormat.Hex:
                        var txtDataH = new TextBox();
                        groupBoxData.Controls.Add(txtDataH);
                        txtDataH.Size = new Size(55, 20);
                        txtDataH.Location = new Point(screenX + 40, screenY - 2);
                        txtDataH.TextAlign = HorizontalAlignment.Right;
                        txtDataH.Tag = idxControl;
                        txtDataH.MaxLength = 5;
                        txtDataH.Leave += TxtDataHexLeave;
                        txtDataH.Enter += txtData_Enter;
                        txtDataH.KeyPress += txtDataHexKeyPress;
                        screenY = screenY + txtDataH.Size.Height + 5;
                        labData.Text = Convert.ToString(StartAddress + idxControl);
                        break;
                    case DisplayFormat.Integer:
                        var txtData = new TextBox();
                        groupBoxData.Controls.Add(txtData);
                        txtData.Size = new Size(55, 20);
                        txtData.Location = new Point(screenX + 40, screenY - 2);
                        txtData.TextAlign = HorizontalAlignment.Right;
                        txtData.Tag = idxControl;
                        txtData.MaxLength = 5;
                        txtData.Leave += TxtDataLeave;
                        txtData.Enter += txtData_Enter;
                        txtData.KeyPress += txtDataIntegerKeyPress;
                        screenY = screenY + txtData.Size.Height + 5;
                        labData.Text = Convert.ToString(StartAddress + idxControl);
                        break;
                    case DisplayFormat.FloatReverse:
                        // Float values require two registers, thus skip every second control
                        // hide even controls
                        labData.Text = Convert.ToString(StartAddress + idxControl);
                        if ((idxControl & 1) == 0)
                        {
                            var txtFloatReverse = new TextBox();
                            groupBoxData.Controls.Add(txtFloatReverse);
                            txtFloatReverse.Size = new Size(55, 40);
                            txtFloatReverse.Location = new Point(screenX + 40, screenY - 2);
                            txtFloatReverse.TextAlign = HorizontalAlignment.Right;
                            txtFloatReverse.Tag = idxControl;
                            txtFloatReverse.MaxLength = 5;
                            txtFloatReverse.Leave += TxtFloatReverseLeave;
                            txtFloatReverse.Enter += TxtFloatReverse_Enter;
                            txtFloatReverse.KeyPress += txtDataFloatReverseKeyPress;
                            screenY = screenY + txtFloatReverse.Size.Height *2 + 10; // Float Values Require Two Registers
                            labData.Visible = true;
                        }
                        else
                        {
                            labData.Visible = false;
                        }
                        break;
                }

                idxControl++;
                if (screenY > groupBoxData.Size.Height - 30)
                {
                    var inc = DisplayFormat == DisplayFormat.Binary ? 200 : 100;
                    screenX = screenX + inc;
                    screenY = 20;
                }
            }
            _displayCtrlCount = idxControl;
            UpdateDataTable();
            groupBoxData.Visible = true;

        }

        void txtDataBinaryKeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            // this will only allow valid binary values [0-1] [delete] to be entered. 
            char c = e.KeyChar;
            if (c != '\b' && c != 0x30 && c != 0x31 && c != Delete)
            {
                e.Handled = true;
            }
        }

        void txtDataHexKeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            // this will only allow valid hex values [0-9][a-f][A-F] [delete] to be entered. 
            char c = e.KeyChar;
            if (c != '\b' && !((c <= 0x66 && c >= 61) || (c <= 0x46 && c >= 0x41) || (c >= 0x30 && c <= 0x39)) && c != Delete)
            {
                e.Handled = true;
            }
        }

        void txtDataIntegerKeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        void txtDataFloatReverseKeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete && e.KeyChar != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
        }

        void txtData_Enter(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!String.IsNullOrEmpty(textBox.Text))
            {
                textBox.Clear();
            }
        }

        void TxtFloatReverse_Enter(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!String.IsNullOrEmpty(textBox.Text))
            {
                textBox.Clear();
            }
        }

        void TxtDataLeave(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var textBoxNumber = Int32.Parse(textBox.Tag.ToString());
            UInt16 res;
            if (UInt16.TryParse(textBox.Text, out res))
            {
                RegisterData[StartAddress + textBoxNumber] = res;
            }
            else
            {
                textBox.Text = "0";
            }
        }

        void TxtFloatReverseLeave(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var textBoxNumber = Int32.Parse(textBox.Tag.ToString());

            float res;
            if (float.TryParse(textBox.Text, out res))
            {
                var intRes = BitConverter.ToUInt32(BitConverter.GetBytes(res), 0);

                var firstPart = (ushort) (intRes >> 16);
                var secondPart = (ushort)(intRes & 0xFFFF);

                RegisterData[StartAddress +     textBoxNumber] = firstPart;
                RegisterData[StartAddress + 1 + textBoxNumber] = secondPart;
            }
            else
            {
                textBox.Text = "0.0";
            }
        }

        void TxtDataHexLeave(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var textBoxNumber = Int32.Parse(textBox.Tag.ToString());
            ushort res;
            if (UInt16.TryParse(textBox.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out res))
            {
                RegisterData[StartAddress + textBoxNumber] = res;
            }
            else
            {
                textBox.Text = "0x0000";
            }
            textBox.Text = string.Format("0x{0}", textBox.Text.ToLower().PadLeft(4, '0'));
        }

        void TxtDataBinaryLeave(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var textBoxNumber = Int32.Parse(textBox.Tag.ToString());
            try
            {
                RegisterData[StartAddress + textBoxNumber] = Convert.ToUInt16(textBox.Text, 2);
                textBox.Text = textBox.Text.PadLeft(16, '0');
            }
            catch (Exception)
            {
                textBox.Text = "0000000000000000";
            }
        }


        private void BulbClick(object sender, EventArgs e)
        {
            var bulb = (LedBulb)sender;
            bulb.On = !bulb.On;
            var bulbNumber = Int32.Parse(bulb.Tag.ToString());
            var index = bulbNumber / 16;
            var bulbHiByte = (bulbNumber & 0x0008) != 0;
            ushort shifter = bulbHiByte ? (ushort)0x0001 : (ushort)0x0100;
            var shift = bulbNumber & 0x0007;
            var mask = Convert.ToUInt16(shifter << shift);
            if (bulb.On)
            {
                RegisterData[StartAddress + index] |= mask;
            }
            else
            {
                mask = (ushort)~mask; ;
                RegisterData[StartAddress + index] &= mask;
            }
        }


        public void UpdateDataTable()
        {
            // for float values we need two registers for one value
            // add one extra data value for the last control
            var data = new uint[_displayCtrlCount+1];
            for (int i = 0; i < _displayCtrlCount+1; i++)
            {
                var index = StartAddress + i;
                if (index >= RegisterData.Length)
                {
                    break;
                }
                data[i] = RegisterData[index];
            }
            // ------------------------------------------------------------------------
            // Put new data into text boxes
            foreach (Control ctrl in groupBoxData.Controls)
            {
                if (ctrl is TextBox)
                {
                    int x = Convert.ToInt16(ctrl.Tag);
                    if (x <= data.GetUpperBound(0))
                    {
                        switch (DisplayFormat)
                        {
                            case DisplayFormat.Binary:
                                ctrl.Text = Convert.ToString(data[x], 2).PadLeft(16, '0');
                                break;
                            case DisplayFormat.Hex:
                                ctrl.Text = String.Format("0x{0:x4}", data[x]);
                                break;
                            case DisplayFormat.Integer:
                                ctrl.Text = data[x].ToString(CultureInfo.InvariantCulture);
                                break;
                            case DisplayFormat.FloatReverse:
                                uint twoWords = ((uint)data[x] << 16) + data[x + 1];
                                float r = BitConverter.ToSingle(BitConverter.GetBytes(twoWords), 0);
                                ctrl.Text = String.Format("{0:0.000}", r);
                                break;
                        }
                        ctrl.Visible = true;
                    }
                    else ctrl.Text = "";
                }
                else if (ctrl is LedBulb)
                {
                    var led = (LedBulb)ctrl;
                    var bulbNumber = Convert.ToInt16(ctrl.Tag);
                    var index = bulbNumber / 16;
                    var bulbHiByte = (bulbNumber & 0x0008) != 0;
                    var shift = bulbNumber & 0x0007;
                    ushort shifter = bulbHiByte ? (ushort)0x0001 : (ushort)0x0100;
                    var mask = Convert.ToUInt16(shifter << shift);
                    led.On = (mask & data[index]) != 0;
                }
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (txtStartAdress.Text != "")
            {
                try
                {
                    var address = StartAddress;
                    if (OnApply != null) OnApply(this, new EventArgs());
                    RefreshData();
                }
                catch (Exception)
                {
                    txtStartAdress.Text = "";
                }
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (RegisterData != null)
            {
                var count = DataLength;
                for (int i = StartAddress; i < RegisterData.Length && count-- != 0; i++)
                {
                    RegisterData[i] = 0;
                }
                RefreshData();                    
            }
        }
        #endregion

    }
}
