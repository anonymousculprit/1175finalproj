using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRope : MonoBehaviour
{
    public GameObject m_LinkPrefab          = null;
    public int        m_nLinks              = 8;
    public float      m_OverlapPercentage   = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert( GetComponent<Rigidbody2D>() != null );
        Debug.Assert( m_LinkPrefab != null );
        Debug.Assert( m_LinkPrefab.GetComponent<Rigidbody2D>() != null );
        Debug.Assert( m_LinkPrefab.GetComponent<HingeJoint2D>() != null );

        // This is how long our sprite link is
        var LinkSpriteHeight = m_LinkPrefab.GetComponent<SpriteRenderer>().size.y/2;
        LinkSpriteHeight -= LinkSpriteHeight * m_OverlapPercentage;

        // Initial Position
        var InitPosY = m_LinkPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        InitPosY -= InitPosY/2.0f * m_OverlapPercentage;

        // Create Links
        var PrevRB = GetComponent<Rigidbody2D>();
        for ( int i=0; i< m_nLinks; i++ )
        {
            var ChainObj = Instantiate(m_LinkPrefab, this.transform.position + new Vector3(0, -InitPosY * i, 0), this.transform.rotation );
            var Hinge    = ChainObj.GetComponent<HingeJoint2D>();
            var RB       = ChainObj.GetComponent<Rigidbody2D>();

            Hinge.autoConfigureConnectedAnchor  = false;
            Hinge.connectedBody                 = PrevRB;
            Hinge.anchor                        = new Vector2(0,  LinkSpriteHeight);

            if(i != 0) Hinge.connectedAnchor = new Vector2(0, -LinkSpriteHeight);

            PrevRB = RB;
        }
    }
}
