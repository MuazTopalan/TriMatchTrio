using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneUIManager : MonoBehaviour
{
    public TextMeshProUGUI CurrentUserNameText;

    private void Start()
    {
        CurrentUserNameText.text = FirebaseAuthManager.Instance.User.DisplayName;
    }
}
