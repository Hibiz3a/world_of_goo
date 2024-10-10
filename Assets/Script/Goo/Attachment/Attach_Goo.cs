using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attach_Goo : MonoBehaviour, IPointerClickHandler
{
    private SpringJoint2D Sj;
    private CircleCollider2D Cc;

    [SerializeField] private int RadiusDetection;
    [SerializeField] private SpringJoint2D[] springJoints; 
    [SerializeField] private LineRenderer LineTrace;
    
    
    public static Attach_Goo selectedGoo1;
    private Rigidbody2D rb2d;
    private int JointCount;
    private List<LineRenderer> activeLines = new List<LineRenderer>();
    
    
    private GameObject AttachGoo;

    private RaycastHit2D[] hit;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        springJoints = GetComponents<SpringJoint2D>();
        
        Sj = gameObject.GetComponent<SpringJoint2D>();
        Cc = gameObject.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        foreach (var line in activeLines)
        {
            if (line != null)
            {
                SpringJoint2D joint = springJoints[0];  // On suppose qu'il y a un SpringJoint
                if (joint.connectedBody != null)
                {
                    // Mettre à jour la position du LineRenderer avec les positions actuelles des deux objets connectés
                    line.SetPosition(0, transform.position);  // Position du premier objet
                    line.SetPosition(1, joint.connectedBody.transform.position);  // Position du corps connecté
                }
            }
        }
    }
    

    private bool DetectGooInRange()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(gameObject.transform.position, RadiusDetection, Vector2.zero,
            RadiusDetection);
        hit = hits;
        if (hits.Length > 0)
        {
            for (int i = 0; i <= hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Goo"))
                {
                    Debug.Log("true");
                    return true;
                }
            }
        }

        return false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (CompareTag("Goo") && DetectGooInRange())
        {
            if (selectedGoo1 == null)
            {

                selectedGoo1 = this;
                Debug.Log("Premier objet sélectionné : " + gameObject.name);
            }
            else if (selectedGoo1 != this)
            {

                if (selectedGoo1.JointCount < 2)
                {

                    CreateSpringJoint(selectedGoo1, this);
                }
                else if (JointCount < 2)
                {

                    CreateSpringJoint(this, selectedGoo1);
                }

                selectedGoo1 = null;  
            }
        }
    }
    

    private void CreateSpringJoint(Attach_Goo goo1, Attach_Goo goo2)
    {
        SpringJoint2D availableJoint = null;
        foreach (var joint in goo1.springJoints)
        {
            if (joint.connectedBody == null)  
            {
                availableJoint = joint;
                break;
            }
        }

        if (availableJoint != null)
        {
            availableJoint.connectedBody = goo2.rb2d;
            availableJoint.autoConfigureConnectedAnchor = false;
            availableJoint.connectedAnchor = Vector2.zero;
            goo1.JointCount++;  
            goo2.JointCount++;  
            
            CreateLineRenderer(goo1, goo2);

            Debug.Log("Connexion créée entre " + goo1.gameObject.name + " et " + goo2.gameObject.name);
        }
    }
    
    private void CreateLineRenderer(Attach_Goo goo1, Attach_Goo goo2)
    {
        LineRenderer line = this.AddComponent<LineRenderer>();
        
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.positionCount = 2;
        line.material = new Material(Shader.Find("Sprites/Default"));  // Utilise un matériau par défaut
        line.startColor = Color.black;
        line.endColor = Color.black;

        // Définir les positions initiales de la ligne
        line.SetPosition(0, goo1.transform.position);  // Position du premier Goo
        line.SetPosition(1, goo2.transform.position);  // Position du deuxième Goo

        // Ajouter la ligne à la liste des connexions actives
        activeLines.Add(line);
    }
}