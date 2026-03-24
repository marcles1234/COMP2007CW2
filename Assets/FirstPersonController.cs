using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public float walkSpeed = 1;
    public float runSpeed = 2;
    public float turnSpeed = 1;
    public float gravity = -9.81f;
    public float verticalVelocity;
    public Transform camTransform;
    private CharacterController character;
    private float pitch = 0;
    public StaminaBar staminaRegen;
    public Slider slider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        character = GetComponent<CharacterController>();
    }


    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * y;

        if (Input.GetButton("Sprint") && slider.value > 0)
        {
            character.Move(moveDirection * runSpeed * Time.deltaTime);
            staminaRegen.increase = 1;
        } else if (Input.GetButtonUp("Sprint"))
        {
            character.Move(moveDirection * walkSpeed * Time.deltaTime);
            StartCoroutine(WaitToRegen());
        } else
        {
            character.Move(moveDirection * walkSpeed * Time.deltaTime);
        }
       

        float mX = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        float mY = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;

        pitch -= mY;

        pitch = Mathf.Clamp(pitch, -90, 90);

        camTransform.localRotation = Quaternion.Euler(pitch, 0, 0);

        transform.Rotate(Vector3.up * mX);

        // Apply gravity
        if (character.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // keeps player "stuck" to ground
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Apply vertical movement
        Vector3 verticalMove = new Vector3(0, verticalVelocity, 0);
        character.Move(verticalMove * Time.deltaTime);
    }

    IEnumerator WaitToRegen()
    {
        staminaRegen.increase = 0;
        yield return new WaitForSeconds(2f);
        staminaRegen.increase = 2;
    }
}
