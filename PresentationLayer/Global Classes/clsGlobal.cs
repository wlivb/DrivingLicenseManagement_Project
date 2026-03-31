using BuisnessLogicLayer.DataManagement;
using DVLD_DTOs;
using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using Microsoft.Win32;

namespace PresentationLayer.Global_Classes
{
    [Serializable]
    internal class clsGlobal
    {
        public static clsUser CurrentUser;
        private static string _registryPath = @"SOFTWARE\DVLD";
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(_registryPath))
                {
                    if (string.IsNullOrEmpty(Username))
                    {
                        if (key != null)
                        {
                            key.DeleteValue("Username", false);
                            key.DeleteValue("Password", false);
                        }
                        return true;
                    }

                    key.SetValue("Username", Username);
                    key.SetValue("Password", Password);
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
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath))
                {
                    if (key != null)
                    {
                        Username = key.GetValue("Username") as string;
                        Password = key.GetValue("Password") as string;

                        return (!string.IsNullOrEmpty(Username));
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }
    }
}
