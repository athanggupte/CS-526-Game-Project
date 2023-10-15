using UnityEngine;

public class AimedBombThrower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        m_mouseAiming = GetComponent<MouseAiming>();
        m_bombThrower = GetComponent<BombThrower>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0 /* LMB */))
        {
            m_bombThrower.ThrowBomb(m_mouseAiming.ThrowVector);
        }
    }

    private MouseAiming m_mouseAiming;
    private BombThrower m_bombThrower;
}
