/*
 * Copyright (c) 2021 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using UnityEngine;

namespace RayWenderlich.SpaceInvadersUnity
{
    public class BulletSpawner : MonoBehaviour
    {
        // For linking a bullet spawner with an invader 
        // The currentRow is updated if the player's bullets hit this spawner.
        internal int currentRow;
        // The column is set once and won't change.
        internal int column;

        [SerializeField] 
        private AudioClip shooting;

        [SerializeField] 
        private GameObject bulletPrefab;

        [SerializeField] 
        private Transform spawnPoint;

        [SerializeField] 
        private float minTime;

        [SerializeField]
        private float maxTime;

        private float timer;
        private float currentTime;
        private Transform followTarget;

        internal void Setup()
            {
                currentTime = Random.Range(minTime, maxTime);
                followTarget = InvaderSwarm.Instance.GetInvader(currentRow, column);
            }
        /*For updating the position of the bullet spawner to match the followTarget.position
        and incrementing the timer until it reaches currentTime*/
        private void Update()
            {
                transform.position = followTarget.position;

                timer += Time.deltaTime;
                if (timer < currentTime) 
                    {
                        return;
                    }

                Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
                GameManager.Instance.PlaySfx(shooting);
                timer = 0f;
                currentTime = Random.Range(minTime, maxTime);
            }

        /*OnCollisionEnter2D returns without doing anything if the object that hit 
        the bullet spawner wasn't of type Bullet.*/
        private void OnCollisionEnter2D(Collision2D other)
            {
                if (!other.collider.GetComponent<Bullet>()) 
                    {
                        return;
                    }

                GameManager.Instance.
                UpdateScore(InvaderSwarm.Instance.GetPoints(followTarget.gameObject.name));

                InvaderSwarm.Instance.IncreaseDeathCount();

                followTarget.GetComponentInChildren<SpriteRenderer>().enabled = false;
                currentRow = currentRow - 1;
                if (currentRow < 0) 
                    {
                        gameObject.SetActive(false);
                    }
                else 
                    {
                        Setup();
                    }
            }
    }
}