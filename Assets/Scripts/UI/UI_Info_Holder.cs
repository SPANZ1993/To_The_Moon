using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using TMPro;


public class UI_Info_Holder
{


    private Dictionary<string, int> ShaderStringToID = new Dictionary<string, int>()
    {
        {"TextMeshPro/Distance Field (UnityEngine.Shader)", 0}
    };


    public string[] GetMaterialFaces(Shader shader){
        int shaderId = ShaderStringToID[shader.ToString()];
        switch(shaderId){
            case 0: // TextMeshPro/Distance Field (UnityEngine.Shader)
            {
                return new string[]{
                    "_FaceColor",
                    "_OutlineColor",
                    "_SpecularColor",
                    "_ReflectFaceColor",
                    "_ReflectOutlineColor",
                    "_UnderlayColor",
                    "_GlowColor"
                };
                break;
            }
            default:
            {
                throw new ArgumentException("Did not provide a proper shader name...\n possible shaders are: " 
                + string.Join(", ", ShaderStringToID.Keys));
                return new string[]{};
            }
        }
    }

}
