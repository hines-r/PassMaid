using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid
{
    public static class SecureStringExtension
    {
        public static string GetString(this SecureString source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            int length = source.Length;
            char[] chars = new char[length];
            IntPtr pointer = IntPtr.Zero;

            string result = null;

            try
            {
                pointer = Marshal.SecureStringToBSTR(source);
                Marshal.Copy(pointer, chars, 0, length);

                result = new string(chars);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Could not get data from SecureString!", e);
            }
            finally
            {
                if (pointer != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(pointer);
                }
            }

            return result;
        }
    }
}
