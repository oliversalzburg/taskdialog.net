using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TaskDialogNet.UserInterface {
  public class RichTextBoxEx : RichTextBox {
    
    private const UInt32 CFE_LINK = 32;
    private const UInt32 CFM_LINK = 32;
    private const int WM_USER = 1024;
    private const int EM_SETCHARFORMAT = ( WM_USER + 68 );
    private const int SCF_SELECTION = 1;

    private Dictionary<string,string> Links { get; set; }

    public RichTextBoxEx( ) {
      DetectUrls  = false;
      Links       = new Dictionary<string, string>();
    }

    public void ConvertLinks() {
      // <A HREF=\\"([^"]*)">([^<]*)</A>
      // <a href=\\\"(?<href>[^\"]*)\\\">(?<title>[^<]*)</a>
      Links.Clear();
      Regex findLinks = new Regex( "<a href=\\\"(?<href>[^\"]*)\\\">(?<title>[^<]*)</a>", RegexOptions.IgnoreCase );
      MatchCollection matches = findLinks.Matches( Text );
      foreach( Match match in matches ) {
        SelectionStart  = match.Groups[ "title" ].Index;
        SelectionLength = match.Groups[ "title" ].Length;
        SetSelectionStyle( CFM_LINK, CFE_LINK );
        Links.Add( match.Groups[ "title" ].Value, match.Groups[ "href" ].Value );
      }

      ReadOnly = false;
      Regex findRemainder = new Regex( "(?<in><a [^>]+>)[^<]*(?<out></a>)", RegexOptions.IgnoreCase );
      matches = findRemainder.Matches( Text );
      for( int i = matches.Count - 1; i >= 0; i-- ) {
        SelectionStart  = matches[ i ].Groups[ "out" ].Index;
        SelectionLength = matches[ i ].Groups[ "out" ].Length;
        SelectedText    = string.Empty;
        SelectionStart  = matches[ i ].Groups[ "in" ].Index;
        SelectionLength = matches[ i ].Groups[ "in" ].Length;
        SelectedText    = string.Empty;
      }
      
      SelectionStart  = 0;
      SelectionLength = 0;
      ReadOnly = true;  
    }

    /// <summary>
    /// Retrieves the URL for a given link text.
    /// This can be problematic if multiple links with the same text are given.
    /// </summary>
    /// <param name="linkText"></param>
    /// <returns></returns>
    public string GetUrlForLinkText( string linkText ) {
      return Links[ linkText ];
    }

    public void SetSelectionStyle( UInt32 mask, UInt32 effect ) {
      CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
      cf.cbSize = (uint)Marshal.SizeOf( cf );
      cf.dwMask = mask;
      cf.dwEffects = effect;
      IntPtr wpar = new IntPtr( SCF_SELECTION );
      IntPtr lpar = Marshal.AllocCoTaskMem( Marshal.SizeOf( cf ) );
      Marshal.StructureToPtr( cf, lpar, false );
      IntPtr res = SendMessage( this.Handle, EM_SETCHARFORMAT, wpar, lpar );
      Marshal.FreeCoTaskMem( lpar );
    }

    [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = false )]
    internal static extern IntPtr SendMessage( IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam );

    [StructLayout( LayoutKind.Sequential )]
    private struct CHARFORMAT2_STRUCT {
      public UInt32 cbSize;
      public UInt32 dwMask;
      public UInt32 dwEffects;
      public Int32 yHeight;
      public Int32 yOffset;
      public Int32 crTextColor;
      public Byte bCharSet;
      public Byte bPitchAndFamily;
      [MarshalAs( UnmanagedType.ByValArray, SizeConst = 32 )]
      public Char[] szFaceName;
      public UInt16 wWeight;
      public UInt16 sSpacing;
      public int crBackColor;
      public int lcid;
      public int dwReserved;
      public Int16 sStyle;
      public Int16 wKerning;
      public Byte bUnderlineType;
      public Byte bAnimation;
      public Byte bRevAuthor;
      public Byte bReserved1;
    }
  }
}