using UnityEngine;
using System.Collections;
using System;

public class ControlNave : MonoBehaviour {

    public float fuerzaGravedad = 0.5f;

    public float posicionPiso = -3.4f;
    public float fuerzaGiro = 0.5f;
    public float fuerzaCohete = 1;
    public float maximoFuerzaMovimiento = 1;

    public float movimientoX = 0, movimientoY = 0;

    Vector2 posicionInicial;

    public float minimaPosicionX = -20;
    public float maximaPosicionX = 20;
    public float maximaPosicionY = 15;

    // Use this for initialization
    void Start () {
        posicionInicial = transform.position;
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

        //Verificamos el máximo de fuerza de movimiento:
        if (movimientoX + movimientoY > maximoFuerzaMovimiento)
        {
            float relacion = movimientoX / movimientoY;

            movimientoY = maximoFuerzaMovimiento / (1 + relacion);
            movimientoX = maximoFuerzaMovimiento - movimientoY;
        }

        //Aplicamos los movimientos resultantes:
        nuevaPosicion.x = nuevaPosicion.x + movimientoX;
        nuevaPosicion.y = nuevaPosicion.y + movimientoY;

        //Actualizamos la posicion:
        transform.position = nuevaPosicion;

        //Aplicamos el giro resultante:
        transform.eulerAngles = new Vector3(0, 0, giroActual);

        if (transform.position.y <= posicionPiso)
        {
            reiniciar();
        }
        else if (transform.position.x < minimaPosicionX
            || transform.position.x > maximaPosicionX
            || transform.position.y > maximaPosicionY)
        {
            reiniciar();
            transform.position = posicionInicial;
        }
	
	}

    private void reiniciar()
    {
        transform.position = new Vector2(transform.position.x, posicionPiso);

        transform.localEulerAngles = new Vector3(0, 0, 0);

        movimientoX = 0;
        movimientoY = 0;
    }

    private bool presionandoMotorIzquierdo()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) return true;

        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 3) return true;
        }

        return false;
    }

    private bool presionandoMotorDerecho()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) return true;
        
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x > 2 * Screen.width / 3) return true;
        }

        return false;
    }
}
