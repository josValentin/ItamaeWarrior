using UnityEngine;

public class RSushi : Enemy
{

    //public GameObject Pez;
    //====================================================================
    //=====			Rotar
    //====================================================================
    public float RPS;
    public float rotar;
    public float TDest;
    public float inte;


    protected override void OnStart()
    {

    }

    private float t;

    public void Update()
    {
        t += Time.deltaTime;
        if (t >= TDest)
        {
            t = 0;

            DieNoAnimation();
            return;
        }

        while (rotar == 0)
        {
            rotar = Random.Range(-1, 2);
        }

        this.transform.Rotate(new Vector3(0, 0, rotar * RPS));
    }

}
