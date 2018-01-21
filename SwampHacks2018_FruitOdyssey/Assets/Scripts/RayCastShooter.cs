using UnityEngine;
using System.Collections;

public class RayCastShooter : MonoBehaviour
{

    public int gunDamage = 1;                                           // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float fireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
    public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
    public float hitForce = 100f;                                       // Amount of force which will be added to objects with a rigidbody shot by the player
    public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun
	public GameObject gun;

	private OVRCameraRig fpsCam;                                              // Holds a reference to the first person camera
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
    private AudioSource gunAudio;                                       // Reference to the audio source which will play our shooting sound effect
    private LineRenderer laserLine;                                     // Reference to the LineRenderer component which will display our laserline
    private float nextFire;                                            // Float to store the time the player will be allowed to fire again, after firing
	private string[] fruits = {"Apple ", "Banana", "cherry", "Orange ", "Pear"};
    private ArrayList masterSequenceList = new ArrayList();
    private ArrayList sequenceSoFarList = new ArrayList();
    private int maxCount = 6; // change it
    private int currentCount = 0;
    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Text messageText;
	public UnityEngine.UI.Text sequenceSoFar;
    private string sequenceSoFarText = "";

    void Start()
    {
        // Get and store a reference to our LineRenderer component
        laserLine = GetComponent<LineRenderer>();
		// Get and store a reference to our AudioSource component
        gunAudio = GetComponent<AudioSource>();

        // Get and store a reference to our Camera by searching this GameObject and its parents
		fpsCam = GetComponentInParent<OVRCameraRig>();
        generateSequence("");
    }

    void generateSequence(string message)
    {
        currentCount = 0;
        masterSequenceList = new ArrayList(); //re-initialize
        sequenceSoFarList = new ArrayList(); //re-initialize
        sequenceSoFarText = "";
        int i = 0;
        System.Random random = new System.Random();
        string tempMsgStr = "";
        while (i < maxCount-1)
        {
            int index = random.Next(0, 5);
            masterSequenceList.Add(fruits[index]);
            tempMsgStr += fruits[index]+", ";
            i++;
        }

        int index_1 = random.Next(0, 5);
        masterSequenceList.Add(fruits[index_1]);
        tempMsgStr += fruits[index_1];


        currentCount = 0; //re-initialize
		messageText.text = message + "Your challenge is to pick the fruits in the following order: \n" + tempMsgStr; //initially no message
        scoreText.text = "Score: " + currentCount + "/" + maxCount.ToString();
        sequenceSoFar.text = "";
    }


    void Update()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(ShotEffect());


			System.Console.Write ("before hit");

			// Create a vector at the center of our camera's viewport
			Vector3 rayOrigin = fpsCam.centerEyeAnchor.position;//(new Vector3(0.5f, 0.5f, 0.0f));
			//Vector3 rayOrigin = gun.transform.position- transform.position;
            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            //laserLine.SetPosition(0, gunEnd.position);
			messageText.text = "before raycast stuff";


            // Check if our raycast has hit anything
			if (Physics.Raycast(gunEnd.position, transform.forward, out hit, weaponRange))

			//if (Physics.Raycast(transform.position, fpsCam.transform.forward, out hit, weaponRange))
            {
				// Set the end position for our laser line 
				messageText.text = "before rigid body " + hit.collider.gameObject.name;
                //laserLine.SetPosition(1, hit.point);
				//messageText.text = "before rigid body " + hit.rigidbody.gameObject.name;

                // Get a reference to a health script attached to the collider we hit
                //ShootableBox health = hit.collider.GetComponent<ShootableBox>();

                // If there was a health script attached
                //if (health != null)
                //{
                    // Call the damage function of that script, passing in our gunDamage variable
                //    health.Damage(gunDamage);
                //}

                // Check if the object we hit has a rigidbody attached
				if (hit.collider != null)
                {
                    // Add force to the rigidbody we hit, in the direction from which it was hit
					//hit.collider.AddForce(-hit.normal * hitForce);
                    //check the type
					string tag = hit.collider.gameObject.name;
					messageText.text = "";
					if (tag.Contains((string)masterSequenceList[currentCount]) && !tag.Contains("Plant") && !tag.Contains("Tree") && currentCount<maxCount)
					{
                        string hitNow = hit.collider.gameObject.name;
						//messageText.text = hitNow;
						//System.Console.Write ("inside");
						GameObject.Destroy (hit.collider.gameObject);
						//hit.rigidbody.gameObject.SetActive(false);
                        currentCount++;
                        scoreText.text = "Score: " + currentCount.ToString() + "/" + maxCount.ToString();
                        sequenceSoFarList.Add(hitNow);
                        if (currentCount > 1)
                        {
                            sequenceSoFarText += ", ";
                        }
                        sequenceSoFarText += hitNow;
                        sequenceSoFar.text = sequenceSoFarText;

                        if (currentCount == maxCount)
                        {
                            generateSequence("Congratulations! If you would like to exit the game, click the Exit button. Else...\n");
                        }

                    } else
                    {


                    }
                }
            }
            //else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                //laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }


    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        gunAudio.Play();

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}
