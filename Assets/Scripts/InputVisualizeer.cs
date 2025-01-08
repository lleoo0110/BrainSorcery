using HolmonUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 入力の可視化を行う
/// </summary>
public class InputVisualizeer : MonoBehaviour
{
    [Header("入力")]
    [SerializeField] private InputBase NewtralInput;
    [SerializeField] private InputBase FireInput;
    [SerializeField] private InputBase ElectricInput;

    [Header("可視化オブジェクト")]
    [SerializeField] private Image FireIcon;
    [SerializeField] private Color FireIconColor;
    [SerializeField] private Image ElectricImage;
    [SerializeField] private Color ElectricColor;



    //現在入力中の魔法タイプ
    private MagicType magicType;



    private void Start()
    {
        //各入力のコールバックを受け取った時に、magicTypeを変更する
        NewtralInput.AssignInputOnCallback(() => { magicType = MagicType.None; });
        FireInput.AssignInputOnCallback(() => { magicType = MagicType.Fire; });
        ElectricInput.AssignInputOnCallback(() => { magicType = MagicType.Electric; });
    }

    private void Update()
    {
        //magicTypeに応じて、可視化オブジェクトの表示状態を変更する
        FireIcon.color = magicType == MagicType.Fire ? FireIconColor : Color.black;
        ElectricImage.color = magicType == MagicType.Electric ? ElectricColor : Color.black;
    }
}