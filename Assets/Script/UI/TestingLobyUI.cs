using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingLobyUI : MonoBehaviour
{
    public void CreatGameButton()
    {
        KichenGameMultipler.Instance.StartHost();
        Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
    }
    public void JoinGameButton()
    {
        KichenGameMultipler.Instance.StartClient();

    }

}
