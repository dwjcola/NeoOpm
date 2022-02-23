using NeoOPM;
using UnityEditor;
public class UIViewItemEditorWin : EditorWindow
{ 
    private static UIViewItemEditorWin uiMonoEditorWin;
    private static UIMonoItem uimono;
    public static UIViewItemEditorWin OnInit(UIMonoItem uimono1)
    {
        uimono = uimono1;
        uiMonoEditorWin = EditorWindow.GetWindow<UIViewItemEditorWin>(false, "编辑引用", true);
        uiMonoEditorWin.Show();
        return uiMonoEditorWin;
    }  
    void OnGUI()
    {
        //if (uimono != null) UIViewItemEditor.ShowInfo();
        //if ( uimono != null ) UIViewItemEditor.ShowStringInfo ( );
        if (uimono != null)
        {
            UIViewItemEditor.baseEditor.ShowGUI(uimono);
        }
    } 
}
