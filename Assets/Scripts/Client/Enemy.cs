using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using RiptideNetworking;
using Client.Net;

namespace Client
{
    public class Enemy : MonoBehaviour
    {

        public float health = 100f;

        public GameObject enemy;
        public GameObject ground;
        public Transform player;

        public bool isActive;

        public bool isInRange;

        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);

        }

        private void Respawn()
        {
            Vector3 pos = new Vector3(Random.Range(ground.transform.position.x + 50, ground.transform.position.x - 50), 2.67f, Random.Range(ground.transform.position.z + 35, ground.transform.position.z - 50));
            Instantiate(enemy, pos, Quaternion.identity);
        }

        private void Update()
        {
            SendPositionAndRotation();
        }

        #region Shooting

        private void Start()
        {
            StartCoroutine(FindPlayer(1.5f));
        }

        private IEnumerator Attack(float time)
        {
            RaycastHit hit;


            if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
            {
                Debug.Log(hit.transform.name + "dsadsa");
            }

            yield return new WaitForSeconds(time);
        }

        private IEnumerator FindPlayer(float time)
        {
            while (isActive)
            {
                transform.LookAt(player);
                if (!isInRange)
                {
                    transform.position += transform.forward * 10;
                }
                else
                {
                    StartCoroutine(Attack(1));
                }
                yield return new WaitForSeconds(time);

            }
        }

        private void SendPositionAndRotation()
        {
            Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.enemypos);
            message.AddVector3(transform.position);
            message.AddQuaternion(transform.rotation);

            NetworkManager.Singleton.Client.Send(message);
        }
        #endregion
    }

}
