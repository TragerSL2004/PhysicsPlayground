using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RopeBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ropeObject;

    [SerializeField]
    private int m_sectionAmount;

    [SerializeField]
    private int m_sectionDistance;

    private Vector3 newPosition;

    private void Awake()
    {
        AddRope(m_sectionAmount);
    }

    private void AddRope(int ropeLength)
    {
        //Get a position a certain distance from the parent to spawn sections at
        newPosition = new Vector3(transform.position.x, transform.position.y + m_sectionDistance, transform.position.z);

        //The object the rope will be made out of
        GameObject currentSection = Instantiate(m_ropeObject, transform.position, Quaternion.identity);

        for (int i = 1; i < ropeLength; i++)
        {
            //Creates the set number of clones of the current section at a new position
            currentSection.transform.localPosition = transform.position;
            GameObject nextSection = Instantiate(m_ropeObject, newPosition, currentSection.transform.rotation);
            currentSection.GetComponent<SpringJoint>().connectedBody = nextSection.GetComponent<Rigidbody>();
            currentSection = nextSection;
        }
    }
}
