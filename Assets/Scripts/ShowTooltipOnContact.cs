using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class ShowTooltipOnContact : MonoBehaviour
{
    public string text;

    [SerializeField] public GameObject tooltipPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Showing tooltip");

            GameObject textGo = Instantiate(tooltipPrefab);
            textGo.GetComponent<TextMeshPro>().text = text;
            ContextualTooltip tooltip = textGo.GetComponent<ContextualTooltip>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(player);
            tooltip.StickToTarget(player, new Vector3(0, 3, 2));
            tooltip.Deploy(3.5f);
        }
    }

}
