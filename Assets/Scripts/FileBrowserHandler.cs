using UnityEngine;
using System.Runtime.InteropServices;

public class FileBrowserHandler : MonoBehaviour
{
    [DllImport("FileBrowserPlugin.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern System.IntPtr OpenFileDialog();

    public string OpenFileBrowserDialog()
    {
        return Marshal.PtrToStringAnsi(OpenFileDialog());
    }
}
