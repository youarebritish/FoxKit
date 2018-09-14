using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;

public class MaterialInspectorMockup : MonoBehaviour
{
    [FoldoutGroup("Data"), ReadOnly]
    public string referencePath = "cypr_tr_grss002";

    [FoldoutGroup("Material")]
    public string materialName = string.Empty;

    [FoldoutGroup("Material"), FilePath]
    public string shader = string.Empty;

    [FoldoutGroup("Material"), FilePath(Extensions = "ftex")]
    public string diffuseTexture =
        "/Assets/tpp/common_source/fmtr/terrain/cyprus/cm_cypr_tr_grss002/sourceimages/cm_cypr_tr_grss002_bsm.ftex";

    [FoldoutGroup("Material"), FilePath(Extensions = "ftex")]
    public string srmTexture =
        "/Assets/tpp/common_source/fmtr/terrain/cyprus/cm_cypr_tr_grss002/sourceimages/cm_cypr_tr_grss002_srm.ftex";

    [FoldoutGroup("Material"), FilePath(Extensions = "ftex")]
    public string normalTexture =
        "/Assets/tpp/common_source/fmtr/terrain/cyprus/cm_cypr_tr_grss002/sourceimages/cm_cypr_tr_grss002_nrm.ftex";

    [FoldoutGroup("Material"), FilePath(Extensions = "ftex")]
    public string materialMapTexture = string.Empty;

    [FoldoutGroup("Material")]
    public byte materialIndex = 21;

    [FoldoutGroup("Material")]
    public Color diffuseColor = Color.white;

    [FoldoutGroup("Material")]
    public Color specularColor = Color.black;

    [FoldoutGroup("Material"), FilePath]
    public string fmtrPath = string.Empty;

    [FoldoutGroup("Material")]
    public bool residentFlag = false;
}
