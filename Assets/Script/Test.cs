using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
//    float speed;
//    float damage;
//    float hp;
//    GameObject gameObjectTest;
//    Transform transformTest;
//    float timer = 10f;

//    public float Damage { get => damage; set => damage = value; }

//    public event EventHandler OnSpawn; // Создане евента 
//    private void Start()
//    {
//        InvokeRepeating("Spawn", timer, timer);
//    }
//    private void Update()
//    {
//        if(Time.time >= timer)
//        {
//            timer += timer;
//            Spawn();
//        }
//    }
//    void SetSpeed()
//    {
//        if (Input.GetKeyDown(KeyCode.W))
//        {
//            speed += 10;
//        }
//        else
//        {
//            speed = 5;
//        }
//    }
//    void SetDamage()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            Damage += 10f;
//        }
//        if (Input.GetButtonDown("Fire1"))
//        {

//        }
//    }

//    private void Die()
//    {
//        Destroy(gameObject);    
//    }

//    void SetHp()
//    {
//        if (hp == 0f)
//        {
//            Die();
//        }
//    }
//    void Spawn()
//    {
//        GameObject newGameObject = Instantiate(gameObjectTest, transformTest.position, Quaternion.identity);
//        //newGameObject.transform.position += new Vector3(0, 1, 0) * speed;
//        newGameObject.GetComponent<Rigidbody2D>().velocity = new Vector3 (0,1,0) * speed; // передвижение объекта с помощью физики 
//        Destroy(newGameObject, 5f);
//        OnSpawn?.Invoke(this, EventArgs.Empty); // вызов в задоном месте 
//    }

    
   
//}
//class Tes1t : MonoBehaviour , IDamage
//{ 
//    public Test test;

//    private void Start()
//    {
//        test.OnSpawn += Test_OnSpawn; 
//    }

//    private void Test_OnSpawn(object sender, EventArgs e) // что должны делать при вызове евента 
//    {
//        test.Damage = 10f;
//    }
//}
//interface IDamage
//{
//    float damage { get; set; }

//    void Hit();

}