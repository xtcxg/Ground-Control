﻿#pragma checksum "C:\Users\jiajia\source\repos\Ground-Control\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "231F47E82900443FE09A1468C4BAF3863BB5DC819C3E088F820B7261861EBCD0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ground_Control
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 3: // MainPage.xaml line 23
                {
                    this.cmd_panel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 4: // MainPage.xaml line 49
                {
                    this.kv_panel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 5: // MainPage.xaml line 50
                {
                    this.AssociateName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // MainPage.xaml line 51
                {
                    this.AssociatePhoneNumber = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7: // MainPage.xaml line 52
                {
                    this.TargetInstallDate = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                }
                break;
            case 8: // MainPage.xaml line 53
                {
                    this.InstallTime = (global::Windows.UI.Xaml.Controls.TimePicker)(target);
                }
                break;
            case 9: // MainPage.xaml line 25
                {
                    this.script_chose = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.script_chose).SelectionChanged += this.ScriptChange;
                }
                break;
            case 10: // MainPage.xaml line 45
                {
                    this.cmd_list = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 11: // MainPage.xaml line 34
                {
                    this.add_cmd = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.add_cmd).Click += this.AddCmd;
                }
                break;
            case 12: // MainPage.xaml line 35
                {
                    this.submit_cmd = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.submit_cmd).Click += this.SubmitCmd;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
