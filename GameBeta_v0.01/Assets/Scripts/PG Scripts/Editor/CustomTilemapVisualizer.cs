using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

[CustomEditor(typeof(TileBase))]
public class CustomTilemapVisualizer : Editor
{
    TileBase tileBase;
    private void OnEnable()
    {      
        tileBase = target as TileBase;
        
    }

    public override void OnInspectorGUI()
    {
       
        base.OnInspectorGUI();
        var tile = (Tile)tileBase;
        if (tile.sprite == null)
            return;

        //Convert the weaponSprite (see SO script) to Texture
        Texture2D texture = AssetPreview.GetAssetPreview(tile.sprite);
        //We crate empty space 80x80 (you may need to tweak it to scale better your sprite
        //This allows us to place the image JUST UNDER our default inspector
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        //Draws the texture where we have defined our Label (empty space)
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);

    }
   
}
