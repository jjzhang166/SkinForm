
using System;
using System.Windows.Forms;
using Controls.SkinForm;

namespace TestSkinForm
{
    public partial class Form1 : Form
    {
        SkinForm skinForm;
        CustomSkin[] customSkins = new CustomSkin[3];

        string[] skinConfigFiles = { "Office2007Luna.xml", "Office2007Silver.xml", "Office2007Obsidian.xml" };
        public Form1()
        {
            InitializeComponent();
            customSkins[0] = new CustomSkin(skinConfigFiles[0]);
            customSkins[1] = new CustomSkin(skinConfigFiles[1]);
            customSkins[2] = new CustomSkin(skinConfigFiles[2]);
            skinForm = new SkinForm();
        }

        private void radSilver_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == radLuna && radLuna.Checked)
            {
                skinForm.Skin = customSkins[0];
                skinForm.SkiningForm = this;
            }
            else if (sender == radSilver && radSilver.Checked)
            {
                skinForm.Skin = customSkins[1];
                skinForm.SkiningForm = this;
            }
            else if (sender == radObsidian && radObsidian.Checked)
            {
                skinForm.Skin = customSkins[2];
                skinForm.SkiningForm = this;
            }
        }
    }
}
