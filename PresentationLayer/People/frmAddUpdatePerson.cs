using BuisnessLogicLayer.DataManagement;
using DVLD.Classes;
using DVLD_DTOs;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace PresentationLayer.People
{
    public partial class frmAddUpdatePerson : Form
    {

        public delegate void DataBackEventHandler(object sender, int PersonID);

        public event DataBackEventHandler DataBack;
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enGendor { Male = 0, Female = 1 };

        private enMode _Mode;
        private int _PersonID = -1;
        clsPerson _Person;
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _PersonID = PersonID;
        }
        private void _ResetDefualtValues()
        {
            _FillCountriesInComoboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblTitle.Text = "Update Person";
            }

            //hide/show the remove linke incase there is no image for the person.
            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

            //we set the max date to 18 years from today, and set the default value the same.
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            //should not allow adding age more than 100 years
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            //this will set default country to jordan.
            cbCountry.SelectedIndex = cbCountry.FindString("Iraq");

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
        }
        private void _FillCountriesInComoboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
            if (cbCountry.Items.Count > 0)
                cbCountry.SelectedIndex = 0;
        }
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            //the following code will not be executed if the person was not found
            lblPersonID.Text = _PersonID.ToString();
            _ApplyPersonDtoToForm(_Person.DTO);
        }
        private void _ApplyPersonDtoToForm(PersonDTO dto)
        {
            txtFirstName.Text = dto.FirstName;
            txtSecondName.Text = dto.SecondName;
            txtThirdName.Text = dto.ThirdName;
            txtLastName.Text = dto.LastName;
            txtNationalNo.Text = dto.NationalNo;
            dtpDateOfBirth.Value = dto.DateOfBirth;

            if (dto.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            txtAddress.Text = dto.Address;
            txtPhone.Text = dto.Phone;
            txtEmail.Text = dto.Email;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.NationalityCountryName);

            //load person image incase it was set.
            if (!string.IsNullOrWhiteSpace(dto.ImagePath))
            {
                pbPersonImage.ImageLocation = dto.ImagePath;

            }

            //hide/show the remove linke incase there is no image for the person.
            llRemoveImage.Visible = !string.IsNullOrWhiteSpace(dto.ImagePath);
        }
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }
        private string _GetSelectedImagePath()
        {
            return pbPersonImage.ImageLocation ?? string.Empty;
        }
        private bool _HandlePersonImage(string oldImagePath, ref string newImagePath)
        {
            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


            // oldImagePath contains the old image, we check if it changed then copy new image.
            if (oldImagePath != newImagePath)
            {
                if (!string.IsNullOrWhiteSpace(oldImagePath))
                {
                    //first we delete the old image from the folder in case there is any.

                    try
                    {
                        File.Delete(oldImagePath);
                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        //log it later   
                    }
                }

                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    //then we copy the new image to the image folder after we rename it
                    string sourceImageFile = newImagePath;

                    if (clsUtil.CopyImageToProjectImagesFolder(ref sourceImageFile))
                    {
                        newImagePath = sourceImageFile;
                        pbPersonImage.ImageLocation = sourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            }
            return true;
        }
        private PersonDTO _CreatePersonDtoFromForm(int personId, string imagePath, int nationalityCountryID)
        {
            return new PersonDTO
            {
                PersonID = personId,
                FirstName = txtFirstName.Text.Trim(),
                SecondName = txtSecondName.Text.Trim(),
                ThirdName = txtThirdName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                NationalNo = txtNationalNo.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                DateOfBirth = dtpDateOfBirth.Value,
                Gendor = rbMale.Checked ? (byte)enGendor.Male : (byte)enGendor.Female,
                NationalityCountryID = nationalityCountryID,
                ImagePath = imagePath
            };
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            clsCountry SelectedCountry = clsCountry.Find(cbCountry.Text);

            if (SelectedCountry == null)
            {
                MessageBox.Show("Please select a valid country!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int NationalityCountryID = SelectedCountry.DTO.CountryID;

            string oldImagePath = _Person.DTO.ImagePath;
            string selectedImagePath = _GetSelectedImagePath();
            if (!_HandlePersonImage(oldImagePath, ref selectedImagePath))
                return;

            int personId = _Person.DTO.PersonID;
            _Person.DTO = _CreatePersonDtoFromForm(personId, selectedImagePath, NationalityCountryID);

            if (_Person.Save())
            {
                lblPersonID.Text = _Person.DTO.PersonID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "Update Person";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Person.DTO.PersonID);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
            }
        }
        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            llRemoveImage.Visible = false;
        }
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {

            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }

        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            //no need to validate the email incase it's empty.
            if (txtEmail.Text.Trim() == "")
                return;

            //validate email format
            if (!clsValidatoin.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
            ;

        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            //Make sure the national number is not used by another person
            if (txtNationalNo.Text.Trim() != _Person.DTO.NationalNo && clsPerson.IsExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");

            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }
        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }
        private void cbCountry_Validating(object sender, CancelEventArgs e)
        {
            if (cbCountry.SelectedIndex == -1)
            {
                e.Cancel = true;
                errorProvider1.SetError(cbCountry, "Please select a country!");
            }
            else
            {
                errorProvider1.SetError(cbCountry, null);
            }
        }
    }
}
