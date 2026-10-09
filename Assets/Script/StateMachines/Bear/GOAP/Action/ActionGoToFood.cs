using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGoToFood : GAction
{
    public override bool PrePerform()
    {
        // Tự động tìm vị trí khối Cube B đã gắn Tag "Food"
        target = GameObject.FindWithTag("Food");
        if (target == null)
        {
            Debug.LogWarning("Không tìm thấy đồ ăn! Hãy kiểm tra lại Tag 'Food'.");
            return false;
        }
        return true;
    }

    public override bool PostPerform() => true;
}
