using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Sushi2;
	public float times;

    Transform playerT;
    private void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("PlayerBody").transform;
    }

    void Update()
    {
		times += Time.deltaTime;
		float timeRand = Random.Range(2, 6);
		if(times >= timeRand && times <= timeRand + 0.1f){
			lanzarS();
			times = 0;
		}
    }

	void lanzarS()
	{
        float randForceX = Random.Range(2.5f, 4.7f);
        float randForceY = Random.Range(1.5f, 2.7f);

        int randForce = Random.Range(8, 18);

        Vector2 force = (playerT.position - transform.position).normalized * randForce;

        if (this.transform.position.x < 0 && this.transform.position.y < 0){

			Vector3 der = new Vector2(randForceX, randForceY);
			GameObject newS = Instantiate(Sushi2, (Vector2)this.transform.position, Quaternion.identity);
			newS.GetComponent<Rigidbody2D>().gravityScale = 0;
			newS.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
		}else if(this.transform.position.x < 0 && this.transform.position.y > 0){

			Vector3 der = new Vector2(randForceX, -randForceY);
			GameObject newS = Instantiate(Sushi2, (Vector2)this.transform.position, Quaternion.identity);
			newS.GetComponent<Rigidbody2D>().gravityScale = 0;
			newS.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
		}else if(this.transform.position.x > 0 && this.transform.position.y > 0){
	
			Vector3 der = new Vector2(-randForceX, -randForceY);
			GameObject newS = Instantiate(Sushi2, (Vector2)this.transform.position, Quaternion.identity);
			newS.GetComponent<Rigidbody2D>().gravityScale = 0;
			newS.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
		}else if(this.transform.position.x > 0 && this.transform.position.y < 0){

			Vector3 der = new Vector2(-randForceX, randForceY);
			GameObject newS = Instantiate(Sushi2, (Vector2)this.transform.position, Quaternion.identity);
			newS.GetComponent<Rigidbody2D>().gravityScale = 0;
			newS.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
		}
	}
}
