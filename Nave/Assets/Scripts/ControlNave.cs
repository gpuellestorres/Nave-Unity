using UnityEngine;
using System.Collections;

public class ControlNave : MonoBehaviour {

    public float fuerzaGravedad = 0.5f;
    public float posicionPiso = -3.4f;
    public float fuerzaGiro = 0.5f;
    public float fuerzaCohete = 1;

    public float movimientoX = 0, movimientoY = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float tiempoTranscurrido = Time.deltaTime;
        Vector2 nuevaPosicion= transform.position;
        float giroActual = transform.localEulerAngles.z;

        print((giroActual + " - " + Mathf.Sin(giroActual * Mathf.Deg2Rad) + " - " + Mathf.Cos(giroActual * Mathf.Deg2Rad)));

        if (presionandoMotorIzquierdo()) 
        {
            giroActual -= fuerzaGiro * tiempoTranscurrido;
            movimientoX -= Mathf.Sin(giroActual * Mathf.Deg2Rad) * fuerzaCohete * tiempoTranscurrido;
            movimientoY += Mathf.Cos(giroActual * Mathf.Deg2Rad) * fuerzaCohete * tiempoTranscurrido;
        }
        if (presionandoMotorDerecho())
        {
            giroActual += fuerzaGiro * tiempoTranscurrido;
            movimientoX -= Mathf.Sin(giroActual * Mathf.Deg2Rad) * fuerzaCohete * tiempoTranscurrido;
            movimientoY += Mathf.Cos(giroActual * Mathf.Deg2Rad) * fuerzaCohete * tiempoTranscurrido;
        }

        //Aplicamos la gravedad:
        movimientoY -= fuerzaGravedad * tiempoTranscurrido;

        //Aplicamos los movimientos resultantes:
        nuevaPosicion.x = nuevaPosicion.x + movimientoX;
        nuevaPosicion.y = nuevaPosicion.y + movimientoY;

        //Actualizamos la posicion:
        transform.position = nuevaPosicion;

        //Aplicamos el giro resultante:
        transform.eulerAngles = new Vector3(0, 0, giroActual);

        if (transform.position.y <= posicionPiso) 
        {
            transform.position = new Vector2(transform.position.x, posicionPiso);

            transform.localEulerAngles = new Vector3(0,0,0);

            movimientoX = 0;
            movimientoY = 0;
        }
	
	}

    private bool presionandoMotorIzquierdo()
    {
        return Input.GetKey(KeyCode.A);
    }

    private bool presionandoMotorDerecho()
    {
        return Input.GetKey(KeyCode.D);
    }
}
