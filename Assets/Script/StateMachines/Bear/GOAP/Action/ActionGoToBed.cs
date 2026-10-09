using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGoToBed : GAction
{
    public override bool PrePerform()
    {
        target = GameObject.FindWithTag("Bed");
        if (target == null)
        {
            Debug.LogWarning("Không tìm thấy chỗ ngủ! Hãy kiểm tra lại Tag 'Bed'.");
            return false;
        }
        return true;
    }

    public override bool PostPerform() => true;
}
