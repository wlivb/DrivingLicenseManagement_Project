using BuisnessLogicLayer.DataManagement;
using DVLD_DTOs;
using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;

namespace PresentationLayer.Global_Classes
{
    [Serializable]
    internal class clsGlobal
    {
        public static clsUser CurrentUser;
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "currentUser.json");

                if (string.IsNullOrEmpty(Username))
                {
                    if (File.Exists(filePath)) File.Delete(filePath);
                    return true;
                }

                UserDTO userToSave = new UserDTO();
                userToSave.UserName = Username;
                userToSave.Password = Password;

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserDTO));

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    serializer.WriteObject(stream, userToSave);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "currentUser.json");
                
                if (!File.Exists(filePath)) return false;

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserDTO));

                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    UserDTO storedUser = (UserDTO)serializer.ReadObject(fs);

                    if (storedUser != null)
                    {
                        Username = storedUser.UserName;
                        Password = storedUser.Password;
                        return true;
                    }
                }

                return true; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }
    }
}
