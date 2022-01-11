using UnityEditor;
using NeoOPM;

public class UIViewEditorWin : EditorWindow
{ 
    private static UIViewEditorWin uiMonoEditorWin;
    private static UIMonoPanel uimono;
    public static UIViewEditorWin OnInit(UIMonoPanel uimono1)
    {
        uimono = uimono1;
        uiMonoEditorWin = EditorWindow.GetWindow<UIViewEditorWin>(false, "编辑引用", true);
        uiMonoEditorWin.Show();
        return uiMonoEditorWin;
    }  
    void OnGUI()
    {
        if (uimono != null) UIViewEditor.ShowInfo();
        if ( uimono != null ) UIViewEditor.ShowStringInfo ( );
      
    } 
}
