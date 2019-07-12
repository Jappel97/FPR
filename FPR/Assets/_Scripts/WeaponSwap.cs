using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{

    public GameObject weakSword;
    public GameObject strongSword;

    private GameObject currSword;
    
    // Start is called before the first frame update
    void Start()
    {
        weakSword.GetComponent<SwordPickup>().enabled = false;
        strongSword.GetComponent<SwordPickup>().enabled = false;
        weakSword.SetActive(true);
        currSword = weakSword;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void upgradeSword()
    {
        currSword.SetActive(false);
        currSword = strongSword;
        currSword.SetActive(true);
    }

    public int getLow()
    {
        if(currSword == strongSword)
        {
            return 5;
        }
        else
        {
            return 2;
        }
    }

    public int getHigh()
    {
        if(currSword == strongSword)
        {
            return 10;
        }
        else
        {
            return 7;
        }
    }
}
