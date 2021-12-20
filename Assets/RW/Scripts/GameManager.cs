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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RayWenderlich.SpaceInvadersUnity
{
    public class GameManager : MonoBehaviour
    {
        internal static GameManager Instance;

            [SerializeField] 
                private AudioSource sfx;

            [SerializeField] 
                private GameObject explosionPrefab;

            [SerializeField] 
                private float explosionTime = 1f;

            [SerializeField] 
                private AudioClip explosionClip;

            [SerializeField] 
                private int maxLives = 3;

            [SerializeField] 
                private Text livesLabel;
                private int lives;

            [SerializeField] 
                private MusicControl music;

            [SerializeField] 
                private Text scoreLabel;

            [SerializeField] 
                private GameObject gameOver;

            //Stores a reference to the All Clear panel, which displays when the player eliminates all the invaders.
            [SerializeField] 
                private GameObject allClear;

            [SerializeField] 
                private Button restartButton;
                private int score;

            //UpdateScore increments score by the value passed to it and updates the UI label to reflect the changes.
            internal void UpdateScore(int value)
                {
                    score += value;
                    scoreLabel.text = $"Score: {score}";
                }
            /*TriggerGameOver shows the Game Over panel if failure is true. It also shows the All Clear panel.
             It also enables the restartButton, pauses the game and stops the music.*/
            internal void TriggerGameOver(bool failure = true)
                {
                    gameOver.SetActive(failure);
                    allClear.SetActive(!failure);
                    restartButton.gameObject.SetActive(true);
                    Time.timeScale = 0f;
                    music.StopPlaying();
                }

            //Script for lives of player
            internal void UpdateLives()
                {
                    lives = Mathf.Clamp(lives - 1, 0, maxLives);
                    livesLabel.text = $"Lives: {lives}";

                    //Trigger for game over
                    if (lives > 0) 
                        {
                            return;
                        }
                    
                    TriggerGameOver();
                }

            /*CreateExplosion creates an explosion at position with a 
            random rotation along the Z-axis, and destroys it after explosionTime seconds*/
            internal void CreateExplosion(Vector2 position)
                {
                    PlaySfx(explosionClip);

                    var explosion = Instantiate(explosionPrefab, position,
                    Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)));
                    Destroy(explosion, explosionTime);
                }

            internal void PlaySfx(AudioClip clip) => sfx.PlayOneShot(clip);
            
            //Awake handles the onClick event for the restart button. It reloads the scene when clicked.
            private void Awake()
                {
                    if (Instance == null) 
                        {
                             Instance = this;
                        }
                    else if (Instance != this) 
                        {
                            Destroy(gameObject);
                        }
                    //Script for setting the default value for lives and also updates the UI label.
                    lives = maxLives;
                    livesLabel.text = $"Lives: {lives}";
                    
                    score = 0;
                    scoreLabel.text = $"Score: {score}";
                    gameOver.gameObject.SetActive(false);
                    allClear.gameObject.SetActive(false);

                    restartButton.onClick.AddListener(() =>
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                            Time.timeScale = 1f;
                        });
                    restartButton.gameObject.SetActive(false);
                }
            

    }
}