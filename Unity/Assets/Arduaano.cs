using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Arduaano : MonoBehaviour {
    SerialPort stream;

    float pos;
    bool pressedA;
    bool pressedB;
    bool inHole;
    bool tilted;
    [SerializeField] float speed = .75f;
    [SerializeField] float flip_speed = 10;
    [SerializeField] float acceleration = .35f;
    [SerializeField] float bounds = 8;
    [SerializeField] float slide = 64;
    [SerializeField] float tumble = .55f;
    [SerializeField] bool wobbling = false;
    [SerializeField] float wobble_intensity = 0.08f;
    [SerializeField] float wobble_speed = 7.1f;
    [SerializeField] Transform visual;
    [SerializeField] float visual_range = 5;
    [SerializeField] float visual_offset = 1.2f;
    float wobble = 0;
    float moveTo = 0;
    [SerializeField] Sprite neutral, butt;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] GameObject carA, carB;
    [SerializeField] AudioSource src, src2, src3, src4;
    [SerializeField] GameObject loseText;
    [SerializeField] GameObject[] spawnables;
    [SerializeField] HandleScore score;
    bool wallchance = true;

    private void OnDestroy() {
        stream.Close();
        loseText.SetActive(true);
    }

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        Time.timeScale = 0;
        for (int i = 0; i < 12; i++) {
            try { stream = new SerialPort($"COM{i}", 9600); stream.Open(); i = 14; }
            catch { }
        }
        Time.timeScale = 1;
    }

    private void Start() {
        StartCoroutine(ReadDataFromSerialPort());
        StartCoroutine(SpawnProcess());
    }

    IEnumerator SpawnProcess() {
        while (true) {
            wallchance = !wallchance;
            Instantiate(spawnables[UnityEngine.Random.Range(10, spawnables.Length)], new Vector3(UnityEngine.Random.Range(-bounds + 1, bounds - 1), transform.position.y + 20, 4), Quaternion.identity);
            Instantiate(spawnables[UnityEngine.Random.Range(wallchance ? 0 : 10, spawnables.Length)], new Vector3(UnityEngine.Random.Range(-bounds + 1, bounds - 1), transform.position.y + 20, 4), Quaternion.identity);
            yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(3, 7));
        }
    }
    //Getting the inputs
    IEnumerator ReadDataFromSerialPort() {
        while (true) {
            if (stream.BytesToRead > 0) {
                string tmpA = stream.ReadLine();
                string[] tmpB = tmpA.Split(',');
                if (tmpB[0] == "D4") {
                    tilted = Convert.ToBoolean(int.Parse(tmpB[1]));
                }
                if (tmpB[0] == "D5") {
                    inHole = Convert.ToBoolean(int.Parse(tmpB[1]));
                }
                if (tmpB[0] == "D6") {
                    pressedA = Convert.ToBoolean(int.Parse(tmpB[1]));
                }
                if (tmpB[0] == "D7") {
                    pressedB = Convert.ToBoolean(int.Parse(tmpB[1]));
                }
                if (tmpB[0] == "A0") {
                    pos = (float.Parse(tmpB[1]) - .5f) * acceleration;
                }
            }
            score.addScore(1);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    private void Update() {
        FingeringHole();
        Shoot();
        Flip();
        MovePlayer();
    }

    void FingeringHole() {
        if (inHole)
            src3.Play();
        else
            src4.Play();
    }

    void Shoot() {
        //Shoot car
        if (pressedA && !tilted && !inHole) {
            pressedA = false;
            Instantiate(carA, transform.position + Vector3.forward, transform.rotation);
        }
        //Shoot more car
        if (pressedB && !tilted && !inHole) {
            pressedB = false;
            Instantiate(carB, transform.position + Vector3.forward, transform.rotation);
        }
        //Shoot no car, only fart
        else if ((pressedA || pressedB) && !src.isPlaying && tilted) {

            src.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            src.Play();
        }
    }

    void Flip() {
        float zRot = 0f;
        float offset = 0f;

        offset = Mathf.Tan((visual.position.x - transform.position.x) / (visual.position.y - transform.position.y)) * Mathf.Rad2Deg;
        offset *= -1;

        src2.volume = 0;

        if (tilted) {
            renderer.sprite = butt;
            //Make upside down!
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 180 + offset), Time.deltaTime * flip_speed);
        }
        else if (transform.rotation != Quaternion.Euler(0, 0, zRot + offset)) {
            //Put upright!
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, zRot + offset), Time.deltaTime * flip_speed);

            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * flip_speed);
            renderer.sprite = neutral;
        }
        if (Mathf.Abs(moveTo) > slide * tumble && !tilted && !inHole) {
            transform.rotation = Quaternion.Euler(0, 0, moveTo * Time.time * 8000);
            src2.volume = 100;
        }
    }

    void MovePlayer() {
        //Wobble character around when needed
        if (wobbling) wobble = (float)Math.Sin(Time.time * wobble_speed) * wobble_intensity;
        moveTo += wobble * (inHole ? 0 : 1);

        //Update next pos data
        moveTo += pos;
        moveTo = Mathf.Clamp(moveTo, -slide, slide);

        //Get some cool data
        Vector3 plr = transform.position;
        Vector3 dir = new Vector3(moveTo * speed * (inHole ? .5f : 1), 0, 0) * Time.deltaTime;

        //Display input indicator
        visual.position = new Vector3(plr.x + pos * visual_range, plr.y + visual_offset, plr.z + 5);

        //Check boundaries, otherwise move
        if (plr.x < -bounds) {
            transform.position = new Vector3(-bounds, plr.y, plr.z);
            moveTo = 0;
        }
        if (plr.x > bounds) {
            transform.position = new Vector3(bounds, plr.y, plr.z);
            moveTo = 0;
        }
        else
            transform.position += dir;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "DestructableWall") {
            if (tilted)
                Destroy(collision.gameObject);
            else
                Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy") {
            Destroy(gameObject);
        }
    }
}
