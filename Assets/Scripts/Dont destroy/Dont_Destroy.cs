using UnityEngine;

public class Dont_Destroy : MonoBehaviour
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    // Sharing this gameobject between scenes
    private void Awake () => DontDestroyOnLoad(transform.gameObject);

}
