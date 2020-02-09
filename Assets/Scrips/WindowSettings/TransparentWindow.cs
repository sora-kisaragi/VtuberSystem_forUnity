///
/// Copyright (c) 2019 wakagomo
///
/// This source code is released under the MIT License.
/// http://opensource.org/licenses/mit-license.php
///

using UnityEngine;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Make the window transparent.
/// </summary>
public class TransparentWindow : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN

    #region WINDOWS API
    /// <summary>
    /// Returned by the GetThemeMargins function to define the margins of windows that have visual styles applied.
    /// </summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/api/uxtheme/ns-uxtheme-_margins
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    /// <summary>
    /// Retrieves the window handle to the active window attached to the calling thread's message queue.
    /// </summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getactivewindow
    [DllImport("User32.dll")]
    private static extern IntPtr GetActiveWindow();
    /// <summary>
    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
    /// </summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setwindowlonga
    [DllImport("User32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    /// <summary>
    /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
    /// </summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setwindowpos
    [DllImport("User32.dll")]
    private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    /// <summary>
    /// Extends the window frame into the client area.
    /// </summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/api/dwmapi/nf-dwmapi-dwmextendframeintoclientarea
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
    #endregion

    private void Awake()
    {
        IntPtr windowHandle = GetActiveWindow();

        { // SetWindowLong
            const int GWL_STYLE = -16;
            const int GWL_EXSTYLE = -20;
            const uint WS_POPUP = 0x80000000;
            const uint WS_EX_LAYERD = 0x080000;
            const uint WS_EX_TRANSPARENT = 0x00000020;

            SetWindowLong(windowHandle, GWL_STYLE, WS_POPUP);
            //SetWindowLong(windowHandle, GWL_EXSTYLE, WS_EX_LAYERD | WS_EX_TRANSPARENT);
            SetWindowLong(windowHandle, GWL_EXSTYLE, WS_EX_LAYERD);
        }

        { // SetWindowPos
            IntPtr HWND_TOPMOST = new IntPtr(-1);
            IntPtr HWND_TOP     = new IntPtr(0);
            IntPtr HWND_BOTTOM = new IntPtr(1);
            IntPtr HWND_NOTOPMOST = new IntPtr(2);
            const uint SWP_NOSIZE = 0x0001;
            const uint SWP_NOMOVE = 0x0002;
            const uint SWP_NOACTIVE = 0x0010;
            const uint SWP_SHOWWINDOW = 0x0040;

            SetWindowPos(windowHandle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVE | SWP_SHOWWINDOW);
        }

        { // DwmExtendFrameIntoClientArea
            MARGINS margins = new MARGINS()
            {
                cxLeftWidth = -1
            };

            DwmExtendFrameIntoClientArea(windowHandle, ref margins);
        }
    }


#endif // !UNITY_EDITOR && UNITY_STANDALONE_WIN
}
